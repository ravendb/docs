using Raven.Client.Document;

namespace RavenCodeSamples.Intro
{
	public class ConnectingToDatastore : CodeSampleBase
	{
		public void RunInServerMode()
		{
			var documentStore = new DocumentStore { Url = "http://myravendb.mydomain.com/" };
			documentStore.Initialize();
		}
	}
}
