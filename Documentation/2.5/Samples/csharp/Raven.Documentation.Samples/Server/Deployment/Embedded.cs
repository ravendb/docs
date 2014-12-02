namespace RavenCodeSamples.Server.Deployment
{
	using Raven.Client.Embedded;

	public class Embedded
	{
		public void InitEmbeddedSample()
		{
			#region embedded1
			var documentStore = new EmbeddableDocumentStore
			{
				DataDirectory = "Data"
			};
			#endregion
		}

		public void InitEmbeddedHttpSample()
		{
			#region embedded2
			var documentStore = new EmbeddableDocumentStore
			{
				DataDirectory = "Data",
				UseEmbeddedHttpServer = true
			};
			#endregion
		}
	}
}
