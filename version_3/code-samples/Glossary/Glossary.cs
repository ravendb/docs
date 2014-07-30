using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
using Raven.Abstractions.FileSystem;
using Raven.Abstractions.Indexing;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.Glossary
{
	public class Glossary
	{
		/*
		#region bulk_insert_operation
		public class BulkInsertOperation
		{
			public delegate void BeforeEntityInsert(
				string id,
				RavenJObject data,
				RavenJObject metadata);

			public event BeforeEntityInsert OnBeforeEntityInsert = delegate { };
		
			public bool IsAborted { get { ... } }
		
			public void Abort() { ... }
		
			public Guid OperationId { get { ... } }
		
			public event Action<string> Report { ... }
		
			public void Store(object entity) { ... }
		
			public void Store(object entity, string id) { ... }
		
			public void Dispose() { ... }
		}
		#endregion
		*/

		#region bulk_insert_options
		public class BulkInsertOptions
		{
			public bool OverwriteExisting { get; set; }

			public bool CheckReferencesInIndexes { get; set; }

			public int BatchSize { get; set; }

			public int WriteTimeoutMilliseconds { get; set; }
		}
		#endregion

		#region json_document
		public class JsonDocument
		{
			public RavenJObject DataAsJson { get; set; }

			public RavenJObject Metadata { get; set; }

			public Etag Etag { get; set; }

			public string Key { get; set; }

			public DateTime? LastModified { get; set; }

			public bool? NonAuthoritativeInformation { get; set; }

			public float? TempIndexScore { get; set; }
		}
		#endregion

		#region json_document_metadata
		public class JsonDocumentMetadata
		{
			public RavenJObject Metadata { get; set; }

			public Etag Etag { get; set; }

			public string Key { get; set; }

			public DateTime? LastModified { get; set; }

			public bool? NonAuthoritativeInformation { get; set; }
		}
		#endregion

		/*
		#region operation
		public class Operation
		{
			public RavenJToken WaitForCompletion() { ... }

			public Task<RavenJToken> WaitForCompletionAsync() { ... }
		}
		#endregion
		*/

		#region index_definition
		public class IndexDefinition
		{
			/// <summary>
			/// Get or set the id of this index
			/// </summary>
			public int IndexId { get; set; }

			/// <summary>
			/// This is the means by which the outside world refers to this index defiintion
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Get or set the index lock mode
			/// </summary>
			public IndexLockMode LockMode { get; set; }

			/// <summary>
			/// All the map functions for this index
			/// </summary>
			public HashSet<string> Maps { get; set; }

			/// <summary>
			/// Gets or sets the reduce function
			/// </summary>
			/// <value>The reduce.</value>
			public string Reduce { get; set; }

			public bool IsCompiled { get; set; }

			/// <summary>
			/// Gets or sets the stores options
			/// </summary>
			/// <value>The stores.</value>
			public IDictionary<string, FieldStorage> Stores { get; set; }

			/// <summary>
			/// Gets or sets the indexing options
			/// </summary>
			/// <value>The indexes.</value>
			public IDictionary<string, FieldIndexing> Indexes { get; set; }

			/// <summary>
			/// Gets or sets the sort options.
			/// </summary>
			/// <value>The sort options.</value>
			public IDictionary<string, SortOptions> SortOptions { get; set; }

			/// <summary>
			/// Gets or sets the analyzers options
			/// </summary>
			/// <value>The analyzers.</value>
			public IDictionary<string, string> Analyzers { get; set; }

			/// <summary>
			/// The fields that are queryable in the index
			/// </summary>
			public IList<string> Fields { get; set; }

			/// <summary>
			/// Gets or sets the suggest options
			/// </summary>
			/// <value>The suggest options.</value>
			public IDictionary<string, SuggestionOptions> Suggestions { get; set; }

			/// <summary>
			/// Gets or sets the term vectors options
			/// </summary>
			/// <value>The term vectors.</value>
			public IDictionary<string, FieldTermVector> TermVectors { get; set; }

			/// <summary>
			/// Gets or sets the spatial options
			/// </summary>
			/// <value>The spatial options.</value>
			public IDictionary<string, SpatialOptions> SpatialIndexes { get; set; }

			/// <summary>
			/// Internal map of field names to expressions generating them
			/// Only relevant for auto indexes and only used internally
			/// </summary>
			public IDictionary<string, string> InternalFieldsMapping { get; set; }

			/// <summary>
			/// Index specific setting that limits the number of map outputs that an index is allowed to create for a one source document. If a map operation applied to
			/// the one document produces more outputs than this number then an index definition will be considered as a suspicious and the index will be marked as errored.
			/// Default value: null means that the global value from Raven configuration will be taken to detect if number of outputs was exceeded.
			/// </summary>
			public int? MaxIndexOutputsPerDocument { get; set; }

			/// <summary>
			/// Prevent index from being kept in memory. Default: false
			/// </summary>
			public bool DisableInMemoryIndexing { get; set; }
		}
		#endregion

		#region transformer_definition
		public class TransformerDefinition
		{
			public string TransformResults { get; set; }

			public int TransfomerId { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region put_command_data
		public class PutCommandData
		{
			public string Key { get; set; }

			public string Method
			{
				get { return "PUT"; }
			}

			public Etag Etag { get; set; }

			public RavenJObject Document { get; set; }

			public TransactionInformation TransactionInformation { get; set; }

			public RavenJObject Metadata { get; set; }

			public RavenJObject AdditionalData { get; set; }
		}
		#endregion

		#region delete_command_data
		public class DeleteCommandData
		{
			public string Key { get; set; }

			public string Method
			{
				get { return "DELETE"; }
			}

			public Etag Etag { get; set; }

			public TransactionInformation TransactionInformation { get; set; }

			public RavenJObject Metadata
			{
				get { return null; }
			}

			public RavenJObject AdditionalData { get; set; }
		}
		#endregion

		#region patch_command_data
		public class PatchCommandData
		{
			public PatchRequest[] Patches { get; set; }

			public PatchRequest[] PatchesIfMissing { get; set; }

			public string Key { get; set; }

			public string Method
			{
				get { return "PATCH"; }
			}

			public Etag Etag { get; set; }

			public TransactionInformation TransactionInformation { get; set; }

			public RavenJObject Metadata { get; set; }

			public RavenJObject AdditionalData { get; set; }
		}
		#endregion

		#region scripted_patch_command_data
		public class ScriptedPatchCommandData
		{
			/// <summary>
			/// ScriptedPatchRequest (using JavaScript) that is used to patch the document
			/// </summary>
			public ScriptedPatchRequest Patch { get; set; }

			/// <summary>
			/// ScriptedPatchRequest (using JavaScript) that is used to patch a default document if the document is missing
			/// </summary>
			public ScriptedPatchRequest PatchIfMissing { get; set; }

			public string Key { get; set; }

			public string Method
			{
				get { return "EVAL"; }
			}

			public Etag Etag { get; set; }

			public TransactionInformation TransactionInformation { get; set; }

			public RavenJObject Metadata { get; set; }

			public bool DebugMode { get; set; }

			public RavenJObject AdditionalData { get; set; }
		}
		#endregion

		#region batch_result
		public class BatchResult
		{
			/// <summary>
			/// ETag generated by the etag (if applicable)
			/// </summary>
			public Etag Etag { get; set; }

			/// <summary>
			/// Method used for the operation (PUT,DELETE,PATCH).
			/// </summary>
			public string Method { get; set; }

			/// <summary>
			/// Document key
			/// </summary>
			public string Key { get; set; }

			/// <summary>
			/// Document metadata.
			/// </summary>
			public RavenJObject Metadata { get; set; }

			/// <summary>
			/// Additional data
			/// </summary>
			public RavenJObject AdditionalData { get; set; }

			/// <summary>
			/// Result of a PATCH operation.
			/// </summary>
			public PatchResult? PatchResult { get; set; }

			/// <summary>
			/// Result of a DELETE operation.
			/// </summary>
			/// <value>true if the document was deleted, false if it did not exist.</value>
			public bool? Deleted { get; set; }
		}
		#endregion

		#region index_merge_results
		public class IndexMergeResults
		{
			/// <summary>
			/// Dictionary of unmergable indexes with reason why they can't be merged
			/// </summary>
			public Dictionary<string, string> Unmergables = new Dictionary<string, string>();

			/// <summary>
			/// List of all merge suggestions
			/// </summary>
			public List<MergeSuggestions> Suggestions = new List<MergeSuggestions>();
		}
		#endregion

		#region merge_suggestions
		public class MergeSuggestions
		{
			/// <summary>
			/// List of all indexes (names) that can be merged together
			/// </summary>
			public List<string> CanMerge = new List<string>();

			/// <summary>
			/// Proposition of a new index definition for a merged index
			/// </summary>
			public IndexDefinition MergedIndex = new IndexDefinition();
		}
		#endregion

		#region database_statistics
		public class DatabaseStatistics
		{
			public Etag LastDocEtag { get; set; }

			public Etag LastAttachmentEtag { get; set; }

			public int CountOfIndexes { get; set; }

			public int InMemoryIndexingQueueSize { get; set; }

			public long ApproximateTaskCount { get; set; }

			public long CountOfDocuments { get; set; }

			public long CountOfAttachments { get; set; }

			public string[] StaleIndexes { get; set; }

			public int CurrentNumberOfItemsToIndexInSingleBatch { get; set; }

			public int CurrentNumberOfItemsToReduceInSingleBatch { get; set; }

			public decimal DatabaseTransactionVersionSizeInMB { get; set; }

			public IndexStats[] Indexes { get; set; }

			public ServerError[] Errors { get; set; }

			public TriggerInfo[] Triggers { get; set; }

			public IEnumerable<ExtensionsLog> Extensions { get; set; }

			public ActualIndexingBatchSize[] ActualIndexingBatchSize { get; set; }

			public FutureBatchStats[] Prefetches { get; set; }

			public Guid DatabaseId { get; set; }

			public bool SupportsDtc { get; set; }

			public class TriggerInfo
			{
				public string Type { get; set; }
				public string Name { get; set; }
			}
		}
		#endregion

		#region actual_indexing_batch_size
		public class ActualIndexingBatchSize
		{
			public int Size { get; set; }
			public DateTime Timestamp { get; set; }
		}
		#endregion

		#region future_batch_stats
		public class FutureBatchStats
		{
			public DateTime Timestamp { get; set; }
			public TimeSpan? Duration { get; set; }
			public int? Size { get; set; }
			public int Retries { get; set; }
		}
		#endregion

		#region extensions_log
		public class ExtensionsLog
		{
			public string Name { get; set; }
			public ExtensionsLogDetail[] Installed { get; set; }
		}
		#endregion

		#region extensions_log_detail
		public class ExtensionsLogDetail
		{
			public string Name { get; set; }
			public string Assembly { get; set; }
		}
		#endregion

		#region admin_statistics
		public class AdminStatistics
		{
			public string ServerName { get; set; }
			public int TotalNumberOfRequests { get; set; }
			public TimeSpan Uptime { get; set; }
			public AdminMemoryStatistics Memory { get; set; }

			public IEnumerable<LoadedDatabaseStatistics> LoadedDatabases { get; set; }
			public IEnumerable<FileSystemStats> LoadedFileSystems { get; set; }
		}
		#endregion

		#region admin_memory_statistics
		public class AdminMemoryStatistics
		{
			public decimal DatabaseCacheSizeInMB { get; set; }
			public decimal ManagedMemorySizeInMB { get; set; }
			public decimal TotalProcessMemorySizeInMB { get; set; }
		}
		#endregion

		#region loaded_database_statistics
		public class LoadedDatabaseStatistics
		{
			public string Name { get; set; }
			public DateTime LastActivity { get; set; }
			public long TransactionalStorageAllocatedSize { get; set; }
			public string TransactionalStorageAllocatedSizeHumaneSize { get; set; }
			public long TransactionalStorageUsedSize { get; set; }
			public string TransactionalStorageUsedSizeHumaneSize { get; set; }
			public long IndexStorageSize { get; set; }
			public string IndexStorageHumaneSize { get; set; }
			public long TotalDatabaseSize { get; set; }
			public string TotalDatabaseHumaneSize { get; set; }
			public long CountOfDocuments { get; set; }
			public long CountOfAttachments { get; set; }
			public decimal DatabaseTransactionVersionSizeInMB { get; set; }
			public DatabaseMetrics Metrics { get; set; }
		}
		#endregion

		#region file_system_stats
		public class FileSystemStats
		{
			public string Name { get; set; }

			public long FileCount { get; set; }

			public FileSystemMetrics Metrics { get; set; }

			public IList<SynchronizationDetails> ActiveSyncs { get; set; }

			public IList<SynchronizationDetails> PendingSyncs { get; set; }
		}
		#endregion

		#region query_header_information
		public class QueryHeaderInformation
		{
			public string Index { get; set; }

			public bool IsStale { get; set; }

			public DateTime IndexTimestamp { get; set; }

			public int TotalResults { get; set; }

			public Etag ResultEtag { get; set; }

			public Etag IndexEtag { get; set; }
		}
		#endregion

		#region attachment
		public class Attachment
		{
			/// <summary>
			/// Function that returns attachment data
			/// </summary>
			public Func<Stream> Data { get; set; }

			/// <summary>
			/// Size of the attachment.
			/// </summary>
			/// <remarks>The max size of an attachment can be 2GB.</remarks>
			public int Size { get; set; }

			/// <summary>
			/// Attachment metadata
			/// </summary>
			public RavenJObject Metadata { get; set; }

			/// <summary>
			/// Attachment ETag
			/// </summary>
			public Etag Etag { get; set; }

			/// <summary>
			/// Attachment key
			/// </summary>
			public string Key { get; set; }
		}
		#endregion

		#region attachment_information
		public class AttachmentInformation
		{
			/// <summary>
			/// Size of the attachment.
			/// </summary>
			public int Size { get; set; }

			/// <summary>
			/// Attachment key
			/// </summary>
			public string Key { get; set; }

			/// <summary>
			/// Attachment metadata
			/// </summary>
			public RavenJObject Metadata { get; set; }

			/// <summary>
			/// Attachment ETag
			/// </summary>
			public Etag Etag { get; set; }
		}
		#endregion

		#region raven_query_statistics
		public class RavenQueryStatistics
		{
			/// <summary>
			/// Whatever the query returned potentially stale results
			/// </summary>
			public bool IsStale { get; set; }

			/// <summary>
			/// The duration of the query _server side_
			/// </summary>
			public long DurationMilliseconds { get; set; }

			/// <summary>
			/// What was the total count of the results that matched the query
			/// </summary>
			public int TotalResults { get; set; }

			/// <summary>
			/// Gets or sets the skipped results
			/// </summary>
			public int SkippedResults { get; set; }

			/// <summary>
			/// The time when the query results were unstale.
			/// </summary>
			public DateTime Timestamp { get; set; }

			/// <summary>
			/// The name of the index queried
			/// </summary>
			public string IndexName { get; set; }

			/// <summary>
			/// The timestamp of the queried index
			/// </summary>
			public DateTime IndexTimestamp { get; set; }

			/// <summary>
			/// The etag of the queried index
			/// </summary>
			public Etag IndexEtag { get; set; }

			/// <summary>
			/// Gets or sets a value indicating whether any of the documents returned by this query
			/// are non authoritative (modified by uncommitted transaction).
			/// </summary>
			public bool NonAuthoritativeInformation { get; set; }

			/// <summary>
			/// The timestamp of the last time the index was queried
			/// </summary>
			public DateTime LastQueryTime { get; set; }

			/// <summary>
			/// Detailed timings for various parts of a query (Lucene search, loading documents, transforming results)
			/// </summary>
			public Dictionary<string, double> TimingsInMilliseconds { get; set; }
		}
		#endregion

		#region suggestion_query
		public class SuggestionQuery
		{
			public static float DefaultAccuracy = 0.5f;

			public static int DefaultMaxSuggestions = 15;

			public static StringDistanceTypes DefaultDistance = StringDistanceTypes.Levenshtein;

			/// <summary>
			/// The term is what the user likely entered, and will used as the basis of the suggestions.
			/// </summary>
			public string Term { get; set; }

			/// <summary>
			/// Field to be used in conjunction with the index.
			/// </summary>
			public string Field { get; set; }

			/// <summary>
			/// Number of suggestions to return.
			/// </summary>
			public int MaxSuggestions { get; set; }

			/// <summary>
			/// Gets or sets the string distance algorithm.
			/// </summary>
			public StringDistanceTypes? Distance { get; set; }

			/// <summary>
			/// Accuracy.
			/// </summary>
			public float? Accuracy { get; set; }

			/// <summary>
			/// Whatever to return the terms in order of popularity
			/// </summary>
			public bool Popularity { get; set; }
		}
		#endregion

		#region string_distance_types
		public enum StringDistanceTypes
		{
			None,
			Default,
			Levenshtein,
			JaroWinkler,
			NGram,
		}
		#endregion

		/*
		#region field_highlightings
		public class FieldHighlightings
		{
			public string FieldName { get; private set; }

			/// <summary>
			/// Document Ids of available highlights.
			/// </summary>
			public IEnumerable<string> ResultIndents { get { ... } }

			/// <summary>
			/// Returns the list of document's field highlighting fragments.
			/// </summary>
			public string[] GetFragments(string documentId) { ... }
		}
		#endregion
		*/

		#region spatial_criteria
		public class SpatialCriteria
		{
			public SpatialRelation Relation { get; set; }

			public object Shape { get; set; }
		}
		#endregion

		/*
		#region spatial_criteria_factory
		public class SpatialCriteriaFactory
		{
			public SpatialCriteria RelatesToShape(
				object shape,
				SpatialRelation relation) { ... }

			public SpatialCriteria Intersects(object shape) { ... }

			public SpatialCriteria Contains(object shape) { ... }

			public SpatialCriteria Disjoint(object shape) { ... }

			public SpatialCriteria Within(object shape) { ... }

			public SpatialCriteria WithinRadiusOf(double radius, double x, double y) { ... }
		}
		#endregion
		*/

		/*
		#region dynamic_aggregation_query
		public class DynamicAggregationQuery<TResult>
		{
			public DynamicAggregationQuery<TResult> AndAggregateOn(
				Expression<Func<TResult, object>> path,
				string displayName = null) { ... }

			public DynamicAggregationQuery<TResult> AndAggregateOn(
				string path,
				string displayName = null) { ... }

			public DynamicAggregationQuery<TResult> AddRanges(
				params Expression<Func<TResult, bool>>[] paths) { ... }

			public DynamicAggregationQuery<TResult> MaxOn(
				Expression<Func<TResult, object>> path) { ... }

			public DynamicAggregationQuery<TResult> MinOn(
				Expression<Func<TResult, object>> path) { ... }

			public DynamicAggregationQuery<TResult> SumOn(
				Expression<Func<TResult, object>> path) { ... }

			public DynamicAggregationQuery<TResult> AverageOn(
				Expression<Func<TResult, object>> path) { ... }

			public DynamicAggregationQuery<TResult> CountOn(
				Expression<Func<TResult, object>> path) { ... }

			public FacetResults ToList() { ... }

			public Lazy<FacetResults> ToListLazy() { ... }

			public Task<FacetResults> ToListAsync() { ... }
		}
		#endregion
		*/

		#region document_change_notification
		public class DocumentChangeNotification : EventArgs
		{
			public DocumentChangeTypes Type { get; set; }

			public string Id { get; set; }

			public string CollectionName { get; set; }

			public string TypeName { get; set; }

			public Etag Etag { get; set; }

			public string Message { get; set; }
		}
		#endregion

		#region document_change_types
		[Flags]
		public enum DocumentChangeTypes
		{
			None = 0,
			Put = 1,
			Delete = 2,
			BulkInsertStarted = 4,
			BulkInsertEnded = 8,
			BulkInsertError = 16,
			Common = Put | Delete
		}
		#endregion

		#region index_change_notification
		public class IndexChangeNotification : EventArgs
		{
			public IndexChangeTypes Type { get; set; }

			public string Name { get; set; }

			public Etag Etag { get; set; }
		}
		#endregion

		#region index_change_types
		[Flags]
		public enum IndexChangeTypes
		{
			None = 0,

			MapCompleted = 1,
			ReduceCompleted = 2,
			RemoveFromIndex = 4,

			IndexAdded = 8,
			IndexRemoved = 16,

			IndexDemotedToIdle = 32,
			IndexPromotedFromIdle = 64,

			IndexDemotedToAbandoned = 128,

			IndexDemotedToDisabled = 256,

			IndexMarkedAsErrored = 512
		}
		#endregion

		#region bulk_insert_change_notification
		public class BulkInsertChangeNotification : DocumentChangeNotification
		{
			public Guid OperationId { get; set; }
		}
		#endregion

		#region transformer_change_notification
		public class TransformerChangeNotification : EventArgs
		{
			public TransformerChangeTypes Type { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region transformer_change_types
		public enum TransformerChangeTypes
		{
			None = 0,

			TransformerAdded = 1,
			TransformerRemoved = 2
		}
		#endregion
	}
}