using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
{
	public class HowToSubscribeToReplicationConflictsChanges
	{
		private interface IFoo
		{
			#region replication_conflicts_changes_1
			IObservableWithTask<ReplicationConflictNotification>
				ForAllReplicationConflicts();
			#endregion
		}

		public HowToSubscribeToReplicationConflictsChanges()
		{
			using (var store = new DocumentStore())
			{
				#region replication_conflicts_changes_2
				var subscribtion = store.Changes()
					.ForAllReplicationConflicts()
					.Subscribe(conflict =>
					{
						if (conflict.ItemType == ReplicationConflictTypes.DocumentReplicationConflict)
						{
							Console.WriteLine("Conflict detected for {0}. Ids of conflicted docs: {1}. " +
											  "Type of replication operation: {2}",
											  conflict.Id,
											  string.Join(", ", conflict.Conflicts),
											  conflict.OperationType);
						}
					});
				#endregion
			}
		}
	}
}