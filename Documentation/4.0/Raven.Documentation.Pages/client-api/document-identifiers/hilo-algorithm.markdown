# HiLo algorithm

The HiLo algorithm is an efficient solution used by [a session](../session/what-is-a-session-and-how-does-it-work) to generate numeric parts of identifiers. In other words it is
responsible for providing numeric values that are combined with collection names and node ID to create keys like `orders/10-A`, `products/93-B` etc. 

The client is able to determine to what type of the collection does an entity belong. In order to differ documents, it also needs to add a unique number and the node tag at the end of the document's key.
To ensure that multiple clients can generate the keys simultaneously, they need some mechanism to avoid duplicates. It is ensured with `Replication/HiLo/collection` documents, stored 
in a database, which are modified by the server. These documents have a very simple construction:

{CODE-BLOCK:json}
{
    "Max": 32,
    "@metadata": {
        "@collection": "@hilo"
    }
}
{CODE-BLOCK/}

The `Max` property means the maximum possible number that has been used by any client to create the key for a given collection. It is used as follows:

1. The client ask the server for a range of numbers that he can use to create document (32 is the default)
2. then the server check the hilo file to see what is the last number he sent to any client for this collection.
3. The client will get from the server the min and the max values he can use (33 - 64 in our case).
4. Then the client generate a range object from the values he got from the server to generate identifiers.
5. When the client reach the max limit he will repeat the process until he done.
