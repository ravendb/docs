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

			public IDictionary<string, string> Headers { get; set; }

			public int Status { get; set; }
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

		#region databasestatistics
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

			public class TriggerInfo
			{
				public string Type { get; set; }
				public string Name { get; set; }
			}

			public ActualIndexingBatchSize[] ActualIndexingBatchSize { get; set; }
			public FutureBatchStats[] Prefetches { get; set; }

			public Guid DatabaseId { get; set; }

			public bool SupportsDtc { get; set; }
		}
		#endregion

		#region actualindexingbatchsize
		public class ActualIndexingBatchSize
		{
			public int Size { get; set; }
			public DateTime Timestamp { get; set; }
		}
		#endregion

		#region futurebatchstats
		public class FutureBatchStats
		{
			public DateTime Timestamp { get; set; }
			public TimeSpan? Duration { get; set; }
			public int? Size { get; set; }
			public int Retries { get; set; }
		}
		#endregion

		#region extensionslog
		public class ExtensionsLog
		{
			public string Name { get; set; }
			public ExtensionsLogDetail[] Installed { get; set; }
		}
		#endregion

		#region extensionslogdetail
		public class ExtensionsLogDetail
		{
			public string Name { get; set; }
			public string Assembly { get; set; }
		}
		#endregion

		#region buildnumber
		public class BuildNumber
		{
			public string ProductVersion { get; set; }

			public string BuildVersion { get; set; }
		}
		#endregion

		#region queryheaderinformation
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
	}

	public class CodeOmitted : Exception
	{
	}
}
