using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class Nesting
	{
		#region transformers_1
		public class Orders_Employees_FirstAndLastName : AbstractTransformerCreationTask<Order>
		{
			public class Result
			{
				public string FirstName { get; set; }

				public string LastName { get; set; }
			}

			public Orders_Employees_FirstAndLastName()
			{
				TransformResults =
					orders =>
					from order in orders
					let employee = LoadDocument<Employee>(order.Employee)
					select TransformWith("Employees/FirstAndLastName", employee);
			}
		}
		#endregion

		public Nesting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
					IList<Orders_Employees_FirstAndLastName.Result> results = session
						.Query<Product>()
						.Where(x => x.Name == "Chocolade")
						.TransformWith<Orders_Employees_FirstAndLastName, Orders_Employees_FirstAndLastName.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}