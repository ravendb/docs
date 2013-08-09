using Raven.Client.Indexes;

namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	class MyBlogPostsIndex : AbstractIndexCreationTask<BlogPost>
	{
		 
	}

	public class Index : CodeSampleBase
	{
		public void StaticIndexes()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region static_indexes1
					var results = session.Query<BlogPost>("MyBlogPostsIndex").ToArray();
					#endregion
				}
			}
		}

		public void StaticIndexes2()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region static_indexes2
					var results = session.Query<BlogPost, MyBlogPostsIndex>().ToArray();
					#endregion
				}
			}
		}
	}
}