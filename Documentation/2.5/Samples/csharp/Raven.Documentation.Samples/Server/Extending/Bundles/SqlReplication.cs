using System.Collections.Generic;
using RavenCodeSamples.Server.Extending.Bundles.Foo;

namespace RavenCodeSamples.Server.Extending.Bundles
{
	namespace Foo
	{
		#region sql_replication_1
		public class SqlReplicationConfig
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public bool Disabled { get; set; }

			public string RavenEntityName { get; set; }
			public string Script { get; set; }

			public string FactoryName { get; set; }
			public string ConnectionString { get; set; }
			public string ConnectionStringName { get; set; }
			public string ConnectionStringSettingName { get; set; }

			public List<SqlReplicationTable> SqlReplicationTables { get; set; }
		}

		public class SqlReplicationTable
		{
			public string TableName { get; set; }
			public string DocumentKeyColumn { get; set; }
		}

		#endregion

		#region sql_replication_2
		public class Order
		{
			public string Id { get; set; }
			public List<OrderLine> OrderLines { get; set; }
		}

		public class OrderLine
		{
			public string Product { get; set; }
			public int Quantity { get; set; }
			public int Cost { get; set; }
		}

		#endregion
	}

	public class SqlReplication : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sql_replication_3
					session.Store(new SqlReplicationConfig
					{
						Id = "Raven/SqlReplication/Configuration/OrdersAndLines",
						Name = "OrdersAndLines",
						ConnectionString = @"
							Data Source=.\SQLEXPRESS;
							Initial Catalog=ExampleDB;
							Integrated Security=SSPI;",
						FactoryName = @"System.Data.SqlClient",
						RavenEntityName = "Orders",
						SqlReplicationTables =
							{
								new SqlReplicationTable
								{
									TableName = "Orders", DocumentKeyColumn = "Id"
								},
								new SqlReplicationTable
								{
									TableName = "OrderLines", DocumentKeyColumn = "OrderId"
								},
							},
						Script = @"
							var orderData = {
								Id: documentId,
								OrderLinesCount: this.OrderLines.length,
								TotalCost: 0
							};

							replicateToOrders(orderData);

							for (var i = 0; i < this.OrderLines.length; i++) {
								var line = this.OrderLines[i];
								orderData.TotalCost += line.Cost;

								replicateToOrderLines({
									OrderId: documentId,
									Qty: line.Quantity,
									Product: line.Product,
									Cost: line.Cost
								});
							}"
					});

					#endregion
				}
			}
		}
	}
}
