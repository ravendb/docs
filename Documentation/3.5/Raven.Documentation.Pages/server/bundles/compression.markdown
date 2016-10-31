# Bundle : Compression

To reduce the cost of I/O operations and reduce the size of the database on a disk, we have introduced the `Compression` bundle. Compression is only applied to documents, indexes are not compressed. This is because RavenDB is performing a lot of random reads from indexes, whereas with documents we almost always read/write the full content. Note that the compression process is fully transparent for the end-user.

## How to enable compression

To activate compression server-wide, just add the `Compression` to the `Raven/ActiveBundles` configuration in the global configuration file or set up a new database with compression bundle turned on, using API or the Studio.

{NOTE:Important}
Any bundle which is not added to ActiveBundles list, will not be active, even if it is built-in or the relevant assembly is in the `Plugins` directory.
{NOTE/}

More on how to create a database with compression enabled using the Studio [here](../../studio/walkthroughs/how-to-setup-compression).

{CODE compression_1@Server\Bundles\Compression.cs /}

The above example demonstrates how to create a new database called `CompressedDB` with  the `Compression` bundle enabled.

{WARNING Activating the **compression** bundle is only supported for **new databases**. Activating or disabling compression on an already existing database will cause DB malfunction. /}
