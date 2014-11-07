using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
	public class WhatChanged
	{
		private interface IFoo
		{
			#region what_changed_1
			bool HasChanges { get; }
			#endregion

			#region what_changed_3
			IDictionary<string, DocumentsChanges[]> WhatChanged();
			#endregion
		}

		public WhatChanged()
		{
			using (var store = new DocumentStore())
			{
				#region what_changed_2
				using (var session = store.OpenSession())
				{
					Assert.False(session.Advanced.HasChanges);

					session.Store(new Employee
									  {
										  FirstName = "John",
										  LastName = "Doe"
									  });

					Assert.True(session.Advanced.HasChanges);
				}
				#endregion

				#region what_changed_4
				using (var session = store.OpenSession())
				{
					session.Store(new Employee
					{
						FirstName = "Joe",
						LastName = "Doe"
					});

					var changes = session.Advanced.WhatChanged();
					var employeeChanges = changes["employees/1"];
					var change = employeeChanges[0].Change; // DocumentsChanges.ChangeType.DocumentAdded
				}
				#endregion

				#region what_changed_5
				using (var session = store.OpenSession())
				{
					var employee = session.Load<Employee>("employees/1"); // 'Joe Doe'
					employee.FirstName = "John";
					employee.LastName = "Shmoe";

					var changes = session.Advanced.WhatChanged();
					var employeeChanges = changes["employees/1"];
					var change1 = employeeChanges[0]; // DocumentsChanges.ChangeType.FieldChanged
					var change2 = employeeChanges[1]; // DocumentsChanges.ChangeType.FieldChanged
				}
				#endregion
			}
		}
	}
}