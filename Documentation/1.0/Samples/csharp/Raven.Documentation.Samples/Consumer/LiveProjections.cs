using System.Collections.Generic;
using System.Linq;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace RavenCodeSamples.Consumer
{
	public class LiveProjections : CodeSampleBase
	{
		public class User
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string AliasId { get; set; }
		}

		public class UserAndAlias
		{
			public string UserName { get; set; }
			public string Alias { get; set; }
		}

		public void BasicSample()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region liveprojections1

					// Storing a sample entity
					var entity = new User {Name = "Ayende"};
					session.Store(entity);
					session.Store(new User {Name = "Oren", AliasId = entity.Id});
					session.SaveChanges();

					// ...
					// ...

					// Get all users, mark AliasId as a field we want to use for Including
					var usersWithAliases = from user in session.Query<User>().Include(x => x.AliasId)
					                       where user.AliasId != null
					                       select user;

					var results = new List<UserAndAlias>(); // Prepare our results list
					foreach (var user in usersWithAliases)
					{
						// For each user, load its associated alias based on that user Id
						results.Add(new UserAndAlias
						            	{
						            		UserName = user.Name,
						            		Alias = session.Load<User>(user.AliasId).Name
						            	}
							);
					}

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region liveprojections3
					var usersWithAliases =
						(from user in session.Query<User, Users_ByAlias>()
						 where user.AliasId != null
						 select user).As<UserAndAlias>();
					#endregion
				}
			}
		}

		#region liveprojections2
		public class Users_ByAlias : AbstractIndexCreationTask<User>
		{
			public Users_ByAlias()
			{
				Map = users => from user in users
				               select new {user.AliasId};

				TransformResults =
					(database, users) => from user in users
										 let alias = database.Load<User>(user.AliasId)
										 select new { Name = user.Name, Alias = alias.Name };
			}
		}
		#endregion
	}
}
