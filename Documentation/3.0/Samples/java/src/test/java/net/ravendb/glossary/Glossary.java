package net.ravendb.glossary;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;
import java.util.UUID;

import org.apache.commons.lang.StringUtils;

import com.mysema.query.types.Path;
import com.mysema.query.types.expr.BooleanExpression;

import net.ravendb.abstractions.basic.EventArgs;
import net.ravendb.abstractions.basic.Lazy;
import net.ravendb.abstractions.basic.SerializeUsingValue;
import net.ravendb.abstractions.basic.UseSharpEnum;
import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.closure.Function0;
import net.ravendb.abstractions.data.AdminMemoryStatistics;
import net.ravendb.abstractions.data.Constants;
import net.ravendb.abstractions.data.DatabaseMetrics;
import net.ravendb.abstractions.data.DocumentChangeNotification;
import net.ravendb.abstractions.data.DocumentChangeTypes;
import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.ExtensionsLogDetail;
import net.ravendb.abstractions.data.FacetAggregation;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.FileSystemMetrics;
import net.ravendb.abstractions.data.FileSystemStats;
import net.ravendb.abstractions.data.FutureBatchStats;
import net.ravendb.abstractions.data.HttpMethods;
import net.ravendb.abstractions.data.IJsonDocumentMetadata;
import net.ravendb.abstractions.data.IndexChangeTypes;
import net.ravendb.abstractions.data.IndexStats;
import net.ravendb.abstractions.data.IndexingBatchInfo;
import net.ravendb.abstractions.data.IndexingError;
import net.ravendb.abstractions.data.LoadedDatabaseStatistics;
import net.ravendb.abstractions.data.PatchRequest;
import net.ravendb.abstractions.data.PatchResult;
import net.ravendb.abstractions.data.ReplicationConflictTypes;
import net.ravendb.abstractions.data.ReplicationOperationTypes;
import net.ravendb.abstractions.data.ScriptedPatchRequest;
import net.ravendb.abstractions.data.StorageStats;
import net.ravendb.abstractions.data.StringDistanceTypes;
import net.ravendb.abstractions.data.SuggestionQuery;
import net.ravendb.abstractions.data.SynchronizationDetails;
import net.ravendb.abstractions.data.TransformerChangeTypes;
import net.ravendb.abstractions.extensions.ExpressionExtensions;
import net.ravendb.abstractions.indexing.FieldIndexing;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.abstractions.indexing.FieldTermVector;
import net.ravendb.abstractions.indexing.IndexDefinition;
import net.ravendb.abstractions.indexing.IndexLockMode;
import net.ravendb.abstractions.indexing.MergeSuggestions;
import net.ravendb.abstractions.indexing.SortOptions;
import net.ravendb.abstractions.indexing.SpatialOptions;
import net.ravendb.abstractions.indexing.SuggestionOptions;
import net.ravendb.abstractions.indexing.SpatialOptions.SpatialRelation;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.abstractions.json.linq.RavenJValue;
import net.ravendb.abstractions.replication.ReplicationDestination.TransitiveReplicationOptions;
import net.ravendb.abstractions.util.NetDateFormat;
import net.ravendb.client.document.BulkInsertOperation.BeforeEntityInsert;
import net.ravendb.client.linq.AggregationQueryDsl;
import net.ravendb.client.linq.DynamicAggregationQuery;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.client.spatial.SpatialCriteria;

@SuppressWarnings("unused")
public class Glossary {
  /*
  //region bulk_insert_operation
  public class BulkInsertOperation {
    public static interface BeforeEntityInsert {
      public void apply(String id, RavenJObject data, RavenJObject metadata);
    }

    public void addOnBeforeEntityInsert(BeforeEntityInsert action) { ... }

    public void removeOnBeforeEntityInsert(BeforeEntityInsert action) { ... }

    private void onBeforeEntityInsert(String id, RavenJObject data, RavenJObject metadata) { ... }

    public boolean isAborted() { ... }

    public void abort() { ... }

    public UUID getOperationId() { ... }

    public Action1<String> getReport() { ... }

    public void setReport(Action1<String> report) { ... }

    public String store(Object entity) throws InterruptedException { ... }

    public void store(Object entity, String id) throws InterruptedException { ... }

    public void store(RavenJObject document, RavenJObject metadata, String id) throws InterruptedException { ... }

    public void store(RavenJObject document, RavenJObject metadata, String id,Integer dataSize) throws InterruptedException { ... }

    public void close() throws Exception { ... }
  }
  //endregion
  */

