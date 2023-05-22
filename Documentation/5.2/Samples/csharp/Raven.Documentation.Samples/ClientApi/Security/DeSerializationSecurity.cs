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

    public class DeSerializationSecurity
    {
        public DeSerializationSecurity()
        {
        }

        void DeSerializationSecurity_RegisterForbiddenNamespace()
        {
            var binder = new DefaultRavenSerializationBinder();
            binder.RegisterForbiddenNamespace("MyNamespace");

            var store = new DocumentStore()
            {
                #region RegisterForbiddenNamespace
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

        void DeSerializationSecurity_RegisterForbiddenType()
        {
            var store = new DocumentStore()
            {
                #region RegisterForbiddenType
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

        void DeSerializationSecurity_RegisterSafeType()
        {
            var store = new DocumentStore()
            {
                #region RegisterSafeType
                Conventions =
                {
                    PreserveDocumentPropertiesNotFoundOnModel = true
                }
                #endregion
            };
        }
    }
}
