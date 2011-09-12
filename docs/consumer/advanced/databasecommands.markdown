# DatabaseCommands

The `DatabaseCommands` is the actual engine behind the Client API. On top of it, the Client API introduces features like Unit-of-Work and transactions. Therefore, while the `DatabaseCommands` are exposed in the Client API to allow for advanced usages, all operations that are performed directly against them are NOT transactional, and are NOT safe by default.

## Document operations

    // Puts the document in the database with the specified key.
    PutResult Put(string key, Guid? etag, RavenJObject document, RavenJObject metadata);

    // Deletes the document with the specified key
    void Delete(string key, Guid? etag);

    // Retrieves the document for the specified key.
    JsonDocument Get(string key);

    // Retrieves documents with the specified ids, optionally specifying includes to fetch along.
    MultiLoadResult Get(string[] ids, string[] includes);
    
    // Retrieves documents for the specified key prefix    
    JsonDocument[] StartsWith(string keyPrefix, int start, int pageSize);

## Attachments

    // Puts a byte array as attachment with the specified key
    void PutAttachment(string key, Guid? etag, byte[] data, RavenJObject metadata);
    
    // Retrieves the attachment with the specified key.
    Attachment GetAttachment(string key);
    
    // Deletes the attachment with the specified key
    void DeleteAttachment(string key, Guid? etag);
    
## Indexes

    // Returns the names of all indexes that exist on the server
    string[] GetIndexNames(int start, int pageSize);
    
    // Resets the specified index
    void ResetIndex(string name);
    
    // Returns the index definition for the specified name
    IndexDefinition GetIndex(string name);
    
    // Creates an index with the specified name, based on an index definition
    string PutIndex(string name, IndexDefinition indexDef);
    string PutIndex(string name, IndexDefinition indexDef, bool overwrite);
    
    // Creates an index with the specified name, based on an index definition that is created by the supplied
    // IndexDefinitionBuilder
    string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef);
    string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef, bool overwrite);
    
    // Queries the specified index in the Raven flavoured Lucene query syntax
    QueryResult Query(string index, IndexQuery query, string[] includes);
    
    // Deletes the specified index
    void DeleteIndex(string name);
    
    // Perform a set based deletes using the specified index, not allowing the operation
    // if the index is stale
    void DeleteByIndex(string indexName, IndexQuery queryToDelete);
    
    // Perform a set based deletes using the specified index
    void DeleteByIndex(string indexName, IndexQuery queryToDelete, bool allowStale);
    
    // Perform a set based update using the specified index, not allowing the operation
    // if the index is stale
    void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests);
    
    // Perform a set based update using the specified index
    void UpdateByIndex(string indexName, IndexQuery queryToUpdate, PatchRequest[] patchRequests, bool allowStale);
    
## Transaction API

    // Commits the specified tx id
    void Commit(Guid txId);

    // Rollbacks the specified tx id.    
    void Rollback(Guid txId);
    
    // Gets a value indicating whether this DB supports promotable transactions
    bool SupportsPromotableTransactions { get; }
    
    // Promotes the transaction
    byte[] PromoteTransaction(Guid fromTxId);
    
    // Stores the recovery information
    void StoreRecoveryInformation(Guid resourceManagerId, Guid txId, byte[] recoveryInformation);
    
## Misc

    // Executed the specified commands as a single batch
    BatchResult[] Batch(IEnumerable<ICommandData> commandDatas);

    // Create a new instance of IDatabaseCommands that will interacts
    // with the specified database
    IDatabaseCommands ForDatabase(string database);

    // Returns a new IDatabaseCommands using the specified credentials
    IDatabaseCommands With(ICredentials credentialsForSession);
    
    // Create a new instance of IDatabaseCommands that will interact
    // with the root database. Useful if the database has works against a tenant database
    IDatabaseCommands GetRootDatabase();
    
    // Returns a list of suggestions based on the specified suggestion query
    SuggestionQueryResult Suggest(string index, SuggestionQuery suggestionQuery);
    
    // Get the all terms stored in the index for the specified field
    // You can page through the results by use fromValue parameter as the 
    // starting point for the next query
    IEnumerable<string> GetTerms(string index, string field, string fromValue, int pageSize);
    
    // Sends a patch request for a specific document, ignoring the document's Etag
    void Patch(string key, PatchRequest[] patches);
    
    // Sends a patch request for a specific document
    void Patch(string key, PatchRequest[] patches, Guid? etag);
    
    // Disable all caching within the given scope
    IDisposable DisableAllCaching();
    
    // Perform a single POST requst containing multiple nested GET requests
    GetResponse[] MultiGet(GetRequest[] requests);

    // Gets or sets the operations headers
    NameValueCollection OperationsHeaders { get; set; }