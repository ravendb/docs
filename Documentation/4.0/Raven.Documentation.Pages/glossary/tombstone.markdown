# Glossary: Tombstones

When a document is deleted, RavenDB will leave behind a "delete marker" which is called a Tombstone. 
  
### Where are They Used?

* Replication and ETL - this is needed so delete operations can be replicated to other nodes
* Indexes use tombstones in order to delete no longer relevant entries that refer to deleted documents
* Periodic Backup uses tombstones in order to backup "deletions" of documents

### Tombstone Cleaning

The tombstones are periodically cleaned.  

Cleaning will occur only for tombstones that were already processed by the modules where they are used:

* Replication
* Indexes
* ETL 
* Periodic Backup

{NOTE: The tombstone retaining period is configurable with config entry name = `Tombstones.CleanupIntervalInMin`. By default, the configuration value it is 5 minutes /}
