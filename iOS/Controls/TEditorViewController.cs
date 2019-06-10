using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using PopColorPicker.iOS;
using System.Threading.Tasks;
using TEditor.Abstractions;

namespace TEditor
{
    public class TEditorViewDelegate : UIWebViewDelegate
    {
        TEditor.Abstractions.TEditor _richTextEditor;

        public TEditorViewDelegate(TEditor.Abstractions.TEditor richTextEditor)
        {
            _richTextEditor = richTextEditor;
        }

        public override bool ShouldStartLoad(UIWebView webView, Foundation.NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            string urlString = request.Url.AbsoluteString;
            if (navigationType == UIWebViewNavigationType.LinkClicked)
                return false;
            return true;
        }

        public override void LoadingFinished(UIWebView webView)
        {
            _richTextEditor.EditorLoaded = true;
            _richTextEditor.SetPlatformAsIOS();
            if (string.IsNullOrEmpty(_richTextEditor.InternalHTML))
                _richTextEditor.InternalHTML = "";
            _richTextEditor.UpdateHTML();

            if (_richTextEditor.AutoFocusInput)
                _richTextEditor.Focus();
        }
    }

    [Foundation.Register("TEditorViewController")]
    public sealed class TEditorViewController : UIViewController
    {
        TEditor.Abstractions.TEditor _richTextEditor;
        UIWebView _webView;
        UIScrollView _toolbarScroll;
        UIToolbar _toolbar;
        UIView _toolbarHolder;
        UIBarButtonItem _keyboardItem;
        List<UIBarButtonItem> _uiToolbarItems;
        double _keyboardHeight;
        PopColorPickerViewController _colorPickerViewController;
        UIPopoverController _popoverController;

        UIView contentView;
        public TEditorViewController() : base()
        {
            _richTextEditor = new TEditor.Abstractions.TEditor();
            _richTextEditor.SetJavaScriptEvaluatingFunction((input) =>
            {
                _webView.EvaluateJavascript(input);
            });
            _richTextEditor.SetJavaScriptEvaluatingWithResultFunction((input) =>
            {
                return Task.Run<string>(() =>
                {
                    string res = string.Empty;
                    InvokeOnMainThread(() =>
                    {
                        res = _webView.EvaluateJavascript(input);
                    });
                    return res;
                });
            });
        }

        void StyleWebView()
        {
            _webView = new UIWebView(new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Height));
            _webView.Delegate = new TEditorViewDelegate(_richTextEditor);

            _webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth
            | UIViewAutoresizing.FlexibleHeight
            | UIViewAutoresizing.FlexibleTopMargin
            | UIViewAutoresizing.FlexibleBottomMargin;
            _webView.KeyboardDisplayRequiresUserAction = false;
            _webView.ScalesPageToFit = true;
            _webView.BackgroundColor = UIColor.White;
            _webView.ScrollView.Bounces = false;

            this.Add(_webView);
            HideFormAccessoryBar.SetHideFormAccessoryBar(true);

            if (IsIpad())
            {
                foreach (var subview in _webView.ScrollView.Subviews)
                {
                    if (subview.Class.Name == "UIWebBrowserView")
                    {
                        contentView = subview;
                    }
                }
                InputAssistantItem.AllowsHidingShortcuts = false;
                InputAssistantItem.LeadingBarButtonGroups = new[] { new UIBarButtonItemGroup(_uiToolbarItems.ToArray(), null) };
            }
        }

        void KeyboardWillShow(NSNotification notification)
        {
            if (contentView != null)
                InputAssistantItem.LeadingBarButtonGroups = new[] { new UIBarButtonItemGroup(_uiToolbarItems.ToArray(), null) };
        }

        void StyleScrollView()
        {
            var width = this.View.Frame.Width - 44;
            if (IsIpad())
                width = this.View.Frame.Width;
            _toolbarScroll = new UIScrollView()
            {
                Frame = new CGRect(0, 0, width, 44)
            };
            _toolbarScroll.BackgroundColor = UIColor.Clear;
            _toolbarScroll.ShowsVerticalScrollIndicator = false;
        }

