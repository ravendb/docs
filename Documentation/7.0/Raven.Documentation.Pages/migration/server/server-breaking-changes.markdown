# Migration: Server Breaking Changes
---

{NOTE: }
The features listed on this page were available in former RavenDB versions.  
In RavenDB `7.0.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:  
   * [RavenDB now incorporates NLog as its logging system](../../migration/server/server-breaking-changes#ravendb-incorporates-nlog-as-its-logging-system)  
   * [Removed obsolete properties](../../migration/server/server-breaking-changes#removed-obsolete-properties)  

{NOTE/}

---

{PANEL: RavenDB incorporates NLog as its logging system}
RavenDB's logging system has changed; the server now incorporates the 
NLog logging framework and writes all log data through it.  
One of the changes that NLog brings to RavenDB is the richer set 
of logging levels, visible right away through Studio's [admin-logs view](../../studio/server/debug/admin-logs).  
Read more about Nlog [in the dedicated article](../../server/troubleshooting/logging).
If you migrate to RavenDB `7.x` from an earlier version, please 
read the section related to NLog in the [migration page](../../migration/server/data-migration).  

{PANEL/}

{PANEL: Removed obsolete properties}
The following properties are no longer in use, and have been removed from RavenDB `7.0`.  

* `ServerOptions`'s `AcceptEula` property is no longer used,  
  Please use `{nameof(Licensing)}.{nameof(LicensingOptions.EulaAccepted)}` instead.  
  {CODE-BLOCK:csharp }
   public bool AcceptEula
  {CODE-BLOCK/}

* The `MemoryInfoResult` struct no longer includes these classes:  
   - `MemoryUsageIntervals`  
     {CODE-BLOCK:csharp }
     public sealed class MemoryUsageIntervals  
     {CODE-BLOCK/}
   - `MemoryUsageLowHigh`  
     {CODE-BLOCK:csharp }
     public sealed class MemoryUsageLowHigh  
     {CODE-BLOCK/}

{PANEL/}

## Related Articles

### Changes API
- [Changes API](../../client-api/changes/what-is-changes-api)  
- [Tracking operations](../../client-api/changes/how-to-subscribe-to-operation-changes)  

### Studio
- [Identity parts separator](../../studio/server/client-configuration#set-the-client-configuration-(server-wide))  
- [SQL connection string](../../studio/database/tasks/import-data/import-from-sql#create-a-new-import-configuration)  

### Server
- [ID Generation](../../server/kb/document-identifier-generation#id-generation-by-server)

### Corax
- [Corax](../../indexes/search-engine/corax)  
- [Complex fields](../../indexes/search-engine/corax#handling-of-complex-json-objects)  
- [Auto indexes](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing)  
