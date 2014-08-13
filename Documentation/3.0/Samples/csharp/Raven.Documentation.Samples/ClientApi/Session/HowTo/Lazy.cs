using System;
using System.Collections.Generic;

using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
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
					Lazy<Person> personLazy = session
						.Advanced
						.Lazily
						.Load<Person>("people/1");

					var person = personLazy.Value; // load operation will be executed here
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region lazy_2
					Lazy<IEnumerable<Person>> peopleLazy = session
						.Query<Person>()
						.Lazily();

					session.Advanced.Eagerly.ExecuteAllPendingLazyOperations(); // query will be executed here

					var people = peopleLazy.Value;
					#endregion
				}
			}
		}
	}
}