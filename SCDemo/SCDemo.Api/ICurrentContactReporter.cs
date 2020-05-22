namespace SCDemo.Api
{
	using Terrasoft.Core;

	public interface ICurrentContactReporter
	{
		UserConnection UserConnection { get; set; }
		string GetCurrentContactName();
	}
}
