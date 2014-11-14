using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Searching
	{
		#region search_1_3
		public class Users_ByName : AbstractIndexCreationTask<User>
		{
			public Users_ByName()
			{
				Map = users => from user in users
							   select new
							   {
								   user.Name
							   };

				Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
			}
		}
		#endregion

		#region search_4_3
		public class Users_ByHobbies : AbstractIndexCreationTask<User>
		{
			public Users_ByHobbies()
			{
				Map = users => from user in users
							   select new
							   {
								   user.Hobbies
							   };

				Indexes.Add(x => x.Hobbies, FieldIndexing.Analyzed);
			}
		}
		#endregion

		#region search_5_3
		public class Users_ByNameAndHobbies : AbstractIndexCreationTask<User>
		{
			public Users_ByNameAndHobbies()
			{
				Map = users => from user in users
							   select new
							   {
								   user.Name,
								   user.Hobbies
							   };

				Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
				Indexes.Add(x => x.Hobbies, FieldIndexing.Analyzed);
			}
		}
		#endregion

		#region search_7_3
		public class Users_ByNameAgeAndHobbies : AbstractIndexCreationTask<User>
		{
			public Users_ByNameAgeAndHobbies()
			{
				Map = users => from user in users
							   select new
							   {
								   user.Name,
								   user.Age,
								   user.Hobbies
							   };

				Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
				Indexes.Add(x => x.Hobbies, FieldIndexing.Analyzed);
			}
		}
		#endregion


		#region highlights_1
		public class SearchItem
		{
			public string Id { get; set; }

			public string Text { get; set; }
		}

		public class ContentSearchIndex : AbstractIndexCreationTask<SearchItem>
		{
			public ContentSearchIndex()
			{
				Map = (docs => from doc in docs
							   select new { doc.Text });

				Index(x => x.Text, FieldIndexing.Analyzed);
				Store(x => x.Text, FieldStorage.Yes);
				TermVector(x => x.Text, FieldTermVector.WithPositionsAndOffsets);
			}
		}
		#endregion

		#region linq_extensions_search_user_class
		public class User
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public byte Age { get; set; }

			public ICollection<string> Hobbies { get; set; }
		}
		#endregion

		public Searching()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_1_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Where(x => x.Name == "John")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_1_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.WhereEquals(x => x.Name, "John")
						.ToList();
					#endregion
				}

				#region search_1_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:John"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_2_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Where(x => x.Name.StartsWith("Jo"))
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_2_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.WhereStartsWith(x => x.Name, "Jo")
						.ToList();
					#endregion
				}

				#region search_2_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:Jo*"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_3_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Search(x => x.Name, "John Adam")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_3_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.Search(x => x.Name, "John Adam")
						.ToList();
					#endregion
				}

				#region search_3_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:(John Adam)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_4_0
					IList<User> users = session
						.Query<User, Users_ByHobbies>()
						.Search(x => x.Hobbies, "looking for someone who likes sport books computers")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_4_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByHobbies>()
						.Search(x => x.Hobbies, "looking for someone who likes sport books computers")
						.ToList();
					#endregion
				}

				#region search_4_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByHobbies",
						new IndexQuery
						{
							Query = "Name:(looking for someone who likes sport books computers)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_5_0
					List<User> users = session
						.Query<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.Search(x => x.Hobbies, "sport")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_5_1
					List<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.Search(x => x.Hobbies, "sport")
						.ToList();
					#endregion
				}

				#region search_5_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByNameAndHobbies",
						new IndexQuery
						{
							Query = "Name:(Adam) OR Hobbies:(sport)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_6_0
					IList<User> users = session
						.Query<User, Users_ByHobbies>()
						.Search(x => x.Hobbies, "I love sport", boost: 10)
						.Search(x => x.Hobbies, "but also like reading books", boost: 5)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_6_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByHobbies>()
						.Search(x => x.Hobbies, "I love sport")
						.Boost(10)
						.Search(x => x.Hobbies, "but also like reading books")
						.Boost(5)
						.ToList();
					#endregion
				}

				#region search_6_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByHobbies",
						new IndexQuery
						{
							Query = "Hobbies:(I love sport)^10 OR Hobbies:(but also like reading books)^5"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_7_0
					IList<User> users = session
						.Query<User, Users_ByNameAgeAndHobbies>()
						.Search(x => x.Hobbies, "computers")
						.Search(x => x.Name, "James")
						.Where(x => x.Age == 20)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_7_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByNameAgeAndHobbies>()
						.Search(x => x.Hobbies, "computers")
						.Search(x => x.Name, "James")
						.WhereEquals(x => x.Age, 20)
						.ToList();
					#endregion
				}

				#region search_7_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByNameAgeAndHobbies",
						new IndexQuery
						{
							Query = "(Hobbies:(computers) OR Name:(James)) AND Age:20"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_8_0
					IList<User> users = session
						.Query<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.Search(x => x.Hobbies, "sport", options: SearchOptions.And)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_8_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.AndAlso()
						.Search(x => x.Hobbies, "sport")
						.ToList();
					#endregion
				}

				#region search_8_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByNameAndHobbies",
						new IndexQuery
						{
							Query = "Name:(Adam) AND Hobbies:(sport)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_9_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Search(x => x.Name, "James", options: SearchOptions.Not)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_9_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.Not
						.Search(x => x.Name, "James")
						.ToList();
					#endregion
				}

				#region search_9_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "-Name:(James)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_10_0
					IList<User> users = session
						.Query<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.Search(x => x.Hobbies, "sport", options: SearchOptions.Not | SearchOptions.And)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_10_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByNameAndHobbies>()
						.Search(x => x.Name, "Adam")
						.AndAlso()
						.Not
						.Search(x => x.Hobbies, "sport")
						.ToList();
					#endregion
				}

				#region search_10_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:(Adam) AND -Hobbies:(sport)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_11_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Search(x => x.Name, "Jo* Ad*", escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_11_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.Search("Name", "Jo* Ad*", escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
						.ToList();
					#endregion
				}

				#region search_11_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:(Jo* Ad*)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_12_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Search(x => x.Name, "*oh* *da*", escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_12_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.Search("Name", "*oh* *da*", escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards)
						.ToList();
					#endregion
				}

				#region search_12_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:(*oh* *da*)"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region search_13_0
					IList<User> users = session
						.Query<User, Users_ByName>()
						.Search(x => x.Name, "*J?n*", escapeQueryOptions: EscapeQueryOptions.RawQuery)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region search_13_1
					IList<User> users = session
						.Advanced
						.DocumentQuery<User, Users_ByName>()
						.Search(x => x.Name, "*J?n*", escapeQueryOptions: EscapeQueryOptions.RawQuery)
						.ToList();
					#endregion
				}

				#region search_13_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Users/ByName",
						new IndexQuery
						{
							Query = "Name:(*J?n*)"
						});
				#endregion
			}
		}
	}
}
