namespace RavenCodeSamples.Server.Bundles
{
	using Raven.Client.Bundles.Versioning;

	public class Versioning : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		private class Loan
		{
			public string Id { get; set; }
		}

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region versioning1
					session.Store(new
					{
						Exclude = false,
						Id = "Raven/Versioning/DefaultConfiguration",
						MaxRevisions = 5
					});

					#endregion

					#region versioning2
					session.Store(new
					{
						Exclude = true,
						Id = "Raven/Versioning/Users",
					});

					#endregion
				}

				#region versioning3
				using (var session = store.OpenSession())
				{
					session.Store(new User { Name = "Ayende Rahien" });
					session.SaveChanges();
				}

				#endregion

				var loan = new Loan { Id = "loans/1" };

				using (var session = store.OpenSession())
				{
					#region versioning_4
					var pastRevisions = session.Advanced.GetRevisionsFor<Loan>(loan.Id, 0, 25);

					#endregion
				}
			}
		}
	}
}