using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.DocumentQuery
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EyeColor { get; set; }
        public byte Age { get; set; }
    }

    public class QueryVsDocumentQuery
    {
        public QueryVsDocumentQuery()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region immutable_query
                    IRavenQueryable<User> query = session
                        .Query<User>()
                        .Where(x => x.Name.StartsWith("A"));

                    IRavenQueryable<User> ageQuery = query
                        .Where(x => x.Age > 21);

                    IRavenQueryable<User> eyeQuery = query
                        .Where(x => x.EyeColor == "blue");
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
                        .UsingDefaultOperator(QueryOperator.Or);
                    #endregion
                }
            }
        }
    }
}
