using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
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
				using (IDocumentSession session = store.OpenSession())
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
				using (IDocumentSession session = store.OpenSession())
				{
					session.Store(new Employee
					{
						FirstName = "Joe",
						LastName = "Doe"
					});

					IDictionary<string, DocumentsChanges[]> changes = session.Advanced.WhatChanged();
					DocumentsChanges[] employeeChanges = changes["employees/1-A"];
					DocumentsChanges.ChangeType change = employeeChanges[0].Change; // DocumentsChanges.ChangeType.DocumentAdded
				}
				#endregion

				#region what_changed_5
				using (IDocumentSession session = store.OpenSession())
				{
					Employee employee = session.Load<Employee>("employees/1-A"); // 'Joe Doe'
					employee.FirstName = "John";
					employee.LastName = "Shmoe";

					IDictionary<string, DocumentsChanges[]> changes = session.Advanced.WhatChanged();
					DocumentsChanges[] employeeChanges = changes["employees/1-A"];
					DocumentsChanges change1 = employeeChanges[0]; // DocumentsChanges.ChangeType.FieldChanged
					DocumentsChanges change2 = employeeChanges[1]; // DocumentsChanges.ChangeType.FieldChanged
				}
				#endregion
			}
		}
	}
}
