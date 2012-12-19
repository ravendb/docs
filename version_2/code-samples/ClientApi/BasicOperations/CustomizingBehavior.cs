using System;
using System.Threading.Tasks;
using Raven.Abstractions.Util;
using Raven.Client.Connection;
using Raven.Client.Connection.Async;

namespace RavenCodeSamples.ClientApi.BasicOperations
{
	namespace Foo
	{
		public interface DocumentConvention
		{
			#region customizing_behaviors_8
			DocumentConvention RegisterIdConvention<TEntity>(Func<IDatabaseCommands, TEntity, string> func);

			DocumentConvention RegisterAsyncIdConvention<TEntity>(Func<IAsyncDatabaseCommands, TEntity, Task<string>> func);

			#endregion
		}
	}

	#region customizing_behaviors_1
	public class User
	{
		public string Name { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}

	#endregion

	#region customizing_behaviors_9
	public class PrivilegedUser : User
	{
	}

	#endregion

	public class CustomizingBehavior : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region customizing_behaviors_2
				store.Conventions.RegisterIdConvention<User>((commands, user) => "users/" + user.Name);

				#endregion

				#region customizing_behaviors_3
				store.Conventions.RegisterAsyncIdConvention<User>((commands, user) => new CompletedTask<string>("users/" + user.Name));

				#endregion

				#region customizing_behaviors_4
				using (var session = store.OpenSession())
				{
					session.Store(new User
						{
							Name = "jdoe",
							FirstName = "John",
							LastName = "Doe"
						});

					session.SaveChanges();
				}

				#endregion

				#region customizing_behaviors_5
				using (var session = store.OpenAsyncSession())
				{
					session.Store(new User
					{
						Name = "jcarter",
						FirstName = "John",
						LastName = "Carter"
					});

					session.SaveChangesAsync();
				}

				#endregion
			}

			using (var store = NewDocumentStore())
			{
				#region customizing_behaviors_6
				store.Conventions.RegisterIdConvention<User>((commands, user) => "users/" + user.Name);

				using (var session = store.OpenSession())
				{
					session.Store(new User // users/jdoe
					{
						Name = "jdoe",
						FirstName = "John",
						LastName = "Doe"
					});

					session.Store(new PrivilegedUser // users/jcarter
					{
						Name = "jcarter",
						FirstName = "John",
						LastName = "Carter"
					});

					session.SaveChanges();
				}

				#endregion
			}

			using (var store = NewDocumentStore())
			{
				#region customizing_behaviors_7
				store.Conventions.RegisterIdConvention<User>((commands, user) => "users/" + user.Name);
				store.Conventions.RegisterIdConvention<PrivilegedUser>((commands, user) => "admins/" + user.Name);

				using (var session = store.OpenSession())
				{
					session.Store(new User // users/jdoe
					{
						Name = "jdoe",
						FirstName = "John",
						LastName = "Doe"
					});

					session.Store(new PrivilegedUser // admins/jcarter
					{
						Name = "jcarter",
						FirstName = "John",
						LastName = "Carter"
					});

					session.SaveChanges();
				}

				#endregion
			}
		}
	}
}