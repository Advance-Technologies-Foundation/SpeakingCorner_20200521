namespace SCDemo.Api
{
	using System;
	using Terrasoft.Core;

	public interface IContactReporter
	{
		UserConnection UserConnection { get; set; }
		string GetContactName(Guid contactId);
	}
}
