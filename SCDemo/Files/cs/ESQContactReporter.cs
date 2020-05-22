namespace SCDemo
{
	using System;
	using System.Collections.Generic;
	using SCDemo.Api;
	using Terrasoft.Core;
	using Terrasoft.Core.Entities;

	public class ESQContactReporter
	{
		public UserConnection UserConnection { get; set; }

		public ContactInfo GetInfo(Guid contactId) {
			var response = new ContactInfo();

			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Contact");
			esq.AddColumn("Name");
			var contactEntity = esq.GetEntity(UserConnection, contactId);
			response.ContactName = contactEntity.GetTypedColumnValue<string>("Name");

			var activityEsq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "ActivityParticipant");
			var activityTitleColumn = activityEsq.AddColumn("Activity.Title");
			activityEsq.Filters.Add(activityEsq.CreateFilterWithParameters(FilterComparisonType.Equal, "Participant", contactId));
			var entities = activityEsq.GetEntityCollection(UserConnection);
			response.Activities = new List<string>();
			foreach (var entity in entities) {
				response.Activities.Add(entity.GetTypedColumnValue<string>(activityTitleColumn.Name));
			}
			return response;
		}
	}
}
