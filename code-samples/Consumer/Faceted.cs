using System.Linq;

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

					var firstPage = session.Advanced.DatabaseCommands.GetTerms("indexName", "MyProperty", null, 128);
					var secondPage = session.Advanced.DatabaseCommands.GetTerms("indexName", "MyProperty", firstPage.Last(), 128);

					#endregion
				}
			}
		}
	}
}
