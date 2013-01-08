namespace RavenCodeSamples.ClientApi.Querying
{
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Abstractions.Indexing;
	using Raven.Client;

	public class Searching : CodeSampleBase
	{
		#region linq_extensions_search_user_class
		public class User
		{
			public string Name { get; set; }

			public byte Age { get; set; }

			public ICollection<string> Hobbies { get; set; } 
		}
		#endregion

		public Searching()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region linq_extensions_search_index_users_by_name
				documentStore.DatabaseCommands.PutIndex("UsersByName", new IndexDefinition
				{
					Map = "from user in docs.Users select new { user.Name }",
					Indexes = { { "Name", FieldIndexing.Analyzed } }
				});
				#endregion

				IList<User> users;

				using (var session = documentStore.OpenSession())
				{
					#region linq_extensions_search_where_age
					users = session.Query<User>("UsersByAge").Where(x => x.Age > 20).ToList();
					#endregion

					#region linq_extensions_search_where_name
					users = session.Query<User>("UsersByName").Where(x => x.Name.StartsWith("Jo")).ToList();
					#endregion

					#region linq_extensions_search_name
					users = session.Query<User>("UsersByName").Search(x => x.Name, "John Adam").ToList();
					#endregion
				}

				#region linq_extensions_search_index_users_by_hobbies
				documentStore.DatabaseCommands.PutIndex("UsersByHobbies", new IndexDefinition
				{
					Map = "from user in docs.Users select new { user.Hobbies }",
					Indexes = { { "Hobbies", FieldIndexing.Analyzed } }
				});
				#endregion

				using (var session = documentStore.OpenSession())
				{
					#region linq_extensions_search_hobbies
					users = session.Query<User>("UsersByHobbies")
						.Search(x => x.Hobbies, "looking for someone who likes sport books computers").ToList();
					#endregion
				}

				#region linq_extensions_search_index_users_by_name_and_hobbies
				documentStore.DatabaseCommands.PutIndex("UsersByNameAndHobbies", new IndexDefinition
				{
					Map = "from user in docs.Users select new { user.Name, user.Hobbies }",
					Indexes = { { "Name", FieldIndexing.Analyzed }, { "Hobbies", FieldIndexing.Analyzed } }
				});
				#endregion

				using (var session = documentStore.OpenSession())
				{
					#region linq_extensions_search_users_by_name_and_hobbies
					users = session.Query<User>("UsersByNameAndHobbies")
					               .Search(x => x.Name, "Adam")
					               .Search(x => x.Hobbies, "sport").ToList();
					#endregion

					#region linq_extensions_search_users_by_hobbies_boost
					users = session.Query<User>("UsersByHobbies")
								   .Search(x => x.Hobbies, "I love sport", boost:10)
								   .Search(x => x.Hobbies, "but also like reading books", boost:5).ToList();
					#endregion

					#region linq_extensions_search_users_by_hobbies_guess
					users = session.Query<User>("UsersByNameAndHobbiesAndAge")
								   .Search(x => x.Hobbies, "computers")
								   .Search(x => x.Name, "James")
								   .Where(x => x.Age == 20).ToList();
					#endregion

					#region linq_extensions_search_users_by_name_and_hobbies_search_and
					users = session.Query<User>("UsersByNameAndHobbies")
								   .Search(x => x.Name, "Adam")
								   .Search(x => x.Hobbies, "sport", options: SearchOptions.And).ToList();
					#endregion

					#region linq_extensions_search_users_by_name_not
					users = session.Query<User>("UsersByName")
							.Search(x => x.Name, "James", options: SearchOptions.Not).ToList();
					#endregion

					#region linq_extensions_search_users_by_name_and_hobbies_and_not
					users = session.Query<User>("UsersByNameAndHobbies")
							.Search(x => x.Name, "Adam")
							.Search(x => x.Hobbies, "sport", options: SearchOptions.Not | SearchOptions.And)
							.ToList();
					#endregion

					#region linq_extensions_search_where_name_post_wildcard
					users = session.Query<User>("UsersByName")
						.Search(x => x.Name, "Jo* Ad*", 
								escapeQueryOptions:EscapeQueryOptions.AllowPostfixWildcard).ToList();
					#endregion

					#region linq_extensions_search_where_name_all_wildcard
					users = session.Query<User>("UsersByName")
						.Search(x => x.Name, "*oh* *da*", 
								escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards).ToList();
					#endregion

					#region linq_extensions_search_where_name_raw
					users = session.Query<User>("UsersByName")
						.Search(x => x.Name, "*J?n*",
								escapeQueryOptions: EscapeQueryOptions.RawQuery).ToList();
					#endregion
				}
			}
		}
	}
}
