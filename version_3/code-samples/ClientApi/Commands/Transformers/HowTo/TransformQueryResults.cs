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
				// query for all orders with 'Company' equal to 'companies/1' using 'Orders/Totals' index
				// and transform results using 'Order/Statistics' transformer
				var result = store
					.DatabaseCommands
					.Query(
						"Orders/Totals",
						new IndexQuery
						{
							Query = "Company:companies/1",
							ResultsTransformer = "Order/Statistics"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region query_transformer_2
				// query for all orders with 'Company' equal to 'companies/1' using 'Orders/Totals' index
				// and transform results using 'Order/Statistics' transformer
				// stream the results
				QueryHeaderInformation queryHeaderInfo;
				var result = store
					.DatabaseCommands
					.StreamQuery(
						"Orders/Totals",
						new IndexQuery
						{
							Query = "Company:companies/1",
							ResultsTransformer = "Order/Statistics"
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