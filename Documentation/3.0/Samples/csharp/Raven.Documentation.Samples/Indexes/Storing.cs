using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	public class Storing
	{
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

		#region storing_1
		public class StoresIndex : AbstractIndexCreationTask<BlogPost, BlogPost>
		{
			public StoresIndex()
			{
				Map = posts => from doc in posts
							   select new
										  {
											  doc.Tags,
											  doc.Content
										  };

				Stores.Add(x => x.Title, FieldStorage.Yes);

				Indexes.Add(x => x.Tags, FieldIndexing.NotAnalyzed);
				Indexes.Add(x => x.Comments, FieldIndexing.No);
			}
		}
		#endregion
	}
}