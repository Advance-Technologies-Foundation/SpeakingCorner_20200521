namespace SCDemo.UnitTests
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using NUnit.Framework;
	using Terrasoft.Common;
	using Terrasoft.Configuration.Tests;

	[TestFixture]
	[MockSettings(RequireMock.DBEngine | RequireMock.DBSecurityEngine)]
	public class ContactReporterTests: BaseConfigurationTestFixture
	{
		[SetUp]
		protected override void SetUp() {
			base.SetUp();
			AddCustomizedEntitySchemas();
		}

		private void PrepareTestData(Guid contactId, string name, List<string> activities) {
			SetUpTestData("Contact", data => data.Has(contactId), new Dictionary<string, object>() {
				{ "Name", name }
			});
			var activityData = new List<Dictionary<string, object>>();
			foreach (var activityTitle in activities) {
				activityData.Add(new Dictionary<string, object>() {
					{ "Activity.Title", activityTitle }
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

		private void SetUpScalarTestData<T>(string schemaName, Action<SelectData> filterAction, T value) {
			var selectData = new SelectData(UserConnection, schemaName);
			filterAction.Invoke(selectData);
			selectData.MockScalar<T>(value);
		}

		private void AddCustomizedEntitySchemas() {
			EntitySchemaManager.AddCustomizedEntitySchema("Contact", new Dictionary<string, string> {
				{ "Name", "MediumText"},
			});
			EntitySchemaManager.AddCustomizedEntitySchema("Activity", new Dictionary<string, string> {
				{ "Title", "MediumText"},
			});

			var activityParticipantSchema = EntitySchemaManager.AddCustomizedEntitySchema("ActivityParticipant", new Dictionary<string, string> {
				{ "Activity.Title", "MediumText"}
			});
			activityParticipantSchema.AddLookupColumn("Contact", "Participant");
			activityParticipantSchema.AddLookupColumn("Activity", "Activity");
		}

		[Test]
		public void GetInfo_ShouldReturnExpectedValue() {
			//Arrange
			var contactId = Guid.NewGuid();
			var name = "Expected Name";
			var firstActivityTitle = "Expected first title";
			var secondActivityTitle = "Expected second title";
			var reporter = new ESQContactReporter() {UserConnection = UserConnection};
			PrepareTestData(contactId, name, new List<string>() {firstActivityTitle, secondActivityTitle});

			//Act
			var actual = reporter.GetInfo(contactId);

			//Assert
			Assert.AreEqual(name, actual.ContactName);
			Assert.AreEqual(2, actual.Activities.Count);
			Assert.IsTrue(actual.Activities.Any(x=>x == firstActivityTitle));
			Assert.IsTrue(actual.Activities.Any(x=>x == secondActivityTitle));

		}
	}
}
