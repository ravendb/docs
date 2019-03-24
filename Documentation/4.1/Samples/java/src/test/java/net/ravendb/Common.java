package net.ravendb;

import net.ravendb.client.documents.operations.Operation;
import net.ravendb.client.documents.operations.PatchByQueryOperation;
import net.ravendb.client.documents.queries.IndexQuery;
import net.ravendb.client.documents.queries.QueryOperationOptions;

public class Common {
    public interface OperationSend
    {
        //region sendingSetBasedPatchRequest
        Operation sendAsync(PatchByQueryOperation operation);
        //endregion
    }

    public static class PatchByQueryOperation {
        //region patchBeQueryOperationCtor1
        public PatchByQueryOperation(String queryToUpdate)
        //endregion
        {
        }

        /*
        //region patchBeQueryOperationCtor2
        public PatchByQueryOperation(IndexQuery queryToUpdate);

        public PatchByQueryOperation(IndexQuery queryToUpdate, QueryOperationOptions options);
        //endregion
        */
    }
}
