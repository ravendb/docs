using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.MoreLikeThis;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    #region more_like_this_4
    public class Article
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ArticleBody { get; set; }
    }

    public class Articles_ByArticleBody : AbstractIndexCreationTask<Article>
    {
        public Articles_ByArticleBody()
        {
            Map = docs => from doc in docs
                          select new
                          {
                              doc.ArticleBody
                          };

            Stores.Add(x => x.ArticleBody, FieldStorage.Yes);
            Analyzers.Add(x => x.ArticleBody, "StandardAnalyzer");
        }
    }
    #endregion

    public class MoreLikeThis
    {
        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region more_like_this_1
                    List<Article> articles = session
                        .Query<Article, Articles_ByArticleBody>()
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1"))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region more_like_this_1_1
                    List<Article> articles = session.Advanced
                        .DocumentQuery<Article, Articles_ByArticleBody>()
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.WhereEquals(y => y.Id, "articles/1")))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region more_like_this_2
                    List<Article> articles = session
                        .Query<Article, Articles_ByArticleBody>()
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1")
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { nameof(Article.ArticleBody) }
                            }))
                        .ToList();
                    #endregion

                    #region more_like_this_3
                    session.Store(new MoreLikeThisStopWords
                    {
                        Id = "Config/Stopwords",
                        StopWords = new List<string> { "I", "A", "Be" }
                    });
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region more_like_this_2_1
                    List<Article> articles = session.Advanced
                        .DocumentQuery<Article, Articles_ByArticleBody>()
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.WhereEquals(y => y.Id, "articles/1"))
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { nameof(Article.ArticleBody) }
                            }))
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
