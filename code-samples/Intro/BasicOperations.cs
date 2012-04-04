using System;
using System.Transactions;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;
using System.Linq;

namespace RavenCodeSamples.Intro
{
	public class BasicOperations : CodeSampleBase
	{
		public void UnderstandingSessionObject()
		{
			using (var documentStore = NewDocumentStore())
			{

				#region session_usage_1
				string companyId;
				using (var session = documentStore.OpenSession())
				{
					var entity = new Company { Name = "Company" };
					session.Store(entity);
					session.SaveChanges();
					companyId = entity.Id;
				}

				using (var session = documentStore.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					Console.WriteLine(entity.Name);
				}
				#endregion

				#region session_usage_2
				using (var session = documentStore.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					entity.Name = "Another Company";
					session.SaveChanges(); // will send the change to the database
				}
				#endregion
			}
		}

		public void BasicSamples()
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
				#region open_the_session
				// Saving changes using the session API
				using (IDocumentSession session = store.OpenSession())
				{
					// Operations against session

					// Flush those changes
					session.SaveChanges();
				}
				#endregion

				using (var session = store.OpenSession())
				{
					#region saving_document_2

					// Saving the new instance to RavenDB
					session.Store(post);
					session.SaveChanges();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region editing_document_1
					// blogposts/1 is entity of type BlogPost with Id of 1
					BlogPost existingBlogPost = session.Load<BlogPost>("blogposts/1");
					#endregion

					#region editing_document_2
					existingBlogPost.Title = "Some new title";
					#endregion

					#region editing_document_3
					session.SaveChanges();
					#endregion

					#region deleting_document_1
					session.Delete(existingBlogPost);
					session.SaveChanges();
					#endregion

					#region deleting_document_2
					session.Advanced.DatabaseCommands.Delete("posts/1234", null);
					#endregion

					#region deleting_document_using_defer
					session.Advanced.Defer(new DeleteCommandData {Key = "posts/1234"});
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transaction_support_1
					using (var transaction = new TransactionScope())
					{
						BlogPost entity = session.Load<BlogPost>("blogs/1");

						entity.Title = "Some new title";

						session.SaveChanges();

						session.Delete(entity);
						session.SaveChanges();

						transaction.Complete();
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basic_querying_1
					var results = from blog in session.Query<BlogPost>()
					              where blog.Category == "RavenDB"
					              select blog;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basic_querying_2
					var results = session.Query<BlogPost>()
						.Where(x => x.Comments.Length >= 10)
						.ToList();
					#endregion
				}
			}
		}
	}
}
