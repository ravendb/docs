using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Commands.Indexes
{

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

			#region put_indexes_3_0
			string[] PutIndexes(IndexToAdd[] indexesToAdd);
			#endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_1_1
				string indexName = store
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
				string indexName = store
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

				#region put_indexes_3_1

				store.DatabaseCommands.PutIndexes(new IndexToAdd[]
				{
					new IndexToAdd
					{
						Name = "Orders/Totals",
						Priority = IndexingPriority.Normal,
						Definition = new IndexDefinition
						{
							Map = @"from order in docs.Orders
										select new 
										{
											order.Employee,
											order.Company,
											Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
										}"
						}
					},
					new IndexToAdd
					{
						Name = "Orders/ByCompany",
						Priority = IndexingPriority.Normal,
						Definition = new IndexDefinition
						{
							Map = @"from order in docs.Orders
										select new 
										{ 
											order.Company, 
											Count = 1, 
											Total = order.Lines.Sum(l = >(l.Quantity * l.PricePerUnit) *  ( 1 - l.Discount))
										}",

							Reduce = @"from result in results
										group result by result.Company into g
										select new
										{
											Company = g.Key,
											Count = g.Sum(x=>x.Count),
											Total = g.Sum(x=>x.Total)
										}"
						}
					}
				});

				#endregion
			}
		}
	}
}