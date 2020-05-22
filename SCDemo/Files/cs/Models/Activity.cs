namespace SCDemo.Models
{
	using ATF.Repository;
	using ATF.Repository.Attributes;

	[Schema("Activity")]
	public class Activity: BaseModel
	{
		[SchemaProperty("Title")]
		public string Title { get; set; }
	}
}
