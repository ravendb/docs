# Ongoing Tasks Stats: OLAP ETL Stats
---

{NOTE: }

* **OLAP ETL** is a process that reads data from a RavenDB database, 
  transforms it, and stores it locally or remotely in a format that 
  OLAP (OnLine Analytical Processing) can be performed upon.  
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
![OLAP ETL Stats Expanded View](images/stats-view-16-olap-etl-extracted-view.png "OLAP ETL Stats Expanded View")

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

* **Load/Local**  
   * **Duration**  
     The time it took to store **all documents** on the local storage.  
       {NOTE: }
       The documents are stored locally before they are transferred to a remote destination.
       {NOTE/}

* **Load/Upload**  
   * **Duration**  
     The time it took to upload **all documents** to the remote destination.  
       {NOTE: }
       If the destination is the local storage, the **Load/Upload** bar will not be displayed.
       {NOTE/}

* **Load/Local/X**  
   * **Duration**  
     The time it took to store **each document** on the local storage.  
       {NOTE: }
       The document is stored locally before it is transferred to the remote destination.
       {NOTE/}  

* **Load/Upload/X**  
   * **Duration**  
     The time it took to upload **each document** to the remote destination.  
       {NOTE: }
       If the destination is the local storage, the **Load/Upload/X** bar will not be displayed.
       {NOTE/}

   * **File Name** on the remote destination, constructed of -  
      * The transfer Date and Time  
      * The document Database Name  
      * The OLAP ETL Task Name  
      * The ETL Script Name  
   * **Upload Properties**, including -  
      * Destination Type  
        Can be Amazon S3, Amazon Glacier, Microdoft Azure, Google Cloud Platform, or FTP.  
        {NOTE: }
        If the destination is the local storage, the **Upload Properties** will not be displayed.
        {NOTE/}
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
