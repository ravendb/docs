# Ongoing Tasks Stats: OLAP ETL Stats
---

{NOTE: }

* **OLAP ETL** is a process that reads data from a RavenDB database, 
  transforms it, and stores it in a parquet format, ready for OnLine 
  Analytical Processing (OLAP).  
* Learn more about the OLAP ETL task [here](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task).  

* In this page:  
    * [OLAP ETL Stats Closed View](../../../../studio/database/stats/ongoing-tasks-stats/olap-etl-stats#olap-etl-stats-closed-view)  
    * [OLAP ETL Stats Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/olap-etl-stats#olap-etl-stats-expanded-view)  

{NOTE/}

---

{PANEL: OLAP ETL Stats}

### OLAP ETL Stats Closed View

![OLAP ETL Stats Closed View](images/stats-view-15-olap-etl-closed-view.png "OLAP ETL Stats Closed View")

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

### OLAP ETL Stats Expanded View

![OLAP ETL Stats Expanded View (1)](images/stats-view-16-olap-etl-extracted-view_1.png "OLAP ETL Stats Expanded View (1)")

* **OLAP ETL**  
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
     The time it took to transfer the documents to their destination.  
   * **Successfully Loaded**  
     Documents transfer verification.  
   * **Last Loaded Etag**  
     Last loaded document's identifier.  

![OLAP ETL Stats Expanded View (2)](images/stats-view-16-olap-etl-extracted-view_2.png "OLAP ETL Stats Expanded View (2)")

* **Load/Local**  
   * **Duration**  
     The time it took to store all documents on the **local storage**.  
       {NOTE: }
       The documents are stored locally before they are transferred to a remote destination.
       {NOTE/}

* **Load/Local/X**  
   * **Duration**  
     The time it took to store document X on the local storage.  

* **Load/Upload**  
   * **Duration**  
     The time it took to upload all documents to the **remote destination**.  
       {NOTE: }
       The Load/Upload bars will not be displayed if the destination is set to local storage only.  
       {NOTE/}

* **Load/Upload/X**  
   * **Duration**  
     The time it took to upload document X to the remote destination.  

   * **Parquet File Name** on the remote destination, constructed of -  
      * The upload date and time  
      * The document database name  
      * The OLAP ETL task name  
      * The ETL script name  
   * **File Upload Properties**, including -  
      * Destination Type  
        Can be Amazon S3, Amazon Glacier, Microdoft Azure, Google Cloud Platform, or FTP.  
      * Upload State  
      * Upload Type  
      * File Size  
      * Upload Speed  
       


{PANEL/}


## Related Articles  

### Studio  
[Ongoing Tasks - General Info](../../../../studio/database/tasks/ongoing-tasks/general-info)  
[RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
[External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
[OLAP ETL Task](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task)  

### Client API  
[Data Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Consuming Data Subscription](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  

### Server  
[Ongoing Tasks: ETL Basics](../../../../server/ongoing-tasks/etl/basics#ongoing-tasks-etl-basics)  
[Ongoing Tasks: OLAP ETL Task](../../../../server/ongoing-tasks/etl/olap)  
[Ongoing Tasks: External Replication](../../../../server/ongoing-tasks/external-replication)  
