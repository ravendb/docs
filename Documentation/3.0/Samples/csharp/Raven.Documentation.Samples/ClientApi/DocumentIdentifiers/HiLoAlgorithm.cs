namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
	using Client.Document;
	using Json.Linq;

	public class HiLoAlgorithm
	{
		public HiLoAlgorithm()
		{
			var store = new DocumentStore();

			#region replication_hilo

			store.DatabaseCommands.Put("Raven/ServerPrefixForHilo", null,
				new RavenJObject
				{
					{
						"ServerPrefix", "NorthServer/"
					}
				},
				new RavenJObject());

			#endregion
		} 
	}
}