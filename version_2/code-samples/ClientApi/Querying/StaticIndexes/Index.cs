namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

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
	}
}