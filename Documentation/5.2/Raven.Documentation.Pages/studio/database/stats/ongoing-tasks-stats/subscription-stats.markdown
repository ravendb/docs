# Ongoing Tasks Stats: Subscription Stats
---

{NOTE: }

Data subscription tasks are ongoing operations in which the cluster sends document batches 
to a client in an orderly manner, waiting for the consumption of each batch before sending 
the next.  

Documents transmitted this way may be, for example, orders sent to an accountant client, 
making an otherwise tedious operation extremely easy to manage.  

Learn more about data subscription tasks [here](../../../../studio/database/tasks/ongoing-tasks/subscription-task).  

* In this page:  
   * [Subscription Stats Closed View](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#subscription-stats-closed-view)  
   * [Subscription Stats Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#subscription-stats-expanded-view)  
   * [Connection Event Indicators](../../../../studio/database/stats/ongoing-tasks-stats/subscription-stats#connection-event-indicators)  

{NOTE/}

---

{PANEL: Subscription Stats}

### Subscription Stats Closed View

![Subscription Stats Closed View](images/stats-view-11-data-subscription-closed-view.png "Subscription Stats Closed View")

1. **Task Type**  
   Click the arrow or the task type to toggle Closed/Expanded View.  
2. **Task Name**  
3. **Query**  
   Click to display the subscription query.  
4. **Task Bar**  
    * Hover over the bar to display a tooltip with the task's information.  
    * Click the bar for the expanded view.  
    * Click and Drag the bar to slide the graph.  
    * Zoom in & out using the mouse wheel.  

---

### Subscription Stats Expanded View

![Subscription Stats Expanded View](images/stats-view-12-data-subscription-expanded-view.png "Subscription Stats Expanded View")

* **Client Connection**  
   * **Duration**  
     Overall correspondence time.  
   * **Client URL**  
   * **Strategy**  
     The [strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay) used by the client.  
   * **Number of batches acknowledged**  
     The number of batches whose reception the client confirmed.  
   * **Batches size**  
     Size of each transferred batch.  
* **Documents Batch**  
   * **Batch duration**  
     The amount of time it took to deliver this batch.  
   * **Documents sent in batch**  
     The number of documents delivered in this batch.  
   * **Documents size**  
     Overall size of Transferred documents.  
   * **Included Documents**  
     **Included Counters**  
     **Included Time Series entries**  
     Number of [included](../../../../client-api/data-subscriptions/creation/examples#create-subscription-with-include-statement) 
     documents, counters, and time series  
* **Sending Documents**  
  The time it took the server to send documents in this batch.  
* **Waiting for ACK**  
  The time it took the client to acknowledge this batch.  

---

### Connection Event Indicators

Connection Event Indicators signify events related to the initiation, state, 
or termination of a client connection.  

* Indicators are shown in both the [Closed](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-closed-view) 
  and the [Expanded](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-expanded-view) 
  task views.  
* Hovering over a connection event indicator will popup information related to it, if available.  

---

**Connection Initiated**  
![Connnection Initiated](images/stats-view-connection-initiated.png "Connnection Initiated")  

---

**Connection Aborted**  
![Forcibly-Aborted Connnection](images/stats-view-forcibly-aborted-connection.png "Forcibly-Aborted Connnection")  
The client has forcibly aborted the connection.  

---

**Connection Rejected**  
![Rejected Connnection](images/stats-view-rejected-connection.png "Rejected Connnection Explained")  
Subscription tasks handle a single client/connection at a time.  
A client that uses the *OpenIfFree* [strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay) 
has attempted to open a connection to the subscription task but was rejected 
because the task is already occupied by another client.  

---

**Pending Connection**  
![Pending Connnection](images/stats-view-pending-connection.png "Pending Connnection")  
  
1. Client **A** connects to the subscription task.  
2. Client **B**, using the *WaitForFree* [strategy](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay), 
   attempts to connect the task.  
   Discovering that the task is occupied, Client B is now pending, 
   waiting for Client A to disconnect.  
   The dotted yellow line shows the pending period for Client B.  
3. Client A disconnects from subscription task.  
4. Client **B** starts a new connection to the subscription task.  

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
