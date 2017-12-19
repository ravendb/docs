using Raven.Client.Documents;

namespace Raven.Documentation.Samples.Migration.ClientApi
{
    public class DocumentStoreChanges
    {
        public void Changes()
        {
            var store = new DocumentStore().Initialize();

            #region events_1
            store.OnBeforeStore += (s, e) => { };
            store.OnAfterStore += (s, e) => { };
            store.OnBeforeDelete += (s, e) => { };
            store.OnBeforeQueryExecuted += (s, e) => { };
            #endregion

            #region serialization_1
            new DocumentStore
            {
                Conventions =
                {
                    CustomizeJsonSerializer = serializer => { },
                    DeserializeEntityFromBlittable = (type, blittable) => new object()
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
                        TrySerializeEntityToJsonStream = (o, writer) => true,
                        TrySerializeMetadataToJsonStream = (o, writer) => true
                    }
                }
            }.Initialize();
            #endregion
        }
    }
}
