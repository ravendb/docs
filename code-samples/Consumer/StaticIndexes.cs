using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Database.Server.Responders;

namespace RavenCodeSamples.Consumer
{
	namespace Foo
	{
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

	public class StaticIndexes : CodeSampleBase
	{
		public void BasicStaticIndexes()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region static_indexes1
					var results = session.Query<BlogPost>("MyBlogPostsIndex").ToArray();
					#endregion
				}

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
				Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(MyIndexClass).Assembly, documentStore);
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

					// Altenratively, if we used an AbstractIndexCreationTask, we could use this version
					// Note how we reuse the ReduceResult class to get back the information

					var tagPostsCount = session.Query<BlogPosts_PostsCountByTag.ReduceResult, BlogPosts_PostsCountByTag>()
						.FirstOrDefault(x => x.Tag == "RavenDB")
						?? new BlogPosts_PostsCountByTag.ReduceResult();
					count = tagPostsCount.Count;

					#endregion
				}

				#region analyzers1
				documentStore.DatabaseCommands.PutIndex("AnalyzersTestIdx", new IndexDefinitionBuilder<BlogPost, BlogPost>
																				{
																					Map =
																						users =>
																						from doc in users select new { doc.Tags, doc.Content },
																					Analyzers =
				                                                            			{
				                                                            				{x => x.Tags, "SimpleAnalyzer"},
				                                                            				{x => x.Content, "SnowballAnalyzer"}
				                                                            			},
																				});
				#endregion
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

		#region static_sorting1
		public class SampleIndex1 : AbstractIndexCreationTask<Customer, Customer>
		{
			public SampleIndex1()
			{
				Map = users => from user in users select new { user.Age };

				SortOptions.Add(x => x.Age, Raven.Abstractions.Indexing.SortOptions.Short);
			}
		}
		#endregion

		#region static_sorting2
		public class SampleIndex2 : AbstractIndexCreationTask<Customer, Customer>
		{
			public SampleIndex2()
			{
				Map = users => from doc in users select new { doc.Name };

				SortOptions.Add(x => x.Name, Raven.Abstractions.Indexing.SortOptions.String);

				Analyzers.Add(x => x.Name, "Raven.Database.Indexing.Collation.Cultures.SvCollationAnalyzer, Raven.Database");
			}
		}
		#endregion

		#region stores1
		public class StoresIndex : AbstractIndexCreationTask<BlogPost, BlogPost>
		{
			public StoresIndex()
			{
				Map = posts => from doc in posts
							   select new { doc.Tags, doc.Content };

				Stores.Add(x => x.Title, FieldStorage.Yes);

				Indexes.Add(x => x.Tags, FieldIndexing.NotAnalyzed);
				Indexes.Add(x => x.Comments, FieldIndexing.No);
			}
		}
		#endregion
	}
}
