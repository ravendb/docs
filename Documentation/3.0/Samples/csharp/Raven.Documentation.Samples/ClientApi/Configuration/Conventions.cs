namespace Raven.Documentation.Samples.ClientApi.Configuration
{
	using Client.Document;

	public class Conventions
	{
		public Conventions()
		{
			var store = new DocumentStore();

			#region conventions_1
			DocumentConvention conventions = store.Conventions;
			#endregion
		} 
	}
}