using Raven.Abstractions.Data;
using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class StartBackupRestoreOperations
	{
		private interface IFoo
		{
			#region backup_restore_1
			void StartBackup(
				string backupLocation,
				DatabaseDocument databaseDocument,
				bool incremental,
				string databaseName);
			#endregion

			#region backup_restore_2
            Operation StartRestore(DatabaseRestoreRequest restoreRequest);
			#endregion
		}

		public StartBackupRestoreOperations()
		{
			using (var store = new DocumentStore())
			{
				#region backup_restore_3
				store
					.DatabaseCommands
					.GlobalAdmin
					.StartBackup(
						@"C:\temp\backup\Northwind\",
						new DatabaseDocument(),
						incremental: false,
						databaseName: "Northwind");
				#endregion

				#region backup_restore_4
				store
					.DatabaseCommands
					.GlobalAdmin
					.StartRestore(
						new DatabaseRestoreRequest
							{
								BackupLocation = @"C:\temp\backup\Northwind\",
								DatabaseLocation = @"~\Databases\NewNorthwind\",
								DatabaseName = "NewNorthwind"
							});
				#endregion
			}
		}
	}
}