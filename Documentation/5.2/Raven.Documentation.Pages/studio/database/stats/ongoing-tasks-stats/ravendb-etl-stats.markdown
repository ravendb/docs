# Ongoing Tasks Stats: RavenDB ETL Stats
---

{NOTE: }

* In this page:  
   * [RavenDB ETL Stats](../../../../studio/database/stats/ongoing-tasks-stats/ravendb-etl-stats#ravendb-etl-stats)  
      * [Closed View](../../../../studio/database/stats/ongoing-tasks-stats/ravendb-etl-stats#closed-view)  
      * [Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/ravendb-etl-stats#expanded-view)  

{NOTE/}

---

{PANEL: RavenDB ETL Stats}

### Closed View

![RavenDB ETL Stats Closed View](images/stats-view-09-etl-closed-view.png "RavenDB ETL Stats Closed View")

1. **Task Type**  
   Click arrow or task type to toggle Closed/Expanded View.  
2. **Task Name**  
3. **Transform Script**  
   Click to display the ETL transform script.  
4. **Task Bar**  
    * Hover over the bar to display basic information tooltip.  
    * Click the bar for the expanded task view.  
    * Click and Drag the bar to slide the graph.  

---

### Expanded View
![RavenDB ETL Stats Expanded View](images/stats-view-10-etl-expanded-view.png "RavenDB ETL Stats Expanded View")
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
