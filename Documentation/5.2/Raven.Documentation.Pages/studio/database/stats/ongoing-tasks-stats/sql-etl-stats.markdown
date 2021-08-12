# Ongoing Tasks Stats: SQL ETL Stats
---

{NOTE: }

* **SQL ETL** is a process that reads data from a RavenDB database, transforms it, 
  and stores it in an SQL database.
* Learn more about the SQL ETL task [here](../../../../server/ongoing-tasks/etl/sql).  

* In this page:  
    * [SQL ETL Stats Closed View](../../../../studio/database/stats/ongoing-tasks-stats/sql-etl-stats#sql-etl-stats-closed-view)  
    * [SQL ETL Stats Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/sql-etl-stats#sql-etl-stats-expanded-view)  

{NOTE/}

---

{PANEL: SQL ETL Stats}

### SQL ETL Stats Closed View

![SQL ETL Stats Closed View](images/stats-view-13-sql-etl-closed-view.png "SQL ETL Stats Closed View")

1. **Task Type**  
   Click the arrow or the task type to toggle Closed/Expanded View.  
2. **Task Name**  
3. **Transform Script**  
   Click to display the ETL transform script.  
4. **Task Bar**  
    * Hover over the bar to display a tooltip with the task's information.  
    * Click the bar for the expanded view.  
    * Click and Drag the bar to slide the graph.  
    * Zoom in & out using the mouse wheel.  

---

### SQL ETL Stats Expanded View
![SQL ETL Stats Expanded View](images/stats-view-14-sql-etl-expanded-view.png "SQL ETL Stats Expanded View")

* **SQL ETL**  
   * **Total Duration**  
     Overall ETL operation time, including the Extraction, 
     Transformation, and Loading phases.  
   * **Batch Complete Reason**  
     The reason the operation ended, e.g. -  
     "No items to process",  
     "Stopping the batch because maximum batch size limit was reached",  
     and others.  
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
     The time it took to transfer the documents to their destination SQL table.  
   * **Successfully Loaded**  
     Documents transfer verification.  
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
[SQL ETL Task](../../../../server/ongoing-tasks/etl/sql)  
