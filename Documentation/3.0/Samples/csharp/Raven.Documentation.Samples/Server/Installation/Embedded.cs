using Raven.Client.Embedded;

namespace Raven.Documentation.Samples.Server.Installation
{
	public class Embedded
	{
		public void InitEmbeddedSample()
		{
			#region embedded_1
			EmbeddableDocumentStore store = new EmbeddableDocumentStore
			{
				DataDirectory = "Data"
			};
			#endregion
		}

		public void InitEmbeddedHttpSample()
		{
			#region embedded_2
			EmbeddableDocumentStore store = new EmbeddableDocumentStore
			{
				DataDirectory = "Data",
				UseEmbeddedHttpServer = true
			};
			#endregion
		}
	}
}
