# Data Migration
---

{NOTE: }

* In this page:
    * [Migration to RavenDB `7.x`](../../migration/server/data-migration#migration-to-ravendb-7.x)
    * [Migration from RavenDB 5.x to 6.x](../../migration/server/data-migration#migration-from-ravendb-5.x-to-6.x)
    * [Migration from RavenDB 4.x to RavenDB 5.x and 6.x](../../migration/server/data-migration#migration-from-ravendb-4.x-to-ravendb-5.x-and-6.x)
    * [Migration from RavenDB 3.x](../../migration/server/data-migration#migration-from-ravendb-3.x)
    * [Migrating data into a sharded database](../../migration/server/data-migration#migrating-data-into-a-sharded-database)

{NOTE/}

---

{PANEL: Migration to RavenDB `7.x`}

{CONTENT-FRAME: Migration and NLog:}

Starting with version `7.0`, RavenDB incorporates the 
[NLog logging frmework](../../server/troubleshooting/logging) and writes all log 
data through it.  

Logging settings applied in earlier RavenDB versions are respected by RavenDB `7.x`, 
and logging should continue by these settings without interference after the migration.  

If you want to use NLog-specific features, though, you will have to address a different set 
of settings that NLog requires.  
You can [learn more here about migration and the new logging system](../../server/troubleshooting/logging#customize-after-migration).  

{CONTENT-FRAME/} 

---

{CONTENT-FRAME: Migration and HTTP Compression:}

From RavenDB `7.0` on, the default HTTP compression algorithm is `Zstd`.  
Earlier versions used `Gzip`.  

* If your current server version is `6.0` or higher, the compression algorithm 
  will present no problem while connecting it to a server of version `7.0` and 
  migrating your data.  

* If your current server version is `5.4` or earlier, attempting to connect it 
  to a server that uses the `Zstd` compression algorithm will fail.  
  For the connection to succeed, you need to:  
   1. Temporarily switch the target version `7.0` server compression algorithm to `Gzip`.  
      Do this by defining a `RAVEN_HTTP_COMPRESSION_ALGORITHM` environment variable on 
      the `7.0` server machine and setting its value to `Gzip`, and restarting the server.  
   2. Connect your current server to the new server and perform the migration.  
   3. When the new server is updated, remove the environment variable and restart the server.  

{CONTENT-FRAME/}

{PANEL/}

{PANEL: Migration from RavenDB 5.x to 6.x}

* RavenDB `6.x` supports in-place data migration from RavenDB `5.x`.
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

* RavenDB `5.x` supports in-place data migration from RavenDB `4.x`.
  {INFO: }
  Upgrading directly from version `4.x` to `6.x` is possible,  
  but it is recommended to upgrade RavenDB `4.x` to `5.x` first,  
  and then proceed with an upgrade from version `5.x` to `6.x`.
  {INFO/}
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
