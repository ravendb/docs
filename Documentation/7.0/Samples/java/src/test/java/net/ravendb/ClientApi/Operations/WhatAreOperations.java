package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.attachments.AttachmentType;
import net.ravendb.client.documents.operations.CompactDatabaseOperation;
import net.ravendb.client.documents.operations.DeleteByQueryOperation;
import net.ravendb.client.documents.operations.Operation;
import net.ravendb.client.documents.operations.attachments.CloseableAttachmentResult;
import net.ravendb.client.documents.operations.attachments.GetAttachmentOperation;
import net.ravendb.client.documents.operations.configuration.GetClientConfigurationOperation;
import net.ravendb.client.documents.operations.indexes.StopIndexOperation;
import net.ravendb.client.documents.queries.IndexQuery;
import net.ravendb.client.serverwide.CompactSettings;

public class WhatAreOperations {

    private interface IFoo<TResult, TEntity> {
        /*
        //region Client_Operations_api
        public void send(IVoidOperation operation)

        public void send(IVoidOperation operation, SessionInfo sessionInfo)

        public <TResult> TResult send(IOperation<TResult> operation)

        public <TResult> TResult send(IOperation<TResult> operation, SessionInfo sessionInfo)

        public PatchStatus send(PatchOperation operation, SessionInfo sessionInfo)

        public <TEntity> PatchOperation.Result<TEntity> send(Class<TEntity> entityClass, PatchOperation operation, SessionInfo sessionInfo)
        //endregion

        //region Client_Operations_api_async
        public Operation sendAsync(IOperation<OperationIdResult> operation)

        public Operation sendAsync(IOperation<OperationIdResult> operation, SessionInfo sessionInfo)
        //endregion

        //region Maintenance_Operations_api
        public void send(IVoidMaintenanceOperation operation)

        public <TResult> TResult send(IMaintenanceOperation<TResult> operation)
        //endregion

        //region Maintenance_Operations_api_async
        public Operation sendAsync(IMaintenanceOperation<OperationIdResult> operation)
        //endregion

        //region Server_Operations_api
        public void send(IVoidServerOperation operation)

        public <TResult> TResult send(IServerOperation<TResult> operation)
        //endregion

        //region Server_Operations_api_async
        public Operation sendAsync(IServerOperation<OperationIdResult> operation)
        //endregion
        */
    }

    public WhatAreOperations() {
        try (IDocumentStore store = new DocumentStore()) {
            //region Client_Operations_1
            try (CloseableAttachmentResult fetchedAttachment = store
                .operations()
                .send(new GetAttachmentOperation("users/1", "file.txt", AttachmentType.DOCUMENT, null))) {
                // do stuff with the attachment stream --> fetchedAttachment.data
            }
            //endregion

            {
                //region Client_Operations_1_async
                IndexQuery indexQuery = new IndexQuery();
                indexQuery.setQuery("from users where Age == 5");
                DeleteByQueryOperation operation = new DeleteByQueryOperation(indexQuery);
                Operation asyncOp = store.operations().sendAsync(operation);
                //endregion
            }

            //region Maintenance_Operations_1
            store.maintenance().send(new StopIndexOperation("Orders/ByCompany"));
            //endregion

            //region Server_Operations_1
            GetClientConfigurationOperation.Result result
                = store.maintenance().send(new GetClientConfigurationOperation());
            //endregion

            {
                //region Server_Operations_1_async
                CompactSettings settings = new CompactSettings();
                settings.setDocuments(true);
                settings.setDatabaseName("Northwind");
                Operation operation = store.maintenance().server().sendAsync(new CompactDatabaseOperation(settings));
                operation.waitForCompletion();
                //endregion
            }
        }
    }
}
