using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
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
			void StartRestore(RestoreRequest restoreRequest);
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
						@"C:\temp\backup\DB1\",
						new DatabaseDocument(),
						incremental: false,
						databaseName: "DB1");
				#endregion

				#region backup_restore_4
				store
					.DatabaseCommands
					.GlobalAdmin
					.StartRestore(
						new RestoreRequest
							{
								BackupLocation = @"C:\temp\backup\DB1\",
								DatabaseLocation = @"~\Databases\DB2",
								DatabaseName = "DB2"
							});
				#endregion
			}
		}
	}
}