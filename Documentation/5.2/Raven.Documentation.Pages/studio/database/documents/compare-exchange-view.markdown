# Documents: Compare Exchange View
---

{NOTE: }

* Compare exchange key/value pairs can be used to guarantee the atomicity and overall ACID properties of [cluster-wide transactions](../../../server/clustering/cluster-transactions).  

* RavenDB guarantees cluster-wide ACID safe transactions on each document by automatically including [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards) 
  (compare exchange key/value pairs) in each document.  
  * Atomic Guards are compare exchange values that RavenDB creates and maintains automatically.
    {WARNING: Warning}
    Do not remove or edit atomic guards manually as it will likely disable ACID guarantees for transactions.
    {WARNING/}

* You can also create new compare exchange key/value pairs to use in other situations.  
  See examples in the [overview API article](../../../client-api/operations/compare-exchange/overview#example-i---email-address-reservation).

{NOTE/}

---

{PANEL: The Compare Exchange View}

![Compare Exchange View](images/compare-exchange-view.png "Compare Exchange View")

1. **Documents Tab**  
   Select to see document related options.  
2. **Compare Exchange**  
   Select to see the Compare Exchange Studio view.  
3. **Add new item**  
   Click to add a new compare exchange key/value pair.  
4. **Compare exchange key/value properties**  
   Click the edit button or key name to edit this pair.  
    ![Compare Exchange Single Pair](images/compare-exchange-single-pair.png "Compare Exchange Single Pair")
    1. **Delete**  
       Click to delete this compare exchange key/value pair.  
       {WARNING: Warning}
       Deleting a compare exchange pair will remove ACID guarantees for transactions that use it.
       {WARNING/}
    2. **Key**  
       A unique identifier that is reserved across the cluster.  
         ![Atomic Guard](images/compare-exchange-atomic-guard.png "Atomic Guard")
          * Keys starting with "rvn-atomic" are [Atomic Guards](../../../client-api/operations/compare-exchange/atomic-guards). They are created, edited and removed automatically by RavenDB.  
            We strongly recommend not editing or deleting them manually because unless you're an expert, doing so will likely disable ACID guarantees for transactions.
    3. **Value**  
       Edit to change the value associated with the key.  
       Before a cluster allows a transaction, it needs to see that the value matches the expected value.  
    4. **Metadata**  
       Click to add metadata.  
       The metadata is associated with the key.  
    5. **Raft Index**  
       The raft index is updated automatically each time the value is changed, indicating the value's current version to allow concurrency control.  


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
