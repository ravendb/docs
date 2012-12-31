﻿# Index administration

RavenDB indexes can be administrated easily from the consumer end using either code or the embedded user interface.

## Resetting an index

An index usually need to be reset because it has reached its error quota and been disabled. Resetting an index means forcing RavenDB to re-index all documents matched by the index definition, which can be a very lengthy process.

You can reset an index in one of the following ways:

* Using the embedded API, by calling: `DocumentDatabase.ResetIndex(indexName)`
* Using the HTTP API, by issuing a HTTP call to the index with RESET as the method name: 
RESET /indexes/indexName

## Deleting an index

You can delete an index by calling `DocumentDatabase.DeleteIndex(indexName)` from the Client API.