  //region bulk_insert_options
  public class BulkInsertOptions {
    private boolean overwriteExisting;
    private boolean checkReferencesInIndexes;
    private int batchSize;
    private int writeTimeoutMiliseconds;
    private boolean skipOverwriteIfUnchanged;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region json_document
  public class JsonDocument {
    private RavenJObject dataAsJson;
    private RavenJObject metadata;
    private String key;
    private Boolean nonAuthoritativeInformation;
    private Etag etag;
    private Date lastModified;
    private Float tempIndexScore;

    /*
     * Getters and setters omitted for code clarity
     */

  }
  //endregion

  //region json_document_metadata
  public class JsonDocumentMetadata {
    private RavenJObject metadata;
    private String key;
    private Boolean nonAuthoritativeInformation;
    private Etag etag;
    private Date lastModified;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region operation
  public class Operation {
    public RavenJToken waitForCompletion() { ... }
  }
  //endregion
   */

  //region index_definition
  public class IndexDefinition {
    private int indexId;
    private String name;
    private IndexLockMode lockMode = IndexLockMode.UNLOCK;
    private Set<String> maps;
    private String reduce;
    private boolean isCompiled;
    private Map<String, FieldStorage> stores;
    private Map<String, FieldIndexing> indexes;
    private Map<String, SortOptions> sortOptions;
    private Map<String, String> analyzers;
    private List<String> fields;
    private Map<String, SuggestionOptions> suggestions;
    private Map<String, FieldTermVector> termVectors;
    private Map<String, SpatialOptions> spatialIndexes;
    private Long maxIndexOutputsPerDocument;
    private boolean disableInMemoryIndexing;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region transformer_definition
  public class TransformerDefinition {
    private String transformResults;
    private int indexId;
    private String name;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region put_command_data
  public class PutCommandData {
    private String key;
    private Etag etag;
    private RavenJObject document;
    private RavenJObject metadata;
    private RavenJObject additionalData;

    public HttpMethods getMethod() {
      return HttpMethods.PUT;
    }
    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region delete_command_data
  public class DeleteCommandData {
    private String key;
    private Etag etag;
    private RavenJObject additionalData;

    public HttpMethods getMethod() {
      return HttpMethods.DELETE;
    }
    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region patch_command_data
  public class PatchCommandData {
    private PatchRequest[] patches;
    private PatchRequest[] patchesIfMissing;
    private String key;
    private Etag etag;
    private RavenJObject metadata;
    private RavenJObject additionalData;

    public HttpMethods getMethod() {
      return HttpMethods.PATCH;
    }
    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region scripted_patch_command_data
  public class ScriptedPatchCommandData {
    private ScriptedPatchRequest patch;
    private ScriptedPatchRequest patchIfMissing;
    private String key;
    private Etag etag;
    private RavenJObject metadata;
    private boolean debugMode;
    private RavenJObject additionalData;

    public HttpMethods getMethod() {
      return HttpMethods.EVAL;
    }
    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region batch_result
  public class BatchResult {
    private Etag etag;
    private String method;
    private String key;
    private RavenJObject metadata;
    private RavenJObject additionalData;
    private PatchResult patchResult;
    private Boolean deleted;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region index_merge_results
  public class IndexMergeResults {
    private Map<String, String> unmergables = new HashMap<>();
    private List<MergeSuggestions> suggestions = new ArrayList<>();

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region merge_suggestions
  public class MergeSuggestions {
    private List<String> canMerge = new ArrayList<>();
    private IndexDefinition mergedIndex = new IndexDefinition();

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region database_statistics
  public class DatabaseStatistics {
    private Etag lastDocEtag;
    @Deprecated
    private Etag lastAttachmentEtag;
    private int countOfIndexes;
    private int countOfResultTransformers;
    private int inMemoryIndexingQueueSize;
    private long approximateTaskCount;
    private long countOfDocuments;
    @Deprecated
    private long countOfAttachments;
    private String[] staleIndexes;
    private int currentNumberOfItemsToIndexInSingleBatch;
    private int currentNumberOfItemsToReduceInSingleBatch;
    private float databaseTransactionVersionSizeInMB;
    private IndexStats[] indexes;
    private IndexingError[] errors;
    //TODO triggers, extensions, actualindexing batch size
    private IndexingBatchInfo[] indexingBatchInfo; //TODO remove me?
    private FutureBatchStats[] prefetches;
    private UUID databaseId;
    private boolean supportsDtc;

