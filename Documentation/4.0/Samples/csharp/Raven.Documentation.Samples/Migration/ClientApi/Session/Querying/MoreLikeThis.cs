using System.Collections.Generic;
using System.Linq;
#region more_like_this_6
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.MoreLikeThis;
#endregion

/*
#region more_like_this_5
using Raven.Abstractions.Data;
using Raven.Client.Bundles.MoreLikeThis;
using Raven.Client.Document;
#endregion
*/

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class MoreLikeThis
    {
        private class Foo
        {
            public Foo()
            {
                /*
                #region more_like_this_1
                Article[] articles = session
                    .Advanced
                    .MoreLikeThis<Article>(
                        "Articles/MoreLikeThis",
                        null,
                        new MoreLikeThisQuery
                        {
                            IndexName = "Articles/MoreLikeThis",
                            DocumentId = "articles/1",
                            Fields = new[] { "Body" }
                        });
                #endregion
                */

                /*
                #region more_like_this_3
                Article[] articles = session
                    .Advanced
                    .MoreLikeThis<Article>(
                        "Articles/MoreLikeThis",
                        null,
                        new MoreLikeThisQuery
                        {
                            IndexName = "Articles/MoreLikeThis",
                            DocumentId = "articles/1",
                            Fields = new[] { "Body" },
                            AdditionalQuery = "Category:IT"
                        });
                #endregion
                */
            }
        }

        public MoreLikeThis()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region more_like_this_2
                    List<Article> articles = session
                        .Query<Article>("Articles/MoreLikeThis")
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1")
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { "Body" }
                            }))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region more_like_this_4
                    List<Article> articles = session
                        .Query<Article>("Articles/MoreLikeThis")
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1")
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { "Body" }
                            }))
                        .Where(x => x.Category == "IT")
                        .ToList();
                    #endregion
                }
            }
        }

        private class Article
        {
            public string Id { get; set; }

            public string Category { get; set; }
        }
    }
}
