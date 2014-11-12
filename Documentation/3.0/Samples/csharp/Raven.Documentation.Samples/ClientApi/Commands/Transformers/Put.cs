using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Transformers
{
	public class Put
	{
		private interface IFoo
		{
			#region put_1
			string PutTransformer(string name, TransformerDefinition transformerDef);
			#endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_2
				var transformerName = store
					.DatabaseCommands
					.PutTransformer(
						"Order/Statistics",
						new TransformerDefinition
						{
							TransformResults = @"from order in results
												select new
												{
													order.OrderedAt,
													order.Status,
													order.CustomerId,
													CustomerName = LoadDocument(order.CustomerId).Name,
													LinesCount = order.Lines.Count
												}",
                            Name = "Order/Statistics"
						});
				#endregion
			}
		}
	}
}