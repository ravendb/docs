# Backward Compatibility

## Client v1 to Server v2

Once you upgrade your from version 1 to version 2, you may still have a production application which still make use of the client DLLs of version 1. For the most part, the client of version 1 is compatible with server 2, expect for the following cases:

* **Facets:**	the implementation of facets was change is version 2, and it's not compatible with the Client v1.
* **Reduce with Average or Sum:**	Using the `.Avarage()` or `.Sum()` extentsion methods in the reduce statmenets is not allowed in V2, since it produce inconsistent results.

So if you do not use one of the above cases, you can safly upgrade your server to v2 without you need to upgrade all of you applications to use the v2 of the client, since the v1 of the client should be working fine the v2 of the server.
