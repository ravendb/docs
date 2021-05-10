# Ongoing Tasks Stats: External Replication Stats
---

{NOTE: }

* The External Replication Task replicates your database data to another RavenDB database.  
* Learn more about the External Replication Task [here](../../../../studio/database/tasks/ongoing-tasks/external-replication-task).  
* The data shown in the graph will be either 'Outgoing' or 'Incoming', depending on whether the source or the target database is viewed.  

* In this page:  
   * [External Replication Stats Closed View](../../../../studio/database/stats/ongoing-tasks-stats/external-replication-stats#external-replication-stats-closed-view)  
   * [External Replication Stats Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/external-replication-stats#external-replication-stats-expanded-view)  

{NOTE/}

---

{PANEL: External Replication Stats}

### External Replication Stats Closed View

![External Replication Stats Closed View](images/stats-view-07-external-replication-closed-view.png "External Replication Stats Closed View")

1. **Task Type**  
   The task type will be either 'Outgoing' or 'Incoming', depending on whether the viewed database is the source or the target database.  
   Click the arrow or the task type to toggle Closed/Expanded View.  

2. **Replication URL**  
    * On the Source database: The server URL & database name to which documents are replicated.  
    * On the Target database: The server URL & database name of the source database.  
3. **Task Bar**  
    * Hover over the bar to display a tooltip with the task's information.  
    * Click the bar for the expanded view.  
    * Click and Drag the bar to slide the graph.  
    * Zoom in & out using the mouse wheel.  

---

### External Replication Stats Expanded View

#### Outgoing Replication  

![External Replication Stats Expanded View - Outgoing](images/stats-view-08_1-external-replication-expanded-view-outgoing.png "External Replication Stats Expanded View - Outgoing")

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

---

#### Incoming Replication

![External Replication Stats Expanded View - Incoming](images/stats-view-08_2-external-replication-expanded-view-incoming.png "External Replication Stats Expanded View - Incoming")

* **Incoming External Replication**  
     * **Total Duration**  
       Overall batch replication time.  
     * **Received Last Etag**  
       Last replicated item identifier.  
     * **Network Input Count**  
       The number of transferred items.  
     * **Documents read count**  
       The number of transferred documents.  
     * **Attachments read count**  
       The number of transferred attachments.  

* **Network/Read Duration**  
  The time it took the current batch to be transmitted over the network

* **Network/DocumentRead Duration**  
  The time it took the documents in this batch to be transmitted over the network

* **Storage/Write**  
  The time it took the server to write replicated date to storage.  

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
