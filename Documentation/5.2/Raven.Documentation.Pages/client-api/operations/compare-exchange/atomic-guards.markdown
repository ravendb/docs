# Compare Exchange: Atomic Guards
---

{NOTE: }

* Atomic guards are [compare exchange](../../../client-api/operations/compare-exchange/overview) 
  values that RavenDB sets automatically to enable 
  [atomicity](../../../server/clustering/rachis/what-is-rachis#what-is-raft-?) 
  in cluster-wide transactions.  

* This feature was introduced in RavenDB 5.2. Older versions require users to 
  define compare exchange values manually.  

* You can disable the automatic creation of atomic guards if you wish, and set 
  compare exchange values yourself where needed.  

* Atomic guards expire when the transaction that required them ends.  

* In this page:
  * [Syntax](../../../)
  * [Expiration](../../../)
  * [Examples](../../../)

{NOTE/}

---

{PANEL: Syntax and Usage}

* The creation of atomic guards is **enabled by default** when you open 
  a [cluster-wide session](../../../client-api/session/cluster-transaction#open-cluster-wide-session).  

    In the sample below, atomic guards **are** created.  
    {CODE:csharp atomc-guard_enabled@ClientApi/Operations/CompareExchange.cs /}

* To **disable** the creation of atomic guards, set the 
  `DisableAtomicDocumentWritesInClusterWideTransaction` configuration option to `true`.  
  RavenDB will only validate compare exchange values created explicitly by users, 
  if atomic guards are disabled.  

    In the sample below, atomic guards are **not** created automatically.  
    To enable atomicity in cluster-wide transactions, you'll have to create them yourself.  
    {CODE:csharp atomc-guard_disabled@ClientApi/Operations/CompareExchange.cs /}

{PANEL/}

{PANEL: Expiration}

{PANEL/}

## Related Articles

### Client API
- [Session: How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Compare Exchange Metadata](../../../client-api/operations/compare-exchange/compare-exchange-metadata)

### Server
- [Document Expiration](../../../server/extensions/expiration)
