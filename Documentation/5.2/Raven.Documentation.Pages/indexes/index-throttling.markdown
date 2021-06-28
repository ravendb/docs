# Indexes: Index Throttling
---

{NOTE: }

* **Index Throttling** delays indexing operations by a time interval of your choice.  
  Indexing is triggered normally when items are added or modified, but RavenDB suspends 
  index procession by the set interval.  
* The gaps in indexing activity and the procession of 
  [larger batches](../indexes/index-throttling#throttling-and-batches) 
  can reduce overall indexing CPU utilization.  
* Indexing can be throttled server-wide, per-database, and per-index.  
* Index throttling applies to the procession of **existing** indexes.  
  **New** indexes are processed sequentially without delay.  

* In this page:  
  * [Why Use Index Throttling?](../indexes/index-throttling#why-use-index-throttling?)  
  * [Index Throttling and Batches](../indexes/index-throttling#throttling-and-batches)  
  * [Setting Index Throttling](../indexes/index-throttling#setting-index-throttling)  
     * [Server-Wide](../indexes/index-throttling#server-wide-index-throttling)  
     * [Per-Database](../indexes/index-throttling#index-throttling-per-database)  
     * [Per-Index](../indexes/index-throttling#index-throttling-in-an-index-definition)  

{NOTE/}

---

{PANEL: Why Use Index Throttling?}

Indexing a rapidly modified group of items (e.g. a time series, a group of counters, 
or a patched collection) can preoccupy RavenDB with lengthy consecutive indexing of 
small batches of items.  

![Throttles Index Performance View](images/index-throttling-01.png "Throttles Index Performance View")

The Indexing Performance view shown above demonstrates the procession of a throttled index 
along the timeline, with a 1500 Milliseconds gap between consecutive batches.  
Throttling offers indexing-free time gaps that the database can use for other 
tasks and the procession of larger batches when indexing does take place.  

{INFO: }
Throttled indexes are kept [stale](../indexes/stale-indexes#indexes-stale-indexes) 
as long as their procession is incomplete.  
They are therefore expected to remain stale longer than indexes that are processed sequentially.  
{INFO/}

{PANEL/}

{PANEL: Throttling and Batches}

The delay in index procession increases the size of Item [batches](../server/configuration/indexing-configuration#indexing.mapbatchsize) 
that are processed during indexing intervals.  

In special cases, throttled indexes' batches are processed without delay.  

* The batch following a batch that cannot be processed for known reasons, will be processed 
  without delay.  
* Accumulated data (collected due to batch size configuration, for example) whose procession 
  requires multiple batches, will be processed with no delay between batches.  

{PANEL/}

{PANEL: Setting Index Throttling}

---

### Server-Wide Index Throttling

 Index Throttling can be set server-wide using a designated [configuration option](../server/configuration/configuration-options#json).  
 Setting the server-wide configuration option will apply to all databases on all nodes.  

* Set the index throttling time interval in Milliseconds using the `Indexing.Throttling.TimeIntervalInMs` configuration option.  
  `"Indexing.Throttling.TimeIntervalInMs": "5000"`  

---

### Index Throttling Per-Database 

Enable or disable index throttling for a specific database using the designated database configuration key.  
Setting this property overrides the 
[Server-Wide](../indexes/index-throttling#server-wide-index-throttling) default.  
 
* From Studio:  

    ![Database Configuration Keys](images/index-throttling-02.png "Database Configuration Keys")

      1. Open **Settings** > **Database Settings** view.  
      2. **Filter Keys** - Enter a search string to locate the configuration key.  
      3. **Edit** - Click to edit values (see next image for details).  
      4. **Configuration Key** -  
         `Indexing.Throttling.TimeIntervalInMs` - Index Throttling configuration key.  
      5. **Effective Value** - The current configuration.  
      6. **Origin** - The origin of the current configuration.  
         Can be - Default | Database  

    ![Edit Values](images/index-throttling-03.png "Edit Values")

      1. **Override** - Toggle to override the server-wide configuration.  
      2. **Edit Value** - Enter a new time in Milliseconds, or leave empty for null (no throttling).  
      3. **Set Default** - Click 'Set Default' to select the server-wide default value.  
      4. **Save** - Apply changes.  
         {WARNING: }
         An edited configuration key's value will become effective only after the database is reloaded.  
         {WARNING/}

---

### Index Throttling in an Index Definition

Set throttling for a specific index using the `Indexing.ThrottlingTimeInterval` property.  
Setting this property overrides [server-wide](../indexes/index-throttling#server-wide-index-throttling) 
and [database](../indexes/index-throttling#index-throttling-per-database) settings configuration.  
  
{CODE-BLOCK:csharp}
var index = new Orders_ByOrder
{
    Configuration =
    {
        [RavenConfiguration.GetKey(x => x.Indexing.ThrottlingTimeInterval)] = "2000",
    }
};
await index.ExecuteAsync(store);
{CODE-BLOCK/}

The batch size can be left for RavenDB to decide, or you can set it yourself using 
the [MapBatchSize](../server/configuration/indexing-configuration#indexing.mapbatchsize) property.  
{CODE-BLOCK:csharp}
[RavenConfiguration.GetKey(x => x.Indexing.MapBatchSize)] = "500"
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Indexes
- [auto Indexes](../indexes/creating-and-deploying#auto-indexes)  
- [static Indexes](../indexes/creating-and-deploying#static-indexes)  

### Server
- [configuration options](../server/configuration/configuration-options#json)  