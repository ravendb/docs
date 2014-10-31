# Bundle : Compression

To reduce the cost of I/O operations and reduce the size of the database on disk, we have introduced the `Compression` bundle. Compression is only applied on documents, indexes are not compressed. This is because RavenDB is performing a lot of random reads from indexes, whereas with documents, we almost always read/write the full content. Worth noting is that compression process is fully transparent for the end-user.

## How to enable compression

To activate compression server-wide just add `Compression` to `Raven/ActiveBundles` configuration in global configuration file or setup new database with compression bundle turned on using API or Studio.

How to create a database with compression enabled using Studio can be found [here]().

{CODE compression_1@Server\Bundles\Compression.cs /}

Above example demonstrates how to create new database called `CompressedDB` with `Compression` bundle enabled.

{WARNING Activating **compression** bundle is only supported for **new databases**. Activating or disabling compression on an already existing database will cause DB malfunction. /}

## Related articles

TODO