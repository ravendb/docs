using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying.Lucene
{
	public class HowToUseLucene
	{
		private class MyCustomIndex : AbstractIndexCreationTask<Person>
		{
		}

		private interface IFoo
		{
			#region document_query_1
			IDocumentQuery<T> DocumentQuery<T, TIndexCreator>() 
				where TIndexCreator : AbstractIndexCreationTask, new();

			IDocumentQuery<T> DocumentQuery<T>(
				string indexName,
				bool isMapReduce = false);

			IDocumentQuery<T> DocumentQuery<T>();
			#endregion
		}

		public HowToUseLucene()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region document_query_2
					// load up to 128 entities from 'People' collection
					var people = session
						.Advanced
						.DocumentQuery<Person>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_3
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					var people = session
						.Advanced
						.DocumentQuery<Person>()
						.WhereEquals(x => x.FirstName, "John")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_4
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					// using 'My/Custom/Index'
					var people = session
						.Advanced
						.DocumentQuery<Person>("My/Custom/Index")
						.WhereEquals(x => x.FirstName, "John")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_5
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					// using 'My/Custom/Index'
					var people = session
						.Advanced
						.DocumentQuery<Person, MyCustomIndex>()
						.WhereEquals("FirstName", "John")
						.ToList();
					#endregion
				}
			}
		}
	}
}