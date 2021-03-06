﻿using Foundation;
using UIKit;
using WindowsAzure.Messaging;
using System;
//version funcionando chingon
namespace democrachain
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		private SBNotificationHub Hub { get; set; }

		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
					   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			Hub = new SBNotificationHub(Constants.ConnectionString, Constants.NotificationHubPath);

			Hub.UnregisterAllAsync(deviceToken, (error) =>
			{
				if (error != null)
				{
					Console.WriteLine("Error calling Unregister: {0}", error.ToString());
					return;
				}

				NSSet tags = null; // create tags if you want
				Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
						{
							if (errorCallback != null)
								Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
						});
			});
		}

		//public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		//{
		//	// show an alert
		//	UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
		//	okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
		//	viewController.PresentViewController(okayAlertController, true, null);

		//	// reset our badge
		//	UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		//}

		//public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		//{
		//	if (application.ApplicationState == UIApplicationState.Active)
		//	{
		//	}
		//	else if (application.ApplicationState == UIApplicationState.Background)
		//	{
		//	}
		//	else if (application.ApplicationState == UIApplicationState.Inactive)
		//	{
		//	}
		//}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		//public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		//{

		//	UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
		//	ViewController Controller = navigationController.Storyboard.InstantiateViewController("ViewController") as ViewController;
		//	//Controller.Region = notification.Region;
		//	navigationController.PushViewController(Controller, false);
		//	UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

		//}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			// Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;

				//Extract the alert text
				// NOTE: If you're using the simple alert by just specifying
				// "  aps:{alert:"alert msg here"}  ", this will work fine.
				// But if you're using a complex alert with Localization keys, etc.,
				// your "alert" object from the aps dictionary will be another NSDictionary.
				// Basically the JSON gets dumped right into a NSDictionary,
				// so keep that in mind.
				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps[new NSString("alert")] as NSString).ToString();

				//If this came from the ReceivedRemoteNotification while the app was running,
				// we of course need to manually process things like the sound, badge, and alert.
				if (!fromFinishedLaunching)
				{
					//Manually show an alert
					if (!string.IsNullOrEmpty(alert))
					{
						try
						{
							UINavigationController navigationController = (UINavigationController)this.Window.RootViewController;
							ViewController Controller = navigationController.Storyboard.InstantiateViewController("ViewController") as ViewController;
							//Controller.Region = notification.Region;
							navigationController.PushViewController(Controller, false);
							UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
						}
						catch (Exception ex)
						{
							var message = ex.Message;
						}

					}
				}
			}
		}
	}
}



