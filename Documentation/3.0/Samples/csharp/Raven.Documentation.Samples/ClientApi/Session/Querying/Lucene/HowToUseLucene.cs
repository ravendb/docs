using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying.Lucene
{
	public class HowToUseLucene
	{
		private class MyCustomIndex : AbstractIndexCreationTask<Employee>
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
					// load up to 128 entities from 'Employees' collection
					var employees = session
						.Advanced
						.DocumentQuery<Employee>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_3
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					var employees = session
						.Advanced
						.DocumentQuery<Employee>()
						.WhereEquals(x => x.FirstName, "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_4
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					// using 'My/Custom/Index'
					var employees = session
						.Advanced
						.DocumentQuery<Employee>("My/Custom/Index")
						.WhereEquals(x => x.FirstName, "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region document_query_5
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					// using 'My/Custom/Index'
					var employees = session
						.Advanced
						.DocumentQuery<Employee, MyCustomIndex>()
						.WhereEquals("FirstName", "Robert")
						.ToList();
					#endregion
				}
			}
		}
	}
}