using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
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
					IRavenQueryable<User> query = session.Query<User>().Where(x => x.Name.StartsWith("A"));

					IRavenQueryable<User> ageQuery = query.Where(x => x.Age > 21);

					IRavenQueryable<User> eyeQuery = query.Where(x => x.EyeColor == "blue");
					#endregion

					#region mutable_lucene_query
					IDocumentQuery<User> documentQuery = session
						.Advanced
						.DocumentQuery<User>()
						.WhereStartsWith(x => x.Name, "A");

					IDocumentQuery<User> ageDocumentQuery = documentQuery
						.WhereGreaterThan(x => x.Age, 21);

					IDocumentQuery<User> eyeDocumentQuery = documentQuery
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