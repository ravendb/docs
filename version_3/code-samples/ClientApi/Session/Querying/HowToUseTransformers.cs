using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

using Xunit;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToUseTransformers
	{
		private class PersonNoLastName : AbstractTransformerCreationTask<Person>
		{
		}

		#region transformers_5
		public class PersonWithAddress
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }

			public Address Address { get; set; }
		}
		#endregion

		#region transformers_4
		private class PersonAddress : AbstractTransformerCreationTask<Person>
		{
			public PersonAddress()
			{
				TransformResults =
					people => from person in people 
							  select new
								{
									FirstName = person.FirstName, 
									LastName = person.LastName, 
									Address = LoadDocument<Address>(person.AddressId)
								};
			}
		}
		#endregion

		private interface IFoo
		{
			#region transformers_1
			IRavenQueryable<TResult> TransformWith<TTransformer, TResult>()
				where TTransformer : AbstractTransformerCreationTask, new();

			IRavenQueryable<TResult> TransformWith<TResult>(string transformerName);
			#endregion
		}

		public HowToUseTransformers()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
					// return up to 128 entities from 'People' collection
					// transform results using 'PersonNoLastName' transformer
					// which returns 'LastName' as 'null'
					var results = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.TransformWith<PersonNoLastName, Person>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_3
					var address = new Address { Street = "Crystal Oak Street" };
					session.Store(address);
					session.Store(new Person
						              {
							              FirstName = "John",
										  LastName = "Doe",
										  AddressId = address.Id
						              });
					session.SaveChanges();

					// return 1 entity from 'People' collection
					// transform results using 'PersonAddress' transformer
					// project results to 'PersonWithAddress' class
					var person = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.TransformWith<PersonAddress, PersonWithAddress>()
						.First();

					Assert.Equal("John", person.FirstName);
					Assert.Equal("Doe", person.LastName);
					Assert.Equal("Crystal Oak Street", person.Address.Street);
					#endregion
				}
			}
		}
	}
}