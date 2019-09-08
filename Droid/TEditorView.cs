using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TEditor.Abstractions;
using static Android.Widget.LinearLayout.LayoutParams;
using Color = Android.Graphics.Color;

namespace TEditor.Controls
{
    public class TEditorView : LinearLayout, View.IOnCreateContextMenuListener
    {
        const int ToolbarFixHeight = 60;
        TEditorWebView _editorWebView;
        LinearLayoutDetectsSoftKeyboard _rootLayout;
        LinearLayout _toolbarLayout;
        Android.Support.V7.Widget.Toolbar _topToolBar;
        public FrameLayout DecoreView => (FrameLayout)((Activity)Context).Window.DecorView;


        public static ToolbarBuilder ToolbarBuilder = null;
        public static EventHandler<ToolbarBuilderEventArgs> ToolbarBuilderOnClick = null;

        public static Action<bool, string> SetOutput { get; set; }

        protected TEditorView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TEditorView(Context context) : base(context)
        {

        }
        
        void IDisposable.Dispose()
        {
            SetOutput.Invoke(false, null);
        }
        public override void OnViewRemoved(View child)
        {
            SetOutput.Invoke(false, null);
            base.OnViewRemoved(child);
        }

        public TEditorView(Context context, string value1, bool value2, ToolbarBuilder toolbarBuilder = null, EventHandler<ToolbarBuilderEventArgs> toolbarBuilderOnClick = null) : base(context)
        {
            Tag = "TEditor.Controls.TEditorView";
            ToolbarBuilder = toolbarBuilder;
            if (ToolbarBuilder == null)
                ToolbarBuilder = new ToolbarBuilder();

            ToolbarBuilderOnClick = toolbarBuilderOnClick;
            SetBackgroundColor(Color.White);
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            LinearLayout myRoot = new LinearLayout(context);
            View itemView = inflater.Inflate(Resource.Layout.TEditorActivity, myRoot);
            this.AddView(itemView);
            InitializeComponent(value1, value2);
        }

     
        private void InitializeComponent(string htmlStringvalue, bool autoFocusInputvalue)
        {
            _topToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TopToolbar);
            _topToolBar.Title = CrossTEditor.PageTitle;
            _topToolBar.SetNavigationIcon(Resource.Drawable.backbutton);
            _topToolBar.SetTitleTextColor(Android.Graphics.Color.White);
            _topToolBar.SetOnClickListener(new BackClicked(DecoreView,this));
            _topToolBar.InflateMenu(Resource.Menu.TopToolbarMenu);
            _topToolBar.MenuItemClick += async (sender, e) =>
            {
                if (SetOutput != null)
                {
                    if (e.Item.TitleFormatted.ToString() == "Save")
                    {
                        string html = await _editorWebView.GetHTML();
                        SetOutput.Invoke(true, html);
                    }
                    else
                    {
                        SetOutput.Invoke(false, null);
                    }
                }

                DecoreView.RemoveView(this);

            };

            _rootLayout = FindViewById<LinearLayoutDetectsSoftKeyboard>(Resource.Id.RootRelativeLayout);
            _editorWebView = FindViewById<TEditorWebView>(Resource.Id.EditorWebView);
            _toolbarLayout = FindViewById<LinearLayout>(Resource.Id.ToolbarLayout);

            _rootLayout.onKeyboardShown += HandleSoftKeyboardShwon;
            _editorWebView.SetOnCreateContextMenuListener(this);
            BuildToolbar();

            string htmlString = htmlStringvalue;
            _editorWebView.SetHTML(htmlString);

            bool autoFocusInput = autoFocusInputvalue;
            _editorWebView.SetAutoFocusInput(autoFocusInput);
        }

        public TEditorView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public TEditorView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public TEditorView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _rootLayout.onKeyboardShown -= HandleSoftKeyboardShwon;
        }

        public void BuildToolbar()
        {

            foreach (var item in ToolbarBuilder)
            {
                ImageButton imagebutton = new ImageButton(this.Context);
                imagebutton.Click += async (sender, e) =>
                {
                    ToolbarBuilderOnClick?.Invoke(_editorWebView.RichTextEditor, new ToolbarBuilderEventArgs() { Action = item.Action });
                    item.ClickFunc.Invoke(_editorWebView.RichTextEditor);
                };
                string imagename = item.ImagePath.Split('.')[0];
                int resourceId = (int)typeof(Resource.Drawable).GetField(imagename).GetValue(null);
                imagebutton.SetImageResource(resourceId);
                var toolbarItems = FindViewById<LinearLayout>(Resource.Id.ToolbarItemsLayout);
                toolbarItems.AddView(imagebutton);
            }
        }
        
        public void HandleSoftKeyboardShwon(bool shown, int newHeight)
        {
            if (ToolbarBuilder.Count == 0)
                return;

            if (shown)
            {
                _toolbarLayout.Visibility = Android.Views.ViewStates.Visible;
                int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                _toolbarLayout.Measure(widthSpec, heightSpec);
                int toolbarHeight = _toolbarLayout.MeasuredHeight == 0 ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density) : _toolbarLayout.MeasuredHeight;
                int topToolbarHeight = _topToolBar.MeasuredHeight == 0 ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density) : _topToolBar.MeasuredHeight;
                int editorHeight = newHeight - toolbarHeight - topToolbarHeight;


                _editorWebView.LayoutParameters.Height = editorHeight;
                _editorWebView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
                _editorWebView.RequestLayout();
            }
            else
            {
                if (newHeight != 0)
                {
                    _toolbarLayout.Visibility = Android.Views.ViewStates.Invisible;
                    _editorWebView.LayoutParameters = new LinearLayout.LayoutParams(-1, -1);
                }
            }
        }

        public void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {


        }

     
    }

    public class BackClicked : Java.Lang.Object, View.IOnClickListener
    {
        public FrameLayout DecoreView { get; set; }
        public TEditorView EditorView { get; set; }

        public BackClicked(FrameLayout decoreView, TEditorView editorView)
        {
            DecoreView = decoreView;
            EditorView = editorView;
        }

        public void OnClick(View v)
        {
            TEditorView.SetOutput.Invoke(false, null);
            DecoreView.RemoveView(EditorView);
        }
    }
}