// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace democrachain
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton approvedButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel countryLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView divider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton notApprovedButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sinceLabel { get; set; }

        [Action ("UIButton7_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton7_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (approvedButton != null) {
                approvedButton.Dispose ();
                approvedButton = null;
            }

            if (countryLabel != null) {
                countryLabel.Dispose ();
                countryLabel = null;
            }

            if (divider != null) {
                divider.Dispose ();
                divider = null;
            }

            if (notApprovedButton != null) {
                notApprovedButton.Dispose ();
                notApprovedButton = null;
            }

            if (sinceLabel != null) {
                sinceLabel.Dispose ();
                sinceLabel = null;
            }
        }
    }
}