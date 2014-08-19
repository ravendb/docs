using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Transformers
{
	public class Recurse
	{
		#region transformers_4
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

		#region transformers_1
		public class Category
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string ParentId { get; set; }
		}
		#endregion

		#region transformers_5
		public class BlogPosts_Comments_Authors : AbstractTransformerCreationTask<BlogPost>
		{
			public class Result
			{
				public string Title { get; set; }

				public List<string> Authors { get; set; }
			}

			public BlogPosts_Comments_Authors()
			{
				TransformResults =
					posts => from post in posts
							 let comments = Recurse(post, x => x.Comments)
							 select new
								{
									post.Title,
									Authors = comments.Select(x => x.Author)
								};
			}
		}
		#endregion

		#region transformers_2
		public class Categories_WithParents : AbstractTransformerCreationTask<Category>
		{
			public class Result
			{
				public string Name { get; set; }

				public List<string> Parents { get; set; }
			}

			public Categories_WithParents()
			{
				TransformResults =
					categories => from category in categories
								  let parentCategories = Recurse(category, c => LoadDocument<Category>(c.ParentId))
								  select new
									{
										category.Name,
										Parents = parentCategories.Select(parent => parent.Name)
									};
			}
		}
		#endregion

		public Recurse()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_3
					IList<Categories_WithParents.Result> results = session
						.Query<Category>()
						.TransformWith<Categories_WithParents, Categories_WithParents.Result>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_6
					IList<BlogPosts_Comments_Authors.Result> results = session
						.Query<BlogPost>()
						.TransformWith<BlogPosts_Comments_Authors, BlogPosts_Comments_Authors.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}