using System.Linq;
using Raven.Abstractions.Data;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using Raven.Json.Linq;

namespace RavenCodeSamples.Server.Bundles
{
	public class IndexedProperties : CodeSampleBase
	{
		#region indexed_properties_1
		public class Customer
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public decimal AverageOrderAmount { get; set; }
		}

		public class Order
		{
			public string Id { get; set; }

			public string CustomerId { get; set; }

			public decimal TotalAmount { get; set; }
		}

		#endregion

		#region indexed_properties_2
		public class OrderResults
		{
			public string CustomerId { get; set; }

			public decimal AverageOrderAmount { get; set; }
		}

		#endregion

		#region indexed_properties_3
		public class OrdersAverageAmount : AbstractIndexCreationTask<Order, OrderResults>
		{
			public OrdersAverageAmount()
			{
				Map = orders => from order in orders
								select new
									{
										CustomerId = order.CustomerId,
										AverageOrderAmount = order.TotalAmount
									};

				Reduce = results => from result in results
				                    group result by result.CustomerId
				                    into g
				                    select new
					                    {
						                    CustomerId = g.Key,
						                    AverageOrderAmount = g.Average(x => x.AverageOrderAmount)
					                    };
			}
		}

		#endregion

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region indexed_properties_0
				store.DatabaseCommands.CreateDatabase(new DatabaseDocument
					{
						Id = "ExampleDB",
						Settings =
				            {
					            {"Raven/ActiveBundles", "IndexedProperties"}
				            }
					});

				#endregion

				#region indexed_properties_4
				var ordersAverageAmount = new OrdersAverageAmount();
				ordersAverageAmount.Execute(store);

				store.DatabaseCommands.Put("Raven/IndexedProperties/" + ordersAverageAmount.IndexName,
										   null,
										   RavenJObject.FromObject(new IndexedPropertiesSetupDoc
											   {
												   DocumentKey = "CustomerId",
												   FieldNameMappings =
                                                       {
                                                           {"AverageOrderAmount", "AverageOrderAmount"}
                                                       }
											   }),
										   new RavenJObject());

				#endregion

				#region indexed_properties_5
				using (var session = store.OpenSession())
				{
					session.Store(new Customer { Id = "customers/1", Name = "Customer 1" });
					session.Store(new Customer { Id = "customers/2", Name = "Customer 2" });

					session.Store(new Order { Id = "orders/1", CustomerId = "customers/1", TotalAmount = 10 });
					session.Store(new Order { Id = "orders/2", CustomerId = "customers/1", TotalAmount = 5 });
					session.Store(new Order { Id = "orders/3", CustomerId = "customers/1", TotalAmount = 3 });

					session.Store(new Order { Id = "orders/4", CustomerId = "customers/2", TotalAmount = 1 });
					session.Store(new Order { Id = "orders/5", CustomerId = "customers/2", TotalAmount = 3 });

					session.SaveChanges();
				}

				#endregion

				#region indexed_properties_6
				using (var session = store.OpenSession())
				{
					var customer1 = session.Load<Customer>("customers/1"); // AverageOrderAmount is 6
					var customer2 = session.Load<Customer>("customers/2"); // AverageOrderAmount is 1.5
				}

				#endregion
			}
		}
	}
}