        void StyleToolbar()
        {
            _toolbar = new UIToolbar(_toolbarScroll.Frame);
            _toolbar.BackgroundColor = UIColor.Clear;
            _toolbarScroll.AddSubview(_toolbar);
            _toolbarScroll.AutoresizingMask = _toolbar.AutoresizingMask;
        }

        void AddColorPickerControl()
        {
            _colorPickerViewController = new PopColorPickerViewController();

            _colorPickerViewController.CancelButton.Clicked += (object sender, EventArgs e) =>
            {
                if (IsIpad())
                    _popoverController.Dismiss(true);
                else
                    _colorPickerViewController.NavigationController.PopViewController(true);
            };

            _colorPickerViewController.DoneButton.Clicked += (object sender, EventArgs e) =>
            {
                if (IsIpad())
                    _popoverController.Dismiss(true);
                else
                    _colorPickerViewController.NavigationController.PopViewController(true);


                nfloat r, g, b, a;
                _colorPickerViewController.SelectedColor.GetRGBA(out r, out g, out b, out a);
                _richTextEditor.SetTextColor((int)(r * 255), (int)(g * 255), (int)(b * 255));
            };

            _richTextEditor.LaunchColorPicker = () =>
            {
                if (IsIpad())
                {
                    var navController = new UINavigationController(_colorPickerViewController);

                    _popoverController = new UIPopoverController(navController);
                    _popoverController.PresentFromRect(_toolbarHolder.Frame, View, UIPopoverArrowDirection.Down, true);
                }
                else
                {
                    this.NavigationController.PushViewController(_colorPickerViewController, true);
                }
            };
        }

        void LayoutToolBarHolder()
        {
            float toolbarHeight = _uiToolbarItems.Count > 0 ? 44f : 0f;

            UIToolbar backgroundToolbar = new UIToolbar()
            {
                Frame = new CGRect(0, 0, this.View.Frame.Width, toolbarHeight)
            };
            backgroundToolbar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            _toolbarHolder = new UIView(new CGRect(0, this.View.Frame.Height, this.View.Frame.Width, toolbarHeight));
            _toolbarHolder.AutoresizingMask = _toolbar.AutoresizingMask;
            _toolbarHolder.AddSubview(_toolbarScroll);
            _toolbarHolder.InsertSubview(backgroundToolbar, 0);
            // Hide Keyboard
            if (!IsIpad())
            {
                // Toolbar holder used to crop and position toolbar
                UIView toolbarCropper = new UIView(new CGRect(this.View.Frame.Width - toolbarHeight, 0, 44, toolbarHeight));
                toolbarCropper.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin;
                toolbarCropper.ClipsToBounds = true;

                // Use a toolbar so that we can tint
                UIToolbar keyboardToolbar = new UIToolbar(new CGRect(-7, -1, 44, toolbarHeight));
                toolbarCropper.AddSubview(keyboardToolbar);

                _keyboardItem = new UIBarButtonItem(UIImage.FromFile("ZSSkeyboard.png"), UIBarButtonItemStyle.Plain, delegate (object sender, EventArgs e)
                {
                    this.View.EndEditing(true);
                });

                keyboardToolbar.Items = new[] { _keyboardItem };

                _toolbarHolder.AddSubview(toolbarCropper);

                UIView line = new UIView(new CGRect(0, 0, 0.6, 44));
                line.BackgroundColor = UIColor.LightGray;
                line.Alpha = (nfloat)0.7;
                toolbarCropper.AddSubview(line);
            }

            float toolbarWidth = _uiToolbarItems.Count == 0 ? 0.0f : ((_uiToolbarItems.Count * 39) + 50);

            _toolbar.Items = _uiToolbarItems.ToArray();
            _toolbar.Frame = new CGRect(0, 0, toolbarWidth, toolbarHeight);
            _toolbarScroll.ContentSize = new CGSize(_toolbar.Frame.Width, _toolbar.Frame.Height);

            if (!IsIpad())
            {
                this.View.AddSubview(_toolbarHolder);
            }
        }

