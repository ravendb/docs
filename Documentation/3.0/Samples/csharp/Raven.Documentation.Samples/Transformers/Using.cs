using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Document.SessionOperations;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.Transformers
{
	public class Using
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

		#region transformers_1_2
		public class Products_ProductAndCategoryName : AbstractTransformerCreationTask<Product>
		{
			public class Result
			{
				public string ProductName { get; set; }

				public string CategoryName { get; set; }
			}

			public Products_ProductAndCategoryName()
			{
				TransformResults =
					products =>
					from product in products
					let category = LoadDocument<Category>(product.Category)
					select new
					{
						ProductName = product.Name,
						CategoryName = category.Name
					};
			}
		}
		#endregion

		#region transformers_1_3
		public class Employees_BirthDay : AbstractTransformerCreationTask<Employee>
		{
			public class Result
			{
				public string FirstName { get; set; }

				public string LastName { get; set; }

				public string BirthDay { get; set; }
			}

			public Employees_BirthDay()
			{
				TransformResults =
					employees =>
					from employee in employees
					// if no parameter is passed use 't' format
					let dateFormat = ParameterOrDefault("DateFormat", "t").Value<string>()
					select new
					{
						FirstName = employee.FirstName,
						LastName = employee.LastName,
						BirthDay = employee.Birthday.ToString(dateFormat)
					};
			}
		}
		#endregion

		#region transformers_1_4
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
					select TransfromWith("Employees/FirstAndLastName", employee);
			}
		}
		#endregion

		public Using()
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

				using (var session = store.OpenSession())
				{
					#region transformers_1_4
					IList<Employees_BirthDay.Result> results = session
						.Query<Employee>()
						.TransformWith<Employees_BirthDay, Employees_BirthDay.Result>()
						.AddTransformerParameter("DateFormat", "D")
						.ToList();
					#endregion
				}
			}
		}
	}
}