# Ongoing Tasks Stats: External Replication Stats
---

{NOTE: }

* In this page:  
   * [External Replication Stats](../../../../studio/database/stats/ongoing-tasks-stats/external-replication-stats#external-replication-stats)  
      * [Closed View](../../../../studio/database/stats/ongoing-tasks-stats/external-replication-stats#closed-view)  
      * [Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/external-replication-stats#expanded-view)  

{NOTE/}

---

{PANEL: External Replication Stats}

### Closed View

![External Replication Stats Closed View](images/stats-view-07-external-replication-closed-view.png "External Replication Stats Closed View")

1. **Task Type**  
   Click arrow or task type to toggle Closed/Expanded View.  
2. **Replication Target URL**  
3. **Task Bar**  
    * Hover over the bar to display basic information tooltip.  
    * Click the bar for the expanded task view.  
    * Click and Drag the bar to slide the graph.  

---

### Expanded View

![External Replication Stats Expanded View](images/stats-view-08-external-replication-expanded-view.png "External Replication Stats Expanded View")

* **Outgoing External Replication**  
     * **Total Duration**  
       Overall replication time.  
     * **Sent Last Etag**  
       Last replicated item identifier.  
     * **Storage Input Count**  
       The number of items read from storage.  
     * **Documents output count**  
       The number of replicated documents.  
     * **Attachments read count**  
       The number of attachments read from storage.  

* **Storage/Read Duration**  
  The time it took the server to read all replicated data from storage.  

* **Network/Write Duration**  
  The time it took the server to transmit all replicated data.  

* **Storage/DocumentRead Duration**  
  The time it took the server to read replicated documents from storage.  

* **Storage/TimeSeriesRead Duration**  
  The time it took the server to read replicated time series from storage.  

{PANEL/}


## Related Articles  

### Studio  
[Ongoing Tasks - General Info](../../../../studio/database/tasks/ongoing-tasks/general-info)  
[RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
[External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
### Client API  
[Data Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Consuming Data Subscription](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  

### Server  
[Ongoing Tasks: ETL Basics](../../../../server/ongoing-tasks/etl/basics#ongoing-tasks-etl-basics)  
[Ongoing Tasks: External Replication](../../../../server/ongoing-tasks/external-replication)  
