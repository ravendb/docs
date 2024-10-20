using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.XRay.Recorder.Handlers.System.Net;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Http;
using Raven.Client.Json.Serialization;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using Sparrow;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
    public class Conventions
    {
        public Conventions()
        {
            #region conventions_1
            using (var store = new DocumentStore()
            {
                Conventions =
                {
                    // Set conventions HERE, e.g.:
                    MaxNumberOfRequestsPerSession = 50,
                    AddIdFieldToDynamicObjects = false
                    // ...
                }
            }.Initialize())
            {
                // * Here you can interact with the RavenDB store:
                //   open sessions, create or query for documents, perform operations, etc.
                
                // * Conventions CANNOT be set here after calling Initialize()
            }
            #endregion
        }
        
        #region FirstChar
        private string FirstCharToLower(string str) => $"{Char.ToLower(str[0])}{str.Substring(1)}";
        #endregion

        public void Examples()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region MaxHttpCacheSize
                    MaxHttpCacheSize = new Size(256, SizeUnit.Megabytes) // Set max cache size
                    #endregion
                    ,
                    #region UseOptimisticConcurrency
                    UseOptimisticConcurrency = true
                    #endregion
                    ,
                    #region RequestTimeout
                    RequestTimeout = TimeSpan.FromSeconds(90)
                    #endregion
                    ,
                    #region OperationStatusFetchMode
                    OperationStatusFetchMode = OperationStatusFetchMode.ChangesApi // ChangesApi | Polling
                    #endregion
                    ,
                    #region TopologyCacheLocation
                    TopologyCacheLocation = @"C:\RavenDB\TopologyCache"
                    #endregion
                    ,
                    #region PropertyCasing
                    Serialization = new NewtonsoftJsonSerializationConventions
                    {
                        // .Net properties will be serialized as camelCase in the JSON document when storing an entity
                        // and deserialized back to PascalCase
                        CustomizeJsonSerializer = s => s.ContractResolver = new CamelCasePropertyNamesContractResolver()
                    },
                    
                    // In addition, the following convention is required when
                    // making a query that filters by a field name and when indexing. 
                    PropertyNameConverter = memberInfo => FirstCharToLower(memberInfo.Name)
                    #endregion
                    ,
                    #region WaitForIndexesAfterSaveChangesTimeout
                    WaitForIndexesAfterSaveChangesTimeout = TimeSpan.FromSeconds(10)
                    #endregion
                    ,
                    #region WaitForReplicationAfterSaveChangesTimeout
                    WaitForReplicationAfterSaveChangesTimeout = TimeSpan.FromSeconds(10)
                    #endregion
                    ,
                    #region WaitForNonStaleResultsTimeout
                    WaitForNonStaleResultsTimeout = TimeSpan.FromSeconds(10)
                    #endregion
                    ,
                    #region CreateHttpClient
                    CreateHttpClient = handler =>
                    {
                        // Your HTTP client code here, e.g.:
                        var httpClient = new MyHttpClient(new HttpClientXRayTracingHandler(new HttpClientHandler()));
                        return httpClient;
                    }
                    #endregion
                    ,
                    #region HttpClientType
                    // The type of HTTP client you are using
                    HttpClientType = typeof(MyHttpClient)
                    #endregion
                    ,
                    #region FirstBroadcastAttemptTimeout
                    FirstBroadcastAttemptTimeout = TimeSpan.FromSeconds(10)
                    #endregion
                    ,
                    #region SecondBroadcastAttemptTimeout
                    SecondBroadcastAttemptTimeout = TimeSpan.FromSeconds(20)
                    #endregion
                    ,
                    #region FindIdentityProperty
                    // If there exists a property with name "CustomizedId" then it will be the entity's ID property 
                    FindIdentityProperty = memberInfo => memberInfo.Name == "CustomizedId"
                    #endregion
                    ,
                    #region FindIdentityPropertyNameFromCollectionName
                    // Will use property "CustomizedId" as the ID property
                    FindIdentityPropertyNameFromCollectionName = collectionName => "CustomizedId"
                    #endregion
                    ,
                    #region FindClrType
                    // The default implementation is:
                    FindClrType = (_, doc) =>
                    {
                        if (doc.TryGet(Constants.Documents.Metadata.Key, out BlittableJsonReaderObject metadata) &&
                            metadata.TryGet(Constants.Documents.Metadata.RavenClrType, out string clrType))
                            return clrType;

                        return null;
                    }
                    #endregion
                    ,
                    #region FindCollectionName
                    // Here the collection name will be the type name separated by dashes
                    FindCollectionName = type => String.Join("-", type.Name.ToCharArray())
                    #endregion
                    ,
                    #region FindCollectionNameForDynamic
                    // Here the collection name will be some property of the dynamic entity
                    FindCollectionNameForDynamic = dynamicEntity => dynamicEntity.SomeProperty
                    #endregion
                    ,
                    #region FindClrTypeNameForDynamic
                    // The dynamic entity's type is returned by default
                    FindClrTypeNameForDynamic = dynamicEntity => dynamicEntity.GetType()
                    #endregion
                    ,
                    #region ResolveTypeFromClrTypeName
                    // The type itself is returned by default
                    ResolveTypeFromClrTypeName = clrType => clrType.GetType()
                    #endregion
                    ,
                    #region FindPropertyNameForIndex
                    // The default function:
                    FindPropertyNameForIndex = (Type indexedType, string indexedName, string path, string prop) => 
                        (path + prop).Replace("[].", "_").Replace(".", "_")
                    #endregion
                    ,
                    #region FindPropertyNameForDynamicIndex
                    // The DEFAULT function:
                    FindPropertyNameForDynamicIndex = (Type indexedType, string indexedName, string path, string prop) =>
                        path + prop
                    #endregion
                    ,
                    #region AsyncDocumentIdGenerator
                    // Customize ID generation for all collections
                    AsyncDocumentIdGenerator = (database, obj) =>
                    {
                        var objectType = obj.GetType().Name;  // e.g., Person, Order, etc.
                        var timestamp = DateTime.UtcNow.Ticks; // Get the current timestamp

                        // Format the ID as {ObjectType}/{Ticks}
                        var id = $"{objectType}/{timestamp}";

                        return Task.FromResult(id);
                    }
                    #endregion
                }
            };
            
            var store2 = new DocumentStore()
            {
                Conventions =
                {
                    #region disable_cache
                    MaxHttpCacheSize = new Size(0, SizeUnit.Megabytes) // Disable caching
                    #endregion
                }
            };
        }
        
        #region AddIdFieldToDynamicObjectsSyntax
        // Syntax:
        public bool AddIdFieldToDynamicObjects { get; set; }
        #endregion
        
        #region AggressiveCacheDurationSyntax
        // Syntax:
        public TimeSpan Duration { get; set; }
        #endregion
        
        #region AggressiveCacheModeSyntax
        // Syntax:
        public AggressiveCacheMode Mode { get; set; }
        #endregion
        
        #region AsyncDocumentIdGeneratorSyntax
        // Syntax:
        public Func<string, object, Task<string>> AsyncDocumentIdGenerator { get; set; }
        #endregion
        
        #region CreateHttpClientSyntax
        // Syntax:
        public Func<HttpClientHandler, HttpClient> CreateHttpClient { get; set; }
        #endregion
        
        #region DisableAtomicDocumentWritesInClusterWideTransactionSyntax
        // Syntax:
        public bool? DisableAtomicDocumentWritesInClusterWideTransaction { get; set; }
        #endregion
        
        #region DisableTcpCompressionSyntax
        // Syntax:
        public bool DisableTcpCompression { get; set; }
        #endregion
        
        #region DisableTopologyCacheSyntax
        // Syntax:
        public bool DisableTopologyCache { get; set; }
        #endregion
        
        #region DisableTopologyUpdatesSyntax
        // Syntax:
        public bool DisableTopologyUpdates { get; set; }
        #endregion
        
        #region FindClrTypeSyntax
        // Syntax:
        public Func<string, BlittableJsonReaderObject, string> FindClrType { get; set; }
        #endregion
        
        #region FindClrTypeNameSyntax
        // Syntax:
        public Func<Type, string> FindClrTypeName { get; set; }
        #endregion
        
        #region FindClrTypeNameForDynamicSyntax
        // Syntax:
        public Func<dynamic, string> FindClrTypeNameForDynamic { get; set; }
        #endregion
        
        #region FindCollectionNameSyntax
        // Syntax:
        public Func<Type, string> FindCollectionName { get; set; }
        #endregion
        
        #region FindCollectionNameForDynamicSyntax
        // Syntax:
        public Func<dynamic, string> FindCollectionNameForDynamic { get; set; }
        #endregion
        
        #region FindIdentityPropertySyntax
        // Syntax:
        public Func<MemberInfo, bool> FindIdentityProperty { get; set; }
        #endregion
                
        #region FindIdentityPropertyNameFromCollectionNameSyntax
        // Syntax:
        public Func<string, string> FindIdentityPropertyNameFromCollectionName { get; set; }
        #endregion

        #region FindProjectedPropertyNameForIndexSyntax
        // Syntax:
        public Func<Type, string, string, string, string> FindProjectedPropertyNameForIndex { get; set; }
        #endregion
        
        #region FindPropertyNameForDynamicIndexSyntax
        // Syntax:
        public Func<Type, string, string, string, string> FindPropertyNameForDynamicIndex { get; set; }
        #endregion
                
        #region FindPropertyNameForIndexSyntax
        // Syntax:
        public Func<Type, string, string, string, string> FindPropertyNameForIndex { get; set; }
        #endregion
        
        #region FirstBroadcastAttemptTimeoutSyntax
        // Syntax:
        public TimeSpan FirstBroadcastAttemptTimeout { get; set; }
        #endregion
        
        #region HttpClientTypeSyntax
        // Syntax:
        public Type HttpClientType { get; set; }
        #endregion
        
        #region HttpVersionSyntax
        // Syntax:
        public Version HttpVersion { get; set; }
        #endregion
        
        #region IdentityPartsSeparatorSyntax
        // Syntax:
        public char IdentityPartsSeparator { get; set; }
        #endregion
        
        #region MaxHttpCacheSizeSyntax
        // Syntax:
        public Size MaxHttpCacheSize { get; set; }
        #endregion
        
        #region MaxNumberOfRequestsPerSessionSyntax
        // Syntax:
        public int MaxNumberOfRequestsPerSession { get; set; }
        #endregion
        
        #region SerializationSyntax
        // Syntax:
        public ISerializationConventions Serialization { get; set; }
        #endregion
        
        #region OperationStatusFetchModeSyntax
        // Syntax:
        public OperationStatusFetchMode OperationStatusFetchMode { get; set; }
        #endregion
        
        #region PreserveDocumentPropertiesNotFoundOnModelSyntax
        // Syntax:
        public bool PreserveDocumentPropertiesNotFoundOnModel { get; set; }
        #endregion
        
        #region RequestTimeoutSyntax
        // Syntax:
        public TimeSpan? RequestTimeout { get; set; } 
        #endregion
        
        #region ResolveTypeFromClrTypeNameSyntax
        // Syntax:
        public Func<string, Type> ResolveTypeFromClrTypeName { get; set; }
        #endregion
                
        #region SaveEnumsAsIntegersSyntax
        // Syntax:
        public bool SaveEnumsAsIntegers { get; set; } 
        #endregion
        
        #region SecondBroadcastAttemptTimeoutSyntax
        public TimeSpan SecondBroadcastAttemptTimeout { get; set; } 
        #endregion

        #region SendApplicationIdentifierSyntax
        // Syntax:
        public bool SendApplicationIdentifier { get; set; } 
        #endregion
        
        #region ThrowIfQueryPageSizeIsNotSetSyntax
        // Syntax:
        public bool ThrowIfQueryPageSizeIsNotSet { get; set; }
        #endregion
        
        #region TopologyCacheLocationSyntax
        // Syntax:
        public string TopologyCacheLocation { get; set; } 
        #endregion
            
        #region TransformTypeCollectionNameToDocumentIdPrefixSyntax
        // Syntax:
        public Func<string, string> TransformTypeCollectionNameToDocumentIdPrefix { get; set; }
        #endregion
        
        #region UseCompressionSyntax
        // Syntax:
        public bool UseCompression { get; set; }
        #endregion
        
        #region UseOptimisticConcurrencySyntax
        // Syntax:
        public bool UseOptimisticConcurrency { get; set; }
        #endregion
        
        #region WaitForIndexesAfterSaveChangesTimeoutSyntax
        // Syntax:
        public TimeSpan WaitForIndexesAfterSaveChangesTimeout { get; set; }
        #endregion
        
        #region WaitForNonStaleResultsTimeoutSyntax
        // Syntax:
        public TimeSpan WaitForNonStaleResultsTimeout { get; set; }
        #endregion
        
        #region WaitForReplicationAfterSaveChangesTimeoutSyntax
        // Syntax:
        public TimeSpan WaitForReplicationAfterSaveChangesTimeout { get; set; }
        #endregion
    }
    
    public class MyHttpClient : HttpClient
    {
        public MyHttpClient(HttpMessageHandler handler)
        { }
    }
}
