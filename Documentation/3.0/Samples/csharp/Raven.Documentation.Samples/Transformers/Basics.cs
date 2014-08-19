using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class Basics
	{
		/*
		#region transformers_1
		public class Employees_FirstAndLastName : AbstractTransformerCreationTask<Employee>
		#endregion
		*/

		public class Employees_FirstAndLastName : AbstractTransformerCreationTask<Employee>
		{
			#region transformers_5
			public class Result
			{
				public string FirstName { get; set; }

				public string LastName { get; set; }
			}
			#endregion

			#region transformers_2
			public Employees_FirstAndLastName()
			{
				TransformResults = employees => from employee in employees
												select new
												{
													FirstName = employee.FirstName,
													LastName = employee.LastName
												};
			}
			#endregion
		}

		/*
		public class Employees_FirstAndLastName : AbstractTransformerCreationTask<Employee>
		{
			#region transformers_3
			public Employees_FirstAndLastName()
			{
				TransformResults = employees => employees
					.Select(employee => new
					{
						FirstName = employee.FirstName, 
						LastName = employee.LastName
					});
			}
			#endregion
		}
		*/

		/*
		#region transformers_7
		public class Employees_FirstAndLastName : AbstractTransformerCreationTask<Employee>
		{
			public class Result
			{
				public string FirstName { get; set; }

				public string LastName { get; set; }
			}

			public Employees_FirstAndLastName()
			{
				TransformResults = employees => from employee in employees
								select new
								{
									FirstName = employee.FirstName,
									LastName = employee.LastName
								};
			}
		}
		#endregion
		*/

		#region transformers_8
		public class Employees_FirstName : AbstractTransformerCreationTask<Employee>
		{
			public Employees_FirstName()
			{
				TransformResults = employees => from employee in employees select employee.FirstName;
			}
		}
		#endregion

		#region transformers_1_0
		public class Employees_Address : AbstractTransformerCreationTask<Employee>
		{
			public Employees_Address()
			{
				TransformResults = employees => from employee in employees select employee.Address;
			}
		}
		#endregion

		public Basics()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_4
					IList<dynamic> results = session
						.Query<Employee>()
						.TransformWith<Employees_FirstAndLastName, dynamic>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_6
					IList<Employees_FirstAndLastName.Result> results = session
						.Query<Employee>()
						.TransformWith<Employees_FirstAndLastName, Employees_FirstAndLastName.Result>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_9
					IList<string> results = session
						.Query<Employee>()
						.TransformWith<Employees_FirstName, string>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_1_1
					IList<Address> results = session
						.Query<Employee>()
						.TransformWith<Employees_Address, Address>()
						.ToList();
					#endregion
				}
			}
		}
	}
}