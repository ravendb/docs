using System.Collections.Generic;
using System.Linq;

using Lucene.Net.Analysis.Standard;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client.Bundles.MoreLikeThis;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Server.Bundles
{
	#region more_like_this_4
	public class Article
	{
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
			Analyzers.Add(x => x.ArticleBody, typeof(StandardAnalyzer).FullName);
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
					string key = "articles/1";

					#region more_like_this_1
					var list = session.Advanced.MoreLikeThis<Article, Articles_ByArticleBody>(key);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					string key = "articles/1";

					#region more_like_this_2
					var list = session.Advanced.MoreLikeThis<Article, Articles_ByArticleBody>(new MoreLikeThisQuery
					{
						DocumentId = key,
						Fields = new[] { "ArticleBody" }
					});
					#endregion

					#region more_like_this_3
					session.Store(new StopWordsSetup
					{
						Id = "Config/Stopwords",
						StopWords = new List<string> { "I", "A", "Be" }
					});
					#endregion
				}
			}
		}
	}
}