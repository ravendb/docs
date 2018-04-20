# FAQ : What is a Collection

A collection in RavenDB is a set of documents with the same `@collection` metadata property which is filled in by the client based on the type of entity object that you store (the function responsible for tagging documents can by overwritten by using [customizations](../../client-api/configuration/identifier-generation/global#findtypetagname-and-finddynamictagname)). 

If the documents are inserted through the studio, a `@collection` metadata will be generated for them, e.g `users|`/`users/`/`users/17` will have `@collection:Users`.

Documents that are in the same collection can have a completely different structure. This is a major advantage of using a schemaless database.

The collection is just a virtual concept. There is no influence on how or where the documents within the same collection are stored. 

The collection concept has a great meaning for three RavenDB features: [the Studio](../../studio/database/documents/documents-and-collections), [the indexes](../../indexes/what-are-indexes) and [the document ID generation](../../client-api/document-identifiers/working-with-document-identifiers) on the client side.

##Collections Usage

###Studio

The first time you encounter the collection will likely be while accessing the studio. You will see that, for example, the `Order` entity that you have just stored is visible under the `Orders` collection. By default, the client pluralizes the collection name based on the type name. 

How that happens in the virtual concept of the collections is visualized in the studio. Each RavenDB database has a storage index per collection, which is used internally to query the database and retrieve only documents from the specified collection. This way the Studio can group the documents into the collections.

###Indexing

Each RavenDB index is built against a collection (or collections when using [multi map index](../../indexes/multi-map-indexes)) and we iterate over the collection's documents during the indexing process.

###Document IDs

The default convention is that documents have the identifiers like `orders/1-A` where `orders` is just a collection name and `1` is the identity value and `A` is the `Server's Tag`. 

The ranges of available identity values returned by [HiLo algorithm](../../client-api/document-identifiers/hilo-algorithm) are per collection name.
