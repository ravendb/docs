# List of Differences in Client API between 3.x and 4.0

The list of differences in the public Client API between 3.x and 4.0 contains the following changes:

- removed fields and methods in types that exists in both 3.x and 4.0,
- added fields and methods in types that exists in both 3.x and 4.0,
- types that don't exists in 4.0 API.

## Types with removed or added members

### BulkInsertOperation
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.BulkInsert`
#### Removed methods

* `void add_OnBeforeEntityInsert(Raven.Client.Document.BulkInsertOperation/BeforeEntityInsert)`
* `void add_Report(Action<string>)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_DatabaseCommands()`
* `bool get_IsAborted()`
* `Guid get_OperationId()`
* `void remove_OnBeforeEntityInsert(Raven.Client.Document.BulkInsertOperation/BeforeEntityInsert)`
* `void remove_Report(Action<string>)`
* `void Store(Raven.Json.Linq.RavenJObject, Raven.Json.Linq.RavenJObject, string, Nullable<int>)`
* `Task WaitForLastTaskToFinish()`

#### Added fields

* `System.IO.Compression.CompressionLevel CompressionLevel`

#### Added methods

* `System.Threading.Tasks.Task AbortAsync()`
* `System.Threading.Tasks.Task<string> StoreAsync(object, Raven.Client.Documents.Session.IMetadataDictionary)`
* `System.Threading.Tasks.Task StoreAsync(object, string, Raven.Client.Documents.Session.IMetadataDictionary)`

### IConnectableChanges<T>
#### Namespace changed

* 3.x: `Raven.Client.Changes`
* 4.0: `Raven.Client.Documents.Changes`
#### Removed methods

* `Task<T> get_Task()`

#### Added methods

* `void add_ConnectionStatusChanged(System.EventHandler)`
* `void add_OnError(System.Action<System.Exception>)`
* `System.Threading.Tasks.Task<TChanges> EnsureConnectedNow()`
* `bool get_Connected()`
* `void remove_ConnectionStatusChanged(System.EventHandler)`
* `void remove_OnError(System.Action<System.Exception>)`

### IDatabaseChanges
#### Namespace changed

* 3.x: `Raven.Client.Changes`
* 4.0: `Raven.Client.Documents.Changes`
#### Removed methods

* `Raven.Client.Changes.IObservableWithTask<Raven.Abstractions.Data.DataSubscriptionChangeNotification> ForAllDataSubscriptions()`
* `Raven.Client.Changes.IObservableWithTask<Raven.Abstractions.Data.ReplicationConflictNotification> ForAllReplicationConflicts()`
* `Raven.Client.Changes.IObservableWithTask<Raven.Abstractions.Data.TransformerChangeNotification> ForAllTransformers()`
* `Raven.Client.Changes.IObservableWithTask<Raven.Abstractions.Data.BulkInsertChangeNotification> ForBulkInsert(Nullable<Guid>)`
* `Raven.Client.Changes.IObservableWithTask<Raven.Abstractions.Data.DataSubscriptionChangeNotification> ForDataSubscription(long)`

#### Added methods

* `Raven.Client.Documents.Changes.IChangesObservable<Raven.Client.Documents.Operations.OperationStatusChange> ForAllOperations()`
* `Raven.Client.Documents.Changes.IChangesObservable<Raven.Client.Documents.Operations.OperationStatusChange> ForOperationId(long)`

### BatchOptions
#### Namespace changed

* 3.x: `Raven.Client.Connection`
* 4.0: `Raven.Client.Documents.Commands.Batches`
#### Removed methods

* `TimeSpan get_WaitForReplicasTimout()`
* `void set_WaitForReplicasTimout(TimeSpan)`

#### Added methods

* `System.Nullable<System.TimeSpan> get_RequestTimeout()`
* `System.TimeSpan get_WaitForReplicasTimeout()`
* `void set_RequestTimeout(System.Nullable<System.TimeSpan>)`
* `void set_WaitForReplicasTimeout(System.TimeSpan)`

### DefaultRavenContractResolver
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Conventions`
#### Removed methods

* `IDisposable RegisterForExtensionData(Raven.Imports.Newtonsoft.Json.Serialization.ExtensionDataSetter)`

#### Added fields

* `System.Nullable<System.Reflection.BindingFlags> MembersSearchFlag`

#### Added methods

* `Raven.Client.Documents.Conventions.DefaultRavenContractResolver/ClearExtensionData RegisterExtensionDataGetter(Newtonsoft.Json.Serialization.ExtensionDataGetter)`
* `Raven.Client.Documents.Conventions.DefaultRavenContractResolver/ClearExtensionData RegisterExtensionDataSetter(Newtonsoft.Json.Serialization.ExtensionDataSetter)`

### DocumentStore
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents`
#### Removed methods

* `bool CanEnlistInDistributedTransactions(string)`
* `string get_ApiKey()`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `string get_ConnectionStringName()`
* `ICredentials get_Credentials()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `string get_DefaultDatabase()`
* `bool get_HasJsonRequestFactory()`
* `Func<System.Net.Http.HttpMessageHandler> get_HttpMessageHandlerFactory()`
* `Raven.Client.Connection.HttpJsonRequestFactory get_JsonRequestFactory()`
* `int get_MaxNumberOfCachedRequests()`
* `Raven.Client.Document.ReplicationBehavior get_Replication()`
* `Task GetObserveChangesAndEvictItemsFromCacheTask(string)`
* `Raven.Client.Connection.IDocumentStoreReplicationInformer GetReplicationInformerForDatabase(string)`
* `Raven.Client.Metrics.RequestTimeMetric GetRequestTimeMetricForUrl(string)`
* `Raven.Client.IDocumentStore Initialize(bool)`
* `void InitializeProfiling()`
* `void ParseConnectionString(string)`
* `void set_ApiKey(string)`
* `void set_ConnectionStringName(string)`
* `void set_Credentials(ICredentials)`
* `void set_DefaultDatabase(string)`
* `void set_HttpMessageHandlerFactory(Func<System.Net.Http.HttpMessageHandler>)`
* `void set_MaxNumberOfCachedRequests(int)`
* `IDisposable SetRequestsTimeoutFor(TimeSpan)`

#### Added methods

* `void add_RequestExecutorCreated(System.EventHandler<Raven.Client.Http.RequestExecutor>)`
* `Raven.Client.Documents.Operations.MaintenanceOperationExecutor get_Maintenance()`
* `Raven.Client.Documents.Operations.OperationExecutor get_Operations()`
* `Raven.Client.Documents.Smuggler.DatabaseSmuggler get_Smuggler()`
* `System.Exception GetLastDatabaseChangesStateException(string)`
* `Raven.Client.Http.RequestExecutor GetRequestExecutor(string)`
* `void remove_RequestExecutorCreated(System.EventHandler<Raven.Client.Http.RequestExecutor>)`
* `System.IDisposable SetRequestsTimeout(System.TimeSpan, string)`

