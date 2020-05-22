namespace SCDemo.Models
{
	using System.Collections.Generic;
	using ATF.Repository;
	using ATF.Repository.Attributes;

	[Schema("Contact")]
	public class Contact: BaseModel
	{
		[SchemaProperty("Name")]
		public string Name { get; set; }

		[DetailProperty("Participant")]
		public virtual List<ActivityParticipant> ActivityParticipants { get; set; }
	}
}
