using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Filtering
	{
		#region filtering_0_4
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByFirstAndLastName()
			{
				Map = employees => from employee in employees
								   select new
									{
										FirstName = employee.FirstName,
										LastName = employee.LastName
									};
			}
		}
		#endregion

		#region filtering_1_4
		private class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
		{
			public Products_ByUnitsInStock()
			{
				Map = products => from product in products
								  select new
									{
										product.UnitsInStock
									};
			}
		}
		#endregion

		#region filtering_2_4
		private class Order_ByOrderLinesCount : AbstractIndexCreationTask<Order>
		{
			public Order_ByOrderLinesCount()
			{
				Map = orders => from order in orders
								select new
								{
									Lines_Count = order.Lines.Count
								};
			}
		}
		#endregion

		#region filtering_3_4
		public class Order_ByOrderLines_ProductName : AbstractIndexCreationTask<Order>
		{
			public class Result
			{
				public string ProductName { get; set; }
			}

			public Order_ByOrderLines_ProductName()
			{
				Map = orders => from order in orders
								from line in order.Lines
								select new
								{
									ProductName = line.ProductName
								};
			}
		}
		#endregion

		public Filtering()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region filtering_0_1
					IList<Employee> results = session
						.Query<Employee, Employees_ByFirstAndLastName>()		// query 'Employees/ByFirstAndLastName' index
						.Where(x => x.FirstName == "Robert" && x.LastName == "King")	// filtering predicates
						.ToList();							// materialize query by sending it to server for processing
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region filtering_0_2
					IList<Employee> results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstAndLastName>()	// query 'Employees/ByFirstAndLastName' index
						.WhereEquals(x => x.FirstName, "Robert")			// filtering predicates
						.AndAlso()							// by default OR is between each condition
						.WhereEquals(x => x.LastName, "King")				// filtering predicates
						.ToList();							// materialize query by sending it to server for processing
					#endregion
				}

				#region filtering_0_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstAndLastName",
						new IndexQuery
							{
								Query = "FirstName:Robert AND LastName:King"
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region filtering_1_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()		// query 'Products/ByUnitsInStock' index
						.Where(x => x.UnitsInStock > 50)			// filtering predicates
						.ToList();						// materialize query by sending it to server for processing
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region filtering_1_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()	// query 'Products/ByUnitsInStock' index
						.WhereGreaterThan(x => x.UnitsInStock, 50)		// filtering predicates
						.ToList();						// materialize query by sending it to server for processing
					#endregion
				}

				#region filtering_1_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock_Range:{Ix50 TO NULL}"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region filtering_2_1
					IList<Order> results = session
						.Query<Order, Order_ByOrderLinesCount>()	// query 'Order/ByOrderLinesCount' index
						.Where(x => x.Lines.Count > 50)			// filtering predicates
						.ToList();					// materialize query by sending it to server for processing
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region filtering_2_2
					IList<Order> results = session
						.Advanced
						.DocumentQuery<Order, Order_ByOrderLinesCount>()	// query 'Order/ByOrderLinesCount' index
						.WhereGreaterThan(x => x.Lines.Count, 50)		// filtering predicates
						.ToList();						// materialize query by sending it to server for processing
					#endregion
				}

				#region filtering_2_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Order/ByOrderLinesCount",
						new IndexQuery
						{
							Query = "Lines.Count_Range:{Ix50 TO NULL}"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region filtering_3_1
					IList<Order> results = session
						.Query<Order_ByOrderLines_ProductName.Result, Order_ByOrderLines_ProductName>()	// query 'Order/ByOrderLinesCount' index
						.Where(x => x.ProductName == "Teatime Chocolate Biscuits")			// filtering predicates
						.OfType<Order>()								// change result type to 'Order'
						.ToList();									// materialize query by sending it to server for processing
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region filtering_3_2
					IList<Order> results = session
						.Advanced
						.DocumentQuery<Order_ByOrderLines_ProductName.Result, Order_ByOrderLines_ProductName>()	// query 'Order/ByOrderLinesCount' index
						.WhereEquals(x => x.ProductName, "Teatime Chocolate Biscuits")				// filtering predicates
						.OfType<Order>()									// change result type to 'Order' (cheated in Query to get strongly-typed syntax)
						.ToList();										// materialize query by sending it to server for processing
					#endregion
				}

				#region filtering_3_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Order/ByOrderLinesCount",
						new IndexQuery
						{
							Query = "ProductName:\"Teatime Chocolate Biscuits\""
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region filtering_4_1
					IList<Employee> results = session
						.Query<Employee, Employees_ByFirstAndLastName>()	// query 'Employees/ByFirstAndLastName' index
						.Where(x => x.FirstName.In("Robert", "Nancy"))		// filtering predicates (remember to add `Raven.Client.Linq` namespace to usings)
						.ToList();						// materialize query by sending it to server for processing
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region filtering_4_2
					IList<Employee> results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstAndLastName>()	// query 'Employees/ByFirstAndLastName' index
						.WhereIn(x => x.FirstName, new[] { "Robert", "Nancy" })		// filtering predicates
						.ToList();							// materialize query by sending it to server for processing
					#endregion
				}

				#region filtering_4_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstAndLastName",
						new IndexQuery
						{
							Query = "@in<FirstName>:(Robert, Nancy)"
						});
				#endregion
			}
		}
	}
}