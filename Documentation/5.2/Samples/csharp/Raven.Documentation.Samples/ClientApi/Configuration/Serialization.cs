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

    public class Serialization
    {
        public Serialization()
        {
        }

        void Serialization_CustomizeJsonSerializer()
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
                }
            };
        }

        void Serialization_JsonContractResolver()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region json_contract_resolver
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        JsonContractResolver = new CustomJsonContractResolver()
                    }
                    #endregion 
                }
            };
        }

        void Serialization_MaxNumberOfRequestsPerSession()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region MaxNumberOfRequestsPerSession
                    MaxNumberOfRequestsPerSession = 10
                    #endregion
                }
            };
        }

        void Serialization_UseOptimisticConcurrency()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region UseOptimisticConcurrency
                    UseOptimisticConcurrency = true
                    #endregion
                }
            };
        }

        void Serialization_TrySerializeEntityToJsonStream()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
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

    #region custom_json_contract_resolver
    public class CustomJsonContractResolver : IContractResolver
    {
        public JsonContract ResolveContract(Type type)
        {
            throw new CodeOmitted();
        }
    }
    #endregion

    class foo
    {
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
