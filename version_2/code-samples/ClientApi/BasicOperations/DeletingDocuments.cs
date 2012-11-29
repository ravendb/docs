namespace RavenCodeSamples.ClientApi.BasicOperations
{
	using Raven.Abstractions.Commands;

	public class DeletingDocuments : CodeSampleBase
	{
		public void Deleting()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					var existingBlogPost = session.Load<BlogPost>("blogposts/1");

					#region deleting_document_1
					session.Delete(existingBlogPost);
					session.SaveChanges();
					#endregion

					#region deleting_document_2
					session.Advanced.DocumentStore.DatabaseCommands.Delete("posts/1234", null);
					#endregion

					#region deleting_document_using_defer
					session.Advanced.Defer(new DeleteCommandData { Key = "posts/1234" });
					#endregion
				}
			}
		}
	}
}