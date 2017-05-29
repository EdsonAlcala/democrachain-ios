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
			};
		}

		public async Task<bool> DownloadHomepage()
		{
			var httpClient = new HttpClient(); // Xamarin supports HttpClient!
			//httpClient.DefaultRequestHeaders.Accept.Clear();
			//httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpResponseMessage response = await httpClient.GetAsync("http://democrachain-api.azurewebsites.net/vote");
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
	}
}
