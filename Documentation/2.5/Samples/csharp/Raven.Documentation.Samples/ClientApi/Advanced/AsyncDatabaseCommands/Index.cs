using Raven.Abstractions.Extensions;
using Raven.Abstractions.Util;

namespace RavenCodeSamples.ClientApi.Advanced.AsyncDatabaseCommands
{
	namespace Foo
	{
		using System;
		using System.Collections.Generic;
		using System.Net;
		using System.Threading.Tasks;

		using Raven.Abstractions.Commands;
		using Raven.Abstractions.Data;
		using Raven.Abstractions.Indexing;
		using Raven.Json.Linq;

		public interface IAsyncDatabaseCommands
		{
			#region index_1
			// Begins an async get operation
			Task<JsonDocument> GetAsync(string key);

			// Begins an async multi get operation
			Task<MultiLoadResult> GetAsync(string[] keys, string[] includes, bool metadataOnly = false);

			// Begins an async get operation for documents
			Task<MultiLoadResult> GetAsync(string[] keys, string[] includes, string transformer = null, Dictionary<string, RavenJToken> queryInputs = null, bool metadataOnly = false);

			// Begins an async get operation for documents. This is primarily useful for administration of a database
			Task<JsonDocument[]> GetDocumentsAsync(int start, int pageSize, bool metadataOnly = false);

			// Deletes the document for the specified id asynchronously
			Task DeleteDocumentAsync(string id);

			// Puts the document with the specified key in the database
			Task<PutResult> PutAsync(string key, Etag etag, RavenJObject document, RavenJObject metadata);

			// Streams the documents by etag OR starts with the prefix and match the matches
			// Will return *all* results, regardless of the number of itmes that might be returned.
			Task<IAsyncEnumerator<RavenJObject>> StreamDocsAsync(Etag fromEtag = null, string startsWith = null, string matches = null, int start = 0, int pageSize = int.MaxValue);

			#endregion

			#region index_2
			// Puts the attachment with the specified key asynchronously
			Task PutAttachmentAsync(string key, Etag etag, byte[] data, RavenJObject metadata);

			// Gets the attachment by the specified key asynchronously
			Task<Attachment> GetAttachmentAsync(string key);

			// Get documents with id of a specific prefix
			Task<JsonDocument[]> StartsWithAsync(string keyPrefix, int start, int pageSize, bool metadataOnly = false);

			// Deletes the attachment with the specified key asynchronously
			Task DeleteAttachmentAsync(string key, Etag etag);

			#endregion

			#region index_3
			// Gets the index names from the server asynchronously
			Task<string[]> GetIndexNamesAsync(int start, int pageSize);

			// Gets the indexes from the server asynchronously
			Task<IndexDefinition[]> GetIndexesAsync(int start, int pageSize);

			// Gets the index definition for the specified name asynchronously
			Task<IndexDefinition> GetIndexAsync(string name);

			// Resets the specified index asynchronously
			Task ResetIndexAsync(string name);

			// Puts the index definition for the specified name asynchronously
			Task<string> PutIndexAsync(string name, IndexDefinition indexDef, bool overwrite);

			// Deletes the index definition for the specified name asynchronously
			Task DeleteIndexAsync(string name);

			// Puts the transformer definition for the specified name asynchronously
			Task<string> PutTransformerAsync(string name, TransformerDefinition transformerDefinition);

			// Gets the transformer definition for the specified name asynchronously
			Task<TransformerDefinition> GetTransformerAsync(string name);

			// Gets the transformers from the server asynchronously
			Task<TransformerDefinition[]> GetTransformersAsync(int start, int pageSize);

			// Deletes the transformer definition for the specified name asynchronously
			Task DeleteTransformerAsync(string name);

			// Perform a set based deletes using the specified index.
			Task DeleteByIndexAsync(string indexName, IndexQuery queryToDelete, bool allowStale);

			// Perform a set based update using the specified index
			Task UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch, bool allowStale);

			// Perform a set based update using the specified index, not allowing the operation
			// if the index is stale
			Task UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch);

			// Begins the async query.
			Task<QueryResult> QueryAsync(string index, IndexQuery query, string[] includes, bool metadataOnly = false);

