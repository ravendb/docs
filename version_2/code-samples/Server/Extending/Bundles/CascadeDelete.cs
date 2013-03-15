namespace RavenCodeSamples.Server.Bundles
{
	using Raven.Json.Linq;

	public class CascadeDelete : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				var parent = new User();

				#region cascadedelete1
				using (var session = documentStore.OpenSession())
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