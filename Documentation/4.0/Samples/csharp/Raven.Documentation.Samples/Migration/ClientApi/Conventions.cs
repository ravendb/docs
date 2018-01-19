using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Http;
using Sparrow;
using Sparrow.Json;

namespace Raven.Documentation.Samples.Migration.ClientApi
{
    public class Conventions
    {
        public void Changes()
        {
            var store = new DocumentStore().Initialize();

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
                        TrySerializeEntityToJsonStream = (entity, metadata, writer) => throw new CodeOmitted(),
                    }
                }
            }.Initialize();
            #endregion
        }
    }
}
