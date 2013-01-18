namespace RavenCodeSamples.ClientApi.Advanced
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Abstractions.Data;

	public class FullQuerySyntax : CodeSampleBase
	{
		public class FullName
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		public class User
		{
			public FullName FullName { get; set; }
			public IList<Tag> Tags { get; set; }
			public DateTime DateOfBirth { get; set; }
		}

		public class Tag
		{
			public string Name { get; set; }
		}

		public FullQuerySyntax()
		{
			using (var store = NewDocumentStore())
			{
				#region tokenized_field
				QueryResult users = store.DatabaseCommands.Query(
					"Raven/DocumentsByEntityName",
					new IndexQuery
						{
							Query = "Tag:[[Users]]"
						}, null);
				#endregion

				#region name_null_or_empty
				QueryResult usersWithNullOrEmptyName = store.DatabaseCommands.Query(
					"Users/ByName",
					new IndexQuery
						{
							Query = "(Name:[[NULL_VALUE]] OR Name:[[EMPTY_STRING]])"
						}, null);

				#endregion

				#region nested_properties
				using (var session = store.OpenSession())
				{
					session.Store(new User()
					{
						FullName = new FullName()
						{
							FirstName = "John",
							LastName = "Smith"
						}
					});

					session.SaveChanges();
				}

				var usersByNameJohn = store.DatabaseCommands.Query("dynamic", new IndexQuery()
				{
					Query = "Name.FirstName:John"
				}, null);
				#endregion

				
				using (var session = store.OpenSession())
				{
					#region users_by_tag_sportsman
					session.Store(new User()
					{
						Tags = new List<Tag>()
						                     {
							                     new Tag()
							                     {
								                     Name = "sportsman"
							                     }
						                     }
					});

					session.SaveChanges();

					var usersTagggedAsSportsman = session.Advanced.LuceneQuery<User>()
													.Where("Tags,Name:sportsman")
													.ToList();
					#endregion
				}
				

				using (var session = store.OpenSession())
				{
					#region users_by_dob
					var usersbyDoB = session.Advanced.LuceneQuery<User>()
										.Where("DateOfBirth:[1980-01-01 TO 1999-12-31T00:00:00.0000000]")
										.ToList();
					#endregion
				}

				
				//using (var session = store.OpenSession())
					//{
					//	session.Advanced.LuceneQuery<
					//		Product>().Where()
					//}
				}
		}
	}
}