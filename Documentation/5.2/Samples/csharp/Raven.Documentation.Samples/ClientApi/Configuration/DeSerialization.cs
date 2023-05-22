using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Json.Serialization;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class DeSerialization
    {
        public DeSerialization()
        {
        }

        void Deserialization_CustomizeJsonDeserializer()
        {
            var store = new DocumentStore()
            {
                #region customize_json_deserializer
                Conventions =
                {
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        CustomizeJsonDeserializer = serializer => throw new CodeOmitted()
                    }
                }
                #endregion
            };
        }

        void Deserialization_DeserializeEntityFromBlittable()
        {
            var store = new DocumentStore()
            {
                #region DeserializeEntityFromBlittable
                Conventions =
                {
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        DeserializeEntityFromBlittable = (type, blittable) => throw new CodeOmitted()
                    }
                }
                #endregion
            };
        }

        void Deserialization_PreserveDocumentPropertiesNotFoundOnModel()
        {
            var store = new DocumentStore()
            {
                #region preserve_doc_props_not_found_on_model
                Conventions =
                {
                    PreserveDocumentPropertiesNotFoundOnModel = true
                }
                #endregion
            };
        }

        void Deserialization_MaxNumberOfRequestsPerSession()
        {
            var store = new DocumentStore()
            {
                #region MaxNumberOfRequestsPerSession
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10
                }
                #endregion
            };
        }

        void Deserialization_UseOptimisticConcurrency()
        {
            var store = new DocumentStore()
            {
                #region UseOptimisticConcurrency
                Conventions =
                {
                    UseOptimisticConcurrency = true
                }
                #endregion
            };
        }
    }
}
