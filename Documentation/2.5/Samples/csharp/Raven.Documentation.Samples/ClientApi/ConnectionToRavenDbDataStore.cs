namespace RavenCodeSamples.ClientApi
{
	using Raven.Client.Document;
	using Raven.Client.Embedded;

	public class ConnectionToRavenDbDataStore : CodeSampleBase
	{
		public void RunningInServerMode()
		{
			#region running_in_server_mode
			var documentStore = new DocumentStore { Url = "http://myravendb.mydomain.com/" };
			documentStore.Initialize();

			#endregion

			documentStore.Dispose();
		}

		public void RunningInEmbeddedMode()
		{
			#region running_in_embedded_mode
			var documentStore = new EmbeddableDocumentStore { DataDirectory = "path/to/database/directory" };
			documentStore.Initialize();

			#endregion

			documentStore.Dispose();
		}

		public void SilverlightSupport()
		{
			#region silverlight_support
			var documentStore = new DocumentStore { Url = "http://myravendb.mydomain.com/" };
			documentStore.Initialize();

			#endregion

			documentStore.Dispose();
		}

		public void UsingConnectionString()
		{
			#region using_connection_string
			var documentStore = new DocumentStore
				                    {
					                    ConnectionStringName = "MyRavenConStr"
				                    };

			#endregion

			documentStore.Dispose();
		}
	}
}