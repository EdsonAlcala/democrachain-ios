using Foundation;
using System;
using UIKit;

namespace democrachain
{
    public partial class ErrorController : UIViewController
    {
        partial void UIButton2362_TouchUpInside(UIButton sender)
        {
            ViewController initPage = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
            if (initPage != null)
            {
                this.NavigationController.PushViewController(initPage, true);
            }
        }

        public ErrorController (IntPtr handle) : base (handle)
        {
        }
    }
}