﻿using System;
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
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region customize_json_serializer
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        CustomizeJsonSerializer = serializer => throw new CodeOmitted()
                    }
                    #endregion
                    /*
                    #region customize_json_deserializer
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        CustomizeJsonDeserializer = serializer => throw new CodeOmitted()
                    }
                    #endregion

                    #region DeserializeEntityFromBlittable
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        DeserializeEntityFromBlittable = (type, blittable) => throw new CodeOmitted()
                    }
                    #endregion

                    #region json_contract_resolver
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        JsonContractResolver = new CustomJsonContractResolver()
                    }
                    #endregion */
                    ,
                    #region preserve_doc_props_not_found_on_model
                    PreserveDocumentPropertiesNotFoundOnModel = true
                    #endregion
                    ,
                    #region MaxNumberOfRequestsPerSession
                    MaxNumberOfRequestsPerSession = 10
                    #endregion
                    ,
                    #region UseOptimisticConcurrency
                    UseOptimisticConcurrency = true
                    #endregion
                    ,
                    #region TrySerializeEntityToJsonStream
                    BulkInsert =
                    {
                        TrySerializeEntityToJsonStream = (entity, metadata, writer) => throw new CodeOmitted(),
                    }
                    #endregion
                }
            };
        }
    }
    class foofoo
    {
        #region custom_json_contract_resolver
        public class CustomJsonContractResolver : IContractResolver
        {
            public JsonContract ResolveContract(Type type)
            {
                throw new CodeOmitted();
            }
        }
        #endregion

        #region custom_json_contract_resolver_based_on_default
        public class CustomizedRavenJsonContractResolver : DefaultRavenContractResolver
        {
            public CustomizedRavenJsonContractResolver(ISerializationConventions conventions) : base(conventions)
            {
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                throw new CodeOmitted();
            }
        }
        #endregion
    }
}
