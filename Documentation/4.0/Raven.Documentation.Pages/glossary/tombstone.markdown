# Tombstones
When a document is deleted, RavenDB will leave behind a "delete marker", which is called a Tombstone. 
  
### Where are they used?
* Replication - this is needed so delete operations can be replicated to another nodes.
* Indexes use tombstones in order to delete no longer relevant entries that refer to deleted documents.
* Periodic Backup uses tombstones in order to backup "deletions" of documents.

### Tombstone Cleaning
The tombstones are periodically cleaned (the period is configurable with config entry name = `Tombstones.CleanupIntervalInMin`, by default it is 5 minutes).  
Cleaning will occur only for tombstones that were already processed by the modules where they are used (Replication, Indexes and Periodic Backup).
