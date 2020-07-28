namespace Raven.Documentation.Samples.FileSystem.ClientApi
{
	using System;
	using System.Threading.Tasks;
	using Abstractions.FileSystem.Notifications;
	using Client.Changes;
	using Client.FileSystem;

	public class Changes
	{
		private interface IFoo
		{
			#region for_folder_1
			IObservableWithTask<FileChangeNotification> ForFolder(string folder);
			#endregion

			#region for_synchronization_1
			IObservableWithTask<SynchronizationUpdateNotification> ForSynchronization();
			#endregion

			#region for_conflicts_1
			IObservableWithTask<ConflictNotification> ForConflicts();
			#endregion

			#region for_configuration_1
			IObservableWithTask<ConfigurationChangeNotification> ForConfiguration();
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			{
				#region for_folder_2

				IDisposable subscription = store
					.Changes()
					.ForFolder("/documents/books")
					.Subscribe(change =>
					{
						switch (change.Action)
						{
							case FileChangeAction.Add:
								// do something
								break;
							case FileChangeAction.Delete:
								// do something
								break;
						}
					});

				#endregion
			}

			{
				#region for_synchronization_2
				IDisposable subscription = store
					.Changes()
					.ForSynchronization()
					.Subscribe(notification =>
					{
						if(notification.Direction == SynchronizationDirection.Outgoing)
							return;

						switch (notification.Action)
						{
							case SynchronizationAction.Start:
								// do something
								break;
							case SynchronizationAction.Finish:
								// do something
								break;
						}
					});
				#endregion
			}

			{
				#region for_conflicts_2
				IDisposable subscription = store
					.Changes()
					.ForConflicts()
					.Subscribe(conflict =>
					{
						switch (conflict.Status)
						{
							case ConflictStatus.Detected:
								Console.WriteLine("New conflict! File name: {0}", conflict.FileName);
								break;
							case ConflictStatus.Resolved:
								Console.WriteLine("Conflict resolved! File name: {0}", conflict.FileName);
								break;
						}
					});
				#endregion
			}

			{
				#region for_configuration_2
				IDisposable subscription = store
					.Changes()
					.ForConfiguration()
					.Subscribe(change =>
					{
						switch (change.Action)
						{
							case ConfigurationChangeAction.Set:
								// do something
								break;
							case ConfigurationChangeAction.Delete:
								// do something
								break;
						}
					});
				#endregion
			}
		}
	}
}
