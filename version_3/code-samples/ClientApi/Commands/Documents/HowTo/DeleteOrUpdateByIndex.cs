namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Documents.HowTo
{
	using Raven.Abstractions.Data;
	using Raven.Client.Connection;
	using Raven.Client.Document;

	public class DeleteOrUpdateByIndex
	{
		private interface IFoo
		{
			#region delete_by_index_1
			Operation DeleteByIndex(string indexName, IndexQuery queryToDelete, bool allowStale = false);
			#endregion

			#region update_by_index_1
			Operation UpdateByIndex(
				string indexName,
				IndexQuery queryToUpdate,
				PatchRequest[] patchRequests,
				bool allowStale = false);
			#endregion

			#region update_by_index_3
			Operation UpdateByIndex(
				string indexName,
				IndexQuery queryToUpdate,
				ScriptedPatchRequest patch,
				bool allowStale = false);
			#endregion
		}

		public DeleteOrUpdateByIndex()
		{
			using (var store = new DocumentStore())
			{
				#region delete_by_index_2
				// remove all documents from 'People' collection
				var operation = store
					.DatabaseCommands
					.DeleteByIndex(
						"Raven/DocumentsByEntityName",
						new IndexQuery
							{
								Query = "Tag:People"
							},
						allowStale: false);

				operation.WaitForCompletion();
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region update_by_index_4
				// Set property 'Name' for all documents in collection 'People' to 'Patched Name'
				var operation = store
					.DatabaseCommands
					.UpdateByIndex(
						"Raven/DocumentsByEntityName",
						new IndexQuery
							{
								Query = "Tag:People"
							},
						new[]
							{
								new PatchRequest
									{
										Type = PatchCommandType.Set, 
										Name = "Name", 
										Value = "Patched Name"
									}
							},
						allowStale: false);

				operation.WaitForCompletion();
				#endregion
			}
		}
	}
}