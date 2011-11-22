using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace RavenCodeSamples.Server
{
    public class Bundles : CodeSampleBase
    {
        public void VersioningBundle()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region versioning1
                    session.Store(new {
                                          Exclude = false,
                                          Id = "Raven/Versioning/DefaultConfiguration",
                                          MaxRevisions = 5
                                      });
                    #endregion

                    #region versioning2

                    session.Store(new {
                                          Exclude = true,
                                          Id = "Raven/Versioning/Users",
                                      });

                    #endregion
                }
            }
        }
    }

    #region MoreLikeThis1
    public class Article
    {
        public string Name { get; set; }
        public string ArticleBody { get; set; }
    }

    public class ArticleIndex : AbstractIndexCreationTask<Article>
    {
        public DataIndex()
        {
            Map = docs => from doc in docs
                          select new { doc.ArticleBody };



            Stores = new Dictionary<Expression<Func<Article, object>>, FieldStorage>
                             {
                                 {
                                     x => x.ArticleBody, FieldStorage.Yes
                                 }
                             };

        }
    }
    #endregion
}
