using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#region more_like_this_8
using Raven.Client.Documents;
#endregion
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.MoreLikeThis;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class MoreLikeThis
    {
        private class Foo
        {
            #region more_like_this_2
            public int? MinimumTermFrequency { get; set; } = 2;

            public int? MaximumQueryTerms { get; set; } = 25;

            public int? MaximumNumberOfTokensParsed { get; set; } = 5000;

            public int? MinimumWordLength { get; set; } = 0;

            public int? MaximumWordLength { get; set; } = 0;

            public int? MinimumDocumentFrequency { get; set; } = 5;

            public int? MaximumDocumentFrequency { get; set; } = int.MaxValue;

            public int? MaximumDocumentFrequencyPercentage { get; set; }

            public bool? Boost { get; set; } = false;

            public float? BoostFactor { get; set; } = 1;

            public string StopWordsDocumentId { get; set; }

            public string[] Fields { get; set; }
            #endregion
        }

        private interface IFoo<T>
        {
            #region more_like_this_1
            IRavenQueryable<T> MoreLikeThis<T>(MoreLikeThisBase moreLikeThis);

            IRavenQueryable<T> MoreLikeThis<T>(Action<IMoreLikeThisBuilder<T>> builder);
            #endregion

            #region more_like_this_3
            IMoreLikeThisOperations<T> UsingAnyDocument();

            IMoreLikeThisOperations<T> UsingDocument(string documentJson);

            IMoreLikeThisOperations<T> UsingDocument(Expression<Func<T, bool>> predicate);

            IMoreLikeThisOperations<T> WithOptions(MoreLikeThisOptions options);
            #endregion
        }

        private class Article
        {
            public string Id { get; set; }

            public string Category { get; set; }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region more_like_this_4
                    // Search for similar articles to 'articles/1'
                    // using 'Articles/MoreLikeThis' index and search only field 'Body'
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

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region more_like_this_5
                    // Search for similar articles to 'articles/1'
                    // using 'Articles/MoreLikeThis' index and search only field 'Body'
                    List<Article> articles = await asyncSession
                        .Query<Article>("Articles/MoreLikeThis")
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1")
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { "Body" }
                            }))
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region more_like_this_6
                    // Search for similar articles to 'articles/1'
                    // using 'Articles/MoreLikeThis' index and search only field 'Body'
                    // where article category is 'IT'
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

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region more_like_this_7
                    // Search for similar articles to 'articles/1'
                    // using 'Articles/MoreLikeThis' index and search only field 'Body'
                    // where article category is 'IT'
                    List<Article> articles = await asyncSession
                        .Query<Article>("Articles/MoreLikeThis")
                        .MoreLikeThis(builder => builder
                            .UsingDocument(x => x.Id == "articles/1")
                            .WithOptions(new MoreLikeThisOptions
                            {
                                Fields = new[] { "Body" }
                            }))
                        .Where(x => x.Category == "IT")
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
