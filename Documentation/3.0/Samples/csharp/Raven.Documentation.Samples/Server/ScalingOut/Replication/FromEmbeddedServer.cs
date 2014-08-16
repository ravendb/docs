using Raven.Abstractions.Data;
using Raven.Abstractions.Replication;
using Raven.Client.Embedded;

namespace Raven.Documentation.CodeSamples.Server.ScalingOut.Replication
{
	public class FromEmbeddedServer
	{
		public FromEmbeddedServer()
		{
			using (var store = new EmbeddableDocumentStore())
			{
				#region from_embedded_server_1
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "Northwind",
								// other configuration options omitted for simplicity
								Settings =
									{
										{ Constants.ActiveBundles, "Replication" }
									}
							});
				#endregion

				#region from_embedded_server_2
				using (var session = store.OpenSession("Northwind"))
				{
					session.Store(new ReplicationDocument
					{
						Destinations =
							{
								new ReplicationDestination
									{
										Url = "http://destination-server-url:8080/", 
										Database = "destination_database_name"
									},
							}
					});
				}
				#endregion
			}
		}

		public void Sample()
		{
			#region from_embedded_server_3
			var store = new EmbeddableDocumentStore
				            {
					            UseEmbeddedHttpServer = true
				            };
			#endregion
		}
	}
}