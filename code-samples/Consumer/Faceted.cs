namespace RavenCodeSamples.Consumer
{
	public class Faceted : CodeSampleBase
	{
		public void GetTermsSample()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region getterms1
					session.Advanced.DatabaseCommands.GetTerms("indexName", "MyProperty", null, 128);
					#endregion
				}
			}
		}
	}
}
