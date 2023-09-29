# Migration: Migrating from RavenDB 5.x to 6.x

* RavenDB `6.x` supports in-place migration of RavenDB `5.x` data.  
* During the first startup after replacing the binaries with the new ones, 
  the data will migrate automatically.  
* RavenDB `5.x` product licenses **do not apply** to RavenDB `6.0`.  
  To upgrade a valid `5.x` license to a RavenDB `6.x` license, please 
  use the [License upgrade tool](https://ravendb.net/l/8O2YU1).  

{WARNING: }
Please note that once upgraded, RavenDB `6.x` cannot be downgraded 
to version `5.x`, and the migrated data will no longer be accessible via RavenDB `5.x`.  
**Please create a backup of your data before migrating.**  
{WARNING/}

## Migration from RavenDB 4.x

* RavenDB `5.x` supports in-place migration of RavenDB `4.x` data.  
  {INFO: }
  
   * It is **recommended** to upgrade RavenDB `4.x` to RavenDB `5.x` first, 
     and from `5.x` to version `6.x`.  
  
   * **Note** that RavenDB `4.x` product licenses **do not apply** to RavenDB `6.0`.  
     To upgrade a valid `4.x` license to a RavenDB `6.x` license, please use the 
     [License upgrade tool](https://ravendb.net/l/8O2YU1).  
  
  {INFO/}

* During the first startup after replacing the binaries with the 
  new ones, the data will migrate automatically.  

{WARNING: }
Please note that after migrating the data it will no longer be accessible via RavenDB `4.x`.  
**Please create a backup of your data before migrating.**  
{WARNING/}


## Migration from RavenDB 3.x

The information above relates only to data migration from RavenDB `4.x` to `5.x`/`6.x` 
and from `5.x` to `6.x`.  

* If you want to migrate your data from a RavenDB version earlier than `4.x`,  
  please read the dedicated article 
  [here](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).  

* If you want to migrate your data to a [sharded](../../sharding/overview) 
  database (supported by RavenDB `6.0` and above), please read the related 
  article [here](../../sharding/migration).  
