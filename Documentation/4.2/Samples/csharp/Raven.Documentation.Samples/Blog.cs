using System;

namespace Raven.Documentation.Samples
{
	#region blog_post
	public class BlogPost
	{
		public string Id { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public string Content { get; set; }

		public DateTime PublishedAt { get; set; }

		public string[] Tags { get; set; }

		public BlogComment[] Comments { get; set; }
	}
	#endregion

	#region blog_comment
	public class BlogComment
	{
		public string Title { get; set; }

		public string Content { get; set; }
	}
	#endregion
}
