using System;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using AssetsLibrary;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http.Headers;

namespace democrachain
{
	public partial class ViewController : UIViewController
	{
		LoadingOverlay loadPop;
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic		
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			approvedButton.TouchUpInside += async (sender, e) =>
			{
				Task<bool> sizeTask = DownloadHomepage();
				var intResult = await sizeTask;
				//UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
				//avAlert.Show();
				var alert = UIAlertController.Create("Success", "Your vote has been sent!", UIAlertControllerStyle.Alert);
				if (alert.PopoverPresentationController != null)
					alert.PopoverPresentationController.BarButtonItem = sender as UIBarButtonItem;

				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, action => Ok()));
				//alert.AddAction(UIAlertAction.Create("Snooze", UIAlertActionStyle.Default, action => Snooze()));

				PresentViewController(alert, animated: true, completionHandler: null);

				//navigate to the other page

			};
		}

		//void Show
		void Ok()
		{
			//throw new NotImplementedException();
			TestController homePage = this.Storyboard.InstantiateViewController("TestController") as TestController;
			if (homePage != null)
			{
				this.NavigationController.PushViewController(homePage, true);
			}
		}

		public async Task<bool> DownloadHomepage()
		{
			var bounds = UIScreen.MainScreen.Bounds;

			// show the loading overlay on the UI thread using the correct orientation sizing
			loadPop = new LoadingOverlay(bounds); // using field from step 2
			View.Add(loadPop);

			var httpClient = new HttpClient(); // Xamarin supports HttpClient!
											   //httpClient.DefaultRequestHeaders.Accept.Clear();
											   //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response = await httpClient.GetAsync("http://democrachain-api.azurewebsites.net/vote");
			loadPop.Hide();
			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationController.SetNavigationBarHidden(true, false);
		}
	}
}
