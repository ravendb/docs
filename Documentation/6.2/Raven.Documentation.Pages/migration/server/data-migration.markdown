# Data Migration
---

{NOTE: }

* In this page:
    * [Migration from RavenDB 5.x to 6.x](../../migration/server/data-migration#migration-from-ravendb-5.x-to-6.x)
    * [Migration from RavenDB 4.x to RavenDB 5.x and 6.x](../../migration/server/data-migration#migration-from-ravendb-4.x-to-ravendb-5.x-and-6.x)
    * [Migration from RavenDB 3.x](../../migration/server/data-migration#migration-from-ravendb-3.x)
    * [Migrating data into a sharded database](../../migration/server/data-migration#migrating-data-into-a-sharded-database)

{NOTE/}

---

{PANEL/}

{PANEL: Migration from RavenDB 5.x to 6.x}

* RavenDB `6.x` supports in-place migration of data from RavenDB `5.x`.
* During the first startup after [replacing the binaries](../../migration/server/data-migration#replacing-the-binaries) with the new ones, the data will migrate automatically.
* RavenDB `5.x` product licenses **do not apply** to RavenDB `6.x`.  
  To upgrade a valid `5.x` license to a RavenDB `6.x` license,  
  please use the **License upgrade tool** [as explained here](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x).

{WARNING: }
Please note that once upgraded, RavenDB `6.x` cannot be downgraded to version `5.x`,  
and the migrated data will no longer be accessible via RavenDB `5.x`.  
**Please create a backup of your data before migrating.**  
{WARNING/}

{PANEL/}

{PANEL: Migration from RavenDB 4.x to RavenDB 5.x and 6.x}

* RavenDB `5.x` supports in-place migration of data from RavenDB `4.x`.
  {INFO: }
  It is also possible to upgrade directly from version `4.x` to `6.x`,  
  but it is recommended to first upgrade RavenDB `4.x` to `5.x`, and then proceed from `5.x` to `6.x`.
  {INFO/}
* During the first startup after [replacing the binaries](../../migration/server/data-migration#replacing-the-binaries) with the new ones, the data will migrate automatically.
* RavenDB `4.x` product licenses **do not apply** to RavenDB `6.x`.  
  To upgrade a valid `4.x` license to a RavenDB `6.x` license,  
  please use the **License upgrade tool** [as explained here](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x).

{WARNING: }
Please note that once upgraded, RavenDB `6.x` cannot be downgraded to version `4.x`,  
and data migrated to `5.x` or `6.x` will no longer be accessible via RavenDB `4.x`.  
**Please create a backup of your data before migrating.**  
{WARNING/}

{PANEL/}

{PANEL: Migration from RavenDB 3.x}

* The information above relates only to data migration from RavenDB `4.x` to `5.x`/`6.x` and from `5.x` to `6.x`.
* If you want to migrate your data from a RavenDB version earlier than `4.x`,  
  please read the dedicated article [here](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration).

{PANEL/}

{PANEL: Migrating data into a sharded database}

If you want to migrate your data to a [sharded](../../sharding/overview) database (supported by RavenDB `6.0` and above),  
please read the related article [here](../../sharding/migration).

{PANEL/}
