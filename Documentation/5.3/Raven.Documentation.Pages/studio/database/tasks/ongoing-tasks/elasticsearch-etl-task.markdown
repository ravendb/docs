# Elasticsearch ETL Task

---

{NOTE: }

* An **Elasticsearch ETL Task** creates an ETL process that transfers data 
  from selected RavenDB collections to an Elasticsearch destination.  
  The transferred data can be filtered and modified by transformation scripts.  
* An Elasticsearch ETL task transfers **documents only**.  
  Document extensions like attachments, counters, or time series, will not be transferred.  
* This page explains how to create an Elasticsearch ETL task using the Studio.  
  Learn more about Elasticsearch ETL tasks and how to create one using 
  the client API in [Ongoing Tasks: Elasticsearch ETL](../../../../server/ongoing-tasks/etl/elasticsearch)  

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
   * Enter a name for your task  
   * If no name is provided, the cluster will create a name based on the defined connection string,  
     e.g. *ElasticSearch ETL to ElasticConStr*  

2. **Task State**  
   Select the task state:  
   Enabled - The task runs in the background, transforming and sending documents as defined in this view.  
   Disabled - No documents are transformed and sent.  

3. **Responsible Node** (Optional)  
  * Select a node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be responsible for this task.  
  * If no node is selected, the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)).  

4. **Connection String**  
    * Select an existing connection string from the list or create a new one.  
    * The connection string defines the destination Elasticsearch URLs and the 
      authentication method.  
      !["Create Connection String"](images/elasticsearch-connection-string.png "Create Connection String")
        * a. **Name** - Enter a name for the connection string.  
        * b. **Nodes URLs** - Provide the URL(s) of the destination Elasticsearch node(s).  
        * c. **Authentication** - Select the authentication method relevant for the 
          Elasticsearch node(s)
          !["Authentication Methods"](images/elasticsearch-connection-string-authentication.png "Authentication Methods")

{PANEL/}

{PANEL: Elasticsearch Indexes}

Elasticsearch uses [Indexes](https://www.elastic.co/blog/what-is-an-elasticsearch-index) to store, access, and delete documents.  
Use the task's **Elasticsearch Indexes** settings to determine which Elastsicsearch 
Indexes the ETL task will access.  

!["Define Elasticsearch Index"](images/elasticsearch-indexes-define-index.png "Define Elasticsearch Index")

#### 1. Add Index

* Click to add an Elasticsearch index to the list.  

#### 2. Index Name

* Enter the Elasticsearch index name.  
  The index name must match the index name provided in the 
  [loadTo\\\<indexName\\\>](../../../../server/ongoing-tasks/etl/basics#transform) command 
  in the transformation script.  
  E.g., using the command `loadToOrders` in the transformation script requires you 
  to define here an index by the name of `orders`.  
* The index name entered here must be all lower case, as required by Elasticsearch.  
  E.g. `orders`
* The index name used in the transformation script command can be either lower 
  or upper-case.  
  E.g. both `loadToOrders` and `loadToorders` are permitted.  

#### 3. Document ID Property Name

* Enter the name of a transformation script property that contains `id(this)`.  
  This property will be created in each generated Elasticsearch document, and 
  allow the ETL task to recognize the documents by their original RavenDB IDs 
  even when they are hosted by Elasticsearch.  

* E.g. -  
   * The transformation script property: `OrderId: id(this)`  
   * The property name you enter as **Document ID Property Name**: `OrderId`  
   * In each document that the transformation script creates in Elasticsearch, 
     it will create a property named `OrderId`, that contains the original 
     RavenDB document ID.  

#### 4. Insert Only

* By default, the ETL task will:  
  1. **Delete** from the Elasticsearch destination all documents that 
     match the provided document ID field.  
     To do that, the ETL task will send the Elasticsearch destination 
     a `_delete_by_query` command.  
  2. **Append** new documents.  
     To do that, the ETL task will send the Elasticsearch destination 
     a `_bulk` command.  

* Check **Insert Only** to **skip the first step** and append new documents **without** 
  deleting existing ones first.  
  {WARNING: }
   Enabling **Insert Only** would accumulate new document versions on 
   your Elasticsearch destination without ever removing them.  
  {WARNING/}

#### 5. Confirm

* Click to add this index to the list.  

#### 6. Cancel

* Click to cancel the operation without adding the index to the list.  

#### 7. An index that was added

* Can be edited or deleted.  

{PANEL/}

{PANEL: Transformation Script}

The transformation script defines the JSON document that will be sent to the 
Elasticsearch destination per RavenDB document from the selected collections.  

### Add Transformation Script

!["List of transformation Scripts"](images/elasticsearch-transformation-scripts-list.png "List of transformation Scripts")

1. **Add Transformation Script**  
   Click to add a new transformation script to the list.  

2. **Existing Script**  
     a. **Script** name & **Collections** on which it is defined. (Informative)  
     b. **Edit** - Click to edit the script.  
     c. **Remove** - Click to remove the script from the list.  

---

### Edit Transformation Script

!["Edit Transformation Script"](images/elasticsearch-edit-transformation-script.png "Edit Transformation Script")

1. **Script Name**  
   Enter a name for the script (Optional).  
   A default name will be generated if no name is entered, e.g. Script_1  

2. **Script**  
   Edit the transformation script.  
   * Define a **document object** whose contents will be extracted from 
     each RavenDB document processed by the ETL task and appended as 
     a document to the Elasticsearch destination.  
     E.g., `var orderData` in the above example.  
   * Make sure that one of the properties of the document object 
     is given the value `id(this)`.  
     {NOTE: }
     The ETL task will use this property [to identify](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#document-id-property-name) 
     documents that reside on the Elasticsearch destination by their source 
     RavenDB document ID.  
     {NOTE/}
   * Use the loadTo\<indexName\> method to pass the document object 
     to the Elasticsearch destination.  

3. **Syntax**  
   Click for a transformation script Syntax Sample.  

4. **Collections**  
    * **Select (or enter) a collection**  
      Type or select the names of the collections your script is using.  
    * **Collections Selected**  
      A list of collections that were already selected.  

5. **Apply script to documents from beginning of time (Reset)**  
    * When this option is **enabled**, the script will be executed over **all existing documents in the 
      specified collections** the first time the task runs.  
    * When this option is **disabled**, the script will be executed **only over new and modified documents**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **enabled**,  
      RavenDB documents will be appended to Elasticsearch **without deleting documents from Elasticsearch first**.  
    * If [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) is **disabled**,
      documents will be **deleted from Elasticsearch first**, and then appended to it from RavenDB.  

6. **Add/Update**  
   Click to add a new script or update the task with changes made in an existing script.  

7. **Cancel**  
   Click to cancel your changes.  

8. **Test Script**  
   Click to **test** the transformation script (read more about this option below).  

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
