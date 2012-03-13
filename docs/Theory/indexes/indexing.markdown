#Indexing information

##Resetting an index
An index usually need to be reset because it have reached its error quota and been disabled. Resetting an index means forcing re-indexing of all the matching documents. You can reset an index in one of the following ways:

* Using the embedded API, by calling: DocumentDatabase.ResetIndex(indexName)
* Using the HTTP API, by issuing a HTTP call to the index with RESET as the method name:  
    RESET /indexes/indexName

##Special document properties
Each Raven document has a set of pre-defined document automatically created by RavenDB to manage the document. In some circumstances it might be useful to create an index against these properties.

*doc.__document_id - the document id used to access the document. This property is indexed when using simple indexes (without a reduce query). You can query for just the document ids by issues a query on an index and asking just for the __document_id property.