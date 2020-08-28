# HiLo Algorithm

The HiLo algorithm is an efficient solution used by [a session](../session/what-is-a-session-and-how-does-it-work) to generate numeric parts of identifiers. It is responsible for providing numeric values that are combined with collection names and node tags to create identifiers like `orders/10-A`, `products/93-B` etc. 

The client is able to determine to what type of the collection an entity belongs to. In order to distinguish documents between nodes and avoid conflicts, it also needs to add a unique number and the node tag at the end of the document's identifier.

To ensure that multiple clients can generate the identifiers simultaneously, they need some mechanism to avoid duplicates. It is ensured with `Raven/HiLo/collection` documents, stored in a database, which are modified by the server. These documents have a very simple construction:

{CODE-BLOCK:json}
{
    "Max": 32,
    "@metadata": {
        "@collection": "@hilo"
    }
}
{CODE-BLOCK/}

The `Max` property means the maximum possible number that has been used by any client to create the identifier for a given collection. It is used as follows:

1. The client asks the server for a range of numbers that he can use to create document (32 is the initial capacity, the actual range size is calculated based on the frequency of getting HiLo by the client.)
2. Then, the server checks the HiLo file to see what is the last number he sent to any client for this collection.
3. The client will get from the server the min and the max values he can use (33 - 64 in our case).
4. Then, the client generates a range object from the values he got from the server to generate identifiers.
5. When the client reaches the max limit, he will repeat the process until he is done.

{NOTE: Returning HiLo ranges}
When the document store is disposed, the client sends to the server the last value it used to create an identifier and the max value he got from the server.

If the max value in the server-side is equal to the max value of the client and the last used value by the client is smaller or equal to the max of the server-side, the server will update the `Max` value to the last used value by the client.

{CODE return_hilo_1@ClientApi\DocumentIdentifiers\HiloAlgorithm.cs /}

After execution of the code above, the `Max` value of the Hilo document in the server will be 1. That's because the client used only one identifier from the range he got before we disposed the store.

The next time that a client asks for a range of numbers from the server for this collection he will get (in our example) the range 2 - 33.

{CODE return_hilo_2@ClientApi\DocumentIdentifiers\HiloAlgorithm.cs /}

{NOTE/}

{INFO: Identity Parts Separator}
By default, document IDs created by the server use the character / to separate their components. 
This separator can be changed to any other character except | in the 
[Document Store Conventions](../../client-api/configuration/conventions#changing-the-identity-separator).  
{NOTE/}

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