    public class TriggerInfo {
      private String type;
      private String name;
    }

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //TODO: check me!
  //region actual_indexing_batch_size
  public class ActualIndexingBatchSize {
    private int size;
    private String timestamp;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region future_batch_stats
  public class FutureBatchStats {

    private String timestamp;
    private String duration;
    private int size;
    private int retries;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region extensions_log
  public class ExtensionsLog {
    private String name;
    private ExtensionsLogDetail[] installed;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region extensions_log_detail
  public class ExtensionsLogDetail {
    private String name;
    private String assembly;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region admin_statistics
  public class AdminStatistics {
    private String serverName;
    private int totalNumberOfRequests;
    private String uptime;
    private AdminMemoryStatistics memory;
    private List<LoadedDatabaseStatistics> loadedDatabases;
    private List<FileSystemStats> loadedFileSystems;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region admin_memory_statistics
  public class AdminMemoryStatistics {
    private double databaseCacheSizeInMB;
    private double managedMemorySizeInMB;
    private double totalProcessMemorySizeInMB;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region loaded_database_statistics
  public class LoadedDatabaseStatistics {
    private String name;
    private Date lastActivity;

    private long transactionalStorageAllocatedSize;
    private String transactionalStorageAllocatedSizeHumaneSize;
    private long transactionalStorageUsedSize;
    private String transactionalStorageUsedSizeHumaneSize;

    private long indexStorageSize;
    private String indexStorageHumaneSize;
    private long totalDatabaseSize;
    private String totalDatabaseHumaneSize;
    private long countOfDocuments;

    @Deprecated
    private long countOfAttachments;
    private double databaseTransactionVersionSizeInMB;
    private DatabaseMetrics metrics;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region file_system_stats
  public class FileSystemStats {
    private String name;
    private Long fileCount;
    private FileSystemMetrics metrics;
    private List<SynchronizationDetails> activeSyncs;
    private List<SynchronizationDetails> pendingSyncs;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region query_header_information
  public class QueryHeaderInformation {
    private String index;
    private boolean isStale;
    private Date indexTimestamp;
    private int totalResults;
    private Etag resultEtag;
    private Etag indexEtag;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region attachment
  public class Attachment {
    private byte[] data;
    private int size;
    private RavenJObject metadata;
    private Etag etag;
    private String key;
    private boolean canGetData;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region attachment_information
  public class AttachmentInformation {
    private int size;
    private String key;
    private RavenJObject metadata;
    private Etag etag;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region raven_query_statistics
  public class RavenQueryStatistics {

    private boolean stale;
    private long durationMiliseconds;
    private int totalResults;
    private int skippedResults;
    private Date timestamp;
    private String indexName;
    private Date indexTimestamp;
    private Etag indexEtag;
    private boolean nonAuthoritativeInformation;
    private Date lastQueryTime;
    private Map<String, Double> timingsInMilliseconds;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region suggestion_query
  public static class SuggestionQuery {
    public static final float DEFAULT_ACCURACY = 0.5f;
    public static final int DEFAULT_MAX_SUGGESTIONS = 15;
    public static final StringDistanceTypes DEFAULT_DISTANCE = StringDistanceTypes.LEVENSHTEIN;

    private String term;
    private String field;
    private int maxSuggestions;
    private StringDistanceTypes distance;
    private Float accuracy;
    private boolean popularity;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region string_distance_types
  @UseSharpEnum
  public enum StringDistanceTypes {
    /**
     * Default, suggestion is not active
     */
    NONE,
    /**
     *  Default, equivalent to Levenshtein
     */
    DEFAULT,
    /**
     * Levenshtein distance algorithm (default)
     */
    LEVENSHTEIN,
    /**
     * JaroWinkler distance algorithm
     */
    JARO_WINKLER,
    /**
     * NGram distance algorithm
     */
    N_GRAM;
  }
  //endregion

  //TODO: check me!
  /*
  //region field_highlightings
  public class FieldHighlightings {
    public String getFieldName() { ... }

    public Map<String, String[]> getHighlightings() { ... }

    public String[] getFragments(String documentId) { ... }
  }
  //endregion
   */

  //region spatial_criteria
  public class SpatialCriteria {
    private SpatialRelation relation;
    private Object shape;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region spatial_criteria_factory
  public class SpatialCriteriaFactory {
    public SpatialCriteria relatesToShape(Object shape, SpatialRelation relation) { ... }

    public SpatialCriteria intersects(Object shape) { ... }

    public SpatialCriteria contains(Object shape) { ... }

