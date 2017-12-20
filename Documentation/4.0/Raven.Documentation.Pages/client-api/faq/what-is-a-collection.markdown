#What is a collection?

A collection in RavenDB is a set of documents with the same `@collection` metadata property which is filled in by the client based on the type of 
an entity object that you store (the function responsible for tagging documents can by overwritten by using [customizations](../../client-api/configuration/conventions/identifier-generation/global#findtypetagname-and-finddynamictagname)). 
Also if the documents are inserted through the studio a `@collection` metadata will be generated for them, e.g `users|`/`users/`/`users/17` will have `@collection:Users`.
Note that documents that are in the same collection can have a completely different structure, which is fine because RavenDB is schema-less.


The collection is just a virtual concept. There is no influence on how or where the documents within the same collection are stored. However the collection concept
has a great meaning for three RavenDB features: [the studio](../../studio/overview/documents/documents-view), [the indexes](../../indexes/what-are-indexes) and [the document key generation](../../client-api/document-identifiers/working-with-document-ids) on the client side.

##Collections usage

###Studio

Probably the first time you encounter the collection will be while accessing the studio. Then you will see that, for example, the `Order` entity that you have just stored is visible under 
`Orders` collection (by default the client pluralizes the collection name based on the type name). But how does it happen that the virtual concept of the collections is
visualized in the studio. The answer is that each RavenDB database has a storage index per collection, which is used internally to query the database and retrieve
only  documents from the specified collection. This way the studio can group the documents into the collections.


###Indexing

Each Ravendb index is built against a collection (or collections when using [multi map index](../../indexes/multi-map-indexes)) we use a storage index to iterate documents of the same collection in the indexing process.

###Document keys

The default convention is that documents have the identifiers like `orders/1-A` where `orders` is just a collection name and `1` is the identity value and `A` is the `Server's Tag`. 
Also the ranges of available identity values returned by [HiLo algorithm](../../client-api/document-identifiers/hilo-algorithm) are per collection name.
