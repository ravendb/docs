using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes
{
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;
	using Raven.Client.Indexes;

	public class Put
	{
		private interface IFoo
		{
			#region put_1_0
			string PutIndex(string name, IndexDefinition indexDef);

			string PutIndex(string name, IndexDefinition indexDef, bool overwrite);
			#endregion

			#region put_2_0
			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef);

			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef, bool overwrite);
			#endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_1_1
				var indexName = store
					.DatabaseCommands
					.PutIndex(
						"Orders/Totals",
						new IndexDefinition
							{
								Map = @"from order in docs.Orders
										select new 
										{
											order.Employee,
											order.Company,
											Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
										}"
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region put_2_1
				var indexName = store
					.DatabaseCommands
					.PutIndex(
						"Orders/Totals",
						new IndexDefinitionBuilder<Order>
							{
								Map = orders => from order in orders
												select new
												{
													order.Employee,
													order.Company,
													Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
												}
							});
				#endregion
			}
		}
	}
}