namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents
{
	using System;
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
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
						new [] { "Address" });

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
	}
}