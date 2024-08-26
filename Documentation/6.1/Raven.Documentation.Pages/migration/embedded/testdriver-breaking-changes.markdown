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

RavenDB `5.4` introduced the `ThrowOnInvalidOrMissingLicense` exception, thrown 
if TestDriver runs using an unlicensed server to notify users they may miss out 
on much of their system's potential.  
a `TestServerOptions.License.ThrowOnInvalidOrMissingLicense` flag was introduced 
along with it, determining whether the exception would be thrown or not when the 
server is used without license.  

In version `5.4`, the default value we gave this flag was `false`; an exception 
**wouldn't** be thrown even if the server was unlicensed. For an exception to be 
thrown, users needed to change the flag to `true` on their own initiative.  

### The breaking change:

In version `6.1`, we **changed** the default value of the `TestServerOptions.License.ThrowOnInvalidOrMissingLicense` 
flag to `true`; a `ThrowOnInvalidOrMissingLicense` exception **would** be thrown if 
a license wasn't provided yet while running TestDriver.  

Users that prefer not to license their embedded server for some reason can disable 
the exception by changing the value of this flag to `false`. TestDriver tests will 
still run, but server capabilities will be limited to those defined by the basic 
[AGPL](https://ravendb.net/legal/ravendb/commercial-license-eula) license.  

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
