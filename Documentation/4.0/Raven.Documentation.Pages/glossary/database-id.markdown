# Glossary: Database Id
  
### What are Database Ids?
Each database in RavenDB has a Guid, which must be unique in the cluster. 

### Generating New Database ID
When restoring a snapshot, a new database Id may be generated on restore. This is needed if the same snapshot needs to be restored on multiple nodes so that the uniqueness of database IDs in the cluster is conserved.
