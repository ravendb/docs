# Ongoing Tasks Stats: Subscription Stats
---

{NOTE: }

* In this page:  
     * [Subscription Stats](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#subscription-stats)  
        * [Closed View](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#closed-view)  
        * [Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#expanded-view)  
        * [Subscription Stats Connection Indicators](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#subscription-stats-connection-indicators)  

{NOTE/}

---

{PANEL: Subscription Stats}

### Closed View

![Subscription Stats Closed View](images/stats-view-11-data-subscription-closed-view.png "Subscription Stats Closed View")

1. **Task Type**  
   Click arrow or task type to toggle Closed/Expanded View.  
2. **Task Name**  
3. **Query**  
   Click to display the subscription query.  
4. **Task Bar**  
    * Hover over the bar to display basic information tooltip.  
    * Click the bar for the expanded task view.  
    * Click and Drag the bar to slide the graph.  

---

### Expanded View

![Subscription Stats Expanded View](images/stats-view-12-data-subscription-expanded-view.png "Subscription Stats Expanded View")

* **Client Connection**  
   * Connection duration  
   * Client URL  
   * Client [Strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay)  
   * Number of acknowledged batches  
   * Batches size  
* **Documents Batch**  
   * Batch duration  
   * Number of documents sent in this batch  
   * Transferred documents size  
   * Number of [included](../../../../client-api/data-subscriptions/creation/examples#create-subscription-with-include-statement) 
     documents, counters, and time series  
* **Sending Documents**  
  The time it took the server to send this batch of documents.  
* **Waiting for ACK**  
  The time it took the client to acknowledge this batch of documents.  

---

### Subscription Stats Connection Indicators

* Connection Initiated  
  ![Connnection Initiated](images/stats-view-connection-initiated.png "Connnection Initiated")  

* Connection Aborted  
  ![Forcibly-Aborted Connnection](images/stats-view-forcibly-aborted-connection.png "Forcibly-Aborted Connnection")  
  The client has forcibly aborted the connection.  

* Connection Rejected  
  ![Rejected Connnection](images/stats-view-rejected-connection.png "Rejected Connnection Explained")  
  A client that uses the `OpenIfFree` [strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay) 
  has attempted to open a connection to the subscription task and was rejected 
  because the task is already occupied by another client.  

* Pending Connection  
  ![Pending Connnection](images/stats-view-pending-connection.png "Pending Connnection")  
  
  1. Client **A** connects the subscription task.  
  2. Client **B**, that uses the `WaitForFree` [strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay), 
     attempts to connects the task and is transited to **Pending** state.  
  3. Client **A**, having received all the documents is needed, gracefully disconnects 
     the task.  
  4. Client **B** connects the task.  

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
