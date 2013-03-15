namespace RavenCodeSamples.ClientApi.Advanced
{
	using System.Linq;

	public class QueryingForTerms : CodeSampleBase
	{
		public void Querying()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region getterms1
					var firstPage = session.Advanced.DocumentStore.DatabaseCommands.GetTerms("indexName", "MyProperty", null, 128);
					var secondPage = session.Advanced.DocumentStore.DatabaseCommands.GetTerms("indexName", "MyProperty", firstPage.Last(), 128);

					#endregion
				}
			}
		}
	}
}