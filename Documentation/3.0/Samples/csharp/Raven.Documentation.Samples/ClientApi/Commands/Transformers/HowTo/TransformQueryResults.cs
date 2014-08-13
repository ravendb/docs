using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Transformers.HowTo
{
	public class TransformQueryResults
	{
		public TransformQueryResults()
		{
			using (var store = new DocumentStore())
			{
				#region query_transformer_1
				// query for all users with 'Name' equal to 'James' using 'Users/ByName' index
				// and transform results using 'Users/FullName' transformer
				var result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:James",
							ResultsTransformer = "Users/FullName"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region query_transformer_2
				// query for all users with 'Name' equal to 'James' using 'Users/ByName' index
				// and transform results using 'Users/FullName' transformer
				// stream the results
				QueryHeaderInformation queryHeaderInfo;
				var result = store
					.DatabaseCommands
					.StreamQuery(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:James",
							ResultsTransformer = "Users/FullName"
						},
						out queryHeaderInfo);
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region query_transformer_3
				// Search for similar documents to 'articles/1'
				// using 'Articles/MoreLikeThis' index, search only field 'Body'
				// and transform results using 'Articles/NoComments' transformer
				QueryHeaderInformation queryHeaderInfo;
				var result = store
					.DatabaseCommands
					.MoreLikeThis(
						new MoreLikeThisQuery
						{
							IndexName = "Articles/MoreLikeThis",
							DocumentId = "articles/1",
							Fields = new[] { "Body" },
							ResultsTransformer = "Articles/NoComments"
						});
				#endregion
			}
		}
	}
}