using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToPerformProjection
	{
		private class PersonFirstAndLastName
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }
		}

		public HowToPerformProjection()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region projection_1
					// request 'FirstName' and 'LastName' from server
					// and project it to anonymous class
					var results = session
						.Query<Person>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projection_2
					// request 'FirstName' and 'LastName' from server
					// and project it to 'PersonFirstAndLastName'
					var results = session
						.Query<Person>()
						.Select(x => new PersonFirstAndLastName
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projection_3
					// request all public fields/properties available in 'PersonFirstAndLastName'
					// ('FirstName' and 'LastName')
					// and project it to instance of this class
					var results = session
						.Query<Person>()
						.ProjectFromIndexFieldsInto<PersonFirstAndLastName>()
						.ToList();
					#endregion
				}
			}
		}
	}
}