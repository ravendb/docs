using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Lazy
	{
		public Lazy()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region lazy_1
					Lazy<Employee> employeeLazy = session
						.Advanced
						.Lazily
						.Load<Employee>("employees/1-A");

					Employee employee = employeeLazy.Value; // load operation will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_2
					Lazy<IEnumerable<Employee>> employeesLazy = session
						.Query<Employee>()
						.Lazily();

					session.Advanced.Eagerly.ExecuteAllPendingLazyOperations(); // query will be executed here

					IEnumerable<Employee> employees = employeesLazy.Value;
					#endregion
				}
			}
		}
	}
}
