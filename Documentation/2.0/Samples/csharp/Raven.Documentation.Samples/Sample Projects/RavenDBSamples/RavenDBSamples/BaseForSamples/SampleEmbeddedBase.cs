using Raven.Client;
using Raven.Client.Embedded;

namespace RavenDBSamples.BaseForSamples
{
	class SampleEmbeddedBase
	{
		public IDocumentStore Store { get; set; }

		public SampleEmbeddedBase()
		{
			Store = new EmbeddableDocumentStore
			{
				Url = "http://localhost:8080",
				DefaultDatabase = "SampleDatabase"
			}.Initialize();
		}
	}
}
