namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System;
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Client.Connection;
	using Client.Connection.Async;
	using Client.Document;
	using Client.FileSystem;

	public class Admin
	{
		private interface IFoo
		{
			#region create_fs_1
			Task CreateFileSystemAsync(FileSystemDocument filesystemDocument);
			#endregion

			#region create_or_update_fs_1
			Task CreateOrUpdateFileSystemAsync(FileSystemDocument filesystemDocument, string newFileSystemName = null);
			#endregion

			#region ensure_fs_exists_1
			Task EnsureFileSystemExistsAsync(string fileSystem);
			#endregion

			#region delete_fs_1
			Task DeleteFileSystemAsync(string fileSystemName = null, bool hardDelete = false);
			#endregion

			#region start_backup_1
			Task StartBackup(string backupLocation, FileSystemDocument fileSystemDocument, bool incremental, string fileSystemName);
			#endregion

			#region start_restore_1
			Task<long> StartRestore(FilesystemRestoreRequest restoreRequest);
			#endregion

			#region start_compact_1
			Task<long> StartCompact(string filesystemName);
			#endregion

			#region reset_indexes_1
			Task ResetIndexes(string filesystemName);
			#endregion

			#region get_names_1
			Task<string[]> GetNamesAsync();
			#endregion

			#region get_stats_1
			Task<FileSystemStats[]> GetStatisticsAsync();
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region create_fs_2

			await store.AsyncFilesCommands.Admin
				.CreateFileSystemAsync(new FileSystemDocument
				{
					Id = "Raven/FileSystem/NorthwindFS",
					Settings =
					{
						{ Constants.FileSystem.DataDirectory, "~/FileSystems/NorthwindFS" },
                        { Constants.ActiveBundles, "Versioning" }
                    }
                });

            #endregion

            #region create_fs_3

            await store.AsyncFilesCommands.Admin
                .CreateFileSystemAsync(MultiDatabase.CreateFileSystemDocument("NorthwindFS"));

            #endregion

            #region ensure_fs_exists_2

            await store.AsyncFilesCommands.Admin
					.EnsureFileSystemExistsAsync("NorthwindFS");

			#endregion

			#region delete_fs_2

			await store.AsyncFilesCommands.Admin
					.DeleteFileSystemAsync(hardDelete: true);

			#endregion

			#region start_backup_2
			await store.AsyncFilesCommands.Admin
					.StartBackup(@"C:\backups\NorthwindFS", null, false, "NorthwindFS");
			#endregion

			#region start_backup_3
			BackupStatus status = await store.AsyncFilesCommands.Configuration
				.GetKeyAsync<BackupStatus>(BackupStatus.RavenBackupStatusDocumentKey);
			
			if (status.IsRunning)
			{
				// ... //
			}
			#endregion

			#region start_restore_2
			long restoreOperationId = await store.AsyncFilesCommands.Admin
												.StartRestore(new FilesystemRestoreRequest()
												{
													BackupLocation = @"C:\backups\NorthwindFS",
													FilesystemLocation = @"~\FileSystems\NewNorthwindFS",
													FilesystemName = "NewNorthwindFS"
												});
			#endregion

			#region start_restore_3
			using (var sysDbStore = new DocumentStore
			{
				Url = "http://localhost:8080/"
			}.Initialize())
			{
				await new Operation((AsyncServerClient)sysDbStore.AsyncDatabaseCommands, restoreOperationId)
							.WaitForCompletionAsync();
			}
			#endregion

			#region start_compact_2
			long compactOperationId = await store.AsyncFilesCommands.Admin
												.StartCompact("NorthwindFS");
			#endregion

			#region start_compact_3
			using (var sysDbStore = new DocumentStore
			{
				Url = "http://localhost:8080/"
			}.Initialize())
			{
				await new Operation((AsyncServerClient)sysDbStore.AsyncDatabaseCommands, compactOperationId)
							.WaitForCompletionAsync();
			}
			#endregion

			#region reset_indexes_2

			await store.AsyncFilesCommands.Admin
				.ResetIndexes("NorthwindFS");
			#endregion

			#region get_names_2
			string[] fsNames = await store.AsyncFilesCommands.Admin
				.GetNamesAsync();
			#endregion

			#region get_stats_2
			FileSystemStats[] fsStats = await store.AsyncFilesCommands.Admin
												.GetStatisticsAsync();
			#endregion
		}
	}
}