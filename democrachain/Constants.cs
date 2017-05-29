using System;
namespace democrachain
{
	public class Constants
	{
		// Azure app-specific connection string and hub path
		public const string ConnectionString = "Endpoint=sb://democrachain.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=cOJJLfK1QbKGI5HupjEBlecDS4S4vGrIpoUTjxG7GUk=";
		public const string NotificationHubPath = "democrachainhub";

		public Constants()
		{
		}
	}
}
