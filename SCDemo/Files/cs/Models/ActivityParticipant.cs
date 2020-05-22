namespace SCDemo.Models
{
	using System;
	using ATF.Repository;
	using ATF.Repository.Attributes;

	[Schema("ActivityParticipant")]
	public class ActivityParticipant: BaseModel
	{
		[SchemaProperty("Participant")]
		public Guid ParticipantId { get; set; }

		[LookupProperty("Activity")]
		public virtual Activity Activity { get; set; }
	}
}
