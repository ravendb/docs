# HiLo Algorithm
---

{NOTE: }

The HiLo algorithm is an efficient solution used by [a session](../../session/what-is-a-session-and-how-does-it-work) 
to generate numeric parts of identifiers. 
It is responsible for providing numeric values that are combined with collection names and node tags **to 
create unique identifiers** like `orders/10-A`, `products/93-B`, etc. 

See various approaches to [work with document identifiers](../../client-api/document-identifiers/working-with-document-identifiers).  

To ensure that the HiLo algorithm is used to create the ID, include `myGenerator.Next()` as the second parameter of the 
`Session.Store()` method for creating a new document:  
`Session.Store(the collection name, myGenerator.Next())`  

* In this page:
   * [How the HiLo Algorithm Determines the Numeric Value](../../client-api/document-identifiers/hilo-algorithm#how-the-hilo-algorithm-determines-the-numeric-value)
   * [Returning HiLo Ranges](../../client-api/document-identifiers/hilo-algorithm#returning-hilo-ranges)
   * [Identity Parts Separator](../../client-api/document-identifiers/hilo-algorithm#identity-parts-separator)

{NOTE/}

---

{PANEL: How the HiLo Algorithm Determines the Numeric Value}

The client can determine to what collection an entity belongs.  
To distinguish documents between nodes and create unique identifiers, 
it also needs to add a unique number and add the node tag at the end of the document's identifier.

To ensure that multiple clients can generate the identifiers simultaneously, they need some mechanism to avoid duplicates. 
It is ensured with `Raven/HiLo/collection` documents, stored in a database, which are modified by the server. 
These documents have a very simple construction:

{CODE-BLOCK:json}
{
    "Max": 32,
    "@metadata": {
        "@collection": "@hilo"
    }
}
{CODE-BLOCK/}

The `Max` property means the maximum possible number that has already been used by any client to create the identifier for a given collection. 
It is used as follows:

1. The client asks the server for a range of numbers that it can use to create a document (32 is the initial capacity, the actual range size is calculated based on the frequency of getting HiLo by the client.)
2. Then, the server checks the HiLo file to see what is the last number it sent to any client for this collection.
3. The client will get from the server the min and the max values it can use (33 - 64 in our case).
4. Then, the client generates a range object from the values it got from the server to generate identifiers.
5. When the client reaches the max limit, it will repeat the process until it is done.

{PANEL/}

{PANEL: Returning HiLo Ranges}

When the document store is disposed, the client sends to the server the last value it used to create an identifier 
and the max value it got from the server.

If the max value on the server-side is equal to the max value of the client 
and the last used value by the client is smaller or equal to the max of the server-side. 
the server will update the `Max` value to the last used value by the client.

{CODE return_hilo_1@ClientApi\DocumentIdentifiers\HiloAlgorithm.cs /}

After execution of the code above, the `Max` value of the Hilo document in the server will be 1. 
That's because the client used only one identifier from the range it got before we disposed the store.

The next time that a client asks for a range of numbers from the server for this collection it will get (in our example) the range 2 - 33.

{CODE return_hilo_2@ClientApi\DocumentIdentifiers\HiloAlgorithm.cs /}

{PANEL/}

{PANEL: Identity Parts Separator}

By default, document IDs created by the server use the character `/` to separate their components. 
This separator can be changed to any other character except `|` in the 
[Document Store Conventions](../../client-api/configuration/conventions#changing-the-identity-separator).  

{PANEL/}

## Related Articles

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../server/kb/document-identifier-generation)

### Session

- [How to Get Entity ID](../../client-api/session/how-to/get-entity-id)

### Operations

- [How to Get Identities](../../client-api/operations/maintenance/identities/get-identities)

---

### Code Walkthrough

- [Create a Document](https://demo.ravendb.net/demos/csharp/basics/create-document)
