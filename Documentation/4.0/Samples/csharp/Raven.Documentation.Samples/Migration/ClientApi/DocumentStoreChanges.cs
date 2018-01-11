using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Http;
using Sparrow;
using Sparrow.Json;

namespace Raven.Documentation.Samples.Migration.ClientApi
{
    public class DocumentStoreChanges
    {
        public void Changes()
        {
            var store = new DocumentStore().Initialize();

            #region events_1
            store.OnBeforeStore += (s, e) => { };
            store.OnAfterSaveChanges += (s, e) => { };
            store.OnBeforeDelete += (s, e) => { };
            store.OnBeforeQueryExecuted += (s, e) => { };
            #endregion

            #region urls_1
            new DocumentStore
            {
                Urls = new []
                {
                    "http://ravendb-1:8080",
                    "http://ravendb-2:8080",
                    "http://ravendb-3:8080"
                }
            }.Initialize();
            #endregion

            #region serialization_1
            new DocumentStore
            {
                Conventions =
                {
                    CustomizeJsonSerializer = serializer => throw new CodeOmitted(),
                    DeserializeEntityFromBlittable = (type, blittable) => throw new CodeOmitted()
                }
            }.Initialize();
            #endregion

            #region serialization_2
            new DocumentStore
            {
                Conventions =
                {
                    BulkInsert =
                    {
                        TrySerializeEntityToJsonStream = (o, writer) => throw new CodeOmitted(),
                        TrySerializeMetadataToJsonStream = (o, writer) => throw new CodeOmitted()
                    }
                }
            }.Initialize();
            #endregion

            #region request_executor_1
            RequestExecutor requestExecutor = store.GetRequestExecutor();

            using (requestExecutor.ContextPool.AllocateOperationContext(out JsonOperationContext context))
            {
                var command = new GetDocumentsCommand(start: 0, pageSize: 10);
                requestExecutor.Execute(command, context);

                GetDocumentsResult result = command.Result;
            }
            #endregion

            #region request_executor_2
            int numberOfCachedItems = requestExecutor.Cache.NumberOfItems;

            requestExecutor.Cache.Clear();
            #endregion

            #region request_executor_3
            var numberOfSentRequests = requestExecutor.NumberOfServerRequests;
            #endregion

            #region request_executor_4
            requestExecutor.DefaultTimeout = TimeSpan.FromSeconds(180);
            #endregion

            #region request_executor_5
            using (store.SetRequestsTimeout(TimeSpan.FromMilliseconds(180)))
            {
            }
            #endregion
        }
    }
}
