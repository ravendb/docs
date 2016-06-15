package net.ravendb.clientapi.commands.documents.howto;

import net.ravendb.abstractions.data.BulkOperationOptions;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.PatchCommandType;
import net.ravendb.abstractions.data.PatchRequest;
import net.ravendb.abstractions.data.ScriptedPatchRequest;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.Operation;
import net.ravendb.client.document.DocumentStore;


public class DeleteOrUpdateByIndex {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region delete_by_index_1
    public Operation deleteByIndex(String indexName, IndexQuery queryToDelete);

    public Operation deleteByIndex(String indexName, IndexQuery queryToDelete, BulkOperationOptions options);
    //endregion

    //region update_by_index_1
    public Operation updateByIndex(String indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests);

    public Operation updateByIndex(String indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests, BulkOperationOptions options);
    //endregion

    //region update_by_index_3
    public Operation updateByIndex(String indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch);

    public Operation updateByIndex(String indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch, BulkOperationOptions options);
    //endregion
  }

  public DeleteOrUpdateByIndex() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region delete_by_index_2
      // remove all documents from 'Employees' collection
      Operation operation = store.getDatabaseCommands()
        .deleteByIndex("Raven/DocumentsByEntityName", new IndexQuery("Tag:Employees"));

      operation.waitForCompletion();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region update_by_index_2
      // Set property 'FirstName' for all documents in collection 'Employees' to 'Patched Name'
      PatchRequest patchRequest = new PatchRequest(PatchCommandType.SET, "FirstName", RavenJToken.fromObject("Patched Name"));

      Operation operation = store.getDatabaseCommands().updateByIndex(
        "Raven/DocumentsByEntityName",
        new IndexQuery("Tag:Employees"),
        new PatchRequest[] { patchRequest });

      operation.waitForCompletion();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region update_by_index_4
      // Set property 'FirstName' for all documents in collection 'Employees' to 'Patched Name'
      Operation operation = store.getDatabaseCommands().updateByIndex(
        "Raven/DocumentsByEntityName",
        new IndexQuery("Tag:Employees"),
        new ScriptedPatchRequest("this.FirstName = 'Patched Name';"));

      operation.waitForCompletion();
      //endregion

    }
  }
}
