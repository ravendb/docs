namespace RavenCodeSamples.ClientApi
{
	using System;
	using Raven.Abstractions.Data;
	using Raven.Client.Changes;
	using Raven.Client.Document;

	public class ChangesApi : CodeSampleBase
	{
		public ChangesApi()
		{
			using (var store = NewDocumentStore())
			{
				#region getting_database_changes_instance
				IDatabaseChanges changes = store.Changes("DatabaseName");
				#endregion

				#region subscribe_for_all_documents
				store.Changes()
				     .ForAllDocuments()
				     .Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion

				#region subscribe_documents_starting_with
				store.Changes()
					.ForDocumentsStartingWith("users")
					 .Subscribe(change =>
					 {
						 if (change.Type == DocumentChangeTypes.Put)
						 {
							 Console.WriteLine("New user has been added. Its ID is " + change.Id + ", document ETag " + change.Etag);
						 }
					 });
				#endregion

				#region subscribe_document_delete
				store.Changes()
					.ForDocument("users/1")
					 .Subscribe(change =>
					 {
						 if (change.Type == DocumentChangeTypes.Delete)
						 {
							 Console.WriteLine("User " + change.Id + " has been deleted.");
						 }
					 });
				#endregion

				#region subscribe_indexes
				store.Changes()
					.ForAllIndexes()
					 .Subscribe(change =>
					 {
						 if (change.Type == IndexChangeTypes.IndexAdded)
						 {
							 Console.WriteLine("Index " + change.Name + " has been added.");
						 }
					 });
				#endregion

				#region subscribe_index_reduce_completed
				store.Changes()
					.ForIndex("Orders/Total")
					 .Subscribe(change =>
					 {
						 if (change.Type == IndexChangeTypes.ReduceCompleted)
						 {
							 Console.WriteLine("Index 'Orders/Total' has finished the reduce work.");
						 }
					 });
				#endregion

				#region subscribe_documents_replication_conflict
				store.Changes()
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

				#region subscribe_bulk_insert
				using (var bulkInsert = store.BulkInsert())
				{
					store.Changes()
						.ForBulkInsert(bulkInsert.OperationId)
						.Subscribe(change =>
						{
							switch (change.Type)
							{
								case DocumentChangeTypes.BulkInsertStarted:
									// do something
									break;
								case DocumentChangeTypes.BulkInsertEnded:
									// do something
									break;
								case DocumentChangeTypes.BulkInsertError:
									// do something
									break;
							}
						});

					// process bulk insert here
				}

				#endregion
			}
		}
	}
}