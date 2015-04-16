#region more_like_this_3

using Raven.Abstractions.Data;
using Raven.Client.Bundles.MoreLikeThis;
using Raven.Client.Document;

#endregion

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
	public class MoreLikeThis
	{
		private interface IFoo
		{
			/*
			#region more_like_this_1
			TResult[] MoreLikeThis<TResult, TIndexCreator>(
				this ISyncAdvancedSessionOperation advancedSession,
				MoreLikeThisQuery parameters) 
				where TIndexCreator : AbstractIndexCreationTask, new() { ... }

			TResult[] MoreLikeThis<TResult>(
				this ISyncAdvancedSessionOperation advancedSession,
				string index,
				string documentId) { ... }

			TResult[] MoreLikeThis<TTransformer, TResult, TIndexCreator>(
				this ISyncAdvancedSessionOperation advancedSession,
				string documentId)
				where TIndexCreator : AbstractIndexCreationTask, new()
				where TTransformer : AbstractTransformerCreationTask, new() { ... }
			#endregion
			*/
		}

		private class Article
		{
		}

		public MoreLikeThis()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region more_like_this_2
					// Search for similar articles to 'articles/1'
					// using 'Articles/MoreLikeThis' index and search only field 'Body'
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
				}

				using (var session = store.OpenSession())
				{
					#region more_like_this_4
					// Search for similar articles to 'articles/1'
					// using 'Articles/MoreLikeThis' index and search only field 'Body'
					// where article category is 'IT'
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
				}
			}
		}
	}
}