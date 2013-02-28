namespace RavenCodeSamples.Intro
{
	using System.Dynamic;

	public class SafeByDefault : CodeSampleBase
	{
		public void Sample()
		{
			dynamic postsRepository = new ExpandoObject();
			dynamic commentsRepository = new ExpandoObject();
			dynamic recentComments = new ExpandoObject();

			#region safe_by_default_1
			var posts = postsRepository.GetRecentPosts();
			foreach (var post in posts)
			{
				var comments = commentsRepository.GetRecentCommentsForPost(post);
				recentComments.AddRange(comments);
			}

			#endregion
		}
	}
}