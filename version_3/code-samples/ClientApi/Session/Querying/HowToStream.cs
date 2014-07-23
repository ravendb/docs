using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToStream
	{
		private interface IFoo
		{
			#region stream_1
			IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query);

			IEnumerator<StreamResult<T>> Stream<T>(
				IQueryable<T> query,
				out QueryHeaderInformation queryHeaderInformation);

			IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query);

			IEnumerator<StreamResult<T>> Stream<T>(
				IDocumentQuery<T> query,
				out QueryHeaderInformation queryHeaderInformation);
			#endregion
		}

		public HowToStream()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region stream_2
					var query = session
						.Query<Person>()
						.Where(x => x.FirstName == "John");

					var results = session.Advanced.Stream(query);

					while (results.MoveNext())
					{
						var person = results.Current;
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region stream_3
					var query = session
						.Advanced
						.DocumentQuery<Person>()
						.WhereEquals(x => x.FirstName, "John");

					QueryHeaderInformation queryHeaderInformation;
					var results = session.Advanced.Stream(query, out queryHeaderInformation);

					while (results.MoveNext())
					{
						var person = results.Current;
					}
					#endregion
				}
			}
		}
	}
}