        bool IsIpad()
        {
            return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
        }

        public void BuildToolbar(ToolbarBuilder builder, EventHandler<ToolbarBuilderEventArgs> onClick)
        {
            _uiToolbarItems = new List<UIBarButtonItem>();
            foreach (var toolbaritem in builder)
            {
                _uiToolbarItems.Add(new UIBarButtonItem(
                    UIImage.FromFile(toolbaritem.ImagePath),
                    UIBarButtonItemStyle.Plain,
                    delegate (object sender, EventArgs e)
                    {
                        onClick?.Invoke(_richTextEditor, new ToolbarBuilderEventArgs() { Action = toolbaritem.Action });
                        toolbaritem.ClickFunc.Invoke(_richTextEditor);
                    }
                ));
            }
        }

        public void LoadResource()
        {
            string htmlResource = _richTextEditor.LoadResources();
            _webView.LoadHtmlString(htmlResource, new Foundation.NSUrl("www.xam-consulting.com"));
        }

        public void SetHTML(string html)
        {
            _richTextEditor.InternalHTML = html;
        }

        public async Task<string> GetHTML()
        {
            return await _richTextEditor.GetHTML();
        }

        public void SetAutoFocusInput(bool autoFocusInput)
        {
            _richTextEditor.AutoFocusInput = autoFocusInput;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            StyleWebView();

            StyleScrollView();

            StyleToolbar();

            AddColorPickerControl();

            LayoutToolBarHolder();

            LoadResource();

            _richTextEditor.UpdateHTML();
        }

        NSObject _keyboardWillShowToken;
        NSObject _keyboardWillHideToken;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (IsIpad())
            {
                _keyboardWillShowToken = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShow);
            }
            else
            {
                _keyboardWillShowToken = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShowOrHide);
                _keyboardWillHideToken = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillShowOrHide);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (_keyboardWillShowToken != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardWillShowToken);

            if (_keyboardWillHideToken != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardWillHideToken);

        }

        void KeyboardWillShowOrHide(NSNotification notification)
        {
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

            var info = notification.UserInfo;
            float duration = ((NSNumber)info.ObjectForKey(UIKeyboard.AnimationDurationUserInfoKey)).FloatValue;
            Int32 curve = ((NSNumber)info.ObjectForKey(UIKeyboard.AnimationCurveUserInfoKey)).Int32Value;
            CGRect keyboardEnd = ((NSValue)info.ObjectForKey(UIKeyboard.FrameEndUserInfoKey)).CGRectValue;

            double sizeOfToolbar = _toolbarHolder.Frame.Height;

            if (keyboardEnd.Height != 0)
                _keyboardHeight = keyboardEnd.Height;

            UIViewAnimationOptions animationOptions = (UIViewAnimationOptions)(curve << 16);

            if (notification.Name == UIKeyboard.WillShowNotification)
            {

                UIView.Animate(duration, 0, animationOptions, () =>
                {
                    CGRect frame = _toolbarHolder.Frame;
                    frame.Y = this.View.Frame.Height - (nfloat)(_keyboardHeight + sizeOfToolbar);
                    _toolbarHolder.Frame = frame;

                    CGRect editorFrame = _webView.Frame;
                    editorFrame.Height = this.View.Frame.Height;
                    _webView.Frame = editorFrame;

                }, null);
            }
            else
            {
                UIView.Animate(duration, 0, animationOptions, () =>
                {
                    CGRect frame = _toolbarHolder.Frame;
                    frame.Y = this.View.Frame.Height + (nfloat)_keyboardHeight;
                    _toolbarHolder.Frame = frame;

                    CGRect editorFrame = _webView.Frame;
                    editorFrame.Height = this.View.Frame.Height;
                    _webView.Frame = editorFrame;

                }, null);
            }
        }
    }
}

