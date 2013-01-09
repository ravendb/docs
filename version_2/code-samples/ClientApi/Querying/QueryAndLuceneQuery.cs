namespace RavenCodeSamples.ClientApi.Querying
{
	using Raven.Abstractions.Data;
	using Raven.Client;
	using Raven.Client.Indexes;
	using Raven.Client.Linq;

	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string EyeColor { get; set; }
		public byte Age { get; set; }
	}

	public class QueryAndLuceneQuery : CodeSampleBase
	{
		public QueryAndLuceneQuery()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region immutable_query
					var query = session.Query<User>().Where(x => x.Name.StartsWith("A"));

					var ageQuery = query.Where(x => x.Age > 21);

					var eyeQuery = query.Where(x => x.EyeColor == "blue");
					#endregion

					#region mutable_lucene_query
					var luceneQuery = session.Advanced.LuceneQuery<User>().WhereStartsWith(x => x.Name, "A");

					var ageLuceneQuery = luceneQuery.WhereGreaterThan(x => x.Age, 21);

					var eyeLuceneQuery = luceneQuery.WhereEquals(x => x.EyeColor, "blue");

					// here all of the lucene query variables are the same references
					#endregion

					#region default_operator
					session.Advanced.LuceneQuery<User>().UsingDefaultOperator(QueryOperator.And);
					#endregion
				}
			}
		}
	}
}