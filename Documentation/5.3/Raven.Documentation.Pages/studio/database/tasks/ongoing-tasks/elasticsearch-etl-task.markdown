# Elasticsearch ETL Task

---

{NOTE: }

* An **Elasticsearch ETL task** is an ongoing process that -  
    * **Extracts** chosen documents from the database,  
    * **Transforms** the documents using a user-defined script,  
    * and **Loads** the documents to an Elasticsearch destination.  
* An Elasticsearch ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  
* This page explains how to create an Elasticsearch ETL task using Studio.  
  Learn more about this process [here](../../../../server/ongoing-tasks/etl/elasticsearch).  

* In this page:  
  * [Navigate to the Elasticsearch ETL Task View](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#navigate-to-the-elasticsearch-etl-task-view)  
  * [Define the Elasticsearch ETL Task](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#define-the-elasticsearch-etl-task)  
  * [Elasticsearch Indexes](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes)  
  * [Transformation Script](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#transformation-script)  
     * [Add Transformation Script](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#add-transformation-script)  
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
   * The connection string defines the destination Elasticsearch URLs.  
   * If you already created connection strings, you can select one from the list.  
   * You can create a new connection string:  
     !["Create Connection String"](images/elasticsearch-connection-string.png "Create Connection String")  
     a. **Name** - The connection string name  
     b. **Nodes URLs** - The Elasticsearch destination/s URL/s  
     c. **Authentication** - The authentication method used by the Elasticsearch destination node/s.  
   * Available authentication methods:  
     !["Authentication Methods"](images/elasticsearch-connection-string-authentication.png "Authentication Methods")  

{PANEL/}

{PANEL: Elasticsearch Indexes}

Elasticsearch uses [Indexes](https://www.elastic.co/blog/what-is-an-elasticsearch-index) to store, access, and delete documents.  
Use the ETL task's *Elasticsearch Indexes** settings to choose the indexes the would use.  

!["Define Elasticsearch Index"](images/elasticsearch-indexes-define-index.png "Define Elasticsearch Index")

1. **Add Index** (Optional)  
  * Click to add an Elasticsearch index to the list.  

2. **Index Name**  
   Provide an Elasticsearch Index name, as defined by the transformation script 
   [loadTo\\<Target\\>(obj)](../../../../server/ongoing-tasks/etl/basics#transform) command (where `Target` is the index name 
   and `obj` is the object to be passed to Elasticsearch).  
    * **Elasticsearch** is **case-sensitive**, requiring you to provide an **all lower case** index name (e.g. `orders`).  
    * The transformation script is **not** case sensitive, allowing you to use either `loadToOrders` or `loadToorders` as target.  
    * E.g., a transformation script's `loadToOrders(orderData)` command requires you to define an Elasticsearch `orders` Index.  

3. **Document ID Property Name**  
   Provide the name of a property passed by the transformation script to Elasticsearch, as an ID.  
   Elasticsearch will store your documents by this ID, and you will be able to delete and modify them by it.  
    * E.g., if one of the properties of the object passed by your a transformation script to Elasticsearch is "DocID", 
      you can use DocID as the index's ID Property.  

4. **Insert Only**  
   By default, the ETL task appends a new document only after deleting its existing version using `_delete_by_query`.  
   Enabling `Insert Only` prevents the task from sending `_delete_by_query` messages, allowing you to append 
   documents without removing their existing version first.  
   {WARNING: }
   Enabling **Insert Only** would accumulate new document versions on Elasticsearch without ever removing them.  
   {WARNING/}

5. **Confirm**  
   Click to add this index to the list.  

6. **Cancel**  
   Click to cancel the operation without adding the index to the list.  

---

!["Indexes List"](images/elasticsearch-indexes-list.png "Indexes List")

1. **Defined Index**  
   An Elasticsearch index that has already been added.  
2. **Index**  
   Elasticsearch index name.  
3. **Document ID Property**  
   The RavenDB document property that is used as an Elasticsearch ID.  
4. **Edit Index**  
   Click to edit index properties.  
5. **Remove Index**  
   Click to remove this index from the list.  

{PANEL/}

{PANEL: Transformation Script}

* A transformation script sends Elasticsearch -  
   * a [_refresh](https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-refresh.html) 
     comnmand, to ensure that the index Elasticsearch uses with RavenDB documents is updated.  
   * an optional [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) 
     command, to delete existing document versions before appending new ones.  
     You can [omit](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
     _delete_by_query commands from the script using the task's **Insert Only** option.  
   * a [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) 
     command to append RavenDB documents to Elasticsearch.  

### Add Transformation Script

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
   *  Add all the Elasticsearch indexes your script uses, to the [indexes list](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes).  

3. **Syntax**
   Click for a transformation script Syntax Sample.  

4. **Collection**  
   Type or select the names of the collections your script is using.  

5. **Apply script to documents from beginning of time (Reset)**  
    * When this option is **enabled**, the script will be executed over **all existing documents in the 
      specified collections** the first time the task runs.  
    * When this option is **disabled**, the script will be executed **only over new and modified documents**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **enabled**,  
      RavenDB documents will be appended to Elasticsearch **without deleting documents from Elasticsearch first**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **disabled**,
      documents will be **deleted from Elasticsearch first**, and then appended to it from RavenDB.  

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
   The test will display the commands that would be sent to Elasticsearch, **without** actually sending them.  
3. **Close Test Area**  
   Close this view.  

---

### Test Results

The test results view displays a **preview** of the tested document, and the **commands** the task would send Elasticsearch.  

---

* **Document Preview Tab**  
  !["Document Preview Tab"](images/elasticsearch-test-results-preview-tab.png "Document Preview Tab")  

---

* **Test Results Tab**  
!["Test Results Tab"](images/elasticsearch-test-results.png "Test Results Tab")  

    1. **Test Results Tab**  
       Displays the commands that the task would send Elsticsearch.  
    2. **Elasticsearch Index**  
       The index the commands and data are sent to.  
    3. [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) Segment  
       With a list of IDs by which Elasticsearch would locate and remove existing documents.  
       Deleting existing document versions is **optional**, enable [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) 
       to prevent the task from sending `_delete_by_query` commands.  
    4. [_bulk ](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-bulk.html) Segment  
       With a list of document objects, each with data extracted from RavenDB and an ID that Elasticsearch stores it by.  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: RavenDB ETL](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: SQL ETL](../../../../server/ongoing-tasks/etl/sql)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  
- [Ongoing Tasks: Elasticsearch ETL](../../../../server/ongoing-tasks/etl/elasticsearch)  

### Studio

- [Ongoing Tasks: RavenDB ETL](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
- [Ongoing Tasks: OLAP ETL](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task)  

### Client API

- [How to Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [How to Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
- [How to Add an ETL Task](../../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update an ETL Task](../../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset an ETL Task](../../../../client-api/operations/maintenance/etl/reset-etl)