    public SpatialCriteria disjoint(Object shape) { ... }

    public SpatialCriteria within(Object shape) { ... }

    public SpatialCriteria withinRadiusOf(double radius, double x, double y) { ... }
  }
  //endregion
   */

  /*
  //region dynamic_aggregation_query
  public class DynamicAggregationQuery<T> {
    public DynamicAggregationQuery<T> andAggregateOn(Path<?> path) { ... }

    public DynamicAggregationQuery<T> andAggregateOn(Path<?> path, String displayName) { ... }

    public DynamicAggregationQuery<T> andAggregateOn(String path) { ... }

    public DynamicAggregationQuery<T> andAggregateOn(String path, String displayName) { ... }

    public DynamicAggregationQuery<T> addRanges(BooleanExpression... paths) { ... }

    public DynamicAggregationQuery<T> maxOn(Path<?> path) { ... }

    public DynamicAggregationQuery<T> minOn(Path<?> path) { ... }

    public DynamicAggregationQuery<T> sumOn(Path<?> path) { ... }

    public DynamicAggregationQuery<T> averageOn(Path<?> path) { ... }

    public DynamicAggregationQuery<T> countOn(Path<?> path) { ... }

    public FacetResults toList() { ... }

    public Lazy<FacetResults> toListLazy() { ... }
  }
  //endregion
  */

  //region document_change_notification
  public class DocumentChangeNotification {
    private DocumentChangeTypes type;
    private String id;
    private String collectionName;
    private String typeName;
    private Etag etag;
    private String message;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region document_change_types
  @UseSharpEnum
  @SerializeUsingValue
  public enum DocumentChangeTypes {
    NONE(0),

    PUT(1),
    DELETE(2),
    BULK_INSERT_STARTED(4),
    BULK_INSERT_ENDED(8),
    BULK_INSERT_ERROR(16),

    COMMON(3);

  }
  //endregion
  */

  //region index_change_notification
  public class IndexChangeNotification {
    private IndexChangeTypes type;
    private String name;
    private Etag etag;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region index_change_types
  @UseSharpEnum
  @SerializeUsingValue
  public enum IndexChangeTypes {
    NONE(0),

    MAP_COMPLETED(1),
    REDUCE_COMPLETED(2),
    REMOVE_FROM_INDEX(4),

    INDEX_ADDED(8),
    INDEX_REMOVED(16),

    INDEX_DEMOTED_TO_IDLE(32),
    INDEX_PROMOTED_FROM_IDLE(64),

    INDEX_DEMOTED_TO_ABANDONED(128),

    INDEX_DEMOTED_TO_DISABLED(256),

    INDEX_MARKED_AS_ERRORED(512);
  }
  //endregion
   */

  //region bulk_insert_change_notification
  public class BulkInsertChangeNotification extends DocumentChangeNotification {
    private UUID operationId;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  //region transformer_change_notification
  public class TransformerChangeNotification extends EventArgs {
    private TransformerChangeTypes type;
    private String name;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region transformer_change_types
  @UseSharpEnum
  @SerializeUsingValue
  public enum TransformerChangeTypes {

    NONE(0),

    TRANSFORMER_ADDED(1),

    TRANSFORMER_REMOVED(2);
  }
  //endregion
   */

  //region replication_conflict_notification
  public class ReplicationConflictNotification extends EventArgs {
    private ReplicationConflictTypes itemType;
    private String id;
    private Etag etag;
    private ReplicationOperationTypes operationType;
    private String[] conflicts;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion

  /*
  //region replication_conflict_types
  @UseSharpEnum
  @SerializeUsingValue
  public enum ReplicationConflictTypes {
    NONE(0),

    DOCUMENT_REPLICATION_CONFLICT(1),

    ATTACHMENT_REPLICATION_CONFLICT(2);
  }
  //endregion
   */

  /*
  //region replication_operation_types
  @UseSharpEnum
  @SerializeUsingValue
  public enum ReplicationOperationTypes {

    NONE(0),

    PUT(1),

    DELETE(2);
  }
  //endregion
   */

  //region replication_destination
  public class ReplicationDestination {

    private String url;
    private String username;
    private String password;
    private String domain;
    private String apiKey;
    private String database;
    private TransitiveReplicationOptions transitiveReplicationBehavior;
    private Boolean ignoredClient;
    private Boolean disabled;
    private String clientVisibleUrl;

    /*
     * Getters and setters omitted for code clarity
     */
  }
  //endregion
}
