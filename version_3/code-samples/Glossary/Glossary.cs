using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
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
	}
}