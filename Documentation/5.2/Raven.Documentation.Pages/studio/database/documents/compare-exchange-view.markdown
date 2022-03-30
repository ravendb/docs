# Documents: Compare Exchange View
---

{NOTE: }

* [Compare-Exchange](../../../client-api/operations/compare-exchange/overview) key/value pairs are used to 
  coordinate work between threads, clients, nodes, or sessions that are 
  trying to access a shared resource (such as a document) at the same time.
  
* In a cluster-wide setting, they are shared by and can be used by every node in the cluster to maintain ACIDity 
  by ensuring that two entities cannot write on the same document at the same time.  

* Compare exchange items are created and managed by either of the following:
  * RavenDB [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards)  
    To guarantee ACIDity across the cluster, 
    as of RavenDB 5.2, we automatically create and maintain Atomic Guard CmpXchg items in cluster-wide sessions.  
  * [API Operations](../../../client-api/operations/compare-exchange/overview)
  * [Session - Cluster Transaction](../../../client-api/session/cluster-transaction)
  * Using the [RavenDB Studio](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view)

* This singular key can also be used to reserve a resource in various other situations  
  (see [API Compare-exchange examples](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation)).  

In this page:

* [The Compare Exchange View](../../../studio/database/documents/compare-exchange-view#the-compare-exchange-view)

{NOTE/}

---

{PANEL: The Compare Exchange View}

![Compare Exchange View](images/compare-exchange-view.png "Compare Exchange View")

1. **Documents Tab**  
   Select to see document-related options and the list of documents in this database .  
2. **Compare Exchange**  
   Select to see the Compare Exchange view.  
3. **Add new item**  
   Click to add a new compare exchange key/value pair.  
4. **Compare exchange key/value properties**  
   Click the edit button or key name to edit this item.  
    ![Compare Exchange Single Pair](images/compare-exchange-single-pair.png "Compare Exchange Single Pair")
    1. **Key**  
       A unique identifier that is reserved across the cluster.  
       Enter any string of your choice.  
        {INFO: Atomic Guards}
        If keys start with "rvn-atomic", they are [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards).  
        They are created and maintained automatically to guarantee ACID cluster-wide transactions.  
        **Do not remove or edit these** as this will disable ACID guarantees.  
        
        ![Atomic Guard](images/compare-exchange-atomic-guard.png "Atomic Guard")

        {INFO/}
    2. **Value**  
       Enter a value that will be associated with the key.  
       Values can be numbers, stings, arrays, or objects. Any value that can be represented as JSON is valid.
    3. **Metadata**  
       Click to add metadata.  
       The metadata is associated with the key, similar to document's metadata.  
    4. **Raft Index**  
       The raft index is updated automatically each time the value is changed, indicating the value's current version to allow concurrency control.  
    5. **Delete**  
       Click to delete this compare exchange item.  
       {WARNING: Warning}
       Deleting a compare exchange item will remove ACID guarantees for transactions if the pair was set up to protect ACIDity.  
       Only remove or edit these if you _truly_ know what you're doing.  
       {WARNING/}
    6. **Save**  
       You can only save if the actual current Raft Index version matches the expected Raft Index.  

{PANEL/}


## Related Articles

### Client API

- [Compare Exchange Overview](../../../client-api/operations/compare-exchange/overview)  
- [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards)  

### Server

- [Cluster-Wide Transactions](../../../server/clustering/cluster-transactions)  

<br/>

## Code Walkthrough

- [Create CmpXchg Item](https://demo.ravendb.net/demos/csharp/compare-exchange/create-compare-exchange)  
- [Index CmpXchg Values](https://demo.ravendb.net/demos/csharp/compare-exchange/index-compare-exchange)  
