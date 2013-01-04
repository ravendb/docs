namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Indexes;

	public class Post
	{
		public Post()
		{
			this.Comments = new List<Comment>();
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public IList<Comment> Comments { get; set; }
	}

	public class Comment
	{
		public Comment()
		{
			this.Comments = new List<Comment>();
		}

		public string Id { get; set; }

		public string Author { get; set; }

		public string Text { get; set; }

		public IList<Comment> Comments { get; set; }
	}

	#region indexing_hierarchies_1
	public class SampleRecurseIndex : AbstractIndexCreationTask<Post>
	{
		public SampleRecurseIndex()
		{
			Map = posts => from post in posts
						   from comment in Recurse(post, x => x.Comments)
						   select new
						   {
							   Author = comment.Author,
							   Text = comment.Text
						   };
		}
	}

	#endregion

	public class IndexingHierarchies : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region indexing_hierarchies_2
				new SampleRecurseIndex().Execute(store);

				#endregion

				#region indexing_hierarchies_3
				store.DatabaseCommands.PutIndex("SampleRecurseIndex", new IndexDefinition
				{
					Map = @"from post in docs.Posts
							from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.Comments))
							select new
							{
								Author = comment.Author,
								Text = comment.Text
							}"
				});

				#endregion
			}
		}
	}
}