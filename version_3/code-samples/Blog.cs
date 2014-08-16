using System;

namespace Raven.Documentation.CodeSamples
{
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

	public class BlogComment
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}
}