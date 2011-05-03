using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;

namespace RavenCodeSamples.Consumer
{
	public class DynamicQueries : CodeSampleBase
	{
		public void LinqQuerying()
		{
			using (var store = NewDocumentStore())
			{

				using (var session = store.OpenSession())
				{
					#region linquerying_0
					var companies = session.Query<Company>()
						.Where(x => x.Country == "Israel")
						.Take(5);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region linquerying_1
					var results = session.Query<Company>().ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region linquerying_2
					// Filtering by string comparison on a property
					Company[] results = session.Query<Company>()
						.Where(x => x.Name == "Hibernating Rhinos")
						.ToArray();
					// Numeric property comparison
					results = session.Query<Company>()
						.Where(x => x.NumberOfHappyCustomers > 100)
						.ToArray();
					// Filtering based on a nested property
					results = session.Query<Company>()
						.Where(x => x.Employees.Count > 10)
						.ToArray();
					#endregion
				}
			}
		}
	}
}
