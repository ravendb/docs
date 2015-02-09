#What is RavenFS?

The Raven File System (RavenFS) is a distributed virtual file system integrated with RavenDB to provide a first class support for binary data.
Since RavenDB 3.0 it is the recommended way to store your binary files instead of the deprecated attachment mechanism.

It was designed upfront to handle very large files (multiple GBs) efficiently at API and storage layers level by minimizing the amount of duplicated data between files.
It has a built-in file indexing support that allows you to search files by their associated metadata (such as size of a file, a modification date or custom ones defined by user).

RavenFS is a replicated and highly available system. It provides an optimized file synchronization mechanism which ensures that only differences between a file are transferred
over network to synchronize it between configured nodes. This lets you update very large files and replicate only the changes - everything is transparent for a user, you just need
to specify destination nodes.

##Basic concepts

###File

An essential item that you will work with is a file. Besides binary data that makes up a file's content, each one has associated metadata. There are two kinds of metadata:

* the first one is provided by the system and internally used by it (for instance: `ETag`),
* the second one is defined by a user and can contain any information under a custom key.

As it was already mentioned metadata is available for searching. More details about files are stored internally you will find in [Files](files) article.

###Configuration

A configuration is an item for keeping non-binary data as a collection of key/value properties stored under a unique name. Note that configurations can be 
completely unrelated to your files but they can hold additional information that matters for your application. They are also used internally by RavenFS to store
some configuration settings (i.e. `Raven/Synchronization/Destinations` keeps addresses of synchronization destination nodes).

###Indexing

Files are indexed by default. It allows you to execute the queries against metadata of stored files. Under the hood, the same like in RavenDB, 
Lucene search engine is used. This allows you to do an efficient search by using file name, its size and metadata.

###Synchronization

A synchronization between RavenFS nodes works out of the box. The only thing you need to do is to provide a list of destination file systems. 
Once one of the following events happens, then it will automatically start to synchronize an affected file:

* new file uploaded,
* file content changed,
* file metadata changed.
* file renamed,
* file deleted.

The synchronization task also runs periodically to handle failures and restart scenarios. Each of the above operations is related with a different kind of
synchronization work, which is determined by the server in order to minimize the amount of transferred data across the network. For example if you just change
a file name then there is no need to sent its content, just the destination nodes know what is a new file name. To get more details about implemented synchronization solutions click [here]().

##Management studio

You can easily manage your files by using HTML5 application studio. Databases as well as file systems are handled by the same application accessible under RavenDB server url.

![Figure 1. Studio. File system](images/studio_view.png)  
