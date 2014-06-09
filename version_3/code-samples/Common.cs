namespace Raven.Documentation.CodeSamples
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Raven.Abstractions.Data;
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

		private class CodeOmitted : Exception
		{
		}
	}
}
