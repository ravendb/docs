# DatabaseCommands

The `DatabaseCommands` is the actual engine behind the Client API. The Client API introduces features like Unit-of-Work and transactions. Therefore, while the `DatabaseCommands` are exposed in the Client API to allow for advanced usages, all operations that are performed directly against them are NOT transactional, and are NOT safe by default.

A `DatabaseCommands` object is available from both the session object _and_ the `IDocumentStore` object.

## Document operations

{CODE-START:csharp /}

	//Retrieves documents for the specified key prefix
	JsonDocument[] StartsWith(string keyPrefix, string matches, int start, int pageSize,  bool metadataOnly = false);

	// Retrieves the document for the specified key
	JsonDocument Get(string key);

	// Retrieves documents with the specified ids, optionally specifying includes to fetch along
	MultiLoadResult Get(string[] ids, string[] includes, bool metadataOnly = false);
  
	// Puts the document in the database with the specified key
	PutResult Put(string key, Guid? etag, RavenJObject document, RavenJObject metadata);

	// Deletes the document with the specified key
	void Delete(string key, Guid? etag);

	// Retrieves the document metadata for the specified document key.
	JsonDocumentMetadata Head(string key);

	// Get the full URL for the given document key
	string UrlFor(string documentKey);

{CODE-END/}

## Attachments

{CODE-START:csharp /}

  	// Puts a byte array as attachment with the specified key
	void PutAttachment(string key, Guid? etag, Stream data, RavenJObject metadata);

	// Updates just the attachment with the specified key's metadata
	void UpdateAttachmentMetadata(string key, Guid? etag, RavenJObject metadata);

	// Retrieves the attachment with the specified key
	Attachment GetAttachment(string key);

	// Gets the attachments starting with the specified prefix
	IEnumerable<Attachment> GetAttachmentHeadersStartingWith(string idPrefix, int start, int pageSize);

	// Retrieves the attachment metadata with the specified key, not the actual attachmet
	Attachment HeadAttachment(string key);

	// Deletes the attachment with the specified key
	void DeleteAttachment(string key, Guid? etag);


{CODE-END/}
    
## Indexes

{CODE-START:csharp /}

	// Returns the names of all indexes that exist on the server
	string[] GetIndexNames(int start, int pageSize);  

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

	// Perform a set based update using the specified index
	void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests, bool allowStale);
	void UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch, bool allowStale);

	// Perform a set based update using the specified index, not allowing the operation
	// if the index is stale
	void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests);
	void UpdateByIndex(string indexName, IndexQuery queryToUpdate, ScriptedPatchRequest patch);

	// Queries the specified index in the Raven flavoured Lucene query syntax
	QueryResult Query(string index, IndexQuery query, string[] includes, bool metadataOnly = false, bool indexEntriesOnly = false);

{CODE-END/}
    
## Transaction API

{CODE-START:csharp /}

	// Commits the specified tx id
	void Commit(Guid txId);

	// Rollbacks the specified tx id
	void Rollback(Guid txId);

	// Promotes the transaction
	byte[] PromoteTransaction(Guid fromTxId);

	// Gets a value indicating whether supports promotable transactions.
	bool SupportsPromotableTransactions { get; }
 
{CODE-END/}
    
## Misc

{CODE-START:csharp /}

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

	// Using the given Index, calculate the facets as per the specified doc
	FacetResults GetFacets(string index, IndexQuery query, string facetSetupDoc);

	// Sends a patch request for a specific document
	void Patch(string key, PatchRequest[] patches, Guid? etag);
	void Patch(string key, ScriptedPatchRequest patch, Guid? etag);

	// Sends a patch request for a specific document, ignoring the document's Etag
	void Patch(string key, PatchRequest[] patches);
	void Patch(string key, ScriptedPatchRequest patch);

	// Disable all caching within the given scope
	IDisposable DisableAllCaching();

	// Perform a single POST request containing multiple nested GET requests
	GetResponse[] MultiGet(GetRequest[] requests);

	// Retrieve the statistics for the database
	DatabaseStatistics GetStatistics();

	// Generate the next identity value from the server
	long NextIdentityFor(string name);

	// Returns a new IDatabaseCommands using the specified credentials
	IDatabaseCommands With(ICredentials credentialsForSession);

	// Create a new instance of IDatabaseCommands that will interacts with the specified database
	IDatabaseCommands ForDatabase(string database);

	// Create a new instance of IDatabaseCommands that will interacts with the default database
	IDatabaseCommands ForDefaultDatabase();

	// Force the database commands to read directly from the master, unless there has been a failover.
	void ForceReadFromMaster();

	// Gets or sets the operations headers
	NameValueCollection OperationsHeaders { get; set; }
{CODE-END/}