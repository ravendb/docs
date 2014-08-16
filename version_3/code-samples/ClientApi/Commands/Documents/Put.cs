using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents
{
	public class Put
    {
		private interface IFoo
		{
			 #region put_1
			 PutResult Put(string key, Etag etag, RavenJObject document, RavenJObject metadata);
			 #endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_3
				store
					.DatabaseCommands
					.Put(
						"categories/999",
						null,
						RavenJObject.FromObject(new Category
						{
							Name = "My Category",
							Description = "My Category description"
						}),
						new RavenJObject());
				#endregion
			}
		}
	}
}