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

					#region editing_document_5
					BlogPostWithIntegerId blogPostWithIntegerId = session.Load<BlogPostWithIntegerId>(1);
					#endregion

					#region editing_document_6
					existingBlogPost = session.Load<BlogPost>(1);
					#endregion

					#region editing_document_7
					BlogPost[] blogPosts = session.Load<BlogPost>("blogposts/1",
																  "blogposts/2",
																  "blogposts/3");
					#endregion

					#region editing_document_8
					BlogPostWithIntegerId[] blogPostsWithInts = session.Load<BlogPostWithIntegerId>(1, 2, 3);
					#endregion

					#region editing_document_9_0
					BlogPost[] prefixedResults = session.Advanced.LoadStartingWith<BlogPost>("blogposts/1");
					#endregion

					#region editing_document_9_1
					BlogPost[] prefixedResultsWithMatch = session.Advanced
						.LoadStartingWith<BlogPost>("blogposts/1", "*/Author/*t");

					#endregion

					#region editing_document_9_2
					BlogPost[] prefixedResultsWithMultipleMatch = session.Advanced
						.LoadStartingWith<BlogPost>("blogposts/1", "*/Author/*t|*/Type/*t");

					#endregion
				}
			}
		}

		#region editing_document_4
		public class BlogPostWithIntegerId
		{
			public int Id { get; set; }
			/*
			 ...
			*/
		}
		#endregion
	}
}