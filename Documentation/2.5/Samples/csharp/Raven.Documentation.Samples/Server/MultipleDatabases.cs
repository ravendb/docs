namespace RavenCodeSamples.Server
{
	using Raven.Client.Extensions;

	public class MultipleDatabases : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region multiple_databases_1
				documentStore.DatabaseCommands.EnsureDatabaseExists("Northwind");
				var northwindSession = documentStore.OpenSession("Northwind");

				#endregion
			}
		}
	}
}