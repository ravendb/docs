namespace RavenCodeSamples.ClientApi
{
	using System;

	public class ChangesApi : CodeSampleBase
	{
		public ChangesApi()
		{
/*
			using (var store = NewDocumentStore())
			{

				#region getting_database_changes_instance

				IDatabaseChanges changes = store.Changes("DatabaseName");

				#endregion

				#region subscribe_documents_replication_conflict
				store.Changes()
					.ForAllDocuments()
					 .Subscribe(change =>
					 {
						 if (change.Type == DocumentChangeTypes.ReplicationConflict)
						 {
							 Console.WriteLine("Replication conflict has occurred. Document id: " + change.Id);
						 }
					 });
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
					.ForIndex("Users/ByName")
					 .Subscribe(change =>
					 {
						 if (change.Type == IndexChangeTypes.IndexRemoved)
						 {
							 Console.WriteLine("Index" + change.Name + " has been removed.");
						 }
					 });
				#endregion
			}
*/
		}
	}
}
