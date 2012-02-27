using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenCodeSamples.Consumer;

namespace RavenCodeSamples.Server
{
    public class Bundles : CodeSampleBase
    {
		public class User
		{
			public string Name { get; set; }
		}
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

				#region versioning3
				using (var session = store.OpenSession())
				{
					session.Store(new User { Name = "Ayende Rahien" });
					session.SaveChanges();
				}
				#endregion
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
		public ArticleIndex()
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
