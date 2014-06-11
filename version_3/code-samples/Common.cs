namespace Raven.Documentation.CodeSamples
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;

	using Raven.Abstractions.Data;
	using Raven.Abstractions.Indexing;
	using Raven.Json.Linq;

	public class Common
	{
		#region jsondocument
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

		#region jsondocumentmetadata
		public class JsonDocumentMetadata
		{
			public RavenJObject Metadata { get; set; }

			public Etag Etag { get; set; }

			public string Key { get; set; }

			public DateTime? LastModified { get; set; }

			public bool? NonAuthoritativeInformation { get; set; }
		}
		#endregion

		#region multiloadresult
		public class MultiLoadResult
		{
			public List<RavenJObject> Results { get; set; }

			public List<RavenJObject> Includes { get; set; }
		}
		#endregion

		#region putresult
		public class PutResult
		{
			public string Key { get; set; }

			public Etag ETag { get; set; }
		}
		#endregion

		#region getrequest
		public class GetRequest
		{
			public string Url { get; set; }

			public IDictionary<string, string> Headers { get; set; }

			public string Query { get; set; }
		}
		#endregion

		#region getresponse
		public class GetResponse
		{
			public RavenJToken Result { get; set; }

			public IDictionary<string,string> Headers { get; set; }

			public int Status { get; set; }
		}
		#endregion

		#region operation
		public class Operation
		{
			public RavenJToken WaitForCompletion()
			{
				throw new CodeOmitted();
			}

			public Task<RavenJToken> WaitForCompletionAsync()
			{
				throw new CodeOmitted();
			}
		}
		#endregion

		#region transformerdefinition
		public class TransformerDefinition
		{
			public string TransformResults { get; set; }

			public int TransfomerId { get; set; }

			public string Name { get; set; }
		}
		#endregion

		#region indexdefinition
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

		#region indexmergeresults
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

		#region mergesuggestions
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

		#region attachmentinformation
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

		#region patchrequest
		public class PatchRequest
		{
			/// <summary>
			/// Type of the operation
			/// </summary>
			public PatchCommandType Type { get; set; }

			/// <summary>
			/// Gets or sets the previous value, which is compared against the current value to verify a
			/// change isn't overwriting new values.
			/// If the value is null, the operation is always successful
			/// </summary>
			public RavenJToken PrevVal { get; set; }

			/// <summary>
			/// Value that will be applied
			/// </summary>
			public RavenJToken Value { get; set; }

			/// <summary>
			/// Gets or sets the nested operations to perform. This is only valid when the <see cref="Type"/> is <see cref="PatchCommandType.Modify"/>.
			/// </summary>
			public PatchRequest[] Nested { get; set; }

			/// <summary>
			/// Name of the property that will be patched
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Position in array that will be patched
			/// </summary>
			public int? Position { get; set; }

			/// <summary>
			/// Set this property to true if you want to modify all items in an collection.
			/// </summary>
			public bool? AllPositions { get; set; }
		}
		#endregion

		#region patchcommandtype
		public enum PatchCommandType
		{
			/// <summary>
			/// Set a property
			/// </summary>
			Set,
			/// <summary>
			/// Unset (remove) a property
			/// </summary>
			Unset,
			/// <summary>
			/// Add an item to an array
			/// </summary>
			Add,
			/// <summary>
			/// Append an item to an array
			/// </summary>
			Insert,
			/// <summary>
			/// Remove an item from an array at a specified position
			/// </summary>
			Remove,
			/// <summary>
			/// Modify a property value by providing a nested set of patch operation
			/// </summary>
			Modify,
			/// <summary>
			/// Increment a property by a specified value
			/// </summary>
			Inc,
			/// <summary>
			/// Copy a property value to another property
			/// </summary>
			Copy,
			/// <summary>
			/// Rename a property
			/// </summary>
			Rename
		}
		#endregion

		#region scriptedpatchrequest
		public class ScriptedPatchRequest
		{
			/// <summary>
			/// JavaScript function that will patch a document
			/// </summary>
			/// <value>The type.</value>
			public string Script { get; set; }

			/// <summary>
			/// Additional values that will be passed to function
			/// </summary>
			public Dictionary<string, object> Values { get; set; }
		}
		#endregion

		private class CodeOmitted : Exception
		{
		}
	}
}
