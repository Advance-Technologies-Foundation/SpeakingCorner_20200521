namespace Terrasoft.Configuration.CSDemo
{
	using System;
	using System.Linq;
	using SCDemo.Api;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	[DefaultBinding(typeof(ICurrentContactReporter))]
	public class CurrentContactReporter: ICurrentContactReporter
	{
		public UserConnection UserConnection { get; set; }

		public string GetCurrentContactName() {
			return UserConnection.CurrentUser.ContactName;
		}
	}
}