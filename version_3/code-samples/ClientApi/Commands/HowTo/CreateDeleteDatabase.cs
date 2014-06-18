using Raven.Abstractions.Data;
using Raven.Client.Connection;
using Raven.Client.Document;
#region ensure_database_exists_2
using Raven.Client.Extensions; // required namespace in usings
#endregion

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
{
	static class Foo
	{
		#region ensure_database_exists_1
		public static void EnsureDatabaseExists(
			this IGlobalAdminDatabaseCommands self,
			string name,
			bool ignoreFailures = false)
		{
			throw new CodeOmitted();
		}
		#endregion
	}

	public class CreateDeleteDatabase
	{
		private interface IFoo
		{
			#region create_database_1
			void CreateDatabase(DatabaseDocument databaseDocument);
			#endregion

			#region delete_database_1
			void DeleteDatabase(string dbName, bool hardDelete = false);
			#endregion
		}

		public CreateDeleteDatabase()
		{
			using (var store = new DocumentStore())
			{
				#region create_database_2
				// create database 'NewDatabase' with 'PeriodicExport' bundle enabled
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(new DatabaseDocument
						                {
							                Id = "NewDatabase", 
											Settings =
												{
													{ "Raven/ActiveBundles", "PeriodicExport" }
												}
						                });
				#endregion

				#region delete_database_2
				store.DatabaseCommands.GlobalAdmin.DeleteDatabase("NewDatabase", hardDelete: true);
				#endregion

				#region ensure_database_exists_3
				store
					.DatabaseCommands
					.GlobalAdmin
					.EnsureDatabaseExists("NewDatabase", ignoreFailures: false);
				#endregion
			}
		}
	}
}