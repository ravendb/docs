using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class Storing
	{
		#region storing_1
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByFirstAndLastName()
			{
				Map = employees => from employee in employees
							select new
							{
								employee.FirstName,
								employee.LastName
							};

				Stores.Add(x => x.FirstName, FieldStorage.Yes);
				Stores.Add(x => x.LastName, FieldStorage.Yes);
			}
		}
		#endregion

		public Storing()
		{
			using (var store = new DocumentStore())
			{
				#region storing_2
				store
					.DatabaseCommands
					.PutIndex(
						"Employees_ByFirstAndLastName",
						new IndexDefinition
						{
							Map = @"from employee in docs.Employees
									select new
									{
										employee.FirstName,
										employee.LastName
									}",
							Stores = new Dictionary<string, FieldStorage>
							{
								{ "FirstName", FieldStorage.Yes }, 
								{ "LastName", FieldStorage.Yes }
							}
						});
				#endregion
			}
		}
	}
}