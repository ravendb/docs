# Elasticsearch ETL Task

---

{NOTE: }

* An **Elasticsearch ETL task** is an ongoing process that -  
    * **Extracts** chosen documents from the database,  
    * **Transforms** the documents using a user-defined script,  
    * and **Loads** the documents to a destination [Elasticsearch Index](https://www.elastic.co/blog/what-is-an-elasticsearch-index).  
* An Elasticsearch ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  
* The task sends the Elasticsearch index -  
   * a [_refresh](https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-refresh.html) 
     comnmand to ensure that the index is in sync with its current documents inventory.  
   * an optional [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) 
     command, to delete existing document versions from the target index before appending documents to it.  
     (To prevent the task from sending `_delete_by_query` commands, enable [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
     in the task settings.)  
   * a [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) 
     command to append the documents to the index.  
* This page explains how to create an Elasticsearch ETL task using Studio.  
  Learn more about this process [here](../../../../server/ongoing-tasks/etl/elasticsearch).  

* In this page:  
  * [Navigate to the Elasticsearch ETL Task View](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#navigate-to-the-elasticsearch-etl-task-view)  
  * [Define the Elasticsearch ETL Task](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#define-the-elasticsearch-etl-task)  
     * [ETL Task Properties](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#etl-task-properties)  
     * [Elasticsearch Indexes](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes)  
     * [Transformation Script](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#transformation-script)  
     * [Edit Transformation Script](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#edit-transformation-script)  
     * [Test Transformation Script](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#test-transformation-script)  
     * [Test Results](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#test-results)  
{NOTE/}

---

{PANEL: Navigate to the Elasticsearch ETL Task View}

To begin creating your Elasticsearch ETL task:  

!["Ongoing tasks view"](images/ongoing-tasks-view.png "Ongoing tasks view")

!["Task selection view"](images/elasticsearch-etl-task-selection-view.png "Task selection view")

{PANEL/}

{PANEL: Define the Elasticsearch ETL Task}

---

### ETL Task Properties

!["Define Elasticsearch ETL task"](images/elasticsearch-etl-define-task.png "Define Elasticsearch ETL task")

1. **Task Name** (Optional)  
   * Choose a name for your task  
   * If no name is provided, the cluster will create a name based on the defined connection string (e,g, *ElasticSearch ETL to ElasticConStr*).  

2. **Task State**  
   The task state can be -  
   Enabled - The task runs in the background, transforming and sending documents as defined in this view.  
   Disabled - The task does not transform and send documents.  

3. **Responsible Node** (Optional)  
  * Select a preferred mentor node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

4. **Connection String**  
   * The connection string defines the destination index URLs.  
   * If you already created connection strings, you can select one from the list.  
   * You can create a new connection string:  
     !["Create Connection String"](images/elasticsearch-connection-string.png "Create Connection String")  
     a. **Name** - The connection string name  
     b. **Nodes URLs** - The Elasticsearch destination/s URL/s  
     c. **Authentication** - The authentication method used by the Elasticsearch destination node/s.  
   * Available authentication methods:  
     !["Authentication Methods"](images/elasticsearch-connection-string-authentication.png "Authentication Methods")  

---

### Elasticsearch Indexes

Add to the Elasticsearch Indexes list all the indexes that your transformation script loads data to.  

!["Add New Index"](images/elasticsearch-indexes-list.png "Add New Index")

1. **List of Elasticsearch Indexes**  

2. **Add Index** (Optional)  
  * Click to add an index to the list.  

3. **Index Name**  
  * The Elasticsearch index to which the transformation script loads an object.  
    E.g., if the transformation script uses `loadToOrders(orderData)` to pass an object 
    to the `orders` index, provide "orders" here as an index name.  
  * The index name must be all **lower case**.  

4. **ID Property**  
  * The ID of the object you want to upload to the Elasticsearch index.  
    E.g., if the transformation script uses `Id: id(this)` to define an object, 
    you can provide "Id" here to pass this object to the index.  

5. **Insert Only**  
   * By default, the transformation script sends the index `_delete_by_query` commands to delete existing documents before replacing them with new ones.  
   * Enabling `Insert Only` prevents the task from sending `_delete_by_query` commands.  
   * Be aware that enabling this option would create a new copy of a document on the index each time the document is modified in RavenDB.  

6. **Add** this index to the list.  

7. **Cancel** the operation without adding the index to the list.  

8. **Existing Index**  
     a. **Index Name**  
     b. **ID Property**  
     c. **Edit Index** - Click to edit index properties.  
     d. **Remove Index** - Click to remove this index from the list.  

---

### Transformation Script

!["List of transformation Scripts"](images/elasticsearch-transformation-scripts-list.png "List of transformation Scripts")

1. **Transform Scripts**  
  List of existing transformation scripts.  

2. **Add Transformation Script**  
   Click to add a new transformation script to the list.  

3. **Existing Script**  
     a. **Edit** - Click to edit the script.  
     b. **Remove** - Click to remove the script from the list.  

---

### Edit Transformation Script

!["Edit Transformation Script"](images/elasticsearch-edit-transformation-script.png "Edit Transformation Script")

1. **Script Name** (Optional)  
   The script is named automatically.  
   Optionally, give it a name of your choice.  

2. **Script**  
   *  Add or edit the transformation script.  
   *  Add all the indexes your script uses to the [indexes list](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes).  

3. **Syntax**
   Click for a transformation script Syntax Sample.  

4. **Collection**  
   Type or select the names of the collections your script is using.  

5. **Apply script to documents from beginning of time (Reset)**  
    * When this option is **enabled**, the script will be executed over **all existing documents in the 
      specified collections** the first time the task runs.  
    * When this option is **disabled**, the script will be executed **only over new and modified documents**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **enabled**,  
      RavenDB documents will be appended to the index **without deleting documents from the index first**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **disabled**,
      documents will be **deleted from the index first**, and then appended to it from RavenDB.  

6. **Update**  
   Click to update the task with your changes.  

7. **Cancel**  
   Click to cancel your changes.  

8. **Test Script**  
    * Click to **test** the transformation script before actually using it to transfer documents (see below).  

---

### Test Transformation Script

!["Transformation script"](images/elasticsearch-test-script-view.png "Transformation script")  

1. **Document ID**  
   Type or select the ID of the document you want to test the script with.  
2. **Test**  
   Click to run the test.  
   The test will display the commands that would be sent to the Elasticsearch index, **without** actually sending them to the index.  
3. **Close Test Area**  
   Close this view.  

---

### Test Results

The test results view displays a **preview** of the tested document, and the **commands** the task would send the Elasticsearch index.  

---

* **Document Preview Tab**  
  !["Document Preview Tab"](images/elasticsearch-test-results-preview-tab.png "Document Preview Tab")  

---

* **Test Results Tab**  
!["Test Results Tab"](images/elasticsearch-test-results.png "Test Results Tab")  

    1. **Test Results Tab**  
       Displays the commands that the task would send the Elsticsearch index.  
    2. **Elasticsearch Index**  
       The index the commands and data are sent to.  
    3. [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) Segment  
       With a list of IDs by which the index would locate and remove existing documents.  
       Deleting existing document versions is **optional**, enable [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
       to prevent the task from sending `_delete_by_query` commands.  
    4. [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) Segment  
       With a list of document objects, each with data extracted from RavenDB and an ID the index can store it by.  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  

### Client API

- [Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
