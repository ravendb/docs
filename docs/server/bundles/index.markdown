# RavenDB Bundles

RavenDB in its core is a document database, and as such it is feature complete and very functional. A user may potentially require additional functionality, such that is not implemented by the core, and this is where the bundles come in.

The following bundles are available for RavenDB, to extend its capabilities in various fields out of the box:

[FILES-LIST]

In the next chapter we are going to discuss how to create custom bundles.

Installing a Bundle is as easy as dropping the assembly (and all its dependencies if any) into the Plugins directory of the RavenDB installation. Some bundles may also require some minimal configurations, but thats about it.