using Raven.Client;
using Raven.Client.Document;

namespace RavenDBSamples.BaseForSamples
{
	public class SampleBase
	{
		public IDocumentStore Store { get; set; }

		public SampleBase()
		{
			Store = new DocumentStore
			{
				Url = "http://localhost:8080",
				DefaultDatabase = "SampleDatabase"
			}.Initialize();
		}
	}
}