# Commands: What are the commands?

Commands are a set of low level operations that can be used to manipulate data and change configuration on a server.

They are available in **[DocumentStore](../../client-api/what-is-a-document-store)** under **getDatabaseCommands()**.

{CODE:java what_are_commands_1@ClientApi\commands\WhatAreCommands.java /}

### Document commands

The following commands can be used to manipulate documents:   
- [put](../../client-api/commands/documents/put)   
- [get](../../client-api/commands/documents/get)   
- [getDocuments](../../client-api/commands/documents/get)   
- multiGet   
- [startsWith](../../client-api/commands/documents/get)    
- [head](../../client-api/commands/documents/how-to/get-document-metadata-only)   
- [delete](../../client-api/commands/documents/delete)   
- [deleteByIndex](../../client-api/commands/documents/how-to/delete-or-update-documents-using-index)   
- [updateByIndex](../../client-api/commands/documents/how-to/delete-or-update-documents-using-index)   
- [streamDocs](../../client-api/commands/documents/stream)

### Index commands

The following commands can be used to manipulate indexes:   
- [putIndex](../../client-api/commands/indexes/put)   
- [getIndex](../../client-api/commands/indexes/get)   
- [getIndexes](../../client-api/commands/indexes/get)   
- [getIndexNames](../../client-api/commands/indexes/get)   
- [getIndexMergeSuggestions](../../client-api/commands/indexes/how-to/get-index-merge-suggestions)   
- [deleteIndex](../../client-api/commands/indexes/delete)   
- [resetIndex](../../client-api/commands/indexes/how-to/reset-index)   
- [IndexHasChanged](../../client-api/commands/indexes/how-to/check-if-index-has-changed)   
- [SetIndexLock](../../client-api/commands/indexes/how-to/change-index-lock-mode)   
- [SetIndexPriority](../../client-api/commands/indexes/how-to/change-index-priority)   

### Transformer commands

The following commands can be used to manipulate transformers:   
- [putTransformer](../../client-api/commands/transformers/put)   
- [getTransformer](../../client-api/commands/transformers/get)   
- [getTransformers](../../client-api/commands/transformers/get)   
- [deleteTransformer](../../client-api/commands/transformers/delete)   

### Attachment commands

The following commands can be used to manipulate attachments:   
- [putAttachment](../../client-api/commands/attachments/put)   
- [getAttachment](../../client-api/commands/attachments/get)   
- [getAttachments](../../client-api/commands/attachments/get)   
- [headAttachment](../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [getAttachmentHeadersStartingWith](../../client-api/commands/attachments/how-to/get-attachment-metadata-only)  
- [deleteAttachment](../../client-api/commands/attachments/delete)  
- [updateAttachmentMetadata](../../client-api/commands/attachments/how-to/update-attachment-metadata-only)   

### Patch commands

The following commands can be used to patch:   
- [patch](../../client-api/commands/patches/how-to-work-with-patch-requests)   

### Query commands

The following commands can be used to query:   
- [query](../../client-api/commands/querying/how-to-query-a-database)   
- [streamQuery](../../client-api/commands/querying/how-to-stream-query-results)   

### Batch commands

The following commands can be used to send commands in a batch:   
- [batch](../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)   

### Administrative commands

The following commands can be used to perform administrative tasks on a server:   
- [createDatabase](../../client-api/commands/how-to/create-delete-database)   
- [deleteDatabase](../../client-api/commands/how-to/create-delete-database)   
- [compactDatabase](../../client-api/commands/how-to/compact-database)   
- [getStatistics](../../client-api/commands/how-to/get-database-and-server-statistics)   
- [startBackup](../../client-api/commands/how-to/start-backup-restore-operations)   
- [startRestore](../../client-api/commands/how-to/start-backup-restore-operations)   
- [stopIndexing](../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status)   
- [startIndexing](../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status)   
- [getIndexingStatus](../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status)   

## Related articles

- [How to **switch** commands to a different **database**?](../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to **switch** commands **credentials**?](../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get **server build number**?](../../client-api/commands/how-to/get-server-build-number)   
- [How to get **names of all databases** on a server?](../../client-api/commands/how-to/get-names-of-all-databases-on-a-server)   
