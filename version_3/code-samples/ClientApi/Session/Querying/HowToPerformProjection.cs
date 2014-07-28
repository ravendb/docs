using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToPerformProjection
	{
		#region projection_4
		public class PeopleByCity : AbstractIndexCreationTask<Person>
		{
			public class Result 
			{
				public string City { get; set; }
			}

			public PeopleByCity()
			{
				Map = people => from person in people 
								let address = LoadDocument<Address>(person.AddressId) 
								select new
									       {
										       City = address.City
									       };
			}
		}
		#endregion

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

				using (var session = store.OpenSession())
				{
					#region projection_5
					// query index 'PeopleByCity' 
					// return documents from collection 'People' that live in 'New York'
					// project them to 'Person'
					var results = session
						.Query<PeopleByCity.Result, PeopleByCity>()
						.Where(x => x.City == "New York")
						.OfType<Person>()
						.ToList();
					#endregion
				}
			}
		}
	}
}