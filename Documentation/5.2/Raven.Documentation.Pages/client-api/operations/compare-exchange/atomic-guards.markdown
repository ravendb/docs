# Compare Exchange: Atomic Guards
---

{NOTE: }

* Atomic guards are [compare exchange](../../../client-api/operations/compare-exchange/overview) 
  values that RavenDB sets and uses automatically to guarantee ACID properties in cluster-wide transactions.  

* Prior to the introduction of this feature (in RavenDB 5.2), users had to define 
  and use compare exchange values manually.  
  You can still disable the automatic usage of atomic guards in a session, 
  and create and use compare exchange values [manually](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation) 
  where needed.  

* Atomic guards expire when (or a short while after) the documents that use them expire.  

* In this page:
  * [Syntax and Usage](../../../client-api/operations/compare-exchange/atomic-guards#syntax-and-usage)  
     * [Disabling Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards)  
  * [Expiration](../../../client-api/operations/compare-exchange/atomic-guards#expiration)  

{NOTE/}

---

{PANEL: Syntax and Usage}

The creation of atomic guards is **enabled by default** when you open 
a [cluster-wide session](../../../client-api/session/cluster-transaction#open-cluster-wide-session).  

In the sample below, atomic guards are set and used automatically.  
{CODE:csharp atomic-guards-enabled@ClientApi/Operations/CompareExchange.cs /}

If you [examine](../../../studio/database/documents/documents-and-collections#the-documents-view) 
the compare exchange values list after running the above sample, you will find in it the atomic guard 
that was used by the session.  

![Atomic Guard](images/atomic-guard.png "Atomic Guard")

{WARNING: }
Do **not** remove atomic guards manually, as nodes may assume they are occupied and 
prevent the modification of documents they handle.  
If you accidentally removed an active atomic vector, re-create the document that uses it.  
{WARNING/}

## Disabling Atomic Guards

To **disable** the automatic usage of atomic guards in a session, set its 
`DisableAtomicDocumentWritesInClusterWideTransaction` configuration option to `true`.  

In the sample below, the session uses **no atomic guards**.  
{CODE:csharp atomic-guards-disabled@ClientApi/Operations/CompareExchange.cs /}

{WARNING: }
To **guarantee ACIDity in cluster-wide transactions** when atomic guards are disabled,  
[set and use compare exchange values in your session manually](../../../client-api/operations/compare-exchange/overview).  
{WARNING/}

{PANEL/}

{PANEL: Expiration}

* The expiration and removal of atomic guards are done automatically by RavenDB.  
  You do not need to handle the cleanup manually.  

* Since different cleanup tasks handle the removal of expired (e.g. deleted) documents 
  and the removal of expired atomic guards, it may happen that atomic guards of expirded 
  documents would remain in the compare exchange list for a short while before they are removed.  
   * No need to remove such atomic vector, the cleanup task will take care of it.  
   * You can re-create expired documents whose atomic guards haven't been removed yet.  
     RavenDB will create a new atomic guard for the document, and expire the old one.  

{PANEL/}

## Related Articles

### Client API
- [Session: How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Compare Exchange Metadata](../../../client-api/operations/compare-exchange/compare-exchange-metadata)

### Server
- [Document Expiration](../../../server/extensions/expiration)
