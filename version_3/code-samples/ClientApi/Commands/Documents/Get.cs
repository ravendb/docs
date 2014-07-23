namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents
{
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
	using Raven.Client;
	using Raven.Client.Document;
	using Raven.Json.Linq;

	public class Get
	{
		private interface IFoo
		{
			#region get_1_0
			JsonDocument Get(string key);
			#endregion

			#region get_2_0
			MultiLoadResult Get(
				string[] ids,
				string[] includes,
				string transformer = null,
				Dictionary<string, RavenJToken> queryInputs = null,
				bool metadataOnly = false);
			#endregion

			#region get_3_0
			JsonDocument[] GetDocuments(int start, int pageSize, bool metadataOnly = false);
			#endregion

			#region get_4_0
			JsonDocument[] StartsWith(string keyPrefix, string matches, int start, int pageSize,
								  RavenPagingInformation pagingInformation = null, bool metadataOnly = false,
								  string exclude = null, string transformer = null,
								  Dictionary<string, RavenJToken> queryInputs = null);
			#endregion
		}

		private class Person
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }

			public string Address { get; set; }
		}

		private class Address
		{
			public string Street { get; set; }
		}

		public Get()
		{
			using (var store = new DocumentStore())
			{
				#region get_1_2
				var document = store.DatabaseCommands.Get("people/1"); // null if does not exist
				#endregion

				#region get_2_2
				var resultsWithoutIncludes = store
					.DatabaseCommands
					.Get(new[] { "people/1", "people/2" }, null);
				#endregion

				#region get_2_3
				store
					.DatabaseCommands
					.Put(
						"addresses/1",
						null,
						RavenJObject.FromObject(new Address
						{
							Street = "Crystal Oak Street"
						}),
						new RavenJObject());

				store
					.DatabaseCommands
					.Put(
						"people/1",
						null,
						RavenJObject.FromObject(new Person 
						{ 
							FirstName = "John",
							LastName = "Doe",
							Address = "addresses/1"
						}),
						new RavenJObject());

				store
					.DatabaseCommands
					.Put(
						"people/2",
						null,
						RavenJObject.FromObject(new Person 
						{ 
							FirstName = "Nick",
							LastName = "Doe",
							Address = "addresses/1"
						}),
						new RavenJObject());

				var resultsWithIncludes = store
					.DatabaseCommands
					.Get(
						new[] { "people/1", "people/2" },
						new [] { "AddressId" });

				var results = resultsWithIncludes.Results; // people/1, people/2
				var includes = resultsWithIncludes.Includes; // addresses/1
				#endregion
			}
		}

		public void MissingDocuments()
		{
			using (var store = new DocumentStore())
			{
				#region get_2_4
				store
					.DatabaseCommands
					.Put(
						"people/1",
						null,
						RavenJObject.FromObject(new Person
						{
							FirstName = "John",
							LastName = "Doe"
						}),
						new RavenJObject());

				store
					.DatabaseCommands
					.Put(
						"people/3",
						null,
						RavenJObject.FromObject(new Person
						{
							FirstName = "Nick",
							LastName = "Doe"
						}),
						new RavenJObject());

				var resultsWithIncludes = store
					.DatabaseCommands
					.Get(
						new[] { "people/1", "people/2", "people/3" },
						null);

				var results = resultsWithIncludes.Results; // people/1, null, people/3
				var includes = resultsWithIncludes.Includes; // empty
				#endregion
			}
		}

		public void GetDocuments()
		{
			using (var store = new DocumentStore())
			{
				#region get_3_1
				var documents = store.DatabaseCommands.GetDocuments(0, 10, metadataOnly: false);
				#endregion
			}
		}

		public void StartsWith()
		{
			using (var store = new DocumentStore())
			{
				#region get_4_1
				// return up to 128 documents with key that starts with 'people'
				var result = store.DatabaseCommands.StartsWith("people", null, 0, 128);
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_4_2
				// return up to 128 documents with key that starts with 'people/' 
				// and rest of the key begins with "1" or "2" e.g. people/10, people/25
				var result = store.DatabaseCommands.StartsWith("people/", "1*|2*", 0, 128); 
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_4_3
				// return up to 128 documents with key that starts with 'people/' 
				// and rest of the key have length of 3, begins and ends with "1" 
				// and contains any character at 2nd position e.g. people/101, people/1B1
				var result = store.DatabaseCommands.StartsWith("people/", "1?1", 0, 128);
				#endregion
			}
		}
	}
}