using Raven.Client;
using Raven.Client.Document;

namespace RavenDBSamples.BaseForSamples
{
	public class SampleBase
	{
		public IDocumentStore DocumentStore { get; set; }

		public SampleBase()
		{
			DocumentStore = new DocumentStore
			{
				Url = "http://localhost:8080",
				DefaultDatabase = "SampleDatabase"
			}.Initialize();
		}
	}
}