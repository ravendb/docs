using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq.Indexing;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.Indexes
{
	public class IndexingLinqExtensions
	{
		#region indexes_1
		public class Employees_ByReversedFirstName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByReversedFirstName()
			{
				Map = employees => from employee in employees
							select new
							{
								FirstName = employee.FirstName.Reverse()
							};
			}
		}
		#endregion

		#region indexes_3
		public class Item_Parse : AbstractIndexCreationTask<Item>
		{
			public class Result
			{
				public int MajorWithDefault { get; set; }

				public int MajorWithCustomDefault { get; set; }
			}

			public Item_Parse()
			{
				Map = items => from item in items
							let parts = item.Version.Split('.', StringSplitOptions.None)
							select new
							{
								MajorWithDefault = parts[0].ParseInt(),			// will return default(int) in case of parsing failure
								MajorWithCustomDefault = parts[0].ParseInt(-1)	// will return -1 in case of parsing failure
							};

				StoreAllFields(FieldStorage.Yes);
			}
		}
		#endregion

		#region indexes_4
		public class Item
		{
			public string Version { get; set; }
		}
		#endregion

		public IndexingLinqExtensions()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_2
					IList<Employee> results = session
						.Query<Employee, Employees_ByReversedFirstName>()
						.Where(x => x.FirstName == "treboR")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_5
					session.Store(new Item { Version = "3.0.1" });
					session.Store(new Item { Version = "Unknown" });

					session.SaveChanges();

					var results = session
						.Query<Item_Parse.Result, Item_Parse>()
						.ToList();

					Assert.Equal(2, results.Count);
					Assert.True(results.Any(x => x.MajorWithDefault == 3));
					Assert.True(results.Any(x => x.MajorWithCustomDefault == 3));
					Assert.True(results.Any(x => x.MajorWithDefault == 0));
					Assert.True(results.Any(x => x.MajorWithCustomDefault == -1));
					#endregion
				}
			}
		}
	}
}
