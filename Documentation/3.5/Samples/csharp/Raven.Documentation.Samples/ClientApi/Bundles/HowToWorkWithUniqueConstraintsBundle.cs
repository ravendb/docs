
using Raven.Client.Document;
#region using
using Raven.Client.UniqueConstraints;
#endregion

namespace Raven.Documentation.Samples.ClientApi.Bundles
{
	public class UniqueConstraints
	{
		#region unique_constraints_4
		private class User
		{
			[UniqueConstraint]
			public string Name { get; set; }

			[UniqueConstraint]
			public string Email { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }
		}

		#endregion

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region unique_constraints_1
				store.RegisterListener(new UniqueConstraintsStoreListener());
				#endregion

				using (var session = store.OpenSession())
				{
					#region unique_constraints_2
					User existingUser = session
						.LoadByUniqueConstraint<User>(x => x.Email, "john@gmail.com");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region unique_constraints_3
					User user = new User { Name = "John", Email = "john@gmail.com" };
					UniqueConstraintCheckResult<User> checkResult = session.CheckForUniqueConstraints(user);

					// returns whether its constraints are available
					if (checkResult.ConstraintsAreFree())
					{
						session.Store(user);
					}
					else
					{
						User existingUser = checkResult.DocumentForProperty(x => x.Email);
					}
					#endregion
				}
			}

		}
	}
}