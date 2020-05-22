namespace SCDemo
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Activation;
	using System.ServiceModel.Web;
	using SCDemo.Api;
	using Terrasoft.Core.Factories;
	using Terrasoft.Web.Common;

	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	public class TestContactService: BaseService
	{

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public ContactInfo GetContactInfo(Guid contactId) {
			var reporter = new ContactReporter {
				UserConnection = UserConnection
			};
			return reporter.GetInfo(contactId);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public string GetCurrentContactName() {
			var reporter = ClassFactory.Get<ICurrentContactReporter>();
			reporter.UserConnection = UserConnection;
			return reporter.GetCurrentContactName();
		}

	}
}
