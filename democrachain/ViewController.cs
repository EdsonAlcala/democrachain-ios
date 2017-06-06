using System;
using UIKit;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Json;
using Nethereum.Geth;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Text;

namespace democrachain
{
	public partial class ViewController : UIViewController
	{
		LoadingOverlay loadPop;
		const string url = "http://democrachain-deploy.azurewebsites.net/information";
		Parameter parameters;
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic		
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			parameters = new Parameter();
			notApprovedButton.TouchUpInside += async (sender, e) =>
			{
				ShowLoadingOverlay();
				if (string.IsNullOrEmpty(parameters.ContractAbi))
					parameters = await FetchContractInfoAsync();
                try
                {
					var fromAddress = parameters.FromAddress;
					Web3Geth web3 = new Web3Geth("http://23.98.223.9:8545");

					//var contract = web3.Eth.GetContract(parameters.ContractAbi, parameters.ContractAddress);
					//var voteForOptionFunction = contract.GetFunction("VoteForOption");
					//var voteAddedEvent = contract.GetEvent("VoteAdded");
					//var filterForVoteAddedEvent = await voteAddedEvent.CreateFilterAsync();

					//var transactionHash = await voteForOptionFunction.SendTransactionAsync(fromAddress, "No");

					//var receipt = await MineAndGetReceiptAsync(web3, transactionHash);

					//var logForVoteAdded = await voteAddedEvent.GetFilterChanges<VoteAddedEvent>(filterForVoteAddedEvent);
				}
                catch (Exception)
                {
                    ShowErrorPage();
                    return;
                }
				ShowSuccessMessage(sender);
			};

			approvedButton.TouchUpInside += async (sender, e) =>
			{
				ShowLoadingOverlay();
				if (string.IsNullOrEmpty(parameters.ContractAbi))
					parameters = await FetchContractInfoAsync();
                try
                {
                    var fromAddress = parameters.FromAddress;
                    Web3Geth web3 = new Web3Geth("http://23.98.223.9:8545");

                    //var contract = web3.Eth.GetContract(parameters.ContractAbi, parameters.ContractAddress);
                    //var voteForOptionFunction = contract.GetFunction("VoteForOption");
                    //var voteAddedEvent = contract.GetEvent("VoteAdded");

                    //var filterForVoteAddedEvent = await voteAddedEvent.CreateFilterAsync();
                    ////byte[] sendData = Encoding.ASCII.GetBytes("Yes");
                    //var transactionHash = await voteForOptionFunction.SendTransactionAsync(fromAddress, "Yes");

                    //var receipt = await MineAndGetReceiptAsync(web3, transactionHash);

                    //var logForVoteAdded = await voteAddedEvent.GetFilterChanges<VoteAddedEvent>(filterForVoteAddedEvent);

                }
                catch (Exception)
                {
                    ShowErrorPage();
                    return;
                }
				ShowSuccessMessage(sender);
			};
		}

        private void ShowErrorPage(){
            ErrorController errorPage = this.Storyboard.InstantiateViewController("ErrorController") as ErrorController;
			if (errorPage != null)
			{
				this.NavigationController.PushViewController(errorPage, true);
			}
        }
		public async Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt> MineAndGetReceiptAsync(Web3Geth web3, string transactionHash)
		{

			var miningResult = await web3.Miner.Start.SendRequestAsync(6);

			var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

			while (receipt == null)
			{
				receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
			}

			miningResult = await web3.Miner.Stop.SendRequestAsync();
			return receipt;
		}
		void ShowSuccessMessage(object sender)
		{
			var alert = UIAlertController.Create("Success", "Your vote has been sent!", UIAlertControllerStyle.Alert);
			if (alert.PopoverPresentationController != null)
				alert.PopoverPresentationController.BarButtonItem = sender as UIBarButtonItem;

			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, action => ShowHomePage()));

			PresentViewController(alert, animated: true, completionHandler: null);
		}

		void ShowHomePage()
		{
			TestController homePage = this.Storyboard.InstantiateViewController("TestController") as TestController;
			if (homePage != null)
			{
				this.NavigationController.PushViewController(homePage, true);
			}
		}
		private async Task<Parameter> FetchContractInfoAsync()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync())
			{
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream())
				{
					JsonValue jsonDoc = await Task.Run(() => JsonValue.Load(stream));
					var parameter = new Parameter();
					parameter.ContractAbi = jsonDoc["ContractAbi"];
					parameter.ContractAddress = jsonDoc["ContractAddress"];
					parameter.FromAddress = jsonDoc["FromAddress"];

					return parameter;
				}
			}
		}
		private void HideLoadingOverlay()
		{
			loadPop.Hide();
		}
		private void ShowLoadingOverlay()
		{
			// show the loading overlay on the UI thread using the correct orientation sizing
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds); // using field from step 2
			View.Add(loadPop);
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
	public class VoteAddedEvent
	{
		[Nethereum.ABI.FunctionEncoding.Attributes.Parameter("bytes32", "option", 1, false)]
		public string Option { get; set; }

		[Nethereum.ABI.FunctionEncoding.Attributes.Parameter("uint8", "votesNumber", 2, false)]
		public int VotesNumber { get; set; }
	}

	public class Parameter
	{
		public string ContractAbi
		{
			get;
			set;
		}

		public string ContractAddress
		{
			get;
			set;
		}

		public string FromAddress
		{
			get;
			set;

		}
	}
}
