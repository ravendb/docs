using Raven.Abstractions.Data;
#region more_like_this_3
using Raven.Client.Bundles.MoreLikeThis;
#endregion
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
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
					var articles = session
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
			}
		}
	}
}