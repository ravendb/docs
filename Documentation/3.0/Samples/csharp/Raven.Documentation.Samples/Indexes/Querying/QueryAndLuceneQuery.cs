using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string EyeColor { get; set; }
		public byte Age { get; set; }
	}

	public class QueryAndLuceneQuery
	{
		public QueryAndLuceneQuery()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region immutable_query
					var query = session.Query<User>().Where(x => x.Name.StartsWith("A"));

					var ageQuery = query.Where(x => x.Age > 21);

					var eyeQuery = query.Where(x => x.EyeColor == "blue");
					#endregion

					#region mutable_lucene_query
					var documentQuery = session
						.Advanced
						.DocumentQuery<User>()
						.WhereStartsWith(x => x.Name, "A");

					var ageDocumentQuery = documentQuery
						.WhereGreaterThan(x => x.Age, 21);

					var eyeDocumentQuery = documentQuery
						.WhereEquals(x => x.EyeColor, "blue");

					// here all of the DocumentQuery variables are the same references
					#endregion

					#region default_operator
					session
						.Advanced
						.DocumentQuery<User>()
						.UsingDefaultOperator(QueryOperator.And);
					#endregion
				}
			}
		}
	}
}