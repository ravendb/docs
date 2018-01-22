using System;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Sparrow;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class RequestHandling
    {
        public RequestHandling()
        {
            var store = new DocumentStore();

            var Conventions = store.Conventions;

            #region MaxHttpCacheSize
            Conventions.MaxHttpCacheSize = new Size(256,SizeUnit.Megabytes);
            #endregion

            #region customize_json_serializer
            Conventions.CustomizeJsonSerializer = serializer => { };
            #endregion

            #region json_contract_resolver
            Conventions.JsonContractResolver = new CustomJsonContractResolver();
            #endregion

            #region preserve_doc_props_not_found_on_model
            Conventions.PreserveDocumentPropertiesNotFoundOnModel = true;
            #endregion

            #region MaxNumberOfRequestsPerSession
            Conventions.MaxNumberOfRequestsPerSession = 10;
            #endregion

            #region UseOptimisticConcurrency
            Conventions.UseOptimisticConcurrency = true;
            #endregion
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
}
