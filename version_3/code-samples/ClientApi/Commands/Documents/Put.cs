namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents
{
	using Raven.Abstractions.Data;
	using Raven.Client.Document;
	using Raven.Json.Linq;

	public class Put
    {
		private interface IFoo
		{
			 #region put_1
			 PutResult Put(string key, Etag etag, RavenJObject document, RavenJObject metadata);
			 #endregion
		}

		private class Person
		{
			public string FirstName { get; set; }

			public string LastName { get; set; }
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_3
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
				#endregion
			}
		}
	}
}