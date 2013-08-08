using Raven.Client.Changes;
using Raven.Client.Document;

namespace RavenCodeSamples.ClientApi.Advanced.DatabaseCommands
{
	namespace Foo
	{
		using System;
		using System.Collections.Generic;
		using System.Collections.Specialized;
		using System.IO;
		using System.Net;

		using Raven.Abstractions.Commands;
		using Raven.Abstractions.Data;
		using Raven.Abstractions.Indexing;
		using Raven.Client.Indexes;
		using Raven.Json.Linq;

		public interface IDatabaseCommands
		{
			#region index_1
			//Retrieves documents for the specified key prefix
			JsonDocument[] StartsWith(string keyPrefix, string matches, int start, int pageSize, bool metadataOnly = false);

			// Retrieves the document for the specified key
			JsonDocument Get(string key);

			// Retrieves documents with the specified ids, optionally specifying includes to fetch along
			MultiLoadResult Get(string[] ids, string[] includes, bool metadataOnly = false);

			// Retrieves documents with the specified ids, optionally specifying includes to fetch along and also optionally the transformer
			MultiLoadResult Get(string[] ids, string[] includes, string transformer = null, Dictionary<string, RavenJToken> queryInputs = null, bool metadataOnly = false);

			// Get documents from server. This is primarily useful for administration of a database
			JsonDocument[] GetDocuments(int start, int pageSize, bool metadataOnly = false);

			// Streams the documents by etag OR starts with the prefix and match the matches
			// Will return *all* results, regardless of the number of itmes that might be returned.
			IEnumerator<RavenJObject> StreamDocs(Etag fromEtag = null, string startsWith = null,
				string matches = null, int start = 0, int pageSize = int.MaxValue);

			// Deletes the document with the specified key
			void Delete(string key, Etag etag);

			// Retrieves the document metadata for the specified document key.
			JsonDocumentMetadata Head(string key);

			// Get the full URL for the given document key
			string UrlFor(string documentKey);

			#endregion

			#region index_2
			// Puts a stream as attachment with the specified key
			void PutAttachment(string key, Etag etag, Stream data, RavenJObject metadata);

			// Updates just the attachment with the specified key's metadata
			void UpdateAttachmentMetadata(string key, Etag etag, RavenJObject metadata);

			// Retrieves the attachment with the specified key
			Attachment GetAttachment(string key);

			// Gets the attachments starting with the specified prefix
			IEnumerable<Attachment> GetAttachmentHeadersStartingWith(string idPrefix, int start, int pageSize);

			// Retrieves the attachment metadata with the specified key, not the actual attachmet
			Attachment HeadAttachment(string key);

			// Deletes the attachment with the specified key
			void DeleteAttachment(string key, Etag etag);

			#endregion

			#region index_3
			// Returns the names of all indexes that exist on the server
			string[] GetIndexNames(int start, int pageSize);

			// Gets the indexes from the server
			IndexDefinition[] GetIndexes(int start, int pageSize);

			// Resets the specified index
			void ResetIndex(string name);

			// Gets the index definition for the specified name
			IndexDefinition GetIndex(string name);

			// Creates an index with the specified name, based on an index definition
			string PutIndex(string name, IndexDefinition indexDef);
			string PutIndex(string name, IndexDefinition indexDef, bool overwrite);

			// Creates an index with the specified name, based on an index definition that is created by the supplied
			// IndexDefinitionBuilder
			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef);
			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef, bool overwrite);

			// Deletes the specified index
			void DeleteIndex(string name);

			// Perform a set based deletes using the specified index
			void DeleteByIndex(string indexName, IndexQuery queryToDelete, bool allowStale);

			// Perform a set based deletes using the specified index, not allowing the operation
			// if the index is stale
			void DeleteByIndex(string indexName, IndexQuery queryToDelete);

			// Creates a transformer with the specified name, based on an transformer definition
			string PutTransformer(string name, TransformerDefinition indexDef);

			// Gets the transformer definition for the specified name
			TransformerDefinition GetTransformer(string name);

			// Gets the transformers from the server
			TransformerDefinition[] GetTransformers(int start, int pageSize);

			// Deletes the specified transformer
			void DeleteTransformer(string name);

