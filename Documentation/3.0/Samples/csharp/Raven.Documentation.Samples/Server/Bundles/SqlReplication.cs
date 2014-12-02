using System;
using System.Collections.Generic;

using Raven.Client.Document;

namespace Raven.Documentation.Samples.Server.Bundles
{
	public class SqlReplication
	{
		#region sql_replication_1
		public class SqlReplicationConfig
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public bool Disabled { get; set; }

			public bool ParameterizeDeletesDisabled { get; set; }
			public bool ForceSqlServerQueryRecompile { get; set; }
			public bool PerformTableQuatation { get; set; }

			public string RavenEntityName { get; set; }
			public string Script { get; set; }
			public string FactoryName { get; set; }

			public string ConnectionString { get; set; }

			public string PredefinedConnectionStringSettingName { get; set; }
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

			public string Company { get; set; }

			public string Employee { get; set; }

			public DateTime OrderedAt { get; set; }

			public DateTime RequireAt { get; set; }

			public DateTime? ShippedAt { get; set; }

			public Address ShipTo { get; set; }

			public string ShipVia { get; set; }

			public decimal Freight { get; set; }

			public List<OrderLine> Lines { get; set; }
		}

		public class OrderLine
		{
			public string Product { get; set; }

			public string ProductName { get; set; }

			public decimal PricePerUnit { get; set; }

			public int Quantity { get; set; }

			public decimal Discount { get; set; }
		}

		public class Address
		{
			public string Line1 { get; set; }

			public string Line2 { get; set; }

			public string City { get; set; }

			public string Region { get; set; }

			public string PostalCode { get; set; }

			public string Country { get; set; }
		}
		#endregion

		public void Sample()
		{
			using (var store = new DocumentStore())
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
								OrderLinesCount: this.Lines.length,
								TotalCost: 0
							};

							for (var i = 0; i < this.Lines.length; i++) {
								var line = this.Lines[i];
								var lineCost = ((line.Quantity * line.PricePerUnit) * (1 - line.Discount));
								orderData.TotalCost += lineCost;

								replicateToOrderLines({
									OrderId: documentId,
									Qty: line.Quantity,
									Product: line.Product,
									Cost: lineCost
								});
							}
							
							replicateToOrders(orderData);"
					});

					#endregion
				}
			}
		}
	}
}
