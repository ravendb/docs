# Migration: Migrating from RavenDB 4.x to 5.x

* RavenDB `5.x` supports in-place migration of RavenDB `4.x` data.  
* During the first startup after replacing the binaries with the 
  new ones, the data will migrate automatically.  
* **Please note** that after migrating the data it will **no longer 
  be accessible** via RavenDB `4.x`.  
  {WARNING: }
  Please create a backup of your data before migrating.  
  {WARNING/}

{NOTE:Migration from RavenDB 3.x}
The information above related only to data migration from RavenDB 
`4.x` to `5.x`.  

* If you want to migrate your data from a RavenDB version earlier that `4.x`,  
  please read the dedicated article 
  [here](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).  

* If you want to migrate your data to a [sharded](../../sharding/overview) 
  database (supported by RavenDB `6.0` and above),  
  please read the related article [here](../../sharding/migration).  
{NOTE/}

