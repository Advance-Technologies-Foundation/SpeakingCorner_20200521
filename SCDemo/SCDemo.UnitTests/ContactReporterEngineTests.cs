namespace SCDemo.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NUnit.Framework;
	using Terrasoft.Common;
	using Terrasoft.Configuration.Tests;

	// Use mock DBEngine data

	[TestFixture]
	[MockSettings(RequireMock.DBEngine | RequireMock.DBSecurityEngine)]
	public class ContactReporterEngineTests: BaseConfigurationTestFixture
	{
		private ContactReporter _reporter;
		[SetUp]
		protected override void SetUp() {
			base.SetUp();
			AddCustomizedEntitySchemas();
			_reporter = new ContactReporter() {
				UserConnection = UserConnection
			};
		}

		private void AddCustomizedEntitySchemas() {
			EntitySchemaManager.AddCustomizedEntitySchema("Contact", new Dictionary<string, string> {
				{ "Name", "MediumText"},
			});
			EntitySchemaManager.AddCustomizedEntitySchema("Activity", new Dictionary<string, string> {
				{ "Title", "MediumText"},
			});

			var activityParticipantSchema = EntitySchemaManager.AddCustomizedEntitySchema("ActivityParticipant", new Dictionary<string, string> {
			});
			activityParticipantSchema.AddLookupColumn("Contact", "Participant");
			activityParticipantSchema.AddLookupColumn("Activity", "Activity");
		}

		private void PrepareTestData(Guid contactId, string name, List<string> activities) {
			SetUpTestData("Contact", data => data.Has(contactId), new Dictionary<string, object>() {
				{ "Id", contactId },
				{ "Name", name }
			});
			var activityData = new List<Dictionary<string, object>>();
			foreach (var activityTitle in activities) {
				var activityId = Guid.NewGuid();
				activityData.Add(new Dictionary<string, object>() {
					{ "Id", Guid.NewGuid() },
					{ "ActivityId", activityId },
					{ "ParticipantId", contactId }
				});
				SetUpTestData("Activity", data => data.Has(activityId), new Dictionary<string, object>() {
					{ "Id", activityId },
					{ "Title", activityTitle }
				});
			}
			SetUpTestData("ActivityParticipant", data => data.Has(contactId), activityData.ToArray());
		}

		private void SetUpTestData(string schemaName, Action<SelectData> filterAction, params Dictionary<string, object>[] items) {
			var selectData = new SelectData(UserConnection, schemaName);
			items.ForEach(values => selectData.AddRow(values));
			filterAction.Invoke(selectData);
			selectData.MockUp();
		}

		[Test]
		public void GetContactName_ShouldReturnExpectedValue() {
			//Arrange
			var contactId = Guid.NewGuid();
			var name = "Expected Name";
			PrepareTestData(contactId, name, new List<string>());

			//Act
			var actual = _reporter.GetContactName(contactId);

			//Assert
			Assert.AreEqual(name, actual);
		}

		[Test]
		public void GetInfo_ShouldReturnExpectedValue() {
			//Arrange
			var contactId = Guid.NewGuid();
			var name = "Expected Name";
			var firstActivityTitle = "Expected first title";
			var secondActivityTitle = "Expected second title";
			PrepareTestData(contactId, name, new List<string>() {firstActivityTitle, secondActivityTitle});
			//Act
			var actual = _reporter.GetInfo(contactId);

			//Assert
			Assert.AreEqual(name, actual.ContactName);
			Assert.AreEqual(2, actual.Activities.Count);
			Assert.IsTrue(actual.Activities.Any(x=>x == firstActivityTitle));
			Assert.IsTrue(actual.Activities.Any(x=>x == secondActivityTitle));
		}
	}
}
