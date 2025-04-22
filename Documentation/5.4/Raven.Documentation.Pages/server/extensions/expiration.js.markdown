# Document Expiration
---

{NOTE: }

* Documents can be given a future expiration time in which they'll be automatically deleted.  
* The Expiration feature deletes documents set for expiration, when their time has passed.  
* You can enable or disable the expiration feature while the database is already live with data.  

* In this page:  
  * [Expiration feature usages](../../server/extensions/expiration#expiration-feature-usages)  
  * [Configuring the expiration feature](../../server/extensions/expiration#configuring-the-expiration-feature)  
  * [Setting the document expiration time](../../server/extensions/expiration#setting-the-document-expiration-time)  
  * [Eventual consistency considerations](../../server/extensions/expiration#eventual-consistency-considerations)  
 {NOTE/}

---

{PANEL: Expiration feature usages}

Use the Expiration feature when data is needed only for a given time period.  

Examples:

 * Shopping cart data that is kept only for a certain time period  
 * Email links that need to be expired after a few hours  
 * A web application login session details  
 * Cache data from an SQL server  

{PANEL/}

{PANEL: Configuring the expiration feature}

* By default, the expiration feature is turned **off**.  
* The deletion frequency is configurable.  
  The default frequency is 60 seconds.  
* The Expiration feature can be turned **on** or **off** using **Studio**,  
  see [Setting Document Expiration in Studio](../../studio/database/settings/document-expiration).  
* The Expiration feature can also be configured using the **Client**:
  {CODE:nodejs expiration_1@Server\Expiration\expiration.js /}

{PANEL/}

{PANEL: Setting the document expiration time}

* To set document expiration time add an `@expires` property to the document 
  `@metadata` and set the property's value to the designated expiration time.  
  {NOTE: }
  The date must be in **UTC** format, not local time.  
  {NOTE/}
  {WARNING: }
  Metadata properties starting with `@` are internal for RavenDB usage.  
  Do _not_ use metadata `@expires` property for any other usage but that of the built-in expiration feature.  
  {WARNING/}
* Once the Expiration feature is enabled, the document will be automatically deleted at the specified time.  
  {INFO: }
  Internally, each document that has the `@expires` property in the metadata is tracked 
  by the RavenDB server even if the expiration feature is turned off.  
  This way, once the expiration feature is turned on RavenDB can delete expired documents right away.  
  {INFO/}
* To set the document expiration time from the client, use the following code:
  {CODE:nodejs expiration_2@Server\Expiration\expiration.js /}

{PANEL/}

{PANEL: Eventual consistency considerations}

* Once documents expire, it may take up to the _delete frequency interval_ (60 seconds by default) 
  until they are actually deleted.  
* Expired documents are _not_ filtered out during `load`, `query`, or indexing, so be aware that an expired 
  document may still be included in the results after its expiration, for the period set by _delete frequency interval_.  

{PANEL/}

## Related Articles

### Studio

- [Setting Document Expiration in Studio](../../studio/database/settings/document-expiration)
