# Etags

### What are etags?
RavenDB uses etags to track changes of data. Etags are 64-bit numbers which always increase with each change.
Thus, if comparing etags for two versions of the same document on specific RavenDB instance, the version with a higher etag is more up-to-date.

### Etags in-depth
In RavenDB, etags track changes on many different types of server entities. 
Etags are used to track changes for the following:
* Documents
* Indexes
* Cluster Topology

For developing with RavenDB client API, etags are not needed. Etags are used as a "building block" of a [change vector](../server/clustering/change-vector).

### Etags and Clustering
When looking at etags in the context of a RavenDB cluster, etags are only meaningful within a single database on a specific node.
This means etags are _not_ universally unique per different databases on a single node and are not unique across cluster nodes.