			// Sends an async command that enables indexing
			Task StartIndexingAsync();

			// Sends an async command that disables all indexing
			Task StopIndexingAsync();

			// Get the indexing status
			Task<string> GetIndexingStatusAsync();

			// Queries the specified index in the Raven flavored Lucene query syntax. Will return *all* results, regardless
			// of the number of items that might be returned.
			Task<IAsyncEnumerator<RavenJObject>> StreamQueryAsync(string index, IndexQuery query, Reference<QueryHeaderInformation> queryHeaderInfo);

			#endregion

			#region index_4
			// Begins the async batch operation
			Task<BatchResult[]> BatchAsync(ICommandData[] commandDatas);

			// Returns a list of suggestions based on the specified suggestion query.
			Task<SuggestionQueryResult> SuggestAsync(string index, SuggestionQuery suggestionQuery);

			// Gets or sets the operations headers.
			IDictionary<string, string> OperationsHeaders { get; }

			// Create a new instance of IAsyncDatabaseCommands that will interacts with the specified database
			IAsyncDatabaseCommands ForDatabase(string database);

			// Create a new instance of IAsyncDatabaseCommands that will interacts with the system database
			IAsyncDatabaseCommands ForSystemDatabase();

			// Returns a new IAsyncDatabaseCommands using the specified credentials
			IAsyncDatabaseCommands With(ICredentials credentialsForSession);

			// Retrieve the statistics for the database asynchronously
			Task<DatabaseStatistics> GetStatisticsAsync();

			// Gets the list of databases from the server asynchronously
			Task<string[]> GetDatabaseNamesAsync(int pageSize, int start = 0);

			// Get the possible terms for the specified field in the index asynchronously
			// You can page through the results by use fromValue parameter as the 
			// starting point for the next query
			Task<string[]> GetTermsAsync(string index, string field, string fromValue, int pageSize);

			// Sends a patch request for a specific document
			Task<RavenJObject> PatchAsync(string key, PatchRequest[] patches, Etag etag);
			Task<RavenJObject> PatchAsync(string key, ScriptedPatchRequest patch, Etag etag);

			// Sends a patch request for a specific document, ignoring the document's Etag
			Task<RavenJObject> PatchAsync(string key, PatchRequest[] patches, bool ignoreMissing);
			Task<RavenJObject> PatchAsync(string key, ScriptedPatchRequest patch, bool ignoreMissing);

			// Sends a patch request for a specific document which may or may not currently exist
			Task<RavenJObject> PatchAsync(string key, PatchRequest[] patchesToExisting, PatchRequest[] patchesToDefault, RavenJObject defaultMetadata);
			Task<RavenJObject> PatchAsync(string key, ScriptedPatchRequest patchExisting, ScriptedPatchRequest patchDefault, RavenJObject defaultMetadata);

			// Ensures that the silverlight startup tasks have run
			Task EnsureSilverlightStartUpAsync();

			// Disable all caching within the given scope
			IDisposable DisableAllCaching();

			// Perform a single POST request containing multiple nested GET requests
			Task<GetResponse[]> MultiGetAsync(GetRequest[] requests);

			// Using the given Index, calculate the facets as per the specified doc
			Task<FacetResults> GetFacetsAsync(string index, IndexQuery query, string facetSetupDoc);
			Task<FacetResults> GetFacetsAsync(string index, IndexQuery query, List<Facet> facets, int start, int? pageSize);

			// Gets the Logs
			Task<LogItem[]> GetLogsAsync(bool errorsOnly);

			// Gets the license Status
			Task<LicensingStatus> GetLicenseStatusAsync();

			// Gets the build number
			Task<BuildNumber> GetBuildNumberAsync();

			// Begins an async backup operation
			Task StartBackupAsync(string backupLocation, DatabaseDocument databaseDocument);

			// Begins an async restore operation
			Task StartRestoreAsync(string restoreLocation, string databaseLocation, string databaseName = null);

			// Force the database commands to read directly from the master, unless there has been a failover.
			void ForceReadFromMaster();

			#endregion
		}
	}

	public class Index : CodeSampleBase
	{

	}
}