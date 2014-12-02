using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
	public class IndexingHierarchicalData
	{
		#region indexes_1
		public class BlogPost
		{
			public string Author { get; set; }

			public string Title { get; set; }

			public string Text { get; set; }

			public List<BlogPostComment> Comments { get; set; }
		}

		public class BlogPostComment
		{
			public string Author { get; set; }

			public string Text { get; set; }

			public List<BlogPostComment> Comments { get; set; }
		}
		#endregion

		#region indexes_2
		public class BlogPosts_ByCommentAuthor : AbstractIndexCreationTask<BlogPost>
		{
			public class Result
			{
				public string Author { get; set; }
			}

			public BlogPosts_ByCommentAuthor()
			{
				Map = posts => from post in posts
							   from comment in Recurse(post, x => x.Comments)
							   select new
							   {
								   Author = comment.Author
							   };
			}
		}
		#endregion

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region indexes_3
				store.DatabaseCommands.PutIndex("BlogPosts/ByCommentAuthor", new IndexDefinition
				{
					Map = @"from post in docs.Posts
						from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.Comments))
						select new
						{
							Author = comment.Author
						}"
				});
				#endregion

				using (var session = store.OpenSession())
				{
					#region indexes_4
					IList<BlogPost> results = session
						.Query<BlogPosts_ByCommentAuthor.Result, BlogPosts_ByCommentAuthor>()
						.Where(x => x.Author == "Ayende Rahien")
						.OfType<BlogPost>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_5
					IList<BlogPost> results = session
						.Advanced
						.DocumentQuery<BlogPost, BlogPosts_ByCommentAuthor>()
						.WhereEquals("Author", "Ayende Rahien")
						.ToList();
					#endregion
				}
			}
		}
	}
}