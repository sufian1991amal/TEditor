using System;
using System.Linq;
using System.Threading.Tasks;
using TEditor.Abstractions;
using UIKit;

namespace TEditor
{
    public class TEditorImplementation : BaseTEditor
    {
        UINavigationController GetNavController(UIViewController view)
        {
            switch (view)
            {
                case UINavigationController navigationController:
                    if (navigationController.ViewControllers.First().ChildViewControllers.FirstOrDefault() is UISplitViewController split)
                        return GetNavController(split.ViewControllers[1]);
                    return navigationController;
                case UITabBarController uiTabBarController:
                    return GetNavController(uiTabBarController.SelectedViewController);

                default:
                    {
                        foreach (var vc in view.ChildViewControllers)
                        {
                            var nav = GetNavController(vc);
                            if (nav != null)
                                return nav;
                        }
                    }
                    return null;
            }

        }
        public override Task<TEditorResponse> ShowTEditor(string html, ToolbarBuilder toolbarBuilder = null, EventHandler<ToolbarBuilderEventArgs> toolbarBuilderOnClick = null, bool autoFocusInput = false)
        {
            TaskCompletionSource<TEditorResponse> taskRes = new TaskCompletionSource<TEditorResponse>();
            var tvc = new TEditorViewController();
            ToolbarBuilder builder = toolbarBuilder;
            if (toolbarBuilder == null)
                builder = new ToolbarBuilder();
            tvc.BuildToolbar(builder, toolbarBuilderOnClick);
            tvc.SetHTML(html);
            tvc.SetAutoFocusInput(autoFocusInput);
            tvc.Title = CrossTEditor.PageTitle;
            
            UINavigationController nav = null;

            tvc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(CrossTEditor.CancelText, UIBarButtonItemStyle.Plain, (item, args) =>
            {
                if (nav != null)
                {
                    if (UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                        nav.DismissModalViewController(true);
                    else
                        nav.PopViewController(true);
                }
                taskRes.SetResult(new TEditorResponse() { IsSave = false, HTML = string.Empty });
            }), true);
            tvc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(CrossTEditor.SaveText, UIBarButtonItemStyle.Done, async (item, args) =>
            {
                if (nav != null)
                {
                    if (UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                        nav.DismissModalViewController(true);
                    else
                        nav.PopViewController(true);
                }
                taskRes.SetResult(new TEditorResponse() { IsSave = true, HTML = await tvc.GetHTML() });
            }), true);

            var root = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var window = UIApplication.SharedApplication.Windows;

            var rootWindow = window?.FirstOrDefault()?.RootViewController?.ModalViewController;
            if (rootWindow != null)
            {
                nav = GetNavController(rootWindow);
            }
            else
            {
                nav = GetNavController(root);
            }

            if (nav != null)
            {
                if (UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    var childViewControllers = nav.ChildViewControllers;

                    var newnav = new UINavigationController(tvc);
                    newnav.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
                    nav.PresentModalViewController(newnav, true);
                }
                else
                    nav.PushViewController(tvc, true);
            }
            return taskRes.Task;
        }
    }
}
