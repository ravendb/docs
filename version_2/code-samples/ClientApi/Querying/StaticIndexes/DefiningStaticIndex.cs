using System;
using Raven.Abstractions.Indexing;
using Raven.Client;

namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	using Raven.Client.Indexes;

	namespace Foo
	{
		using System.Collections.Generic;

		using Raven.Abstractions.Indexing;

		#region index_definition
		class IndexDefinition
		{
			/// <summary>
			/// Get or set the name of the index
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the map function
			/// </summary>
			/// <value>The map.</value>
			public string Map { get; set; }

			/// <summary>
			/// Gets or sets the reduce function
			/// </summary>
			/// <value>The reduce.</value>
			public string Reduce { get; set; }

			/// <summary>
			/// Gets or sets the translator function
			/// </summary>
			public string TransformResults { get; set; }

			/// <summary>
			/// Gets or sets the stores options
			/// </summary>
			/// <value>The stores.</value>
			public IDictionary<string, FieldStorage> Stores { get; set; }

			/// <summary>
			/// Gets or sets the indexing options
			/// </summary>
			/// <value>The indexes.</value>
			public IDictionary<string, FieldIndexing> Indexes { get; set; }

			/// <summary>
			/// Gets or sets the sort options.
			/// </summary>
			/// <value>The sort options.</value>
			public IDictionary<string, SortOptions> SortOptions { get; set; }

			/// <summary>
			/// Gets or sets the analyzers options
			/// </summary>
			/// <value>The analyzers.</value>
			public IDictionary<string, string> Analyzers { get; set; }
		}
		#endregion
	}

	public class User
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}

	#region defining_static_index_1
	public class Users_AllProperties : AbstractIndexCreationTask<User, Users_AllProperties.Result>
	{
		public class Result
		{
			public string Query { get; set; }
		}

		public Users_AllProperties()
		{
			Map = users => from user in users
						   select new
							   {
								   Query = AsDocument(user).Select(x => x.Value)
							   };

			Index(x => x.Query, FieldIndexing.Analyzed);
		}
	}

	#endregion

	#region defining_static_index_3
	public class Users_LastModified : AbstractIndexCreationTask<User>
	{
		public class Result
		{
			public DateTime LastModified { get; set; }
		}

		public Users_LastModified()
		{
			Map = users => from user in users
						   select new
							   {
								   LastModified = MetadataFor(user).Value<DateTime>("Last-Modified")
							   };

			TransformResults = (database, results) => from result in results
													  select new
														  {
															  FirstName = result.FirstName,
															  LastName = result.LastName,
															  LastModified = MetadataFor(result).Value<DateTime>("Last-Modified")
														  };
		}
	}

	#endregion

	public class DefiningStaticIndex : CodeSampleBase
	{
		public void CreatingNewIndex()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region static_indexes2
				// Create an index where we search based on a post title
				documentStore.DatabaseCommands.PutIndex("BlogPosts/ByTitles",
														new IndexDefinitionBuilder<BlogPost>
														{
															Map = posts => from post in posts
																		   select new { post.Title }
														});

				#endregion

				#region static_indexes3
				documentStore.DatabaseCommands.PutIndex(
					"BlogPosts/PostsCountByTag",
					new IndexDefinitionBuilder<BlogPost, BlogTagPostsCount>
					{
						// The Map function: for each tag of each post, create a new BlogTagPostsCount
						// object with the name of a tag and a count of one.
						Map = posts => from post in posts
									   from tag in post.Tags
									   select new
									   {
										   Tag = tag,
										   Count = 1
									   },

						// The Reduce function: group all the BlogTagPostsCount objects we got back
						// from the Map function, use the Tag name as the key, and sum up all the
						// counts. Since the Map function gives each tag a Count of 1, when the Reduce
						// function returns we are going to have the correct Count of posts filed under
						// each tag.
						Reduce = results => from result in results
											group result by result.Tag
												into g
												select new
												{
													Tag = g.Key,
													Count = g.Sum(x => x.Count)
												}
					});

				#endregion

				#region static_indexes5
				IndexCreation.CreateIndexes(typeof(MyIndexClass).Assembly, documentStore);

				#endregion

				using (var session = documentStore.OpenSession())
				{
					int count;

					#region static_indexes6
					// This is how to query the first index we defined, using the BlogTagPostsCount class

					var blogTagPostsCount = session.Query<BlogTagPostsCount>("BlogPosts/PostsCountByTag")
						.FirstOrDefault(x => x.Tag == "RavenDB")
						?? new BlogTagPostsCount();
					count = blogTagPostsCount.Count;

					// Alternatively, if we used an AbstractIndexCreationTask, we could use this version
					// Note how we reuse the ReduceResult class to get back the information

					var tagPostsCount = session.Query<BlogPosts_PostsCountByTag.ReduceResult, BlogPosts_PostsCountByTag>()
						.FirstOrDefault(x => x.Tag == "RavenDB")
						?? new BlogPosts_PostsCountByTag.ReduceResult();
					count = tagPostsCount.Count;

					#endregion
				}
			}
		}

		#region static_indexes4
		public class BlogPosts_PostsCountByTag : AbstractIndexCreationTask<BlogPost, BlogPosts_PostsCountByTag.ReduceResult>
		{
			public class ReduceResult
			{
				public string Tag { get; set; }
				public int Count { get; set; }
			}

			// The index name generated by this is going to be BlogPosts/PostsCountByTag
			public BlogPosts_PostsCountByTag()
			{
				Map = posts => from post in posts
							   from tag in post.Tags
							   select new
										  {
											  Tag = tag,
											  Count = 1
										  };

				Reduce = results => from result in results
									group result by result.Tag
										into g
										select new
												   {
													   Tag = g.Key,
													   Count = g.Sum(x => x.Count)
												   };
			}
		}

		#endregion

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region defining_static_index_2
					session.Query<Users_AllProperties.Result, Users_AllProperties>()
						   .Where(x => x.Query == "Ayende") // search first name
						   .OfType<User>()
						   .ToList();

					session.Query<Users_AllProperties.Result, Users_AllProperties>()
						   .Where(x => x.Query == "Rahien") // search last name
						   .OfType<User>()
						   .ToList();

					#endregion

					#region defining_static_index_4
					session.Query<Users_LastModified.Result, Users_LastModified>()
					       .OrderByDescending(x => x.LastModified)
					       .OfType<User>()
					       .ToList();

					#endregion
				}
			}
		}
	}
}