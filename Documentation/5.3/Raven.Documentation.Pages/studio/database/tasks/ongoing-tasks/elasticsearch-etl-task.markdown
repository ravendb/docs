# Elasticsearch ETL Task

---

{NOTE: }

* An **Elasticsearch ETL task** is an ongoing process that -  
    * **Extracts** chosen data from the database,  
    * **Transforms** the data using a user-defined script,  
    * and **Loads** the data to a destination Elasticsearch index.  
* Data is loaded to the Elasticsearch destination in a **single bulk operation**.  
* By default, an existing document is deleted before a new one replaces it.  
  This behavior can be changed in the task settings, to write without 
  deleting documents first.  
* This page explains how to create an Elasticsearch ETL task using Studio.  

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
  By default, the transformation script deletes documents before replacing them with new ones.  
   * Enable this option to insert new documents without deleting existing ones.  

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
   Type or select the name of the collection your script is using.  

5. **Apply script to documents from beginning of time (Reset)**  
    * When this option is enabled, the script will operate on all existing documents in the 
      specified collection the first time the task runs.  
    * When this option is disabled, the script operates only on new and modified documents.  

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
   The test will **not** actually transfer the document.  
3. **Close Test Area**  
   Close this view.  

---

### Test Results

The test view displays a **Preview** of the tested document and the **Test Results** in separate tabs.  

* **Preview Tab**:  
  !["Test Results Preview Tab"](images/elasticsearch-test-results-preview-tab.png "Test Results Preview Tab")  
* **Test Results Tab** -  
  !["Test Results Tab"](images/elasticsearch-test-results-1.png "Test Results Tab")  
* Here is the same test, this time with the [Insert Only](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task#elasticsearch-indexes) option enabled 
  to prevent the script from deleting documents before inserting new documents.  
  Note the absence of [_delete_by_query](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html) statements in the transformed object.  
  !["Test Results with Insert Only Enabled"](images/elasticsearch-test-results-2.png "Test Results with Insert Only Enabled")  

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  

### Client API

- [Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
