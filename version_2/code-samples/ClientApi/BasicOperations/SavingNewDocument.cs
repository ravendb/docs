namespace RavenCodeSamples.ClientApi.BasicOperations
{
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
				store.Conventions.DocumentKeyGenerator = (commands, entity) => store.Conventions.GetTypeTagName(entity.GetType()) + "/";

				#endregion
			}
		}
	}
}