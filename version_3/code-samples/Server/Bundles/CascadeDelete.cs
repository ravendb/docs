using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.Server.Bundles
{
	public class CascadeDelete
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				var parent = new User();

				#region cascadedelete1
				using (var session = store.OpenSession())
				{
					session.Store(parent);
					session.Advanced.GetMetadataFor(parent)["Raven-Cascade-Delete-Documents"] = RavenJToken.FromObject(new[] { "childId1", "childId2" });
					session.Advanced.GetMetadataFor(parent)["Raven-Cascade-Delete-Attachments"] = RavenJToken.FromObject(new[] { "attachmentId1", " attachmentId2" });
					session.SaveChanges();
				}
				#endregion
			}
		}
	}
}