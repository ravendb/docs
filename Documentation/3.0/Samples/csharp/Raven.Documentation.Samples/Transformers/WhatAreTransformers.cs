using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class WhatAreTransformers
	{
		#region transformers_1
		public class Employees_NameAndPhone : AbstractTransformerCreationTask<Employee>
		{
			public class Result
			{
				public string FirstName { get; set; }

				public double LastName { get; set; }

				public double HomePhone { get; set; }
			}

			public Employees_NameAndPhone()
			{
				TransformResults = employees => from employee in employees 
								select new
								{
									employee.FirstName, 
									employee.LastName, 
									employee.HomePhone
								};
			}
		}
		#endregion

		public WhatAreTransformers()
		{
			using (var store = new DocumentStore())
			{
				#region transformers_2
				// save transformer on server
				new Employees_NameAndPhone().Execute(store);
				#endregion

				using (var session = store.OpenSession())
				{
					#region transformers_3
					Employees_NameAndPhone.Result result = session
						.Load<Employees_NameAndPhone, Employees_NameAndPhone.Result>("employees/1");
					#endregion

					#region transformers_4
					IList<Employees_NameAndPhone.Result> results = session
						.Query<Employee>()
						.TransformWith<Employees_NameAndPhone, Employees_NameAndPhone.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}