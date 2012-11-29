namespace RavenCodeSamples.ClientApi.BasicOperations
{
	public class LoadingEditingExistingDocument : CodeSampleBase
	{
		public void Editing()
		{
			using (var store = NewDocumentStore())
			{
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
				}
			}
		}
	}
}