### DocumentStoreBase
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents`
#### Removed methods

* `bool CanEnlistInDistributedTransactions(string)`
* `void ExecuteTransformer(Raven.Client.Indexes.AbstractTransformerCreationTask)`
* `Task ExecuteTransformerAsync(Raven.Client.Indexes.AbstractTransformerCreationTask)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `Raven.Client.Document.IAsyncReliableSubscriptions get_AsyncSubscriptions()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `bool get_EnlistInDistributedTransactions()`
* `Raven.Abstractions.Data.FailoverServers get_FailoverServers()`
* `bool get_HasJsonRequestFactory()`
* `Raven.Client.Connection.HttpJsonRequestFactory get_JsonRequestFactory()`
* `Raven.Client.Util.ILastEtagHolder get_LastEtagHolder()`
* `Raven.Client.Document.DocumentSessionListeners get_Listeners()`
* `ReadOnlyCollection<Raven.Client.Listeners.IDocumentConflictListener> get_RegisteredConflictListeners()`
* `ReadOnlyCollection<Raven.Client.Listeners.IDocumentDeleteListener> get_RegisteredDeleteListeners()`
* `ReadOnlyCollection<Raven.Client.Listeners.IDocumentQueryListener> get_RegisteredQueryListeners()`
* `ReadOnlyCollection<Raven.Client.Listeners.IDocumentStoreListener> get_RegisteredStoreListeners()`
* `Guid get_ResourceManagerId()`
* `NameValueCollection get_SharedOperationsHeaders()`
* `Raven.Client.Document.DTC.ITransactionRecoveryStorage get_TransactionRecoveryStorage()`
* `string get_Url()`
* `bool get_UseFipsEncryptionAlgorithms()`
* `Raven.Abstractions.Data.Etag GetLastWrittenEtag()`
* `Raven.Client.Connection.Profiling.ProfilingInformation GetProfilingInformationFor(Guid)`
* `void InitializeProfiling()`
* `Raven.Client.DocumentStoreBase RegisterListener(Raven.Client.Listeners.IDocumentConversionListener)`
* `Raven.Client.DocumentStoreBase RegisterListener(Raven.Client.Listeners.IDocumentQueryListener)`
* `Raven.Client.IDocumentStore RegisterListener(Raven.Client.Listeners.IDocumentStoreListener)`
* `Raven.Client.DocumentStoreBase RegisterListener(Raven.Client.Listeners.IDocumentDeleteListener)`
* `Raven.Client.DocumentStoreBase RegisterListener(Raven.Client.Listeners.IDocumentConflictListener)`
* `void set_EnlistInDistributedTransactions(bool)`
* `void set_FailoverServers(Raven.Abstractions.Data.FailoverServers)`
* `void set_LastEtagHolder(Raven.Client.Util.ILastEtagHolder)`
* `void set_ResourceManagerId(Guid)`
* `void set_TransactionRecoveryStorage(Raven.Client.Document.DTC.ITransactionRecoveryStorage)`
* `void set_Url(string)`
* `void set_UseFipsEncryptionAlgorithms(bool)`
* `void SetListeners(Raven.Client.Document.DocumentSessionListeners)`
* `IDisposable SetRequestsTimeoutFor(TimeSpan)`
* `void SideBySideExecuteIndex(Raven.Client.Indexes.AbstractIndexCreationTask, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteIndexAsync(Raven.Client.Indexes.AbstractIndexCreationTask, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `void SideBySideExecuteIndexes(List<Raven.Client.Indexes.AbstractIndexCreationTask>, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteIndexesAsync(List<Raven.Client.Indexes.AbstractIndexCreationTask>, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`

#### Added methods

* `void add_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void add_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void add_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void add_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void add_TopologyUpdatedInternal(System.Action<string>)`
* `System.Security.Cryptography.X509Certificates.X509Certificate2 get_Certificate()`
* `string get_Database()`
* `Raven.Client.Documents.Operations.MaintenanceOperationExecutor get_Maintenance()`
* `Raven.Client.Documents.Operations.OperationExecutor get_Operations()`
* `System.String[] get_Urls()`
* `Raven.Client.Http.RequestExecutor GetRequestExecutor(string)`
* `void remove_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void remove_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void remove_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void remove_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void remove_TopologyUpdatedInternal(System.Action<string>)`
* `void set_Certificate(System.Security.Cryptography.X509Certificates.X509Certificate2)`
* `void set_Database(string)`
* `void set_Urls(System.String[])`
* `System.IDisposable SetRequestsTimeout(System.TimeSpan, string)`

### GenerateEntityIdOnTheClient
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Identity`
#### Removed methods

* `string GenerateDocumentKeyForStorage(object)`
* `string GetOrGenerateDocumentKey(object)`

#### Added methods

* `string GenerateDocumentIdForStorage(object)`
* `string GetOrGenerateDocumentId(object)`

### IDocumentStore
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents`
#### Removed methods

* `void ExecuteTransformer(Raven.Client.Indexes.AbstractTransformerCreationTask)`
* `Task ExecuteTransformerAsync(Raven.Client.Indexes.AbstractTransformerCreationTask)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `Raven.Client.Document.IAsyncReliableSubscriptions get_AsyncSubscriptions()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `bool get_HasJsonRequestFactory()`
* `Raven.Client.Connection.HttpJsonRequestFactory get_JsonRequestFactory()`
* `Raven.Client.Document.DocumentSessionListeners get_Listeners()`
* `NameValueCollection get_SharedOperationsHeaders()`
* `string get_Url()`
* `Raven.Abstractions.Data.Etag GetLastWrittenEtag()`
* `Raven.Client.Connection.Profiling.ProfilingInformation GetProfilingInformationFor(Guid)`
* `void InitializeProfiling()`
* `void SetListeners(Raven.Client.Document.DocumentSessionListeners)`
* `IDisposable SetRequestsTimeoutFor(TimeSpan)`
* `void SideBySideExecuteIndex(Raven.Client.Indexes.AbstractIndexCreationTask, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteIndexAsync(Raven.Client.Indexes.AbstractIndexCreationTask, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `void SideBySideExecuteIndexes(List<Raven.Client.Indexes.AbstractIndexCreationTask>, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteIndexesAsync(List<Raven.Client.Indexes.AbstractIndexCreationTask>, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`

#### Added methods

* `void add_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void add_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void add_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void add_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `System.Security.Cryptography.X509Certificates.X509Certificate2 get_Certificate()`
* `string get_Database()`
* `Raven.Client.Documents.Operations.MaintenanceOperationExecutor get_Maintenance()`
* `Raven.Client.Documents.Operations.OperationExecutor get_Operations()`
* `System.String[] get_Urls()`
* `Raven.Client.Http.RequestExecutor GetRequestExecutor(string)`
* `void remove_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void remove_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void remove_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void remove_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void set_Database(string)`
* `System.IDisposable SetRequestsTimeout(System.TimeSpan, string)`

### AbstractGenericIndexCreationTask<TReduceResult>
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `bool get_DisableInMemoryIndexing()`
* `void set_DisableInMemoryIndexing(bool)`

### AbstractIndexCreationTask
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `void AfterExecute(Raven.Client.Connection.IDatabaseCommands, Raven.Client.Document.DocumentConvention)`
* `Task AfterExecuteAsync(Raven.Client.Connection.Async.IAsyncDatabaseCommands, Raven.Client.Document.DocumentConvention, CancellationToken)`
* `void Execute(Raven.Client.Connection.IDatabaseCommands, Raven.Client.Document.DocumentConvention)`
* `Task ExecuteAsync(Raven.Client.Connection.Async.IAsyncDatabaseCommands, Raven.Client.Document.DocumentConvention, CancellationToken)`
* `Raven.Abstractions.Indexing.IndexDefinition GetLegacyIndexDefinition(Raven.Client.Document.DocumentConvention)`
* `object LoadAttachmentForIndexing(string)`
* `void SideBySideExecute(Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `void SideBySideExecute(Raven.Client.Connection.IDatabaseCommands, Raven.Client.Document.DocumentConvention, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteAsync(Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideExecuteAsync(Raven.Client.Connection.Async.IAsyncDatabaseCommands, Raven.Client.Document.DocumentConvention, Raven.Abstractions.Data.Etag, Nullable<DateTime>, CancellationToken)`
* `object SpatialClustering(string, Nullable<Double>, Nullable<Double>)`
* `object SpatialClustering(string, Nullable<Double>, Nullable<Double>, int, int)`
* `object SpatialGenerate(Nullable<Double>, Nullable<Double>)`
* `object SpatialGenerate(string, Nullable<Double>, Nullable<Double>)`
* `object SpatialGenerate(string, string)`
* `object SpatialGenerate(string, string, Raven.Abstractions.Indexing.SpatialSearchStrategy)`
* `object SpatialGenerate(string, string, Raven.Abstractions.Indexing.SpatialSearchStrategy, int)`

#### Added methods

* `object CreateSpatialField(System.Nullable<System.Double>, System.Nullable<System.Double>)`
* `object CreateSpatialField(string)`
* `System.Nullable<Raven.Client.Documents.Indexes.IndexLockMode> get_LockMode()`
* `void set_LockMode(System.Nullable<Raven.Client.Documents.Indexes.IndexLockMode>)`

### AbstractIndexCreationTask<TDocument, TReduceResult>
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `Nullable<int> get_MaxIndexOutputsPerDocument()`
* `void set_MaxIndexOutputsPerDocument(Nullable<int>)`

### AbstractMultiMapIndexCreationTask<TReduceResult>
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `Nullable<int> get_MaxIndexOutputsPerDocument()`
* `void set_MaxIndexOutputsPerDocument(Nullable<int>)`

### IndexCreation
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `void CreateIndexes(ExportProvider, Raven.Client.IDocumentStore)`
* `Task CreateIndexesAsync(ExportProvider, Raven.Client.IDocumentStore)`
* `void SideBySideCreateIndexes(Assembly, Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `void SideBySideCreateIndexes(ExportProvider, Raven.Client.Connection.IDatabaseCommands, Raven.Client.Document.DocumentConvention, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `void SideBySideCreateIndexes(ExportProvider, Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideCreateIndexesAsync(ExportProvider, Raven.Client.Connection.Async.IAsyncDatabaseCommands, Raven.Client.Document.DocumentConvention, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideCreateIndexesAsync(Assembly, Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`
* `Task SideBySideCreateIndexesAsync(ExportProvider, Raven.Client.IDocumentStore, Raven.Abstractions.Data.Etag, Nullable<DateTime>)`

### IndexDefinitionBuilder<TDocument, TReduceResult>
#### Namespace changed

* 3.x: `Raven.Client.Indexes`
* 4.0: `Raven.Client.Documents.Indexes`
#### Removed methods

* `bool get_DisableInMemoryIndexing()`
* `Nullable<int> get_MaxIndexOutputsPerDocument()`
* `IDictionary<Expression<Func<TReduceResult, object>>, Raven.Abstractions.Indexing.SortOptions> get_SortOptions()`
* `Dictionary<string, Raven.Abstractions.Indexing.SortOptions> get_SortOptionsStrings()`
* `IDictionary<Expression<Func<TReduceResult, object>>, Raven.Abstractions.Indexing.SuggestionOptions> get_Suggestions()`
* `void set_DisableInMemoryIndexing(bool)`
* `void set_MaxIndexOutputsPerDocument(Nullable<int>)`
* `void set_SortOptions(IDictionary<Expression<Func<TReduceResult, object>>, Raven.Abstractions.Indexing.SortOptions>)`
* `void set_SortOptionsStrings(Dictionary<string, Raven.Abstractions.Indexing.SortOptions>)`
* `void set_Suggestions(IDictionary<Expression<Func<TReduceResult, object>>, Raven.Abstractions.Indexing.SuggestionOptions>)`

#### Added methods

* `System.Collections.Generic.Dictionary<string, string> get_AdditionalSources()`
* `string get_OutputReduceToCollection()`
* `System.Nullable<Raven.Client.Documents.Indexes.IndexPriority> get_Priority()`
* `void set_AdditionalSources(System.Collections.Generic.Dictionary<string, string>)`
* `void set_OutputReduceToCollection(string)`
* `void set_Priority(System.Nullable<Raven.Client.Documents.Indexes.IndexPriority>)`

### IndexingLinqExtensions
#### Namespace changed

* 3.x: `Raven.Client.Linq.Indexing`
* 4.0: `Raven.Client.Documents.Linq.Indexing`
#### Removed methods

* `string ParseShort(object)`
* `string ParseShort(object, Int16)`
* `string StripHtml(object)`

### IRavenQueryable<T>
#### Namespace changed

* 3.x: `Raven.Client.Linq`
* 4.0: `Raven.Client.Documents.Linq`
#### Removed methods

* `Raven.Client.Linq.IRavenQueryable<T> AddQueryInput(string, Raven.Json.Linq.RavenJToken)`
* `Raven.Client.Linq.IRavenQueryable<T> AddTransformerParameter(string, Raven.Json.Linq.RavenJToken)`
* `Type get_OriginalQueryType()`
* `Raven.Client.Linq.IRavenQueryable<T> OrderByDistance(Raven.Abstractions.Indexing.SpatialSort)`
* `void set_OriginalQueryType(Type)`
* `Raven.Client.Linq.IRavenQueryable<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.Linq.IRavenQueryable<TResult> TransformWith<TTransformer, TResult>()`
* `Raven.Client.Linq.IRavenQueryable<TResult> TransformWith<TResult>(string)`

### IRavenQueryProvider
#### Namespace changed

* 3.x: `Raven.Client.Linq`
* 4.0: `Raven.Client.Documents.Linq`
#### Removed methods

* `void AddQueryInput(string, Raven.Json.Linq.RavenJToken)`
* `void AddTransformerParameter(string, Raven.Json.Linq.RavenJToken)`
* `void AfterStreamExecuted(Action<Raven.Json.Linq.RavenJObject>)`
* `string get_ResultTransformer()`
* `Dictionary<string, Raven.Json.Linq.RavenJToken> get_TransformerParameters()`
* `void set_OriginalQueryType(Type)`
* `Raven.Client.IAsyncDocumentQuery<T> ToAsyncLuceneQuery<T>(Expression)`
* `Raven.Client.IDocumentQuery<TResult> ToLuceneQuery<TResult>(Expression)`
* `void TransformWith(string)`

### RavenQueryInspector<T>
#### Namespace changed

* 3.x: `Raven.Client.Linq`
* 4.0: `Raven.Client.Documents.Linq`
#### Removed methods

* `Raven.Client.Linq.IRavenQueryable<T> AddQueryInput(string, Raven.Json.Linq.RavenJToken)`
* `Raven.Client.Linq.IRavenQueryable<T> AddTransformerParameter(string, Raven.Json.Linq.RavenJToken)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `string get_AsyncIndexQueried()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `string get_IndexQueried()`
* `Type get_OriginalQueryType()`
* `Raven.Abstractions.Data.FacetResults GetFacets(string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults GetFacets(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(string, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `KeyValuePair<string, string> GetLastEqualityTerm(bool)`
* `Raven.Client.Linq.IRavenQueryable<T> OrderByDistance(Raven.Abstractions.Indexing.SpatialSort)`
* `void set_OriginalQueryType(Type)`
* `Raven.Client.Linq.IRavenQueryable<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.Linq.IRavenQueryable<TResult> TransformWith<TTransformer, TResult>()`
* `Raven.Client.Linq.IRavenQueryable<TResult> TransformWith<TResult>(string)`

#### Added methods

* `string get_IndexName()`

### LinqExtensions
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents`
#### Removed methods

* `Raven.Client.Linq.IRavenQueryable<TResult> ProjectFromIndexFieldsInto<TResult>(IQueryable)`
* `Raven.Abstractions.Data.SuggestionQueryResult Suggest(IQueryable)`
* `Raven.Abstractions.Data.SuggestionQueryResult Suggest(IQueryable, Raven.Abstractions.Data.SuggestionQuery)`
* `Task<Raven.Abstractions.Data.SuggestionQueryResult> SuggestAsync(IQueryable, Raven.Abstractions.Data.SuggestionQuery, CancellationToken)`
* `Task<Raven.Abstractions.Data.SuggestionQueryResult> SuggestAsync(IQueryable, CancellationToken)`
* `Lazy<Raven.Abstractions.Data.SuggestionQueryResult> SuggestLazy(IQueryable)`
* `Lazy<Raven.Abstractions.Data.SuggestionQueryResult> SuggestLazy(IQueryable, Raven.Abstractions.Data.SuggestionQuery)`
* `Raven.Abstractions.Data.FacetQuery ToFacetQuery<T>(IQueryable<T>, string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetQuery ToFacetQuery<T>(IQueryable<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults ToFacets<T>(IQueryable<T>, string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults ToFacets<T>(IQueryable<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults ToFacets<T>(Raven.Client.IDocumentQuery<T>, string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults ToFacets<T>(Raven.Client.IDocumentQuery<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Task<Raven.Abstractions.Data.FacetResults> ToFacetsAsync<T>(IQueryable<T>, string, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> ToFacetsAsync<T>(IQueryable<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> ToFacetsAsync<T>(Raven.Client.IAsyncDocumentQuery<T>, string, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> ToFacetsAsync<T>(Raven.Client.IAsyncDocumentQuery<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Lazy<Raven.Abstractions.Data.FacetResults> ToFacetsLazy<T>(IQueryable<T>, string, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> ToFacetsLazy<T>(IQueryable<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> ToFacetsLazy<T>(Raven.Client.IDocumentQuery<T>, string, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> ToFacetsLazy<T>(Raven.Client.IDocumentQuery<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> ToFacetsLazyAsync<T>(IQueryable<T>, string, int, Nullable<int>)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> ToFacetsLazyAsync<T>(IQueryable<T>, IEnumerable<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`

#### Added methods

* `Raven.Client.Documents.Queries.Facets.IAggregationQuery<T> AggregateUsing<T>(System.Linq.IQueryable<T>, string)`
* `Raven.Client.Documents.Linq.IRavenQueryable<System.Linq.IGrouping<System.Collections.Generic.IEnumerable<TKey>, TSource>> GroupByArrayContent<TSource, TKey>(System.Linq.IQueryable<TSource>, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TKey>>>)`
* `Raven.Client.Documents.Linq.IRavenQueryable<System.Linq.IGrouping<TKey, TSource>> GroupByArrayValues<TSource, TKey>(System.Linq.IQueryable<TSource>, System.Linq.Expressions.Expression<System.Func<TSource, System.Collections.Generic.IEnumerable<TKey>>>)`
* `Raven.Client.Documents.Linq.IRavenQueryable<TResult> Include<TResult>(System.Linq.IQueryable<TResult>, string)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> MoreLikeThis<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.MoreLikeThis.MoreLikeThisBase)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> MoreLikeThis<T>(System.Linq.IQueryable<T>, System.Action<Raven.Client.Documents.Queries.MoreLikeThis.IMoreLikeThisBuilder<T>>)`
* `System.Linq.IOrderedQueryable<T> OrderBy<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> OrderBy<T>(System.Linq.IQueryable<T>, string, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> OrderByDescending<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> OrderByDescending<T>(System.Linq.IQueryable<T>, string, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, string, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistance<T>(System.Linq.IQueryable<T>, string, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, string, System.Double, System.Double)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, string)`
* `System.Linq.IOrderedQueryable<T> OrderByDistanceDescending<T>(System.Linq.IQueryable<T>, string, string)`
* `Raven.Client.Documents.Linq.IRavenQueryable<TResult> ProjectInto<TResult>(System.Linq.IQueryable)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Spatial<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, System.Func<Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory, Raven.Client.Documents.Queries.Spatial.SpatialCriteria>)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Spatial<T>(System.Linq.IQueryable<T>, string, System.Func<Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory, Raven.Client.Documents.Queries.Spatial.SpatialCriteria>)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Spatial<T>(System.Linq.IQueryable<T>, System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, System.Func<Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory, Raven.Client.Documents.Queries.Spatial.SpatialCriteria>)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Spatial<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Func<Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory, Raven.Client.Documents.Queries.Spatial.SpatialCriteria>)`
* `Raven.Client.Documents.Queries.Suggestions.ISuggestionQuery<T> SuggestUsing<T>(System.Linq.IQueryable<T>, Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `Raven.Client.Documents.Queries.Suggestions.ISuggestionQuery<T> SuggestUsing<T>(System.Linq.IQueryable<T>, System.Action<Raven.Client.Documents.Queries.Suggestions.ISuggestionBuilder<T>>)`
* `System.Linq.IOrderedQueryable<T> ThenBy<T>(System.Linq.IOrderedQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> ThenBy<T>(System.Linq.IOrderedQueryable<T>, string, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> ThenByDescending<T>(System.Linq.IOrderedQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, object>>, Raven.Client.Documents.Session.OrderingType)`
* `System.Linq.IOrderedQueryable<T> ThenByDescending<T>(System.Linq.IOrderedQueryable<T>, string, Raven.Client.Documents.Session.OrderingType)`
* `void ToStream<T>(System.Linq.IQueryable<T>, System.IO.Stream)`
* `void ToStream<T>(Raven.Client.Documents.Session.IDocumentQuery<T>, System.IO.Stream)`
* `System.Threading.Tasks.Task ToStreamAsync<T>(System.Linq.IQueryable<T>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task ToStreamAsync<T>(Raven.Client.Documents.Session.IAsyncDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Where<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, int, bool>>, bool)`
* `Raven.Client.Documents.Linq.IRavenQueryable<T> Where<T>(System.Linq.IQueryable<T>, System.Linq.Expressions.Expression<System.Func<T, bool>>, bool)`

### Operation
#### Namespace changed

* 3.x: `Raven.Client.Connection`
* 4.0: `Raven.Client.Documents.Operations`
#### Removed methods

* `void .ctor(Func<long, Task<Raven.Json.Linq.RavenJToken>>, long)`
* `void .ctor(Raven.Client.Connection.Async.AsyncServerClient, long)`

#### Added methods

* `void OnCompleted()`
* `void OnError(System.Exception)`
* `void OnNext(Raven.Client.Documents.Operations.OperationStatusChange)`
* `TResult WaitForCompletion<TResult>(System.Nullable<System.TimeSpan>)`
* `System.Threading.Tasks.Task<TResult> WaitForCompletionAsync<TResult>(System.Nullable<System.TimeSpan>)`

### SpatialCriteria
#### Namespace changed

* 3.x: `Raven.Client.Spatial`
* 4.0: `Raven.Client.Documents.Queries.Spatial`
#### Removed methods

* `Double get_DistanceErrorPct()`
* `Raven.Abstractions.Indexing.SpatialRelation get_Relation()`
* `object get_Shape()`
* `void set_DistanceErrorPct(Double)`
* `void set_Relation(Raven.Abstractions.Indexing.SpatialRelation)`
* `void set_Shape(object)`

#### Added methods

* `Raven.Client.Documents.Session.Tokens.QueryToken ToQueryToken(string, System.Func<object, string>)`

### SpatialCriteriaFactory
#### Namespace changed

* 3.x: `Raven.Client.Spatial`
* 4.0: `Raven.Client.Documents.Queries.Spatial`
#### Removed methods

* `Raven.Client.Spatial.SpatialCriteria WithinRadiusOf(Double, Double, Double, Double)`

#### Added fields

* `Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory Instance`

### AbstractDocumentQuery<T, TSelf>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void AddOrder(string, bool)`
* `void AddOrder(string, bool, Type)`
* `void AlphaNumericOrdering(string, bool)`
* `Raven.Client.IDocumentQueryCustomization BeforeQueryExecution(Action<Raven.Abstractions.Data.IndexQuery>)`
* `void CustomSortUsing(string, bool)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `Raven.Client.Document.DocumentConvention get_DocumentConvention()`
* `KeyValuePair<string, string> GetLastEqualityTerm(bool)`
* `void Highlight(string, int, int, string)`
* `void Highlight(string, int, int, Raven.Client.FieldHighlightings&)`
* `void Highlight(string, string, int, int, Raven.Client.FieldHighlightings&)`
* `void Include(Expression<Func<T, object>>)`
* `void SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(bool)`
* `void SetHighlighterTags(string, string)`
* `void SetHighlighterTags(String[], String[])`
* `void SetOriginalQueryType(Type)`
* `void SetResultTransformer(string)`
* `Raven.Client.IDocumentQueryCustomization ShowTimings()`
* `Raven.Client.IDocumentQueryCustomization TransformResults(Func<Raven.Abstractions.Data.IndexQuery, IEnumerable<object>, IEnumerable<object>>)`
* `void UsingDefaultField(string)`
* `void WaitForNonStaleResults()`
* `void WaitForNonStaleResultsAsOf(DateTime)`
* `void WaitForNonStaleResultsAsOf(DateTime, TimeSpan)`
* `void WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag)`
* `void WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag, TimeSpan)`
* `void WaitForNonStaleResultsAsOfLastWrite()`
* `void WaitForNonStaleResultsAsOfLastWrite(TimeSpan)`
* `void WaitForNonStaleResultsAsOfNow()`
* `void WaitForNonStaleResultsAsOfNow(TimeSpan)`
* `void Where(string)`
* `void WhereBetweenOrEqual(string, object, object)`

#### Added methods

* `void AddFromAliasToWhereTokens(string)`
* `void AddParameter(string, object)`
* `void AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `void AggregateUsing(string)`
* `Raven.Client.Documents.Session.IDocumentQueryCustomization BeforeQueryExecuted(System.Action<Raven.Client.Documents.Queries.IndexQuery>)`
* `Raven.Client.Documents.Session.IAsyncDocumentSession get_AsyncSession()`
* `string get_CollectionName()`
* `Raven.Client.Documents.Conventions.DocumentConventions get_Conventions()`
* `string get_IndexName()`
* `bool get_IsDynamicMapReduce()`
* `Raven.Client.Documents.Session.Operations.QueryOperation get_QueryOperation()`
* `void GroupBy(string, System.String[])`
* `void GroupBy(System.ValueTuple<string, Raven.Client.Documents.Queries.GroupByMethod>, System.ValueTuple`2<System.String,Raven.Client.Documents.Queries.GroupByMethod>[])`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, T>> GroupBy<TKey>(System.Func<T, TKey>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(System.Func<T, TKey>, System.Func<T, TElement>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, T>> GroupBy<TKey>(System.Func<T, TKey>, System.Collections.Generic.IEqualityComparer<TKey>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(System.Func<T, TKey>, System.Func<T, TElement>, System.Collections.Generic.IEqualityComparer<TKey>)`
* `void GroupByCount(string)`
* `void GroupByKey(string, string)`
* `void GroupBySum(string, string)`
* `Raven.Client.Documents.Queries.MoreLikeThis.MoreLikeThisScope MoreLikeThis()`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistance(string, System.Double, System.Double)`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistance(string, string)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistanceDescending(string, System.Double, System.Double)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistanceDescending(string, string)`
* `void OrderByScore()`
* `void OrderByScoreDescending()`
* `string ProjectionParameter(object)`
* `void RawQuery(string)`
* `void Spatial(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void Spatial(string, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void SuggestUsing(Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `void WhereExists(string)`
* `void WhereLucene(string, string)`
* `void WhereNotEquals(string, object, bool)`
* `void WhereNotEquals(string, Raven.Client.Documents.Session.MethodCall, bool)`
* `void WhereNotEquals(Raven.Client.Documents.Session.WhereParams)`
* `void WhereRegex(string, string)`
* `void WhereTrue()`

### AsyncDocumentQuery<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void .ctor(Raven.Client.Document.AsyncDocumentQuery<T>)`
* `Raven.Client.IAsyncDocumentQuery<T> AddOrder<TValue>(Expression<Func<T, TValue>>, bool)`
* `Raven.Client.IAsyncDocumentQuery<T> ContainsAll<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `Raven.Client.IAsyncDocumentQuery<T> ContainsAny<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `Task<int> CountAsync(CancellationToken)`
* `Lazy<Task<int>> CountLazilyAsync(CancellationToken)`
* `Raven.Client.IAsyncDocumentQuery<T> ExplainScores()`
* `Task<T> FirstAsync()`
* `Task<T> FirstOrDefaultAsync()`
* `string get_AsyncIndexQueried()`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(string, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> GetFacetsLazyAsync(string, int, Nullable<int>, CancellationToken)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> GetFacetsLazyAsync(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Lazy<Task<IEnumerable<T>>> LazilyAsync(Action<IEnumerable<T>>)`
* `Raven.Client.IAsyncDocumentQuery<T> OrderBy<TValue>(Expression`1[])`
* `Raven.Client.IAsyncDocumentQuery<T> OrderByDescending<TValue>(Expression`1[])`
* `Raven.Client.IAsyncDocumentQuery<T> OrderByScore()`
* `Raven.Client.IAsyncDocumentQuery<T> OrderByScoreDescending()`
* `Task<Raven.Abstractions.Data.QueryResult> QueryResultAsync(CancellationToken)`
* `Raven.Client.IAsyncDocumentQuery<T> RelatesToShape(string, string, Raven.Abstractions.Indexing.SpatialRelation, Double)`
* `Raven.Client.IAsyncDocumentQuery<T> Search<TValue>(Expression<Func<T, TValue>>, string, Raven.Client.EscapeQueryOptions)`
* `void SetQueryInputs(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Raven.Client.IAsyncDocumentQuery<TTransformerResult> SetResultTransformer<TTransformer, TTransformerResult>()`
* `Raven.Client.IAsyncDocumentQuery<T> SetTransformerParameters(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Task<T> SingleAsync()`
* `Task<T> SingleOrDefaultAsync()`
* `Raven.Client.IAsyncDocumentQuery<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.IAsyncDocumentQuery<T> Spatial(string, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Task<IList<T>> ToListAsync(CancellationToken)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereBetween<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereBetweenOrEqual<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereEndsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue, bool)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereGreaterThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereGreaterThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereIn<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereLessThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereLessThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WhereStartsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IAsyncDocumentQuery<T> WithinRadiusOf(Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`
* `Raven.Client.IAsyncDocumentQuery<T> WithinRadiusOf(string, Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`

#### Added methods

* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(System.Action<Raven.Client.Documents.Queries.Facets.IFacetBuilder<T>>)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(System.Collections.Generic.IEnumerable<Raven.Client.Documents.Queries.Facets.Facet>)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateUsing(string)`
* `System.Threading.Tasks.Task<Raven.Client.Documents.Queries.QueryResult> GetQueryResultAsync(System.Threading.CancellationToken)`

### AsyncDocumentSession
#### Namespace changed

* 3.x: `Raven.Client.Document.Async`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Raven.Client.IAsyncDocumentQuery<T> AsyncDocumentQuery<T>()`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T, TIndexCreator>()`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T>(string, bool)`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T>()`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncQuery<T>(string, bool)`
* `Task Commit(string)`
* `Task<Raven.Client.Connection.Operation> DeleteByIndexAsync<T, TIndexCreator>(Expression<Func<T, bool>>)`
* `Task<Raven.Client.Connection.Operation> DeleteByIndexAsync<T>(string, Expression<Func<T, bool>>)`
* `Raven.Client.Connection.Async.IAsyncDatabaseCommands get_AsyncDatabaseCommands()`
* `string GetDocumentUrl(object)`
* `Task<Raven.Json.Linq.RavenJObject> GetMetadataForAsync<T>(T)`
* `Lazy<Task<T[]>> LazyLoadInternal<T>(String[], KeyValuePair`2[], Action<T[]>, CancellationToken)`
* `Task<T[]> LoadAsync<T>(IEnumerable<ValueType>)`
* `Task<T[]> LoadAsync<T>(IEnumerable<ValueType>, CancellationToken)`
* `Task<T> LoadAsync<T>(string, CancellationToken)`
* `Task<T[]> LoadAsync<T>(IEnumerable<string>, CancellationToken)`
* `Task<T> LoadAsync<TTransformer, T>(string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult> LoadAsync<TResult>(string, string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>, string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult> LoadAsync<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>, Type, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<IEnumerable<TResult>> LoadStartingWithAsync<TTransformer, TResult>(string, string, int, int, string, Raven.Client.RavenPagingInformation, Action<Raven.Client.ILoadConfiguration>, string, CancellationToken)`
* `Task<T[]> LoadUsingTransformerInternalAsync<T>(String[], KeyValuePair`2[], string, Dictionary<string, Raven.Json.Linq.RavenJToken>, CancellationToken)`
* `Lazy<Task<TResult[]>> MoreLikeThisAsync<TResult>(Raven.Abstractions.Data.MoreLikeThisQuery, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults[]> MultiFacetedSearchAsync(Raven.Abstractions.Data.FacetQuery[])`
* `Task PrepareTransaction(string, Nullable<Guid>, Byte[])`
* `Raven.Client.Linq.IRavenQueryable<T> Query<T>(string, bool)`
* `Task Rollback(string)`
* `Task<Raven.Abstractions.Util.IAsyncEnumerator<Raven.Abstractions.Data.StreamResult<T>>> StreamAsync<T>(Raven.Abstractions.Data.Etag, int, int, Raven.Client.RavenPagingInformation, string, Dictionary<string, Raven.Json.Linq.RavenJToken>, CancellationToken)`
* `Task<Raven.Abstractions.Util.IAsyncEnumerator<Raven.Abstractions.Data.StreamResult<T>>> StreamAsync<T>(string, string, int, int, Raven.Client.RavenPagingInformation, string, string, Dictionary<string, Raven.Json.Linq.RavenJToken>, CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T> AsyncRawQuery<T>(string)`
* `System.Threading.Tasks.Task<bool> ExistsAsync(string)`
* `Raven.Client.Documents.Session.IAttachmentsSessionOperationsAsync get_Attachments()`
* `Raven.Client.Documents.Session.IRevisionsSessionOperationsAsync get_Revisions()`
* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `System.Threading.Tasks.Task LoadIntoStreamAsync(System.Collections.Generic.IEnumerable<string>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task LoadStartingWithIntoStreamAsync(string, System.IO.Stream, string, int, int, string, string, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`

### DocumentQuery<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void .ctor(Raven.Client.Document.DocumentQuery<T>)`
* `Raven.Client.IDocumentQuery<T> AddOrder<TValue>(Expression<Func<T, TValue>>, bool)`
* `Raven.Client.IDocumentQuery<T> ContainsAll<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `Raven.Client.IDocumentQuery<T> ContainsAny<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `int Count()`
* `Lazy<int> CountLazily()`
* `Raven.Client.IDocumentQuery<T> ExplainScores()`
* `T First()`
* `T FirstOrDefault()`
* `string get_IndexQueried()`
* `Raven.Abstractions.Data.QueryResult get_QueryResult()`
* `Raven.Abstractions.Data.FacetResults GetFacets(string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults GetFacets(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> GetFacetsLazy(string, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> GetFacetsLazy(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Lazy<IEnumerable<T>> Lazily()`
* `Lazy<IEnumerable<T>> Lazily(Action<IEnumerable<T>>)`
* `Raven.Client.IDocumentQuery<T> OrderBy<TValue>(Expression`1[])`
* `Raven.Client.IDocumentQuery<T> OrderByDescending<TValue>(Expression`1[])`
* `Raven.Client.IDocumentQuery<T> OrderByScore()`
* `Raven.Client.IDocumentQuery<T> OrderByScoreDescending()`
* `Raven.Client.IDocumentQuery<T> RelatesToShape(string, string, Raven.Abstractions.Indexing.SpatialRelation, Double)`
* `Raven.Client.IDocumentQuery<T> Search<TValue>(Expression<Func<T, TValue>>, string, Raven.Client.EscapeQueryOptions)`
* `Raven.Client.IDocumentQuery<T> SetQueryInputs(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Raven.Client.IDocumentQuery<TTransformerResult> SetResultTransformer<TTransformer, TTransformerResult>()`
* `Raven.Client.IDocumentQuery<T> SetTransformerParameters(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `T Single()`
* `T SingleOrDefault()`
* `Raven.Client.IDocumentQuery<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.IDocumentQuery<T> Spatial(string, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `string ToString()`
* `Raven.Client.IDocumentQuery<T> WhereBetween<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereBetweenOrEqual<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereEndsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue, bool)`
* `Raven.Client.IDocumentQuery<T> WhereGreaterThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereGreaterThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereIn<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `Raven.Client.IDocumentQuery<T> WhereLessThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereLessThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WhereStartsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `Raven.Client.IDocumentQuery<T> WithinRadiusOf(Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`
* `Raven.Client.IDocumentQuery<T> WithinRadiusOf(string, Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`

#### Added methods

* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(System.Action<Raven.Client.Documents.Queries.Facets.IFacetBuilder<T>>)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(System.Collections.Generic.IEnumerable<Raven.Client.Documents.Queries.Facets.Facet>)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateUsing(string)`
* `Raven.Client.Documents.Queries.QueryResult GetQueryResult()`

### DocumentSession
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Task Commit(string)`
* `Raven.Client.Connection.Operation DeleteByIndex<T, TIndexCreator>(Expression<Func<T, bool>>)`
* `Raven.Client.Connection.Operation DeleteByIndex<T>(string, Expression<Func<T, bool>>)`
* `Raven.Client.IDocumentQuery<T> DocumentQuery<T>()`
* `Raven.Client.Connection.IDatabaseCommands get_DatabaseCommands()`
* `string GetDocumentUrl(object)`
* `T Load<T>(ValueType)`
* `T[] Load<T>(ValueType[])`
* `T[] Load<T>(IEnumerable<ValueType>)`
* `TResult Load<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>)`
* `TResult Load<TResult>(string, string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TResult>(IEnumerable<string>, string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult Load<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TResult>(IEnumerable<string>, Type, Action<Raven.Client.ILoadConfiguration>)`
* `T[] LoadInternal<T>(String[], KeyValuePair`2[])`
* `T[] LoadInternal<T>(String[])`
* `TResult[] LoadStartingWith<TTransformer, TResult>(string, string, int, int, string, Raven.Client.RavenPagingInformation, Action<Raven.Client.ILoadConfiguration>, string)`
* `Raven.Client.IDocumentQuery<T> LuceneQuery<T, TIndexCreator>()`
* `Raven.Client.IDocumentQuery<T> LuceneQuery<T>(string, bool)`
* `Raven.Client.IDocumentQuery<T> LuceneQuery<T>()`
* `Lazy<TResult[]> MoreLikeThis<TResult>(Raven.Abstractions.Data.MoreLikeThisQuery)`
* `Raven.Abstractions.Data.FacetResults[] MultiFacetedSearch(Raven.Abstractions.Data.FacetQuery[])`
* `Task PrepareTransaction(string, Nullable<Guid>, Byte[])`
* `Raven.Client.Linq.IRavenQueryable<T> Query<T>()`
* `Task Rollback(string)`

#### Added methods

* `Raven.Client.Documents.Session.IAsyncDocumentQuery<T> AsyncQuery<T>(string, string, bool)`
* `bool Exists(string)`
* `Raven.Client.Documents.Session.IAttachmentsSessionOperations get_Attachments()`
* `Raven.Client.Documents.Session.IRevisionsSessionOperations get_Revisions()`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<object> Include(string)`
* `void LoadIntoStream(System.Collections.Generic.IEnumerable<string>, System.IO.Stream)`
* `void LoadStartingWithIntoStream(string, System.IO.Stream, string, int, int, string, string)`
* `Raven.Client.Documents.Session.IRawDocumentQuery<T> RawQuery<T>(string)`
* `void StreamInto<T>(Raven.Client.Documents.Session.IRawDocumentQuery<T>, System.IO.Stream)`
* `void StreamInto<T>(Raven.Client.Documents.Session.IDocumentQuery<T>, System.IO.Stream)`

### IAbstractDocumentQuery<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void AddOrder(string, bool)`
* `void AddOrder(string, bool, Type)`
* `void AlphaNumericOrdering(string, bool)`
* `void CustomSortUsing(string, bool)`
* `Raven.Client.Document.DocumentConvention get_DocumentConvention()`
* `KeyValuePair<string, string> GetLastEqualityTerm(bool)`
* `void Highlight(string, int, int, string)`
* `void Highlight(string, int, int, Raven.Client.FieldHighlightings&)`
* `void Highlight(string, string, int, int, Raven.Client.FieldHighlightings&)`
* `void SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(bool)`
* `void SetHighlighterTags(string, string)`
* `void SetHighlighterTags(String[], String[])`
* `void SetOriginalQueryType(Type)`
* `void SetResultTransformer(string)`
* `void WaitForNonStaleResults()`
* `void WaitForNonStaleResultsAsOf(DateTime)`
* `void WaitForNonStaleResultsAsOf(DateTime, TimeSpan)`
* `void WaitForNonStaleResultsAsOfNow()`
* `void WaitForNonStaleResultsAsOfNow(TimeSpan)`
* `void Where(string)`
* `void WhereBetweenOrEqual(string, object, object)`

#### Added methods

* `void AddFromAliasToWhereTokens(string)`
* `void AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `void AggregateUsing(string)`
* `string get_CollectionName()`
* `Raven.Client.Documents.Conventions.DocumentConventions get_Conventions()`
* `string get_IndexName()`
* `bool get_IsDynamicMapReduce()`
* `void GroupBy(string, System.String[])`
* `void GroupBy(System.ValueTuple<string, Raven.Client.Documents.Queries.GroupByMethod>, System.ValueTuple`2<System.String,Raven.Client.Documents.Queries.GroupByMethod>[])`
* `void GroupByCount(string)`
* `void GroupByKey(string, string)`
* `void GroupBySum(string, string)`
* `Raven.Client.Documents.Queries.MoreLikeThis.MoreLikeThisScope MoreLikeThis()`
* `void OrderByDescending(string, Raven.Client.Documents.Session.OrderingType)`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistance(string, System.Double, System.Double)`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistance(string, string)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistanceDescending(string, System.Double, System.Double)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistanceDescending(string, string)`
* `void OrderByScore()`
* `void OrderByScoreDescending()`
* `string ProjectionParameter(object)`
* `void Spatial(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void Spatial(string, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void SuggestUsing(Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `void WhereExists(string)`
* `void WhereNotEquals(string, object, bool)`
* `void WhereNotEquals(string, Raven.Client.Documents.Session.MethodCall, bool)`
* `void WhereNotEquals(Raven.Client.Documents.Session.WhereParams)`
* `void WhereRegex(string, string)`
* `void WhereTrue()`

### IAdvancedDocumentSessionOperations
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void ExplicitlyVersion(object)`
* `bool get_AllowNonAuthoritativeInformation()`
* `TimeSpan get_NonAuthoritativeInformationTimeout()`
* `Raven.Abstractions.Data.Etag GetEtagFor<T>(T)`
* `void MarkReadOnly(object)`
* `void set_AllowNonAuthoritativeInformation(bool)`
* `void set_NonAuthoritativeInformationTimeout(TimeSpan)`
* `void UnregisterMissing(string)`

#### Added methods

* `void add_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void add_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void add_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void add_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void Defer(Raven.Client.Documents.Commands.Batches.ICommandData[])`
* `Sparrow.Json.JsonOperationContext get_Context()`
* `Raven.Client.Documents.Session.EntityToBlittable get_EntityToBlittable()`
* `Raven.Client.Http.RequestExecutor get_RequestExecutor()`
* `string GetChangeVectorFor<T>(T)`
* `System.Threading.Tasks.Task<Raven.Client.Http.ServerNode> GetCurrentSessionNode()`
* `System.Nullable<System.DateTime> GetLastModifiedFor<T>(T)`
* `void remove_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void remove_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void remove_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void remove_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`

### IAsyncAdvancedSessionOperations
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Raven.Client.IAsyncDocumentQuery<T> AsyncDocumentQuery<T>()`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T, TIndexCreator>()`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T>(string, bool)`
* `Raven.Client.IAsyncDocumentQuery<T> AsyncLuceneQuery<T>()`
* `Task<Raven.Client.Connection.Operation> DeleteByIndexAsync<T, TIndexCreator>(Expression<Func<T, bool>>)`
* `Task<Raven.Client.Connection.Operation> DeleteByIndexAsync<T>(string, Expression<Func<T, bool>>)`
* `string GetDocumentUrl(object)`
* `Task<Raven.Json.Linq.RavenJObject> GetMetadataForAsync<T>(T)`
* `Task<IEnumerable<TResult>> LoadStartingWithAsync<TTransformer, TResult>(string, string, int, int, string, Raven.Client.RavenPagingInformation, Action<Raven.Client.ILoadConfiguration>, string, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults[]> MultiFacetedSearchAsync(Raven.Abstractions.Data.FacetQuery[])`
* `void Store(object, Raven.Abstractions.Data.Etag)`
* `void Store(object, Raven.Abstractions.Data.Etag, string)`
* `void Store(object)`
* `void Store(object, string)`
* `Task<Raven.Abstractions.Util.IAsyncEnumerator<Raven.Abstractions.Data.StreamResult<T>>> StreamAsync<T>(Raven.Abstractions.Data.Etag, int, int, Raven.Client.RavenPagingInformation, string, Dictionary<string, Raven.Json.Linq.RavenJToken>, CancellationToken)`
* `Task<Raven.Abstractions.Util.IAsyncEnumerator<Raven.Abstractions.Data.StreamResult<T>>> StreamAsync<T>(string, string, int, int, Raven.Client.RavenPagingInformation, string, string, Dictionary<string, Raven.Json.Linq.RavenJToken>, CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T> AsyncRawQuery<T>(string)`
* `System.Threading.Tasks.Task<bool> ExistsAsync(string)`
* `Raven.Client.Documents.Session.IAttachmentsSessionOperationsAsync get_Attachments()`
* `Raven.Client.Documents.Session.IRevisionsSessionOperationsAsync get_Revisions()`
* `void Increment<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Increment<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `System.Threading.Tasks.Task LoadIntoStreamAsync(System.Collections.Generic.IEnumerable<string>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task LoadStartingWithIntoStreamAsync(string, System.IO.Stream, string, int, int, string, string, System.Threading.CancellationToken)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`

### IAsyncDocumentQuery<T>
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Task<int> CountAsync(CancellationToken)`
* `Lazy<Task<int>> CountLazilyAsync(CancellationToken)`
* `Task<T> FirstAsync()`
* `Task<T> FirstOrDefaultAsync()`
* `string get_AsyncIndexQueried()`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(string, int, Nullable<int>, CancellationToken)`
* `Task<Raven.Abstractions.Data.FacetResults> GetFacetsAsync(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> GetFacetsLazyAsync(string, int, Nullable<int>, CancellationToken)`
* `Lazy<Task<Raven.Abstractions.Data.FacetResults>> GetFacetsLazyAsync(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>, CancellationToken)`
* `Raven.Abstractions.Data.IndexQuery GetIndexQuery(bool)`
* `Lazy<Task<IEnumerable<T>>> LazilyAsync(Action<IEnumerable<T>>)`
* `Task<Raven.Abstractions.Data.QueryResult> QueryResultAsync(CancellationToken)`
* `void SetQueryInputs(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Raven.Client.IAsyncDocumentQuery<TTransformerResult> SetResultTransformer<TTransformer, TTransformerResult>()`
* `Raven.Client.IAsyncDocumentQuery<T> SetTransformerParameters(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Task<T> SingleAsync()`
* `Task<T> SingleOrDefaultAsync()`
* `Raven.Client.IAsyncDocumentQuery<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.IAsyncDocumentQuery<T> Spatial(string, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Task<IList<T>> ToListAsync(CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(System.Action<Raven.Client.Documents.Queries.Facets.IFacetBuilder<T>>)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateBy(System.Collections.Generic.IEnumerable<Raven.Client.Documents.Queries.Facets.Facet>)`
* `Raven.Client.Documents.Queries.Facets.IAsyncAggregationDocumentQuery<T> AggregateUsing(string)`
* `string get_IndexName()`
* `bool get_IsDistinct()`
* `System.Threading.Tasks.Task<Raven.Client.Documents.Queries.QueryResult> GetQueryResultAsync(System.Threading.CancellationToken)`
* `Raven.Client.Documents.Session.IAsyncGroupByDocumentQuery<T> GroupBy(string, System.String[])`
* `Raven.Client.Documents.Session.IAsyncGroupByDocumentQuery<T> GroupBy(System.ValueTuple<string, Raven.Client.Documents.Queries.GroupByMethod>, System.ValueTuple`2<System.String,Raven.Client.Documents.Queries.GroupByMethod>[])`
* `Raven.Client.Documents.Session.IAsyncDocumentQuery<T> MoreLikeThis(System.Action<Raven.Client.Documents.Queries.MoreLikeThis.IMoreLikeThisBuilderForAsyncDocumentQuery<T>>)`
* `Raven.Client.Documents.Session.IAsyncDocumentQuery<TResult> OfType<TResult>()`
* `Raven.Client.Documents.Queries.Suggestions.IAsyncSuggestionDocumentQuery<T> SuggestUsing(Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `Raven.Client.Documents.Queries.Suggestions.IAsyncSuggestionDocumentQuery<T> SuggestUsing(System.Action<Raven.Client.Documents.Queries.Suggestions.ISuggestionBuilder<T>>)`

### IAsyncDocumentSession
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `void Delete(string)`
* `Task<T> LoadAsync<T>(ValueType, CancellationToken)`
* `Task<T[]> LoadAsync<T>(CancellationToken, ValueType[])`
* `Task<T[]> LoadAsync<T>(IEnumerable<ValueType>, CancellationToken)`
* `Task<TResult> LoadAsync<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult> LoadAsync<TResult>(string, string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>, string, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult> LoadAsync<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>, Type, Action<Raven.Client.ILoadConfiguration>, CancellationToken)`
* `Raven.Client.Linq.IRavenQueryable<T> Query<T, TIndexCreator>()`
* `Task StoreAsync(object, string, CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### IDocumentQuery<T>
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `int Count()`
* `Lazy<int> CountLazily()`
* `T First()`
* `T FirstOrDefault()`
* `string get_IndexQueried()`
* `Raven.Abstractions.Data.QueryResult get_QueryResult()`
* `Raven.Abstractions.Data.FacetResults GetFacets(string, int, Nullable<int>)`
* `Raven.Abstractions.Data.FacetResults GetFacets(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> GetFacetsLazy(string, int, Nullable<int>)`
* `Lazy<Raven.Abstractions.Data.FacetResults> GetFacetsLazy(List<Raven.Abstractions.Data.Facet>, int, Nullable<int>)`
* `Raven.Abstractions.Data.IndexQuery GetIndexQuery(bool)`
* `Lazy<IEnumerable<T>> Lazily()`
* `Lazy<IEnumerable<T>> Lazily(Action<IEnumerable<T>>)`
* `Raven.Client.IDocumentQuery<T> SetQueryInputs(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `Raven.Client.IDocumentQuery<TTransformerResult> SetResultTransformer<TTransformer, TTransformerResult>()`
* `Raven.Client.IDocumentQuery<T> SetTransformerParameters(Dictionary<string, Raven.Json.Linq.RavenJToken>)`
* `T Single()`
* `T SingleOrDefault()`
* `Raven.Client.IDocumentQuery<T> Spatial(Expression<Func<T, object>>, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.IDocumentQuery<T> Spatial(string, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`

#### Added methods

* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(System.Action<Raven.Client.Documents.Queries.Facets.IFacetBuilder<T>>)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateBy(System.Collections.Generic.IEnumerable<Raven.Client.Documents.Queries.Facets.Facet>)`
* `Raven.Client.Documents.Queries.Facets.IAggregationDocumentQuery<T> AggregateUsing(string)`
* `string get_IndexName()`
* `Raven.Client.Documents.Queries.QueryResult GetQueryResult()`
* `Raven.Client.Documents.Session.IGroupByDocumentQuery<T> GroupBy(string, System.String[])`
* `Raven.Client.Documents.Session.IGroupByDocumentQuery<T> GroupBy(System.ValueTuple<string, Raven.Client.Documents.Queries.GroupByMethod>, System.ValueTuple`2<System.String,Raven.Client.Documents.Queries.GroupByMethod>[])`
* `Raven.Client.Documents.Session.IDocumentQuery<T> MoreLikeThis(System.Action<Raven.Client.Documents.Queries.MoreLikeThis.IMoreLikeThisBuilderForDocumentQuery<T>>)`
* `Raven.Client.Documents.Session.IDocumentQuery<TResult> OfType<TResult>()`
* `Raven.Client.Documents.Queries.Suggestions.ISuggestionDocumentQuery<T> SuggestUsing(Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `Raven.Client.Documents.Queries.Suggestions.ISuggestionDocumentQuery<T> SuggestUsing(System.Action<Raven.Client.Documents.Queries.Suggestions.ISuggestionBuilder<T>>)`

### IDocumentQueryBase<T, TSelf>
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `TSelf AddOrder(string, bool, Type)`
* `void AfterQueryExecuted(Action<Raven.Abstractions.Data.QueryResult>)`
* `void AfterStreamExecuted(Raven.Client.AfterStreamExecutedDelegate)`
* `TSelf AlphaNumericOrdering(string, bool)`
* `TSelf AlphaNumericOrdering<TResult>(Expression<Func<TResult, object>>, bool)`
* `TSelf AndAlso()`
* `TSelf BeforeQueryExecution(Action<Raven.Abstractions.Data.IndexQuery>)`
* `TSelf CloseSubclause()`
* `TSelf ContainsAll(string, IEnumerable<object>)`
* `TSelf ContainsAll<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `TSelf ContainsAny(string, IEnumerable<object>)`
* `TSelf ContainsAny<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `TSelf CustomSortUsing(string, bool)`
* `TSelf ExplainScores()`
* `Raven.Client.Document.DocumentConvention get_DocumentConvention()`
* `TSelf get_Not()`
* `KeyValuePair<string, string> GetLastEqualityTerm(bool)`
* `TSelf Highlight(string, int, int, string)`
* `TSelf Highlight(string, int, int, Raven.Client.FieldHighlightings&)`
* `TSelf Highlight(string, string, int, int, Raven.Client.FieldHighlightings&)`
* `TSelf Highlight<TValue>(Expression<Func<T, TValue>>, int, int, Expression<Func<T, IEnumerable>>)`
* `TSelf Highlight<TValue>(Expression<Func<T, TValue>>, int, int, Raven.Client.FieldHighlightings&)`
* `TSelf Highlight<TValue>(Expression<Func<T, TValue>>, Expression<Func<T, TValue>>, int, int, Raven.Client.FieldHighlightings&)`
* `void InvokeAfterQueryExecuted(Raven.Abstractions.Data.QueryResult)`
* `void InvokeAfterStreamExecuted(Raven.Json.Linq.RavenJObject&)`
* `void NegateNext()`
* `TSelf NoCaching()`
* `TSelf NoTracking()`
* `TSelf OpenSubclause()`
* `TSelf OrElse()`
* `TSelf RelatesToShape(string, string, Raven.Abstractions.Indexing.SpatialRelation, Double)`
* `TSelf Search(string, string, Raven.Client.EscapeQueryOptions)`
* `TSelf Search<TValue>(Expression<Func<T, TValue>>, string, Raven.Client.EscapeQueryOptions)`
* `TSelf SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(bool)`
* `TSelf SetHighlighterTags(string, string)`
* `TSelf SetHighlighterTags(String[], String[])`
* `TSelf SetResultTransformer(string)`
* `TSelf ShowTimings()`
* `TSelf Skip(int)`
* `TSelf SortByDistance()`
* `TSelf SortByDistance(Double, Double)`
* `TSelf SortByDistance(Double, Double, string)`
* `TSelf Statistics(Raven.Client.RavenQueryStatistics&)`
* `TSelf Take(int)`
* `TSelf UsingDefaultField(string)`
* `TSelf UsingDefaultOperator(Raven.Abstractions.Data.QueryOperator)`
* `TSelf WaitForNonStaleResults()`
* `TSelf WaitForNonStaleResults(TimeSpan)`
* `TSelf WaitForNonStaleResultsAsOf(DateTime)`
* `TSelf WaitForNonStaleResultsAsOf(DateTime, TimeSpan)`
* `TSelf WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag)`
* `TSelf WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag, TimeSpan)`
* `TSelf WaitForNonStaleResultsAsOfLastWrite()`
* `TSelf WaitForNonStaleResultsAsOfLastWrite(TimeSpan)`
* `TSelf WaitForNonStaleResultsAsOfNow()`
* `TSelf WaitForNonStaleResultsAsOfNow(TimeSpan)`
* `TSelf Where(string)`
* `TSelf WhereBetween(string, object, object)`
* `TSelf WhereBetween<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `TSelf WhereBetweenOrEqual(string, object, object)`
* `TSelf WhereBetweenOrEqual<TValue>(Expression<Func<T, TValue>>, TValue, TValue)`
* `TSelf WhereEndsWith(string, object)`
* `TSelf WhereEndsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereEquals(string, object)`
* `TSelf WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereEquals(string, object, bool)`
* `TSelf WhereEquals<TValue>(Expression<Func<T, TValue>>, TValue, bool)`
* `TSelf WhereEquals(Raven.Client.WhereParams)`
* `TSelf WhereGreaterThan(string, object)`
* `TSelf WhereGreaterThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereGreaterThanOrEqual(string, object)`
* `TSelf WhereGreaterThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereIn(string, IEnumerable<object>)`
* `TSelf WhereIn<TValue>(Expression<Func<T, TValue>>, IEnumerable<TValue>)`
* `TSelf WhereLessThan(string, object)`
* `TSelf WhereLessThan<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereLessThanOrEqual(string, object)`
* `TSelf WhereLessThanOrEqual<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WhereStartsWith(string, object)`
* `TSelf WhereStartsWith<TValue>(Expression<Func<T, TValue>>, TValue)`
* `TSelf WithinRadiusOf(Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`
* `TSelf WithinRadiusOf(string, Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits)`

#### Added methods

* `TSelf OrderBy<TValue>(System.Linq.Expressions.Expression<System.Func<T, TValue>>, Raven.Client.Documents.Session.OrderingType)`
* `TSelf OrderBy<TValue>(System.Linq.Expressions.Expression`1<System.Func`2<T,TValue>>[])`
* `TSelf OrderByDescending<TValue>(System.Linq.Expressions.Expression<System.Func<T, TValue>>, Raven.Client.Documents.Session.OrderingType)`
* `TSelf OrderByDescending<TValue>(System.Linq.Expressions.Expression`1<System.Func`2<T,TValue>>[])`
* `TSelf OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `TSelf OrderByDistance(System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, System.Double, System.Double)`
* `TSelf OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `TSelf OrderByDistance(System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, string)`
* `TSelf OrderByDistance(System.Linq.Expressions.Expression<System.Func<T, object>>, System.Double, System.Double)`
* `TSelf OrderByDistance(string, System.Double, System.Double)`
* `TSelf OrderByDistance(System.Linq.Expressions.Expression<System.Func<T, object>>, string)`
* `TSelf OrderByDistance(string, string)`
* `TSelf OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `TSelf OrderByDistanceDescending(System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, System.Double, System.Double)`
* `TSelf OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `TSelf OrderByDistanceDescending(System.Func<Raven.Client.Documents.Queries.Spatial.DynamicSpatialFieldFactory<T>, Raven.Client.Documents.Queries.Spatial.DynamicSpatialField>, string)`
* `TSelf OrderByDistanceDescending(System.Linq.Expressions.Expression<System.Func<T, object>>, System.Double, System.Double)`
* `TSelf OrderByDistanceDescending(string, System.Double, System.Double)`
* `TSelf OrderByDistanceDescending(System.Linq.Expressions.Expression<System.Func<T, object>>, string)`
* `TSelf OrderByDistanceDescending(string, string)`

### IDocumentQueryCustomization
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Raven.Client.IDocumentQueryCustomization AddOrder(string, bool)`
* `Raven.Client.IDocumentQueryCustomization AddOrder<TResult>(Expression<Func<TResult, object>>, bool)`
* `Raven.Client.IDocumentQueryCustomization AddOrder(string, bool, Type)`
* `Raven.Client.IDocumentQueryCustomization AlphaNumericOrdering(string, bool)`
* `Raven.Client.IDocumentQueryCustomization AlphaNumericOrdering<TResult>(Expression<Func<TResult, object>>, bool)`
* `Raven.Client.IDocumentQueryCustomization BeforeQueryExecution(Action<Raven.Abstractions.Data.IndexQuery>)`
* `Raven.Client.IDocumentQueryCustomization CustomSortUsing(string)`
* `Raven.Client.IDocumentQueryCustomization CustomSortUsing(string, bool)`
* `Raven.Client.IDocumentQueryCustomization Highlight(string, int, int, string)`
* `Raven.Client.IDocumentQueryCustomization Highlight(string, int, int, Raven.Client.FieldHighlightings&)`
* `Raven.Client.IDocumentQueryCustomization Highlight(string, string, int, int, Raven.Client.FieldHighlightings&)`
* `Raven.Client.IDocumentQueryCustomization Include<TResult>(Expression<Func<TResult, object>>)`
* `Raven.Client.IDocumentQueryCustomization Include<TResult, TInclude>(Expression<Func<TResult, object>>)`
* `Raven.Client.IDocumentQueryCustomization Include(string)`
* `Raven.Client.IDocumentQueryCustomization RelatesToShape(string, string, Raven.Abstractions.Indexing.SpatialRelation, Double)`
* `Raven.Client.IDocumentQueryCustomization SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer(bool)`
* `Raven.Client.IDocumentQueryCustomization SetHighlighterTags(string, string)`
* `Raven.Client.IDocumentQueryCustomization SetHighlighterTags(String[], String[])`
* `Raven.Client.IDocumentQueryCustomization ShowTimings()`
* `Raven.Client.IDocumentQueryCustomization SortByDistance()`
* `Raven.Client.IDocumentQueryCustomization SortByDistance(Double, Double)`
* `Raven.Client.IDocumentQueryCustomization SortByDistance(Double, Double, string)`
* `Raven.Client.IDocumentQueryCustomization Spatial(string, Func<Raven.Client.Spatial.SpatialCriteriaFactory, Raven.Client.Spatial.SpatialCriteria>)`
* `Raven.Client.IDocumentQueryCustomization TransformResults(Func<Raven.Abstractions.Data.IndexQuery, IEnumerable<object>, IEnumerable<object>>)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResults(TimeSpan)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOf(DateTime)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOf(DateTime, TimeSpan)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOf(Raven.Abstractions.Data.Etag, TimeSpan)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOfLastWrite()`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOfLastWrite(TimeSpan)`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOfNow()`
* `Raven.Client.IDocumentQueryCustomization WaitForNonStaleResultsAsOfNow(TimeSpan)`
* `Raven.Client.IDocumentQueryCustomization WithinRadiusOf(Double, Double, Double, Double)`
* `Raven.Client.IDocumentQueryCustomization WithinRadiusOf(string, Double, Double, Double, Double)`
* `Raven.Client.IDocumentQueryCustomization WithinRadiusOf(Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits, Double)`
* `Raven.Client.IDocumentQueryCustomization WithinRadiusOf(string, Double, Double, Double, Raven.Abstractions.Indexing.SpatialUnits, Double)`

#### Added methods

* `Raven.Client.Documents.Session.IDocumentQueryCustomization AfterQueryExecuted(System.Action<Raven.Client.Documents.Queries.QueryResult>)`
* `Raven.Client.Documents.Session.IDocumentQueryCustomization AfterStreamExecuted(System.Action<Sparrow.Json.BlittableJsonReaderObject>)`
* `Raven.Client.Documents.Session.IDocumentQueryCustomization BeforeQueryExecuted(System.Action<Raven.Client.Documents.Queries.IndexQuery>)`
* `Raven.Client.Documents.Session.Operations.QueryOperation get_QueryOperation()`

### IDocumentSession
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `T Load<T>(ValueType)`
* `T[] Load<T>(ValueType[])`
* `T[] Load<T>(IEnumerable<ValueType>)`
* `TResult Load<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>)`
* `TResult Load<TResult>(string, string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TResult>(IEnumerable<string>, string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult Load<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TResult>(IEnumerable<string>, Type, Action<Raven.Client.ILoadConfiguration>)`
* `Raven.Client.Linq.IRavenQueryable<T> Query<T, TIndexCreator>()`
* `void Store(object, string)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, string>>)`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### InMemoryDocumentSessionOperations
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `Task Commit(string)`
* `object ConvertToEntity(Type, string, Raven.Json.Linq.RavenJObject, Raven.Json.Linq.RavenJObject, bool)`
* `string CreateDynamicIndexName<T>()`
* `void ExplicitlyVersion(object)`
* `bool get_AllowNonAuthoritativeInformation()`
* `Raven.Client.Document.EntityToJson get_EntityToJson()`
* `Raven.Client.Document.DocumentSessionListeners get_Listeners()`
* `TimeSpan get_NonAuthoritativeInformationTimeout()`
* `Guid get_ResourceManagerId()`
* `Raven.Abstractions.Data.Etag GetEtagFor<T>(T)`
* `void MarkReadOnly(object)`
* `Task Rollback(string)`
* `void set_AllowNonAuthoritativeInformation(bool)`
* `void set_NonAuthoritativeInformationTimeout(TimeSpan)`
* `void Store(object, Raven.Abstractions.Data.Etag, string)`
* `Task StoreAsync(object, string, CancellationToken)`
* `void TrackIncludedDocument(Raven.Abstractions.Data.JsonDocument)`

#### Added methods

* `void add_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void add_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void add_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void add_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void AssertNotDisposed()`
* `bool CheckIfIdAlreadyIncluded(System.String[], System.Collections.Generic.IEnumerable<string>)`
* `void Defer(Raven.Client.Documents.Commands.Batches.ICommandData[])`
* `void Dispose()`
* `Sparrow.Json.JsonOperationContext get_Context()`
* `int get_DeferredCommandsCount()`
* `Raven.Client.Documents.Session.EntityToBlittable get_EntityToBlittable()`
* `Raven.Client.Http.RequestExecutor get_RequestExecutor()`
* `string GetChangeVectorFor<T>(T)`
* `System.Threading.Tasks.Task<Raven.Client.Http.ServerNode> GetCurrentSessionNode()`
* `System.Nullable<System.DateTime> GetLastModifiedFor<T>(T)`
* `void Increment<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Increment<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void OnAfterSaveChangesInvoke(Raven.Client.Documents.Session.AfterSaveChangesEventArgs)`
* `void OnBeforeQueryExecutedInvoke(Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `void remove_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void remove_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void remove_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void remove_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`

### AsyncMultiLoaderWithInclude<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session.Loaders`
#### Removed methods

* `Task<TResult[]> LoadAsync<TResult>(String[])`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>)`
* `Task<TResult> LoadAsync<TResult>(string)`
* `Task<TResult> LoadAsync<TResult>(ValueType)`
* `Task<TResult[]> LoadAsync<TResult>(ValueType[])`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<ValueType>)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.AsyncMultiLoaderWithInclude<T> Include<TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### IAsyncLazyLoaderWithInclude<T>
#### Namespace changed

* 3.x: `Raven.Client.Document.Batches`
* 4.0: `Raven.Client.Documents.Session.Loaders`
#### Removed methods

* `Lazy<Task<TResult[]>> LoadAsync<TResult>(String[])`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(IEnumerable<string>)`
* `Lazy<Task<TResult>> LoadAsync<TResult>(string)`
* `Lazy<Task<TResult>> LoadAsync<TResult>(ValueType)`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(ValueType[])`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(IEnumerable<ValueType>)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.IAsyncLazyLoaderWithInclude<T> Include(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### IAsyncLoaderWithInclude<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session.Loaders`
#### Removed methods

* `Task<TResult[]> LoadAsync<TResult>(String[])`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<string>)`
* `Task<TResult> LoadAsync<TResult>(string)`
* `Task<TResult> LoadAsync<TResult>(ValueType)`
* `Task<TResult[]> LoadAsync<TResult>(ValueType[])`
* `Task<TResult[]> LoadAsync<TResult>(IEnumerable<ValueType>)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.AsyncMultiLoaderWithInclude<T> Include(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.AsyncMultiLoaderWithInclude<T> Include<TResult>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### ILazyLoaderWithInclude<T>
#### Namespace changed

* 3.x: `Raven.Client.Document.Batches`
* 4.0: `Raven.Client.Documents.Session.Loaders`
#### Removed methods

* `Lazy<TResult[]> Load<TResult>(String[])`
* `Lazy<TResult[]> Load<TResult>(IEnumerable<string>)`
* `Lazy<TResult> Load<TResult>(string)`
* `Lazy<TResult> Load<TResult>(ValueType)`
* `Lazy<TResult[]> Load<TResult>(ValueType[])`
* `Lazy<TResult[]> Load<TResult>(IEnumerable<ValueType>)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.ILazyLoaderWithInclude<T> Include(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### ILoaderWithInclude<T>
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Session.Loaders`
#### Removed methods

* `TResult[] Load<TResult>(String[])`
* `TResult[] Load<TResult>(IEnumerable<string>)`
* `TResult Load<TResult>(string)`
* `TResult Load<TResult>(ValueType)`
* `TResult[] Load<TResult>(ValueType[])`
* `TResult[] Load<TResult>(IEnumerable<ValueType>)`
* `TResult Load<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>)`
* `TResult[] Load<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include<TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### IAsyncLazySessionOperations
#### Namespace changed

* 3.x: `Raven.Client.Document.Batches`
* 4.0: `Raven.Client.Documents.Session.Operations.Lazy`
#### Removed methods

* `Lazy<Task<TResult>> LoadAsync<TResult>(ValueType, CancellationToken)`
* `Lazy<Task<TResult>> LoadAsync<TResult>(ValueType, Action<TResult>, CancellationToken)`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(CancellationToken, ValueType[])`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(IEnumerable<ValueType>, CancellationToken)`
* `Lazy<Task<TResult[]>> LoadAsync<TResult>(IEnumerable<ValueType>, Action<TResult[]>, CancellationToken)`
* `Lazy<Task<TResult>> LoadAsync<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>, Action<TResult>, CancellationToken)`
* `Lazy<Task<TResult>> LoadAsync<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>, Action<TResult>, CancellationToken)`
* `Lazy<Task<TResult[]>> MoreLikeThisAsync<TResult>(Raven.Abstractions.Data.MoreLikeThisQuery, CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.IAsyncLazyLoaderWithInclude<TResult> Include<TResult>(System.Linq.Expressions.Expression<System.Func<TResult, System.Collections.Generic.IEnumerable<string>>>)`

### ILazyOperation
#### Namespace changed

* 3.x: `Raven.Client.Document.Batches`
* 4.0: `Raven.Client.Documents.Session.Operations.Lazy`
#### Removed methods

* `IDisposable EnterContext()`
* `void HandleResponses(Raven.Abstractions.Data.GetResponse[], Raven.Client.Shard.ShardStrategy)`

### ILazySessionOperations
#### Namespace changed

* 3.x: `Raven.Client.Document.Batches`
* 4.0: `Raven.Client.Documents.Session.Operations.Lazy`
#### Removed methods

* `Lazy<TResult> Load<TResult>(ValueType)`
* `Lazy<TResult> Load<TResult>(ValueType, Action<TResult>)`
* `Lazy<TResult[]> Load<TResult>(ValueType[])`
* `Lazy<TResult[]> Load<TResult>(IEnumerable<ValueType>)`
* `Lazy<TResult[]> Load<TResult>(IEnumerable<ValueType>, Action<TResult[]>)`
* `Lazy<TResult> Load<TTransformer, TResult>(string, Action<Raven.Client.ILoadConfiguration>, Action<TResult>)`
* `Lazy<TResult> Load<TResult>(string, Type, Action<Raven.Client.ILoadConfiguration>, Action<TResult>)`
* `Lazy<TResult[]> Load<TTransformer, TResult>(IEnumerable<string>, Action<Raven.Client.ILoadConfiguration>, Action<TResult>)`
* `Lazy<TResult[]> Load<TResult>(IEnumerable<string>, Type, Action<Raven.Client.ILoadConfiguration>, Action<TResult>)`
* `Lazy<TResult[]> MoreLikeThis<TResult>(Raven.Abstractions.Data.MoreLikeThisQuery)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.ILazyLoaderWithInclude<TResult> Include<TResult>(System.Linq.Expressions.Expression<System.Func<TResult, System.Collections.Generic.IEnumerable<string>>>)`

### QueryOperation
#### Namespace changed

* 3.x: `Raven.Client.Document.SessionOperations`
* 4.0: `Raven.Client.Documents.Session.Operations`
#### Removed methods

* `void ForceResult(Raven.Abstractions.Data.QueryResult)`
* `string get_IndexName()`
* `bool IsAcceptable(Raven.Abstractions.Data.QueryResult)`
* `bool ShouldQueryAgain(Exception)`

#### Added methods

* `Raven.Client.Documents.Commands.QueryCommand CreateRequest()`
* `void EnsureIsAcceptable(Raven.Client.Documents.Queries.QueryResult, bool, System.Diagnostics.Stopwatch, Raven.Client.Documents.Session.InMemoryDocumentSessionOperations)`
* `void EnsureIsAcceptableAndSaveResult(Raven.Client.Documents.Queries.QueryResult)`
* `void SetResult(Raven.Client.Documents.Queries.QueryResult)`

### WhereParams
#### Namespace changed

* 3.x: `Raven.Client`
* 4.0: `Raven.Client.Documents.Session`
#### Removed methods

* `bool get_IsAnalyzed()`
* `void set_IsAnalyzed(bool)`

#### Added methods

* `bool get_Exact()`
* `void set_Exact(bool)`

### DocumentSubscriptions
#### Namespace changed

* 3.x: `Raven.Client.Document`
* 4.0: `Raven.Client.Documents.Subscriptions`
#### Removed methods

* `Raven.Client.Document.Subscription<Raven.Json.Linq.RavenJObject> Open(long, Raven.Abstractions.Data.SubscriptionConnectionOptions, string)`
* `Raven.Client.Document.Subscription<T> Open<T>(long, Raven.Abstractions.Data.SubscriptionConnectionOptions, string)`
* `void Release(long, string)`

#### Added methods

* `string Create(Raven.Client.Documents.Subscriptions.SubscriptionCreationOptions, string)`
* `System.Threading.Tasks.Task<string> CreateAsync<T>(Raven.Client.Documents.Subscriptions.SubscriptionCreationOptions<T>, string)`
* `System.Threading.Tasks.Task<string> CreateAsync<T>(System.Linq.Expressions.Expression<System.Func<T, bool>>, Raven.Client.Documents.Subscriptions.SubscriptionCreationOptions, string)`
* `System.Threading.Tasks.Task<string> CreateAsync(Raven.Client.Documents.Subscriptions.SubscriptionCreationOptions, string)`
* `System.Threading.Tasks.Task DeleteAsync(string, string)`
* `void DropConnection(string, string)`
* `System.Threading.Tasks.Task DropConnectionAsync(string, string)`
* `System.Threading.Tasks.Task<System.Collections.Generic.List<Raven.Client.Documents.Subscriptions.SubscriptionState>> GetSubscriptionsAsync(int, int, string)`
* `Raven.Client.Documents.Subscriptions.SubscriptionState GetSubscriptionState(string, string)`
* `System.Threading.Tasks.Task<Raven.Client.Documents.Subscriptions.SubscriptionState> GetSubscriptionStateAsync(string, string)`
* `Raven.Client.Documents.Subscriptions.SubscriptionWorker<object> GetSubscriptionWorker(Raven.Client.Documents.Subscriptions.SubscriptionWorkerOptions, string)`
* `Raven.Client.Documents.Subscriptions.SubscriptionWorker<object> GetSubscriptionWorker(string, string)`
* `Raven.Client.Documents.Subscriptions.SubscriptionWorker<T> GetSubscriptionWorker<T>(Raven.Client.Documents.Subscriptions.SubscriptionWorkerOptions, string)`
* `Raven.Client.Documents.Subscriptions.SubscriptionWorker<T> GetSubscriptionWorker<T>(string, string)`

### ConflictException
#### Removed methods

* `String[] get_ConflictedVersionIds()`
* `Raven.Abstractions.Data.Etag get_Etag()`
* `void set_ConflictedVersionIds(String[])`
* `void set_Etag(Raven.Abstractions.Data.Etag)`

### IndexCreation
#### Removed methods

* `Raven.Client.Documents.Indexes.IndexDefinition[] CreateIndexesToAdd(System.Collections.Generic.IEnumerable<Raven.Client.Documents.Indexes.AbstractIndexCreationTask>, Raven.Client.Documents.Conventions.DocumentConventions)`

### SpatialCriteria
#### Removed methods

* `void .ctor(Raven.Client.Documents.Indexes.Spatial.SpatialRelation, System.Double)`

#### Added methods

* `Raven.Client.Documents.Session.Tokens.QueryToken ToQueryToken(string, System.Func<object, string>)`

### SpatialCriteriaFactory
#### Removed methods

* `void .ctor()`

#### Added fields

* `Raven.Client.Documents.Queries.Spatial.SpatialCriteriaFactory Instance`

### AbstractDocumentQuery<T, TSelf>
#### Removed methods

* `void WithinRadiusOf(string, System.Double, System.Double, System.Double, System.Nullable<Raven.Client.Documents.Indexes.Spatial.SpatialUnits>, System.Double)`

#### Added methods

* `void AddFromAliasToWhereTokens(string)`
* `void AddParameter(string, object)`
* `void AggregateBy(Raven.Client.Documents.Queries.Facets.FacetBase)`
* `void AggregateUsing(string)`
* `Raven.Client.Documents.Session.IDocumentQueryCustomization BeforeQueryExecuted(System.Action<Raven.Client.Documents.Queries.IndexQuery>)`
* `Raven.Client.Documents.Session.IAsyncDocumentSession get_AsyncSession()`
* `string get_CollectionName()`
* `Raven.Client.Documents.Conventions.DocumentConventions get_Conventions()`
* `string get_IndexName()`
* `bool get_IsDynamicMapReduce()`
* `Raven.Client.Documents.Session.Operations.QueryOperation get_QueryOperation()`
* `void GroupBy(string, System.String[])`
* `void GroupBy(System.ValueTuple<string, Raven.Client.Documents.Queries.GroupByMethod>, System.ValueTuple`2<System.String,Raven.Client.Documents.Queries.GroupByMethod>[])`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, T>> GroupBy<TKey>(System.Func<T, TKey>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(System.Func<T, TKey>, System.Func<T, TElement>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, T>> GroupBy<TKey>(System.Func<T, TKey>, System.Collections.Generic.IEqualityComparer<TKey>)`
* `System.Collections.Generic.IEnumerable<System.Linq.IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(System.Func<T, TKey>, System.Func<T, TElement>, System.Collections.Generic.IEqualityComparer<TKey>)`
* `void GroupByCount(string)`
* `void GroupByKey(string, string)`
* `void GroupBySum(string, string)`
* `Raven.Client.Documents.Queries.MoreLikeThis.MoreLikeThisScope MoreLikeThis()`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistance(string, System.Double, System.Double)`
* `void OrderByDistance(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistance(string, string)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, System.Double, System.Double)`
* `void OrderByDistanceDescending(string, System.Double, System.Double)`
* `void OrderByDistanceDescending(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, string)`
* `void OrderByDistanceDescending(string, string)`
* `void OrderByScore()`
* `void OrderByScoreDescending()`
* `string ProjectionParameter(object)`
* `void RawQuery(string)`
* `void Spatial(Raven.Client.Documents.Queries.Spatial.DynamicSpatialField, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void Spatial(string, Raven.Client.Documents.Queries.Spatial.SpatialCriteria)`
* `void SuggestUsing(Raven.Client.Documents.Queries.Suggestions.SuggestionBase)`
* `void WhereExists(string)`
* `void WhereLucene(string, string)`
* `void WhereNotEquals(string, object, bool)`
* `void WhereNotEquals(string, Raven.Client.Documents.Session.MethodCall, bool)`
* `void WhereNotEquals(Raven.Client.Documents.Session.WhereParams)`
* `void WhereRegex(string, string)`
* `void WhereTrue()`

### AsyncDocumentSession
#### Removed methods

* `System.Threading.Tasks.Task LoadAsyncInternal(System.String[], System.IO.Stream, Raven.Client.Documents.Session.Operations.LoadOperation, System.Threading.CancellationToken)`

#### Added methods

* `Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T> AsyncRawQuery<T>(string)`
* `System.Threading.Tasks.Task<bool> ExistsAsync(string)`
* `Raven.Client.Documents.Session.IAttachmentsSessionOperationsAsync get_Attachments()`
* `Raven.Client.Documents.Session.IRevisionsSessionOperationsAsync get_Revisions()`
* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.IAsyncLoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `System.Threading.Tasks.Task LoadIntoStreamAsync(System.Collections.Generic.IEnumerable<string>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task LoadStartingWithIntoStreamAsync(string, System.IO.Stream, string, int, int, string, string, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncRawDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`
* `System.Threading.Tasks.Task StreamIntoAsync<T>(Raven.Client.Documents.Session.IAsyncDocumentQuery<T>, System.IO.Stream, System.Threading.CancellationToken)`

### DocumentSession
#### Removed methods

* `void LoadInternal(System.String[], Raven.Client.Documents.Session.Operations.LoadOperation, System.IO.Stream)`

#### Added methods

* `Raven.Client.Documents.Session.IAsyncDocumentQuery<T> AsyncQuery<T>(string, string, bool)`
* `bool Exists(string)`
* `Raven.Client.Documents.Session.IAttachmentsSessionOperations get_Attachments()`
* `Raven.Client.Documents.Session.IRevisionsSessionOperations get_Revisions()`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<T> Include<T, TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`
* `Raven.Client.Documents.Session.Loaders.ILoaderWithInclude<object> Include(string)`
* `void LoadIntoStream(System.Collections.Generic.IEnumerable<string>, System.IO.Stream)`
* `void LoadStartingWithIntoStream(string, System.IO.Stream, string, int, int, string, string)`
* `Raven.Client.Documents.Session.IRawDocumentQuery<T> RawQuery<T>(string)`
* `void StreamInto<T>(Raven.Client.Documents.Session.IRawDocumentQuery<T>, System.IO.Stream)`
* `void StreamInto<T>(Raven.Client.Documents.Session.IDocumentQuery<T>, System.IO.Stream)`

### InMemoryDocumentSessionOperations
#### Removed methods

* `void Dispose(bool)`
* `object TrackEntity(System.Type, Raven.Client.Documents.Session.DocumentInfo)`

#### Added methods

* `void add_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void add_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void add_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void add_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`
* `void AssertNotDisposed()`
* `bool CheckIfIdAlreadyIncluded(System.String[], System.Collections.Generic.IEnumerable<string>)`
* `void Defer(Raven.Client.Documents.Commands.Batches.ICommandData[])`
* `void Dispose()`
* `Sparrow.Json.JsonOperationContext get_Context()`
* `int get_DeferredCommandsCount()`
* `Raven.Client.Documents.Session.EntityToBlittable get_EntityToBlittable()`
* `Raven.Client.Http.RequestExecutor get_RequestExecutor()`
* `string GetChangeVectorFor<T>(T)`
* `System.Threading.Tasks.Task<Raven.Client.Http.ServerNode> GetCurrentSessionNode()`
* `System.Nullable<System.DateTime> GetLastModifiedFor<T>(T)`
* `void Increment<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Increment<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void OnAfterSaveChangesInvoke(Raven.Client.Documents.Session.AfterSaveChangesEventArgs)`
* `void OnBeforeQueryExecutedInvoke(Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, U>>, U)`
* `void Patch<T, U>(T, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `void Patch<T, U>(string, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<U>>>, System.Linq.Expressions.Expression<System.Func<Raven.Client.Documents.Session.JavaScriptArray<U>, object>>)`
* `void remove_OnAfterSaveChanges(System.EventHandler<Raven.Client.Documents.Session.AfterSaveChangesEventArgs>)`
* `void remove_OnBeforeDelete(System.EventHandler<Raven.Client.Documents.Session.BeforeDeleteEventArgs>)`
* `void remove_OnBeforeQueryExecuted(System.EventHandler<Raven.Client.Documents.Session.BeforeQueryExecutedEventArgs>)`
* `void remove_OnBeforeStore(System.EventHandler<Raven.Client.Documents.Session.BeforeStoreEventArgs>)`

### AsyncMultiLoaderWithInclude<T>
#### Removed methods

* `void .ctor(Raven.Client.Documents.Session.IAsyncDocumentSessionImpl)`

#### Added methods

* `Raven.Client.Documents.Session.Loaders.AsyncMultiLoaderWithInclude<T> Include<TInclude>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<string>>>)`

### QueryOperation
#### Removed methods

* `T Deserialize<T>(string, Sparrow.Json.BlittableJsonReaderObject, Sparrow.Json.BlittableJsonReaderObject, Raven.Client.Documents.Session.Tokens.FieldsToFetchToken, bool, Raven.Client.Documents.Session.InMemoryDocumentSessionOperations)`

#### Added methods

* `Raven.Client.Documents.Commands.QueryCommand CreateRequest()`
* `void EnsureIsAcceptable(Raven.Client.Documents.Queries.QueryResult, bool, System.Diagnostics.Stopwatch, Raven.Client.Documents.Session.InMemoryDocumentSessionOperations)`
* `void EnsureIsAcceptableAndSaveResult(Raven.Client.Documents.Queries.QueryResult)`
* `void SetResult(Raven.Client.Documents.Queries.QueryResult)`

### ConflictException
#### Removed methods

* `void .ctor()`
* `void .ctor(string)`
* `void .ctor(string, System.Exception)`

## The following types are no longer available

#### Raven.Client.AfterStreamExecutedDelegate
#### Raven.Client.Bundles.MoreLikeThis.MoreLikeThisExtensions
#### Raven.Client.Bundles.Versioning.VersioningExtensions
#### Raven.Client.Changes.ConnectionStateBase
#### Raven.Client.Changes.IConnectableChanges
#### Raven.Client.Changes.IObservableWithTask<T>
#### Raven.Client.Changes.RemoteChangesClientBase<TChangesApi, TConnectionState, TConventions>
#### Raven.Client.Changes.RemoteDatabaseChanges
#### Raven.Client.Changes.TaskedObservable<T, TConnectionState>
#### Raven.Client.Connection.AdminRequestCreator
#### Raven.Client.Connection.AdminServerClient
#### Raven.Client.Connection.Async.AsyncAdminServerClient
#### Raven.Client.Connection.Async.AsyncDatabaseCommandsExtensions
#### Raven.Client.Connection.Async.AsyncServerClient
#### Raven.Client.Connection.Async.AsyncServerClientBase<TConvention, TReplicationInformer>
#### Raven.Client.Connection.Async.IAsyncAdminDatabaseCommands
#### Raven.Client.Connection.Async.IAsyncDatabaseCommands
#### Raven.Client.Connection.Async.IAsyncGlobalAdminDatabaseCommands
#### Raven.Client.Connection.Async.IAsyncInfoDatabaseCommands
#### Raven.Client.Connection.Async.NameAndCount
#### Raven.Client.Connection.CachedRequest
#### Raven.Client.Connection.CachedRequestOp
#### Raven.Client.Connection.CompressedStreamContent
#### Raven.Client.Connection.ConnectionOptions
#### Raven.Client.Connection.CreateHttpJsonRequestParams
#### Raven.Client.Connection.DocumentConventionJsonExtensions
#### Raven.Client.Connection.FailoverStatusChangedEventArgs
#### Raven.Client.Connection.HttpConnectionHelper
#### Raven.Client.Connection.HttpContentExtentions
#### Raven.Client.Connection.HttpJsonRequestFactory
#### Raven.Client.Connection.IAdminDatabaseCommands
#### Raven.Client.Connection.IDatabaseCommands
#### Raven.Client.Connection.IDocumentStoreReplicationInformer
#### Raven.Client.Connection.IGlobalAdminDatabaseCommands
#### Raven.Client.Connection.IInfoDatabaseCommands
#### Raven.Client.Connection.Implementation.HttpJsonRequest
#### Raven.Client.Connection.IReplicationInformerBase
#### Raven.Client.Connection.IReplicationInformerBase<TClient>
#### Raven.Client.Connection.ObservableLineStream
#### Raven.Client.Connection.OperationMetadata
#### Raven.Client.Connection.Profiling.IHoldProfilingInformation
#### Raven.Client.Connection.Profiling.ProfilingContext
#### Raven.Client.Connection.Profiling.ProfilingInformation
#### Raven.Client.Connection.Profiling.RequestResultArgs
#### Raven.Client.Connection.Profiling.RequestStatus
#### Raven.Client.Connection.RavenTransactionAccessor
#### Raven.Client.Connection.RavenUrlExtensions
#### Raven.Client.Connection.ReplicationInformer
#### Raven.Client.Connection.ReplicationInformerBase<TClient>
#### Raven.Client.Connection.ReplicationInformerLocalCache
#### Raven.Client.Connection.Request.AsyncOperationResult<T>
#### Raven.Client.Connection.Request.ClusterAwareRequestExecuter
#### Raven.Client.Connection.Request.FailureCounter
#### Raven.Client.Connection.Request.FailureCounters
#### Raven.Client.Connection.Request.FailureTimeSeries
#### Raven.Client.Connection.Request.FailureTimeSeries1
#### Raven.Client.Connection.Request.IRequestExecuter
#### Raven.Client.Connection.Request.ReplicationAwareRequestExecuter
#### Raven.Client.Connection.Request.RequestExecuterSelector
#### Raven.Client.Connection.SerializationHelper
#### Raven.Client.Connection.ServerClient
#### Raven.Client.ConventionBase
#### Raven.Client.Converters.GuidConverter
#### Raven.Client.Converters.Int32Converter
#### Raven.Client.Converters.Int64Converter
#### Raven.Client.Converters.ITypeConverter
#### Raven.Client.Counters.Changes.CountersChangesClient
#### Raven.Client.Counters.Changes.CountersConnectionState
#### Raven.Client.Counters.Changes.ICountersChanges
#### Raven.Client.Counters.CountersConvention
#### Raven.Client.Counters.CounterStore
#### Raven.Client.Counters.ICounterStore
#### Raven.Client.Counters.Operations.CounterOperationsBase
#### Raven.Client.Counters.Operations.CountersBatchOperation
#### Raven.Client.Counters.Replication.CounterReplicationInformer
#### Raven.Client.Document.AfterAcknowledgment
#### Raven.Client.Document.AfterBatch
#### Raven.Client.Document.Async.AsyncDocumentKeyGeneration
#### Raven.Client.Document.AsyncDocumentSubscriptions
#### Raven.Client.Document.AsyncHiLoKeyGenerator
#### Raven.Client.Document.AsyncMultiDatabaseHiLoKeyGenerator
#### Raven.Client.Document.AsyncMultiTypeHiLoKeyGenerator
#### Raven.Client.Document.AsyncShardedDocumentQuery<T>
#### Raven.Client.Document.Batches.LazyFacetsOperation
#### Raven.Client.Document.Batches.LazyMoreLikeThisOperation<T>
#### Raven.Client.Document.Batches.LazyMultiLoadOperation<T>
#### Raven.Client.Document.Batches.LazySuggestOperation
#### Raven.Client.Document.Batches.LazyTransformerLoadOperation<T>
#### Raven.Client.Document.BeforeAcknowledgment
#### Raven.Client.Document.BeforeBatch
#### Raven.Client.Document.ChunkedRemoteBulkInsertOperation
#### Raven.Client.Document.ConsistencyOptions
#### Raven.Client.Document.DocumentConvention
#### Raven.Client.Document.DocumentSessionListeners
#### Raven.Client.Document.DTC.IsolatedStorageTransactionRecoveryContext
#### Raven.Client.Document.DTC.IsolatedStorageTransactionRecoveryStorage
#### Raven.Client.Document.DTC.ITransactionRecoveryStorage
#### Raven.Client.Document.DTC.ITransactionRecoveryStorageContext
#### Raven.Client.Document.DTC.LocalDirectoryTransactionRecoveryStorage
#### Raven.Client.Document.DTC.PendingTransactionRecovery
#### Raven.Client.Document.DTC.VolatileOnlyTransactionRecoveryStorage
#### Raven.Client.Document.EntityToJson
#### Raven.Client.Document.HiLoKeyGenerator
#### Raven.Client.Document.HiLoKeyGeneratorBase
#### Raven.Client.Document.IAsyncReliableSubscriptions
#### Raven.Client.Document.ILowLevelBulkInsertOperation
#### Raven.Client.Document.IndexAndTransformerReplicationMode
#### Raven.Client.Document.IReliableSubscriptions
#### Raven.Client.Document.MultiDatabaseHiLoGenerator
#### Raven.Client.Document.MultiTypeHiLoKeyGenerator
#### Raven.Client.Document.OpenSessionOptions
#### Raven.Client.Document.QueryValueConvertionType
#### Raven.Client.Document.RavenClientEnlistment
#### Raven.Client.Document.RavenLoadConfiguration
#### Raven.Client.Document.RemoteBulkInsertOperation
#### Raven.Client.Document.ReplicationBehavior
#### Raven.Client.Document.SessionOperations.LoadTransformerOperation
#### Raven.Client.Document.SessionOperations.MultiLoadOperation
#### Raven.Client.Document.SessionOperations.ShardLoadOperation
#### Raven.Client.Document.ShardedBulkInsertOperation
#### Raven.Client.Document.ShardedDocumentQuery<T>
#### Raven.Client.Document.Subscription<T>
#### Raven.Client.Document.SubscriptionConnectionInterrupted
#### Raven.Client.DocumentToEntity
#### Raven.Client.EntityStored
#### Raven.Client.EntityToDocument
#### Raven.Client.EscapeQueryOptions
#### Raven.Client.Exceptions.NonAuthoritativeInformationException
#### Raven.Client.Exceptions.ReadVetoException
#### Raven.Client.Exceptions.ServerRequestError
#### Raven.Client.Extensions.AsyncExtensions
#### Raven.Client.Extensions.HttpJsonRequestExtensions
#### Raven.Client.Extensions.MultiDatabase
#### Raven.Client.Extensions.MultiTenancyExtensions
#### Raven.Client.Extensions.TaskExtensions2
#### Raven.Client.Extensions.Time
#### Raven.Client.FieldHighlightings
#### Raven.Client.FileSystem.AbstractFilesQuery<T, TSelf>
#### Raven.Client.FileSystem.AsyncFilesQuery<T>
#### Raven.Client.FileSystem.AsyncFilesServerClient
#### Raven.Client.FileSystem.AsyncFilesSession
#### Raven.Client.FileSystem.Bundles.Versioning.VersioningExtensions
#### Raven.Client.FileSystem.Changes.FilesChangesClient
#### Raven.Client.FileSystem.Changes.FilesConnectionState
#### Raven.Client.FileSystem.Connection.FilesReplicationInformer
#### Raven.Client.FileSystem.Connection.IAsyncFilesCommandsImpl
#### Raven.Client.FileSystem.Connection.IFilesReplicationInformer
#### Raven.Client.FileSystem.Extensions.FilesSynchronizationExtensions
#### Raven.Client.FileSystem.Extensions.FilesTenancyExtensions
#### Raven.Client.FileSystem.FilesConvention
#### Raven.Client.FileSystem.FilesQuery
#### Raven.Client.FileSystem.FilesQueryStatistics
#### Raven.Client.FileSystem.FilesSessionListeners
#### Raven.Client.FileSystem.FilesStore
#### Raven.Client.FileSystem.IAbstractFilesQuery<T>
#### Raven.Client.FileSystem.IAdvancedFilesSessionOperations
#### Raven.Client.FileSystem.IAsyncAdvancedFilesSessionOperations
#### Raven.Client.FileSystem.IAsyncFilesAdminCommands
#### Raven.Client.FileSystem.IAsyncFilesCommands
#### Raven.Client.FileSystem.IAsyncFilesConfigurationCommands
#### Raven.Client.FileSystem.IAsyncFilesOrderedQuery<T>
#### Raven.Client.FileSystem.IAsyncFilesOrderedQueryBase<T, TSelf>
#### Raven.Client.FileSystem.IAsyncFilesQuery<T>
#### Raven.Client.FileSystem.IAsyncFilesQueryBase<T, TSelf>
#### Raven.Client.FileSystem.IAsyncFilesSession
#### Raven.Client.FileSystem.IAsyncFilesStorageCommands
#### Raven.Client.FileSystem.IAsyncFilesSynchronizationCommands
#### Raven.Client.FileSystem.IFilesChanges
#### Raven.Client.FileSystem.IFilesStore
#### Raven.Client.FileSystem.Impl.UpdateMetadataOperation
#### Raven.Client.FileSystem.InMemoryFilesSessionOperations
#### Raven.Client.FileSystem.ISynchronizationServerClient
#### Raven.Client.FileSystem.Listeners.IFilesConflictListener
#### Raven.Client.FileSystem.Listeners.IFilesDeleteListener
#### Raven.Client.FileSystem.Listeners.IMetadataChangeListener
#### Raven.Client.FileSystem.OpenFilesSessionOptions
#### Raven.Client.FileSystem.Shard.AsyncShardedFilesServerClient
#### Raven.Client.FileSystem.Shard.DefaultShardResolutionStrategy
#### Raven.Client.FileSystem.Shard.IShardAccessStrategy
#### Raven.Client.FileSystem.Shard.IShardResolutionStrategy
#### Raven.Client.FileSystem.Shard.SequentialShardAccessStrategy
#### Raven.Client.FileSystem.Shard.ShardingErrorHandle<TRavenFileSystemClient>
#### Raven.Client.FileSystem.Shard.ShardPagingInfo
#### Raven.Client.FileSystem.Shard.ShardRequestData
#### Raven.Client.FileSystem.Shard.ShardResolutionResult
#### Raven.Client.FileSystem.Shard.ShardStrategy
#### Raven.Client.FileSystem.SynchronizationServerClient
#### Raven.Client.ILoadConfiguration
#### Raven.Client.Indexes.AbstractCommonApiForIndexesAndTransformers
#### Raven.Client.Indexes.AbstractScriptedIndexCreationTask
#### Raven.Client.Indexes.AbstractScriptedIndexCreationTask<TDocument>
#### Raven.Client.Indexes.AbstractScriptedIndexCreationTask<TDocument, TReduceResult>
#### Raven.Client.Indexes.AbstractTransformerCreationTask
#### Raven.Client.Indexes.AbstractTransformerCreationTask<TFrom>
#### Raven.Client.Indexes.IClientSideDatabase
#### Raven.Client.Indexes.RavenDocumentsByEntityName
#### Raven.Client.ISyncAdvancedSessionOperation
#### Raven.Client.ITransactionalDocumentSession
#### Raven.Client.Linq.AggregationQuery
#### Raven.Client.Linq.DynamicAggregationQuery<T>
#### Raven.Client.Linq.RenamedField
#### Raven.Client.Listeners.IDocumentConflictListener
#### Raven.Client.Listeners.IDocumentConversionListener
#### Raven.Client.Listeners.IDocumentDeleteListener
#### Raven.Client.Listeners.IDocumentQueryListener
#### Raven.Client.Listeners.IDocumentStoreListener
#### Raven.Client.Metrics.ComplexTimeMetric
#### Raven.Client.Metrics.DecreasingTimeMetric
#### Raven.Client.Metrics.IRequestTimeMetric
#### Raven.Client.Metrics.RequestTimeMetric
#### Raven.Client.QueryConvention
#### Raven.Client.RavenPagingInformation
#### Raven.Client.RavenQueryHighlightings
#### Raven.Client.RavenQueryStatistics
#### Raven.Client.Shard.AsyncShardedDocumentSession
#### Raven.Client.Shard.AsyncShardedHiloKeyGenerator
#### Raven.Client.Shard.BaseShardedDocumentSession<TDatabaseCommands>
#### Raven.Client.Shard.DefaultShardResolutionStrategy
#### Raven.Client.Shard.IShardAccessStrategy
#### Raven.Client.Shard.IShardResolutionStrategy
#### Raven.Client.Shard.ParallelShardAccessStrategy
#### Raven.Client.Shard.SequentialShardAccessStrategy
#### Raven.Client.Shard.ShardedDatabaseChanges
#### Raven.Client.Shard.ShardedDocumentSession
#### Raven.Client.Shard.ShardedDocumentStore
#### Raven.Client.Shard.ShardedHiloKeyGenerator
#### Raven.Client.Shard.ShardedObservableWithTask<T>
#### Raven.Client.Shard.ShardedRavenQueryInspector<T>
#### Raven.Client.Shard.ShardingErrorHandle<TDatabaseCommands>
#### Raven.Client.Shard.ShardRequestData
#### Raven.Client.Shard.ShardStrategy
#### Raven.Client.TimeSeries.Changes.ITimeSeriesChanges
#### Raven.Client.TimeSeries.Changes.TimeSeriesChangesClient
#### Raven.Client.TimeSeries.Changes.TimeSeriesConnectionState
#### Raven.Client.TimeSeries.ITimeSeriesStore
#### Raven.Client.TimeSeries.Operations.TimeSeriesBatchOperation
#### Raven.Client.TimeSeries.Operations.TimeSeriesOperationsBase
#### Raven.Client.TimeSeries.Replication.TimeSeriesReplicationInformer
#### Raven.Client.TimeSeries.TimeSeriesConvention
#### Raven.Client.TimeSeries.TimeSeriesStore
#### Raven.Client.Util.DisposableStream
#### Raven.Client.Util.GlobalLastEtagHolder
#### Raven.Client.Util.HttpClientCache
#### Raven.Client.Util.IDisposableAsync
#### Raven.Client.Util.ILastEtagHolder
#### Raven.Client.Util.NoSynchronizationContext
#### Raven.Client.Util.SimpleCache
#### Raven.Client.Util.Types
#### Raven.Client.Documents.Changes.DatabaseConnectionState
#### Raven.Client.Documents.Changes.EvictItemsFromCacheBasedOnChanges
#### Raven.Client.Documents.Changes.IChangesConnectionState
#### Raven.Client.Documents.Conventions.Inflector
#### Raven.Client.Documents.Indexes.ExpressionOperatorPrecedence
#### Raven.Client.Documents.Indexes.ExpressionOperatorPrecedenceExtension
#### Raven.Client.Documents.Indexes.ExpressionStringBuilder
#### Raven.Client.Documents.Indexes.IndexDefinitionHelper
#### Raven.Client.Documents.Indexes.JSBeautify
#### Raven.Client.Documents.Linq.ExpressionInfo
#### Raven.Client.Documents.Linq.RavenQueryProvider<T>
#### Raven.Client.Documents.Linq.RavenQueryProviderProcessor<T>
#### Raven.Client.Documents.Queries.Facets.AggregationQuery<T>
#### Raven.Client.Documents.Session.IAsyncDocumentSessionImpl
#### Raven.Client.Documents.Session.Loaders.AsyncLazyMultiLoaderWithInclude<T>
#### Raven.Client.Documents.Session.Loaders.LazyMultiLoaderWithInclude<T>
#### Raven.Client.Documents.Session.Loaders.MultiLoaderWithInclude<T>
#### Raven.Client.Documents.Session.Operations.Lazy.LazyLoadOperation<T>
#### Raven.Client.Documents.Session.Operations.Lazy.LazyQueryOperation<T>
#### Raven.Client.Documents.Session.Operations.Lazy.LazyStartsWithOperation<T>
#### Raven.Client.Documents.Session.Operations.LoadOperation
#### Raven.Client.Extensions.ExceptionExtensions
#### Raven.Client.Extensions.HttpExtensions
#### Raven.Client.Http.ServerHash
#### Raven.Client.Json.Converters.JsonLuceneDateTimeConverter
#### Raven.Client.Util.ObjectReferenceEqualityComparer<T>
#### Raven.Client.Util.ReflectionUtil
