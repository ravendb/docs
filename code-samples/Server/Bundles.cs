using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Raven.Client.Indexes;
using Raven.Json.Linq;
using Raven.Client.Authorization;

namespace RavenCodeSamples.Server
{
	public class Bundles : CodeSampleBase
	{
		public class User
		{
			public string Name { get; set; }
		}

		public class Patient
		{
			public void AuthorizeHospitalization()
			{

			}
		}

		public class Question
		{
			public string Title { get; set; }
			public Vote[] Votes { get; set; }
		}

		public class Vote
		{
			public bool Up { get; set; }
			public string Comment { get; set; }
		}

		public class Docs
		{
			public Question[] Questions { get; set; }
		}

		public void IndexReplication()
		{
			#region indexreplication1
			var q = new Question
			{
				Title = "How to replicate to relational database?",
				Votes = new[]
				{
					new Vote {Up = true, Comment = "Good!"},
					new Vote {Up = false, Comment = "Nah!"},
					new Vote {Up = true, Comment = "Nice..."}
				}
			};
			#endregion

			var docs = new Docs();

			var x =

			#region indexreplication2
			// Questions/TitleAndVoteCount
			from question in docs.Questions
			select new
			{
				Title = question.Title,
				VoteCount = question.Votes.Length
			};

			#endregion

			#region indexreplication3
			new Raven.Bundles.IndexReplication.Data.IndexReplicationDestination
			{
				Id = "Raven/IndexReplication/Questions/TitleAndVoteCount",
				ColumnsMapping =
					{
						{"Title", "Title"},
						{"UpVotes", "UpVotes"},
						{"DownVotes", "DownVotes"},
					},
				ConnectionStringName = "Reports",
				PrimaryKeyColumnName = "Id",
				TableName = "QuestionSummaries"
			};

			#endregion
		}


		public void VersioningBundle()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region versioning1

					session.Store(new
					{
						Exclude = false,
						Id = "Raven/Versioning/DefaultConfiguration",
						MaxRevisions = 5
					});

					#endregion

					#region versioning2

					session.Store(new
					{
						Exclude = true,
						Id = "Raven/Versioning/Users",
					});

					#endregion
				}

				#region versioning3

				using (var session = store.OpenSession())
				{
					session.Store(new User { Name = "Ayende Rahien" });
					session.SaveChanges();
				}

				#endregion
			}
		}

		public void ForDocumenting()
		{
			var userSession = new User();
			var parent = new User();
			using (var documentStore = new DocumentStore())
			{
				#region expiration1

				var expiry = DateTime.UtcNow.AddMinutes(5);
				using (var session = documentStore.OpenSession())
				{
					session.Store(userSession);
					session.Advanced.GetMetadataFor(userSession)["Raven-Expiration-Date"] = new RavenJValue(expiry);
					session.SaveChanges();
				}

				#endregion

				#region cascadedelete1
				using (var session = documentStore.OpenSession())
				{
					session.Store(parent);
					session.Advanced.GetMetadataFor(parent)["Raven-Cascade-Delete-Documents"] = RavenJArray.FromObject(new[] { "childId1", "childId2" });
					session.Advanced.GetMetadataFor(parent)["Raven-Cascade-Delete-Attachments"] = RavenJArray.FromObject(new[] { "attachmentId1", " attachmentId2" });
					session.SaveChanges();
				}
				#endregion

				#region replicationconflicts1
				using (var session = documentStore.OpenSession())
				{
					try
					{
						var user = session.Load<User>("users/ayende");
						Console.WriteLine(user.Name);
					}
					catch (ConflictException e)
					{
						Console.WriteLine("Choose which document you want to preserver:");
						var list = new List<JsonDocument>();
						for (int i = 0; i < e.ConflictedVersionIds.Length; i++)
						{
							var doc = documentStore.DatabaseCommands.Get(e.ConflictedVersionIds[i]);
							list.Add(doc);
							Console.WriteLine("{0}. {1}", i, doc.DataAsJson.ToString(Formatting.None));
						}
						var select = int.Parse(Console.ReadLine());
						var resolved = list[select];
						documentStore.DatabaseCommands.Put("users/ayende", null, resolved.DataAsJson, resolved.Metadata);
					}
				}
				#endregion
				using (var session = documentStore.OpenSession())
				{
					#region authorization1

					// Allow nurses to schedule appointment for patients
					session.Store(new AuthorizationRole
					{
						Id = "Authorization/Roles/Nurses",
						Permissions =
							{
								new OperationPermission
								{
									Allow = true,
									Operation = "Appointment/Schedule",
									Tags = new List<string>{"Patient"}
								}
							}
					});

					// Allow doctors to authorize hospitalizations
					session.Store(new AuthorizationRole
					{
						Id = "Authorization/Roles/Doctors",
						Permissions =
							{
								new OperationPermission
								{
									Allow = true,
									Operation = "Hospitalization/Authorize",
									Tags = new List<string>{"Patient"}
								}
							}
					});
					#endregion

					#region authorization2
					// Associate Patient with clinic
					session.SetAuthorizationFor(session.Load<Patient>("Patients/MaryMallon"), new DocumentAuthorization
					{
						Tags = {"Clinics/Kirya", "Patient"}
					});
					
					// Associate Doctor with clinic
					session.Store(new AuthorizationUser
					{
						Id = "Authorization/Users/DrHowser",
						Name = "Doogie Howser",
						Roles = {"Authorization/Roles/Doctors"},
						Permissions =
							{
								new OperationPermission
								{
									Allow = true,
									Operation = "Patient/View",
									Tags = new List<string>{"Clinics/Kirya"}
								},
							}
					});
					#endregion

					#region authorization3
					session.SecureFor("Authorization/Users/DrHowser", "Hospitalization/Authorize");
					var mary = session.Load<Patient>("Patients/MaryMallon");
					mary.AuthorizeHospitalization();
					session.SaveChanges();
					#endregion
				}
			}
		}
	}

	#region MoreLikeThis1
	public class Article
	{
		public string Name { get; set; }
		public string ArticleBody { get; set; }
	}

	public class ArticleIndex : AbstractIndexCreationTask<Article>
	{
		public ArticleIndex()
		{
			Map = docs => from doc in docs
						  select new { doc.ArticleBody };



			Stores = new Dictionary<Expression<Func<Article, object>>, FieldStorage>
							 {
								 {
									 x => x.ArticleBody, FieldStorage.Yes
								 }
							 };

		}
	}
	#endregion
}
