using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	public class Searching
	{
		private interface IHighlights<T>
		{
			#region highlights_3
			IDocumentQuery<T> Highlight(
				string fieldName,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			IDocumentQuery<T> Highlight<TValue>(
				Expression<Func<T, TValue>> propertySelector,
				int fragmentLength,
				int fragmentCount,
				out FieldHighlightings highlightings);

			#endregion

			#region highlights_4
			IDocumentQuery<T> SetHighlighterTags(string preTag, string postTag);

			IDocumentQuery<T> SetHighlighterTags(string[] preTags, string[] postTags);
			#endregion
		}

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
			using (var documentStore = new DocumentStore())
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
								   .Search(x => x.Hobbies, "I love sport", boost: 10)
								   .Search(x => x.Hobbies, "but also like reading books", boost: 5).ToList();
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
								escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard).ToList();
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
