# Documents: Compare Exchange View
---

{NOTE: }

* Compare exchange key/value pairs guarantee the atomicity and overall ACID properties of [cluster-wide transactions](../../../server/clustering/cluster-transactions).  

* Their scope is cluster-wide.  

* RavenDB guarantees cluster-wide ACID safe transactions on each document by automatically including [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  (compare exchange key/value pairs) in each document.  
  * "Atomic Guards" prevent transactions on the same document at the same time.  
  * To maintain ACIDity in cluster-wide transactions, if the automatic Atomic Guards are [manually disabled](../../../client-api/operations/compare-exchange/atomic-guards#disabling-atomic-guards),
    you must explicitly set and use the required [compare exchange key/value pairs via code](../../../client-api/operations/compare-exchange/overview).

* You can also create new compare exchange key/value pairs to use in other situations.  
  See examples in the [overview API article](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation).

{NOTE/}

---

{PANEL: The Compare Exchange View}

![Compare Exchange View](images/compare-exchange-view.png "Compare Exchange View")

1. **Documents Tab**  
   Select to see document related options.  
2. **Compare Exchange**  
   Select to see Compare Exchange Studio view.  
3. **Add new item**  
   Click to add new compare/exchange value pair.  
4. **Single document**  
   Select to edit a single document.  
    ![Compare Exchange Single Document](images/compare-exchange-single-document.png "Compare Exchange Single Document")
    1. **Delete**  
       Select to delete this compare exchange key/value pair.  
       Deleting will remove ACID guarantees for transactions unless you properly set a new key/value pair.
    2. **Key**  
       A unique identifier that is reserved across the cluster.  
    3. **Value**  
       Change a value associated with the key.  
       Before a cluster allows a transaction, it needs to see that the value matches the expected value.  
    4. **Metadata**  
       Click to add metadata.  
       The metadata is associated with the key.  
    5. **Raft Index**  
       The raft index indicates the value's version and is used for concurrency control.  


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
