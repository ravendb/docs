using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class Parameters
	{
		#region transformers_1
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

		public Parameters()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
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