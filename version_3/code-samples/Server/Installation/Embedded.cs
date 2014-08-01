using Raven.Client.Embedded;

namespace Raven.Documentation.CodeSamples.Server.Installation
{
	public class Embedded
	{
		public void InitEmbeddedSample()
		{
			#region embedded_1
			var store = new EmbeddableDocumentStore
			{
				DataDirectory = "Data"
			};
			#endregion
		}

		public void InitEmbeddedHttpSample()
		{
			#region embedded_2
			var store = new EmbeddableDocumentStore
			{
				DataDirectory = "Data",
				UseEmbeddedHttpServer = true
			};
			#endregion
		}
	}
}
