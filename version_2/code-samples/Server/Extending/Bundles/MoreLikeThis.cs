namespace RavenCodeSamples.Server.Bundles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	using Raven.Abstractions.Data;
	using Raven.Abstractions.Indexing;
	using Raven.Client.Bundles.MoreLikeThis;
	using Raven.Client.Indexes;

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
						  select new
									 {
										 doc.ArticleBody
									 };



			Stores = new Dictionary<Expression<Func<Article, object>>, FieldStorage>
							 {
								 {
									 x => x.ArticleBody, FieldStorage.Yes
								 }
							 };

		}
	}

	#endregion

	public class MoreLikeThis : CodeSampleBase
	{
		private class Data
		{
		}

		private class DataIndex : AbstractIndexCreationTask<Data>
		{

		}

		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					string key = "articles/1";

					#region more_like_this_1
					var list = session.Advanced.MoreLikeThis<Article, ArticleIndex>(key);

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					string key = "articles/1";

					#region more_like_this_2
					var list = session.Advanced.MoreLikeThis<Data, DataIndex>(new MoreLikeThisQuery
					{
						DocumentId = key,
						Fields = new[] { "Body" }
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