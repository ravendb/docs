using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
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
					IQueryable<Employee> query = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert");

					IEnumerator<StreamResult<Employee>> results = session.Advanced.Stream(query);

					while (results.MoveNext())
					{
						StreamResult<Employee> employee = results.Current;
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region stream_3
					IDocumentQuery<Employee> query = session
						.Advanced
						.DocumentQuery<Employee>()
						.WhereEquals(x => x.FirstName, "Robert");

					QueryHeaderInformation queryHeaderInformation;
					IEnumerator<StreamResult<Employee>> results = session.Advanced.Stream(query, out queryHeaderInformation);

					while (results.MoveNext())
					{
						StreamResult<Employee> employee = results.Current;
					}
					#endregion
				}
			}
		}
	}
}