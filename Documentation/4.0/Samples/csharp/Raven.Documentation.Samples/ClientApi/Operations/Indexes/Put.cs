using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{

	public class Put
	{
		private interface IFoo
		{
            /*
            #region put_1_0
            public PutIndexesOperation(params IndexDefinition[] indexToAdd)
			#endregion
            */
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
                #region put_1_1
                IndexDefinition definition = new IndexDefinition
                {
                    Maps = new HashSet<string> { @"from order in docs.Orders
                                        select new 
                                        {
                                            order.Employee,
                                            order.Company,
                                            Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                                        }" }
                };

                store.Maintenance.Send(new PutIndexesOperation(definition));
				#endregion
			}

			using (var store = new DocumentStore())
			{
                #region put_1_2
                IndexDefinition definition = new IndexDefinitionBuilder<Order>
                {
                    Map = orders => from order in orders
                                    select new
                                    {
                                        order.Employee,
                                        order.Company,
                                        Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                                    }
                }
                .ToIndexDefinition(store.Conventions);
                store.Maintenance.Send(new PutIndexesOperation(definition));
                #endregion
            }
		}
	}
}
