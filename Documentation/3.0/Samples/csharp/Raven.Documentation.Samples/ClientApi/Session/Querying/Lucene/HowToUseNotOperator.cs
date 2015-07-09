using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.Lucene
{
	public class HowToUseNotOperator
	{
		public HowToUseNotOperator()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region not_1
					// load up to 128 entities from 'Employees' collection
					// where FirstName NOT equals 'Robert'
					List<Employee> employees = session
						.Advanced
						.DocumentQuery<Employee>()
						.Not
						.WhereEquals(x => x.FirstName, "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region not_2
					// load up to 128 entities from 'Employees' collection
					// where FirstName NOT equals 'Robert'
					// and LastName NOT equals 'King'
					List<Employee> employees = session
						.Advanced
						.DocumentQuery<Employee>()
						.Not
						.OpenSubclause()
						.WhereEquals(x => x.FirstName, "Robert")
						.AndAlso()
						.WhereEquals(x => x.LastName, "King")
						.CloseSubclause()
						.ToList();
					#endregion
				}
			}
		}
	}
}