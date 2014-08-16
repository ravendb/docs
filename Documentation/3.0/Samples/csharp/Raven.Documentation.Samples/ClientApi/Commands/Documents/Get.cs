using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents
{
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
			JsonDocument[] StartsWith(
				string keyPrefix, 
				string matches,
				int start,
				int pageSize,
				RavenPagingInformation pagingInformation = null,
				bool metadataOnly = false,
				string exclude = null,
				string transformer = null,
				Dictionary<string, RavenJToken> queryInputs = null);
			#endregion
		}

		public Get()
		{
			using (var store = new DocumentStore())
			{
				#region get_1_2
				var document = store.DatabaseCommands.Get("products/1"); // null if does not exist
				#endregion

				#region get_2_2
				var resultsWithoutIncludes = store
					.DatabaseCommands
					.Get(new[] { "products/1", "products/2" }, null);
				#endregion

				#region get_2_3
				var resultsWithIncludes = store
					.DatabaseCommands
					.Get(
						new[] { "products/1", "products/2" },
						new[] { "Category" });

				var results = resultsWithIncludes.Results; // products/1, products/2
				var includes = resultsWithIncludes.Includes; // categories/1
				#endregion
			}
		}

		public void MissingDocuments()
		{
			using (var store = new DocumentStore())
			{
				#region get_2_4
				// assuming that 'products/9999' does not exist
				var resultsWithIncludes = store
					.DatabaseCommands
					.Get(
						new[] { "products/1", "products/9999", "products/3" },
						null);

				var results = resultsWithIncludes.Results; // products/1, null, products/3
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
				// return up to 128 documents with key that starts with 'products'
				var result = store.DatabaseCommands.StartsWith("products", null, 0, 128);
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_4_2
				// return up to 128 documents with key that starts with 'products/' 
				// and rest of the key begins with "1" or "2" e.g. products/10, products/25
				var result = store.DatabaseCommands.StartsWith("products/", "1*|2*", 0, 128); 
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_4_3
				// return up to 128 documents with key that starts with 'products/' 
				// and rest of the key have length of 3, begins and ends with "1" 
				// and contains any character at 2nd position e.g. products/101, products/1B1
				var result = store.DatabaseCommands.StartsWith("products/", "1?1", 0, 128);
				#endregion
			}
		}
	}
}