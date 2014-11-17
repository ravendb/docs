# Beware of ...

## Numeric identifiers

RavenDB was designed to work with string document identifiers. On the server side every document has a unique string key assigned. However, the identifiers of entities stored 
by the client can be modeled as numbers. RavenDB client has support for such approach and is able to perform a necessary conversion 
(for example `employees/12` key would be changed to `12` if `Employee.Id` was an integer). You can see that there are even a dedicated methods exposed by the session to work in this fashion:

{CODE session_value_types@ClientApi\DocumentIdentifiers\BewareOf.cs /}

Nevertheless, internally, namely in a database, these documents still have string identifiers, and the client just pretends that it is an integer. The problem with this approach is that
you have a difference between the client-side model and the actual data in a database. The client is able to handle that, however components such as indexes and transformers,
which are defined on the client side but executed on the database side, cannot work with such approach without issues. 

## Non-sequential identifiers

[The key generation conventions](../../client-api/configuration/conventions/identifier-generation/global) gives you a great flexibility in creating custom identifiers for documents. You can assign any key you imagine to a document.
Everything is going to work well, yet you have to be aware that some kind of identifiers might cause performance issues when a number of documents with custom generated IDs is very high (millions of documents).

Using non-sequential data as the document key (e.g. created by hash functions or GUID) is not recommended. In such case you might be experiencing a declining 
performance while searching for an existing documents and inserting new ones. If your intention is to use custom identifiers and you expect that the very high number 
of such documents might be saved in your database, then we suggest you make use of incremented identifiers in order to ensure the good performance.

## GetSortKey

`GetSortKey` result is not stable and should **not** be used to make a case sensitive Id unique.
