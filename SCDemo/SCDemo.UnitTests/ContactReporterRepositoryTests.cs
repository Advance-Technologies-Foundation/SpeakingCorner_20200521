namespace SCDemo.UnitTests
{
	using System.Collections.Generic;
	using System.Linq;
	using ATF.Repository;
	using NUnit.Framework;
	using SCDemo.Models;
	using Terrasoft.Configuration.Tests;

	// Use mock repository data

	[TestFixture]
	[MockSettings(RequireMock.DBEngine | RequireMock.DBSecurityEngine)]
	public class ContactReporterRepositoryTests: BaseConfigurationTestFixture
	{
		private IRepository _repository;
		private ContactReporter _reporter;
		[SetUp]
		protected override void SetUp() {
			base.SetUp();
			AddCustomizedEntitySchemas();
			_repository = new Repository() {
				UserConnection = UserConnection,
				UseAdminRight = false
			};
			_reporter = new ContactReporter() {
				UserConnection = UserConnection,
				Repository = _repository
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

		[Test]
		public void GetContactName_ShouldReturnExpectedValue() {
			//Arrange
			var name = "ExpectedName";
			var contact = _repository.CreateItem<Contact>();
			contact.Name = name;

			//Act
			var actual = _reporter.GetContactName(contact.Id);

			//Assert
			Assert.AreEqual(name, actual);
		}

		[Test]
		public void GetInfo_ShouldReturnExpectedValue() {
			//Arrange
			var name = "ExpectedName";
			var firstActivityTitle = "Expected first title";
			var secondActivityTitle = "Expected second title";
			var contact = _repository.CreateItem<Contact>();
			contact.Name = name;
			var firstActivityParticipant = _repository.CreateItem<ActivityParticipant>();
			var secondActivityParticipant = _repository.CreateItem<ActivityParticipant>();
			var firstActivity = _repository.CreateItem<Activity>();
			var secondActivity = _repository.CreateItem<Activity>();
			firstActivity.Title = firstActivityTitle;
			secondActivity.Title = secondActivityTitle;
			firstActivityParticipant.Activity = firstActivity;
			secondActivityParticipant.Activity = secondActivity;
			contact.ActivityParticipants = new List<ActivityParticipant>() {
				firstActivityParticipant,
				secondActivityParticipant
			};
			//Act
			var actual = _reporter.GetInfo(contact.Id);

			//Assert
			Assert.AreEqual(name, actual.ContactName);
			Assert.AreEqual(2, actual.Activities.Count);
			Assert.IsTrue(actual.Activities.Any(x=>x == firstActivityTitle));
			Assert.IsTrue(actual.Activities.Any(x=>x == secondActivityTitle));
		}
	}
}
