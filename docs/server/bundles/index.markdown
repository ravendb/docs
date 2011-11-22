﻿# RavenDB Bundles

RavenDB in its core is a document database, and as such it is feature complete and very functional. A user may require additional functionality, such that is not implemented by the core, and this is where the bundles come in.

The following bundles are available out of the box, to extend RavenDB's capabilities in several common scenarios:

* Sharding and Replication
* Quotas - to put size limits on a database
* Expiration - removes expired documents automatically
* Index Replication - replicates a RavenDB index to SQL Server
* Authentication - authenticates DB users using OAuth
* Authorization - allows to manage user groups, roles and permissions
* Versioning - automatic versioning of documents upon changes or deltes
* Cascade Deletes - automatic cascade deletes operations
* More Like This - Returns documents that are related to a given document

In the next chapter we will discuss how to create custom bundles, to address custom user needs.

Installing a Bundle is as easy as dropping the assembly (and all its dependencies if any) into the Plugins directory of the RavenDB installation. Some bundles may also require some minimal configurations.

The path to the Plugins folder is configurable, see (configurations). By default it is (RavenDB server)\Plugins.

Included in the RavenDB distribution package are two scripts, enabling easy installation and update of Bundles.

// TODO