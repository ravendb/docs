# Embedded Server: TestDriver Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `6.1.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:  
   * [Unlicensed TestDriver throws an exception](../../migration/embedded/testdriver-breaking-changes#unlicensed-testdriver-throws-an-exception)  

{NOTE/}

---

{PANEL: Unlicensed TestDriver throws an exception}

### Background:

The [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) package 
allows users to create [unit tests](../../start/test-driver) for their applications, 
and run the tests using an [embedded server](../../server/embedded) included in the package.  

Like other types of RavenDB server, the features that an embedded server supports 
and the resources it can use are defined by its [license](https://ravendb.net/buy).  
An unlicensed server, for example, can use only 3 CPU cores, while a server 
licensed using a [free developers license](https://ravendb.net/buy#developer) 
can use up to 9 cores and run way faster.  

* When a RavenDB server starts, its license is validated.  
   * If the validation succeeds, the server will run and offer the capabilities defined 
     by its license.  
   * If the validation fails, the server may still run but limit its capabilities to those 
     defined by the basic [AGPL](https://ravendb.net/legal/ravendb/commercial-license-eula) 
     license.  
     {NOTE: }
     If the validation fails because the license expired, and the expiration date precedes 
     the server build date, the server will not start at all.  
     {NOTE/}

* A `TestServerOptions.Licensing.ThrowOnInvalidOrMissingLicense` configuration option 
  is available since RavenDB `5.4`, determining whether to throw a `LicenseExpiredException` 
  exception if TestDriver uses an unlicensed embedded server.  
   * If `ThrowOnInvalidOrMissingLicense` is set to **`true`** and the validation fails, 
     a `LicenseExpiredException` exception will be thrown to **warn TestDriver users** 
     that in lack of a valid license, their server's capabilities are limited and they 
     may therefore miss out on much of their system's potential.  
   * If the configuration option is set to **`false`**, **no exception will be thrown** 
     even if a license cannot be validated.  

---

### The breaking change:

Up until RavenDB version `6.0` we set `TestServerOptions.Licensing.ThrowOnInvalidOrMissingLicense` 
to **`false`** by default, so no exception would be thrown even if license validation fails.  
For an exception to be thrown, users needed to change the flag to **`true`** on their own initiative.  

In version `6.1`, the default value for this configuration option **changed** to **`true`**;  
a `LicenseExpiredException` exception **would** be thrown if the embedded server used by 
TestDriver fails to validate a license.  

Users that prefer that no exception would be thrown if an unlicensed embedded server is 
used, can set `TestServerOptions.Licensing.ThrowOnInvalidOrMissingLicense` to **`false`**.  

{PANEL/}

## Related Articles

### Embedded Server
- [Running an Embedded Instance](../../server/Embedded)  
- [Embedded Server Options](../../server/embedded#server-options)  

### TestDriver
- [TestDriver](../../start/test-driver)  
- [TestDriver Licensing](../../start/test-driver#licensing)  

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
