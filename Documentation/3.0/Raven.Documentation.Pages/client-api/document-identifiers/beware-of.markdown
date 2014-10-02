# Beware of ...

## Numeric identifiers

RavenDB was designed to work with string document identifiers. On the server side every document has a unique string key assigned. However identifiers of entities stored 
by the client can be modeled as numbers. RavenDB client has support for such aproach and is able to perform a necessary conversion 
(for example `employees/12` key would be changed to `12` if `Employee.Id` is an integer). You can see that there are even a dedicated methods exposed by the session
to work in this fashion:

{CODE session_value_types@ClientApi\DocumentIdentifiers\BewareOf.cs /}

However internally in a database these documents still have string identifiers and the client just pretends that it is an integer. The problem with this approach is
you have a difference between the client-side model and actual data in database. The client is able to handle that however such components as indexes and transformes
that are defined on the client side but executed on the database side cannot work with such approach without issues. 

## Unsequential identifiers

[The key generation conventions]() give to you a great flexibility in creating custom identifiers for documents. You are able to assign to a document every key as you can imagine. 
Everything is going to work correctly however you have to be aware that some kind of identifiers might cause performance issues when number of documents with 
custom generated IDs is very high (millions of documents).

The very unrecommended way is to use non-sequential data as the document key (e.g. created by hash functions or Guids). In such case you might be experiencing a declining 
performance of searching for an existing document and inserting new ones. If your intention is to use custom identifiers and you expect that the very high number 
of such documents might be saved in your database, then we suggest to make use of incremented identifiers in order to ensure the good performance.
