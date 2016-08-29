namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Security.Permissions;
	using System.Threading.Tasks;
	using Abstractions.Extensions;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Synchronization
	{
		private interface IFoo
		{
			#region get_destinations_1
			Task<SynchronizationDestination[]> GetDestinationsAsync();
			#endregion

			#region set_destinations_1
			Task SetDestinationsAsync(params SynchronizationDestination[] destinations);
			#endregion

			#region get_sync_status_1
			Task<SynchronizationReport> GetSynchronizationStatusForAsync(string filename);
			#endregion

			#region get_finished_1
			Task<ItemsPage<SynchronizationReport>> GetFinishedAsync(int start = 0, int pageSize = 25);
			#endregion

			#region get_active_1
			Task<ItemsPage<SynchronizationDetails>> GetActiveAsync(int start = 0, int pageSize = 25);
			#endregion

			#region get_pending_1
			Task<ItemsPage<SynchronizationDetails>> GetPendingAsync(int start = 0, int pageSize = 25);
			#endregion

			#region get_conflicts_1
			Task<ItemsPage<ConflictItem>> GetConflictsAsync(int start = 0, int pageSize = 25);
			#endregion

			#region resolve_conflict_1
			Task ResolveConflictAsync(string filename, ConflictResolutionStrategy strategy);
            #endregion

            #region resolve_conflicts_1
            Task ResolveConflictsAsync(ConflictResolutionStrategy strategy);
            #endregion

            #region start_1
            Task<DestinationSyncResult[]> StartAsync(bool forceSyncingAll = false);
			#endregion

			#region start_2
			Task<SynchronizationReport> StartAsync(string filename, SynchronizationDestination destination);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region get_destinations_2
			SynchronizationDestination[] destinations = await store.AsyncFilesCommands.Synchronization
																.GetDestinationsAsync();
			#endregion

			#region set_destinations_2

			await store.AsyncFilesCommands.Synchronization.SetDestinationsAsync(new SynchronizationDestination
			{
				ServerUrl = "http://localhost:8080/",
				FileSystem = "BackupFS"
			});
			#endregion

			#region get_sync_status_2
			SynchronizationReport report = await store.AsyncFilesCommands.Synchronization
				.GetSynchronizationStatusForAsync("/documentation/readme.txt");

			if (report.Exception == null)
				Console.WriteLine("The file {0} has been synchronized successfully. The synchronization type: {1}",
					report.FileName, report.Type);
			else
				Console.WriteLine("The synchronization of the file {0} failed. The exception message: {1}", 
					report.FileName, report.Exception.Message);

			#endregion

			{
				#region get_finished_2

				ItemsPage<SynchronizationReport> page;
				IList<SynchronizationReport> reports = new List<SynchronizationReport>();

				var start = 0;
				const int pageSize = 10;

				do
				{
					page = await store.AsyncFilesCommands.Synchronization.GetFinishedAsync(start, pageSize);

					reports.AddRange(page.Items);

					Console.WriteLine("Retrieved {0} of {1} reports", reports.Count, page.TotalCount);

				} while (page.Items.Count == pageSize);

				#endregion
			}

			{
				#region get_active_2
				ItemsPage<SynchronizationDetails> page = await store.AsyncFilesCommands.Synchronization
																.GetActiveAsync(0, 128);

				page.Items.ForEach(x => 
					Console.WriteLine("Synchronization of {0} to {1} (type: {2}) is in progress", 
											x.FileName, x.DestinationUrl, x.Type));

				#endregion
			}

			{
				#region get_pending_2
				ItemsPage<SynchronizationDetails> page = await store.AsyncFilesCommands.Synchronization
																.GetPendingAsync(0, 128);

				page.Items.ForEach(x =>
					Console.WriteLine("File {0} waits to be synchronized to {1} (modification type: {2})",
											x.FileName, x.DestinationUrl, x.Type));

				#endregion
			}

			{
				#region get_conflicts_2
				ItemsPage<ConflictItem> page = await store.AsyncFilesCommands.Synchronization
																.GetConflictsAsync(0, 128);

				page.Items.ForEach(x =>
					Console.WriteLine("Synchronization of file {0} from {1} file system resulted in conflict", 
					x.FileName, x.RemoteServerUrl));
				#endregion
			}

			#region resolve_conflict_2

			await store.AsyncFilesCommands.Synchronization
				.ResolveConflictAsync("/documents/file.bin", ConflictResolutionStrategy.CurrentVersion);

            #endregion

            #region resolve_conflicts_2

            await store.AsyncFilesCommands.Synchronization
                .ResolveConflictsAsync(ConflictResolutionStrategy.CurrentVersion);

            #endregion

            #region resolve_conflict_3

            await store.AsyncFilesCommands.Synchronization
                .ResolveConflictAsync("/documents/file.bin", ConflictResolutionStrategy.RemoteVersion);
            #endregion

            #region resolve_conflicts_3

            await store.AsyncFilesCommands.Synchronization
                .ResolveConflictsAsync(ConflictResolutionStrategy.RemoteVersion);
            #endregion

            #region start_4

            DestinationSyncResult[] results = await store.AsyncFilesCommands.Synchronization
														.StartAsync();

			foreach (var destinationSyncResult in results)
			{
				Console.WriteLine("Reports of synchronization to server {0}, fs {1}", 
					destinationSyncResult.DestinationServer, destinationSyncResult.DestinationFileSystem);

				if(destinationSyncResult.Exception != null)
					continue;

				foreach (var fileReport in destinationSyncResult.Reports)
				{
					Console.WriteLine("\tFile {0} synchronization type: {1}", fileReport.FileName, fileReport.Type);
				}
			}

			#endregion

			{
				#region start_5

				SynchronizationReport syncReport = await store.AsyncFilesCommands.Synchronization
															.StartAsync("/products/pictures/canon_1.jpg", 
																		new SynchronizationDestination
																		{
																			FileSystem = "NorthwindFS",
																			ServerUrl = "http://localhost:8081/"
																		});

				#endregion
			}
		}
	}
}