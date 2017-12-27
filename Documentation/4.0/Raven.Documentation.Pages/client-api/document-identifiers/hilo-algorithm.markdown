# HiLo algorithm

The HiLo algorithm is an efficient solution used by [a session](../session/what-is-a-session-and-how-does-it-work) to generate numeric parts of identifiers. In other words it is
responsible for providing numeric values that are combined with collection names and node ID to create keys like `orders/10-A`, `products/93-A` etc. 

The client is able to determine to what type of the collection does an entity belong. In order to differ documents, it also needs to add a unique number and the node ID at the end of the document's key.
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

1. The client gets from the server the max and the lowest value of Ids he can use.
2. Then the client  generate a range of numbers it can use to generate identifiers.
3. Then the client start to check if he is in the range and return the current id.
4. In case it reach the limit the client will repeat the process.
