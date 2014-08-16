using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToPerformProjection
	{
		#region projection_4
		public class Products_BySupplierName : AbstractIndexCreationTask<Product>
		{
			public class Result
			{
				public string Name { get; set; }
			}

			public Products_BySupplierName()
			{
				Map =
					products =>
					from product in products
					let supplier = LoadDocument<Supplier>(product.Supplier)
					select new
						       {
							       Name = supplier.Name
						       };
			}
		}
		#endregion

		private class EmployeeFirstAndLastName
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }
		}

		public HowToPerformProjection()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region projection_1
					// request 'FirstName' and 'LastName' from server
					// and project it to anonymous class
					var results = session
						.Query<Employee>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projection_2
					// request 'FirstName' and 'LastName' from server
					// and project it to 'EmployeeFirstAndLastName'
					var results = session
						.Query<Employee>()
						.Select(x => new EmployeeFirstAndLastName
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projection_3
					// request all public fields/properties available 
					// in 'EmployeeFirstAndLastName' ('FirstName' and 'LastName')
					// and project it to instance of this class
					var results = session
						.Query<Employee>()
						.ProjectFromIndexFieldsInto<EmployeeFirstAndLastName>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projection_5
					// query index 'Products_BySupplierName' 
					// return documents from collection 'Products' that have a supplier 'Norske Meierier'
					// project them to 'Products'
					var results = session
						.Query<Products_BySupplierName.Result, Products_BySupplierName>()
						.Where(x => x.Name == "Norske Meierier")
						.OfType<Product>()
						.ToList();
					#endregion
				}
			}
		}
	}
}