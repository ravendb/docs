using System.Linq;

using Lucene.Net.Analysis;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class Analyzers
	{
		private class SnowballAnalyzer
		{
		}

		#region analyzers_1
		public class BlogPosts_ByTagsAndContent : AbstractIndexCreationTask<BlogPost>
		{
			public BlogPosts_ByTagsAndContent()
			{
				Map = posts => from post in posts
						select new
						{
							post.Tags,
							post.Content
						};

				Analyzers.Add(x => x.Tags, typeof(SimpleAnalyzer).FullName);
				Analyzers.Add(x => x.Content, typeof(SnowballAnalyzer).AssemblyQualifiedName);
			}
		}
		#endregion

		#region analyzers_3
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByFirstAndLastName()
			{
				Map = employees => from employee in employees
							select new
							{
								LastName = employee.LastName,
								FirstName = employee.FirstName
							};

				Indexes.Add(x => x.FirstName, FieldIndexing.NotAnalyzed);
			}
		}
		#endregion

		#region analyzers_4
		public class BlogPosts_ByContent : AbstractIndexCreationTask<BlogPost>
		{
			public BlogPosts_ByContent()
			{
				Map = posts => from post in posts 
						select new
						{
							Title = post.Title, 
							Content = post.Content
						};

				Indexes.Add(x => x.Content, FieldIndexing.Analyzed);
			}
		}
		#endregion

		#region analyzers_5
		public class BlogPosts_ByTitle : AbstractIndexCreationTask<BlogPost>
		{
			public BlogPosts_ByTitle()
			{
				Map = posts => from post in posts
						select new
						{
							Title = post.Title,
							Content = post.Content
						};

				Indexes.Add(x => x.Content, FieldIndexing.No);
				Stores.Add(x => x.Content, FieldStorage.Yes);
			}
		}
		#endregion

		public Analyzers()
		{
			using (var store = new DocumentStore())
			{
				#region analyzers_2
				store
					.DatabaseCommands
					.PutIndex(
						"BlogPosts/ByTagsAndContent",
						new IndexDefinitionBuilder<BlogPost>
						{
							Map = posts => from post in posts
									select new
									{
										post.Tags,
										post.Content
									},
							Analyzers =
							{
								{ x => x.Tags, typeof(SimpleAnalyzer).FullName },
								{ x => x.Content, typeof(SnowballAnalyzer).AssemblyQualifiedName }
							},
						});

				#endregion
			}
		}
	}
}