			// Perform a set based update using the specified index
			void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests, bool allowStale);
			void UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch, bool allowStale);

			// Perform a set based update using the specified index, not allowing the operation
			// if the index is stale
			void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests);
			void UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch);

			// Queries the specified index in the Raven flavored Lucene query syntax
			QueryResult Query(string index, IndexQuery query, string[] includes, bool metadataOnly = false, bool indexEntriesOnly = false);

			//  Queries the specified index in the Raven flavored Lucene query syntax. Will return *all* results, regardless
			// of the number of items that might be returned.
			IEnumerator<RavenJObject> StreamQuery(string index, IndexQuery query, out QueryHeaderInformation queryHeaderInfo);
			#endregion

			#region index_4
			// Commits the specified tx id
			void Commit(string txId);

			// Rollbacks the specified tx id
			void Rollback(string txId);

			// Prepares the transaction on the server.
			void PrepareTransaction(string txId);
			#endregion

			#region index_5
			// Returns the names of all tenant databases on the RavenDB server
			string[] GetDatabaseNames(int pageSize, int start = 0);

			// Executed the specified commands as a single batch
			BatchResult[] Batch(IEnumerable<ICommandData> commandDatas);

			// Returns a list of suggestions based on the specified suggestion query
			SuggestionQueryResult Suggest(string index, SuggestionQuery suggestionQuery);

			// Get the all terms stored in the index for the specified field
			// You can page through the results by use fromValue parameter as the 
			// starting point for the next query
			IEnumerable<string> GetTerms(string index, string field, string fromValue, int pageSize);

			// Using the given Index, calculate the facets as per the specified doc with the given start and pageSize
			FacetResults GetFacets(string index, IndexQuery query, string facetSetupDoc, int start = 0, int? pageSize = null);
			FacetResults GetFacets(string index, IndexQuery query, List<Facet> facets, int start = 0, int? pageSize = null);

			// Sends a patch request for a specific document, ignoring the document's Etag and if the document is missing
			RavenJObject Patch(string key, PatchRequest[] patches);
			RavenJObject Patch(string key, ScriptedPatchRequest patch);

			// Sends a patch request for a specific document
			void Patch(string key, PatchRequest[] patches, Etag etag);
			void Patch(string key, ScriptedPatchRequest patch, Etag etag);

			// Sends a patch request for a specific document, ignoring the document's Etag
			RavenJObject Patch(string key, PatchRequest[] patches, bool ignoreMissing);
			RavenJObject Patch(string key, ScriptedPatchRequest patch, bool ignoreMissing);

			// Sends a patch request for a specific document which may or may not currently exist
			RavenJObject Patch(string key, PatchRequest[] patchesToExisting, PatchRequest[] patchesToDefault, RavenJObject defaultMetadata);
			RavenJObject Patch(string key, ScriptedPatchRequest patchExisting, ScriptedPatchRequest patchDefault, RavenJObject defaultMetadata);

			// Disable all caching within the given scope
			IDisposable DisableAllCaching();

			// Perform a single POST request containing multiple nested GET requests
			GetResponse[] MultiGet(GetRequest[] requests);

			// Retrieve the statistics for the database
			DatabaseStatistics GetStatistics();

			// Generate the next identity value from the server
			long NextIdentityFor(string name);

			// Seeds the next identity value on the server
			long SeedIdentityFor(string name, long value);

			// Returns a new IDatabaseCommands using the specified credentials
			IDatabaseCommands With(ICredentials credentialsForSession);

			// Create a new instance of IDatabaseCommands that will interacts with the specified database
			IDatabaseCommands ForDatabase(string database);

			// Create a new instance of IDatabaseCommands that will interacts with the system database
			IDatabaseCommands ForSystemDatabase();

			// Force the database commands to read directly from the master, unless there has been a failover.
			IDisposable ForceReadFromMaster();

			// Gets or sets the operations headers
			NameValueCollection OperationsHeaders { get; set; }

			// Return a list of documents that based on the MoreLikeThisQuery.
			MultiLoadResult MoreLikeThis(MoreLikeThisQuery query);

			//  Get the low level  bulk insert operation
			ILowLevelBulkInsertOperation GetBulkInsertOperation(BulkInsertOptions options, IDatabaseChanges changes);
			#endregion
		}
	}

	public class Index : CodeSampleBase
	{

	}
}