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
			#region saving_new_document_8
			DocumentConvention RegisterIdConvention<TEntity>(Func<string, IDatabaseCommands, TEntity, string> func);

			DocumentConvention RegisterAsyncIdConvention<TEntity>(Func<string, IAsyncDatabaseCommands, TEntity, Task<string>> func);

			#endregion
		}
	}

	#region saving_new_document_1
	public class User
	{
		public string Name { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}

	#endregion

	#region saving_new_document_9
	public class PrivilegedUser : User
	{
	}

	#endregion

	public class SavingNewDocument : CodeSampleBase
	{
		public void Saving()
		{
			#region saving_document_1
			// Creating a new instance of the BlogPost class
			BlogPost post = new BlogPost()
								{
									Title = "Hello RavenDB",
									Category = "RavenDB",
									Content = "This is a blog about RavenDB",
									Comments = new BlogComment[]
			                		           	{
			                		           		new BlogComment() {Title = "Unrealistic", Content = "This example is unrealistic"},
			                		           		new BlogComment() {Title = "Nice", Content = "This example is nice"}

			                		           	}
								};
			#endregion

			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region saving_document_2
					// Saving the new instance to RavenDB
					session.Store(post);
					session.SaveChanges();

					#endregion
				}

				#region saving_document_3
				store.Conventions.DocumentKeyGenerator = (dbname, commands, entity) => store.Conventions.GetTypeTagName(entity.GetType()) + "/";

				#endregion
			}
		}

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region saving_new_document_2
				store.Conventions.RegisterIdConvention<User>((dbname, commands, user) => "users/" + user.Name);

				#endregion

				#region saving_new_document_3
				store.Conventions.RegisterAsyncIdConvention<User>((dbname, commands, user) => new CompletedTask<string>("users/" + user.Name));

				#endregion

				#region saving_new_document_4
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

				#region saving_new_document_5
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
				#region saving_new_document_6
				store.Conventions.RegisterIdConvention<User>((dbname, commands, user) => "users/" + user.Name);

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
				#region saving_new_document_7
				store.Conventions.RegisterIdConvention<User>((dbname, commands, user) => "users/" + user.Name);
				store.Conventions.RegisterIdConvention<PrivilegedUser>((dbname, commands, user) => "admins/" + user.Name);

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