namespace RavenCodeSamples.Faq
{
	public class UniqueConstraints : CodeSampleBase
	{
		private class User
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string Email { get; set; }
		}

		private class EmailReference
		{
			public string Id { get; set; }

			public string UserId { get; set; }
		}

		public void Sample()
		{
			using (var store = this.NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region unique_constraints_1
					session.Advanced.UseOptimisticConcurrency = true;
					session.Store(new User { Id = "users/ayende", Name = "Ayende Rahien", Email = "ayende@ayende.com" });
					session.Store(new EmailReference { Id = "emails/ayende@ayende.com", UserId = "users/ayende" });
					session.SaveChanges(); // if there already exists a document with either name, it would fail the transaction

					#endregion
				}
			}
		}
	}
}