using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;

using Xunit;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
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

					session.Store(new Person
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
					session.Store(new Person
					{
						FirstName = "Joe",
						LastName = "Doe"
					});

					var changes = session.Advanced.WhatChanged();
					var personChanges = changes["people/1"];
					var change = personChanges[0].Change; // DocumentsChanges.ChangeType.DocumentAdded
				}
				#endregion

				#region what_changed_5
				using (var session = store.OpenSession())
				{
					var person = session.Load<Person>("people/1"); // 'Joe Doe'
					person.FirstName = "John";
					person.LastName = "Shmoe";

					var changes = session.Advanced.WhatChanged();
					var personChanges = changes["people/1"];
					var change1 = personChanges[0]; // DocumentsChanges.ChangeType.FieldChanged
					var change2 = personChanges[1]; // DocumentsChanges.ChangeType.FieldChanged
				}
				#endregion
			}
		}
	}
}