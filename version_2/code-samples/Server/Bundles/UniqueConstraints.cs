namespace RavenCodeSamples.Server.Bundles
{
	#region using
	using Raven.Client.UniqueConstraints;
	#endregion

	public class UniqueConstraints : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }

			public string Email { get; set; }
		}

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region unique_constraints_1
				store.RegisterListener(new UniqueConstraintsStoreListener());

				#endregion

				using (var session = store.OpenSession())
				{
					#region unique_constraints_2
					var existingUser = session.LoadByUniqueConstraint<User>(x => x.Email, "john@gmail.com");

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region unique_constraints_3
					var user = new User() { Name = "John", Email = "john@gmail.com" };
					var checkResult = session.CheckForUniqueConstraints(user);

					// returns wheter it's constraints are avaiable
					if (checkResult.ConstraintsAreFree())
					{
						session.Store(user);
					}
					else
					{
						var existingUser = checkResult.DocumentForProperty(x => x.Email);
					}

					#endregion
				}
			}

		}
	}
}