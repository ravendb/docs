using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Embedded;

namespace RavenCodeSamples.Server
{
	public class Deployment
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
