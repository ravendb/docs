# Ongoing Tasks Stats View
---

{NOTE: }

* The **Ongoing Tasks Stats** view monitors the following ongoing tasks:  
   * External Replication  
   * RavenDB ETL  
   * Subscription  
* Tasks' activities are graphically displayed for selected time frames.  

* In this page:  
  * [Ongoing Tasks Stats View](../../../studio/database/stats/ongoing-tasks-stats#ongoing-tasks-stats-view)  
  * [The Timeline Ruler](../../../studio/database/stats/ongoing-tasks-stats#the-timeline-ruler)  
  * [Task Rulers](../../../studio/database/stats/ongoing-tasks-stats#task-rulers)  
     * [External Replication Task Ruler Views](../../../studio/database/stats/ongoing-tasks-stats#external-replication-task-ruler-views)  
     * [RavenDB ETL Task Ruler Views](../../../studio/database/stats/ongoing-tasks-stats#ravendb-etl-task-ruler-views)  
     * [Subscription Task Ruler Views](../../../studio/database/stats/ongoing-tasks-stats#subscription-task-ruler-views)  

{NOTE/}

---

{PANEL: Ongoing Tasks Stats View}

![Figure 1: Ongoing Tasks Stats View](images/stats-view-01-ongoing-tasks-stats-view.png "Figure 1: Ongoing Tasks Stats View")

1. **Ongoing Tasks Stats**  
   Click to open the Ongoing Tasks Stats View.  

2. **Database**  
   Select the database whose ongoing tasks you want to monitor.  
   Data for this database's ongoing tasks will be collected and presented in the graph.  

{PANEL/}

{PANEL: The Timeline Ruler}

![Figure 2: The Timeline Ruler](images/stats-view-02-the-timeline-ruler.png "Figure 2: The Timeline Ruler")

* The timeline ruler allows you to select any timeline segment and explore task activities during this period.  
* The timeline ruler line depicts a summary of the activity volume for all tasks presented in the graph.  

---

![Figure 3: Clear Graph and Selection](images/stats-view-03-selection-and-clear-buttons.png "Figure 3: Clear Graph and Selection")

1. **Selection Frame**  
   Click & drag or resize this frame to select the timeline segment for which the data will be shown in the graph.  
2. **Clear Selection**  
   Click to clear the selected frame.  
3. **Clear Graph**  
   Click to clear all the collected data from the graph.  

---

![Figure 4: Tail Stream End](images/stats-view-04-monitor-tail.png "Figure 4: Tail Stream End")

* **Monitoring (tail -f)**  
  When this option is checked, the selection frame will be continuously reallocated at the end of the data stream.  

{PANEL/}

{PANEL: Task Rulers}

A ruler dedicated to each monitored task displays the task's activities during the time 
specified by the timeline ruler above.  

* Information summary is shown in a single bar.  
* Click this bar for an expanded view that details task events in multiple bars and frames.  
* Hover over bars, frames, and special symbols to popup information related to them.  
* Click and drag the bars to slide the graph.  

---

![Figure 5: Task Rulers Basic Views](images/stats-view-05-task-rulers-basic-views.png "Figure 5: Task Rulers Basic Views")

1. **Selection Frame**  
2. **External Replication Task Basic View**  
3. **RavenDB ETL Task Basic View**  
4. **Subscription Task Basic View**  

---

![Figure 6: Task Rulers Expanded Views](images/stats-view-06-task-rulers-expanded-views.png "Figure 6: Task Rulers Expanded Views")

1. **Selection Frame**  
2. **External Replication Task Expanded View**  
3. **RavenDB ETL Task Expanded View**  
4. **Subscription Task Expanded View**  

---

{INFO: }
**Active** and **Inactive** tasks are shown in different styles.  

An active task:  
![Active Task](images/stats-view-active-task.png "Active Task")

An inactive task:  
![Inactive Task](images/stats-view-inactive-task.png "Inactive Task")
{INFO/}

---

### External Replication Task Ruler Views

* **Basic View**  
  ![Figure 7: External Replication Task Basic View](images/stats-view-07-external-replication-basic-view.png "Figure 7: External Replication Task Basic View")

* **Expanded View**  
  ![Figure 8: External Replication Task Expanded View](images/stats-view-08-external-replication-expanded-view.png "Figure 8: External Replication Task Expanded View")
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

### RavenDB ETL Task Ruler Views

* **Basic View**  
  ![Figure 9: RavenDB ETL Task Basic View](images/stats-view-09-etl-basic-view.png "Figure 9: RavenDB ETL Task Basic View")

* **Expanded View**  
  ![Figure 10: RavenDB ETL Task Expanded View](images/stats-view-10-etl-expanded-view.png "Figure 10: RavenDB ETL Task Expanded View")
    * **Raven ETL**  
       * **Total Duration**  
         Overall ETL operation time, including the Extraction, 
         Transformation, and Loading phases.  
       * **Batch Complete Reason**  
         The reason the operation ended, e.g. 
         "No more items to process", 
         "Stopping the batch because maximum batch size limit was reached", and others.  
       * **Currently Allocated**  
         The amount of memory allocated by the server to handle this task.  
       * **Batch Size**  
         Size of ETL batches.  

    * **Extract Phase**  
       * **Duration**  
         The time it took to extract the documents from the database.  
       * **Extracted Documents**  
         The number of documents extracted from the database.  

    * **Transform Phase**  
       * **Duration**  
         The time it took to process the documents using the transform script.  
       * **Transformed Documents**  
         The number of documents processed by the transform script.  
       * **Documents Processing Speed**  
         Average Doc/Sec transformation speed.  
       * **Last Transformed Etag for Document**  
         Last transformed document's identifier.  

    * **Load Phase**  
       * **Duration**  
         The time it took to transfer the documents to their destination.  
       * **Successfully Loaded**  
         Verification of documents successful transfer to the destination.  
       * **Last Loaded Etag**  
         Last loaded document's identifier.  

### Subscription Task Ruler Views

* **Basic View**  
  ![Figure 11: Data Subscription Task Basic View](images/stats-view-11-data-subscription-basic-view.png "Figure 11: Data Subscription Task Basic View")

* **Expanded View**  
  ![Figure 12: Data Subscription Task Expanded View](images/stats-view-12-data-subscription-expanded-view.png "Figure 12: Data Subscription Task Expanded View")
    * **Client Connection**  
       * Connection duration  
       * Client URL  
       * Client [Strategy](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay)  
       * Number of acknowledged batches  
       * Batches size  
    * **Documents Batch**  
       * Batch duration  
       * Number of documents sent in this batch  
       * Transferred documents size  
       * Number of [included](../../../client-api/data-subscriptions/creation/examples#create-subscription-with-include-statement) 
         documents, counters, and time series  
    * **Sending Documents**  
      The time it took the server to send this batch of documents.  
    * **Waiting for ACK**  
      The time it took the client to acknowledge this batch of documents.  

* **Special Symbols**  
  Special symbols are displayed in both the basic and the expanded views.  
   * Connection Abortion  
     ![Forcibly-Aborted Connnection Explained](images/stats-view-forcibly-aborted-connection.png "Forcibly-Aborted Connnection Explained")  
   * Connection Rejection  
     ![Rejected Connnection Explained](images/stats-view-rejected-connection.png "Rejected Connnection Explained")  
     The popup explains that a client has tried to connect an occupied subscription 
     and was rejected.  

{PANEL/}


## Related Articles  

### Studio  
[Ongoing Tasks - General Info](../../../studio/database/tasks/ongoing-tasks/general-info)  
[RavenDB ETL Task](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
[External Replication Task](../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
### Client API  
[Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Consuming Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  

### Server  
[Ongoing Tasks: ETL Basics](../../../server/ongoing-tasks/etl/basics#ongoing-tasks-etl-basics)  
[Ongoing Tasks: External Replication](../../../server/ongoing-tasks/external-replication)  
