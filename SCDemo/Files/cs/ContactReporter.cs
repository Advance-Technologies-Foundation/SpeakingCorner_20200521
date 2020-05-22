namespace SCDemo
{
	using System;
	using System.Linq;
	using ATF.Repository;
	using SCDemo.Api;
	using SCDemo.Models;
	using Terrasoft.Core;
	using Terrasoft.Core.Factories;

	[DefaultBinding(typeof(IContactReporter))]
	public class ContactReporter: IContactReporter
	{
		public UserConnection UserConnection { get; set; }

		private IRepository _repository;
		public IRepository Repository {
			get => _repository ?? (_repository = new Repository()
				{UserConnection = UserConnection, UseAdminRight = false});
			set => _repository = value;
		}

		public ContactInfo GetInfo(Guid contactId) {
			var response = new ContactInfo();
			var contact = Repository.GetItem<Contact>(contactId);
			response.ContactName = contact.Name;
			response.Activities = contact.ActivityParticipants.Select(x => x.Activity.Title).ToList();
			return response;
		}

		public string GetContactName(Guid contactId) {
			var contact = Repository.GetItem<Contact>(contactId);
			return contact.Name;
		}
	}
}
