using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Querying
{
	public class HowToWorkWithMoreLikeThisQuery
	{
		private interface IFoo
		{
			#region more_like_this_1
			MultiLoadResult MoreLikeThis(MoreLikeThisQuery query);
			#endregion
		}

		public HowToWorkWithMoreLikeThisQuery()
		{
			using (var store = new DocumentStore())
			{
				#region more_like_this_2
				// Search for similar documents to 'articles/1'
				// using 'Articles/MoreLikeThis' index and search only field 'Body'
				var result = store
					.DatabaseCommands
					.MoreLikeThis(
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