using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using TEditor.Abstractions;
using TEditor.Controls;

namespace TEditor
{
    public class TEditorImplementation : BaseTEditor
    {

        private FrameLayout DecoreView => (FrameLayout)(MainActivity).Window.DecorView;
        public static Bundle Bundle { get; set; }
        public static Activity MainActivity { get; set; }

        public static void SetBundel(Bundle bundle, Activity mainActivity)
        {
            Bundle = bundle;
            MainActivity = mainActivity;
        }
        public override Task<TEditorResponse> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null, EventHandler<ToolbarBuilderEventArgs> toolbarBuilderOnClick = null, bool autoFocusInput = false)
        {
            var result = new TaskCompletionSource<TEditorResponse>();
        
            DecoreView.AddView(new TEditorView(MainActivity, html, autoFocusInput, toolbarBuilder, toolbarBuilderOnClick));

            TEditorView.SetOutput = (res, resStr) =>
            {
                TEditorView.SetOutput = null;
                if (res)
                {
                    result.SetResult(new TEditorResponse() { IsSave = true, HTML = resStr });
                }
                else
                    result.SetResult(new TEditorResponse() { IsSave = false, HTML = string.Empty });
            };

            return result.Task;
        }


    }
}
