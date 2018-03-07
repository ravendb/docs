# RavenDB ETL Task
---

{NOTE: }

* A **RavenDB ETL Task** is an [ETL process](../../../../server/ongoing-tasks/etl/basics#basics) 
  that transfers data from the current database to another RavenDB database instance, 
  outside of the [Database Group](../../../../studio/database/settings/manage-database-group)  

* The sent data can be filtered and modified by transformation scripts  

* Learn more about the benefits of using ETL in [Why use ETL](../../../../server/ongoing-tasks/etl/basics#why-use-etl)  

* ETL is different from data replication. See [RavenDB ETL Task -vs- Replication Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task--vs--replication-task)  

* In this page:  
  * [RavenDB ETL Task - Definition](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task---definition)  
  * [RavenDB ETL Task - Transform Scripts](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task---transform-scripts)  
  * [RavenDB ETL Task - Details in Tasks List View](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task---details-in-tasks-list-view)  
  * [RavenDB ETL Task - Offline Behaviour](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task---offline-behaviour)  
  * [RavenDB ETL Task -vs- Replication Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task#ravendb-etl-task--vs--replication-task)  
{NOTE/}

---

{PANEL: RavenDB ETL Task - Definition}

![Figure 1. RavenDB ETL Task Definition](images/ravendb-etl-task-1.png "Create New RavenDB ETL Task")

1. **Task Name** (Optional)  
   * Choose a name of your choice  
   * If no name is given then RavenDB server will create one for you based on the defined connection string  

2. **Preferred Node** (Optional)  
  * Select a preferred mentor node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be the responsible node for this RavenDB ETL Task  
  * If not selected, then the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties))  

3. **Connection String**  
   * Select a [Connection String](../../../todo-update-me-later) from the pre-defined list -or-create a new connection string to be used  
   * The connection string defines the destination database and its database group server nodes URLs  
{PANEL/}

{PANEL: RavenDB ETL Task - Transform Scripts}

![Figure 2. RavenDB ETL Task - Transform Scripts](images/ravendb-etl-task-2.png "RavenDB ETL Task - Transform Scripts")

1. Click to add a new script  

2. Edit or Delete an existing script  

3. Enter the script to use.  
   In the above example, each source document from the 'Products' collection will be sent to 'ProductsInfo' collection in the destination database db3 
   (which is external to the cluster).  
   Each new document will have 2 fields: 'ProductName' & 'SupplierName'.  
   For detailed script options see [Transformation Script Options](../../../../server/ongoing-tasks/etl/raven#transformation-script-options).  

4. By default, updates to the ETL script will _not_ be applied to documents that were already sent.  
   When checking this option RavenDB will start the ETL process for this script from _scratch_ ("beginning of time"),  
   rather than apply the update only to new or updated documents.  

5. Select the collections for the ETL task -or - apply to all collections  
{PANEL/}

{PANEL: RavenDB ETL Task - Details in Tasks List View}

![Figure 3. RavenDB ETL Task - Task List View](images/ravendb-etl-task-3.png "Tasks List View Details")

1. **RavenDB ETL Task Details**:
   *  Task Status - Active / Not Active / Not on Node / Reconnect (see issue RavenDB-10674)  
   *  Connection String - The connection string used  
   *  Destination Database - The destination database to which the data is being sent  
   *  Actual Destination URL - The server URL to which the data is actually being sent,  
      the one that is currently used out of the available _Topology Discovery URLs_  
   *  Topology Discovery URLs - List of the available destination Database Group servers URLs  

2. **Graph view**:  
   Graph view of the responsible node for the External Replication Task  
{PANEL/}

{PANEL: RavenDB ETL Task - Offline Behaviour}

* **When the source cluster is down** (and there is no leader):  

  * Creating a _new_ Ongoing Task is a Cluster-Wide operation,  
    thus, a new Ongoing RavenDB ETL Task ***cannot*** be scheduled.  

  * If a RavenDB ETL Task was _already_ defined and active when the cluster went down,  
    then the task will not be active, data will not be ETL'ed.  

* **When the node responsible for the ETL task is down:**  

  * If the responsible node for the RavenDB ETL Task is down,  
    then another node from the Database Group will take ownership of the task so that the ETL process will continue executing.  

* **When the destination node is down:**  

  * The ETL process will wait until the destination is reachable again and proceed from where it left off.  

  * If there is a cluster on the other side, and the URL addresses of the destination database group nodes are listed in the connection string, 
    then when the destination node is down, RavenDB ETL will simply start transferring data to one of the other nodes specified.  
{PANEL/}

{PANEL: RavenDB ETL Task -vs- Replication Task}

1. **Data ownership**:  

    * When a RavenDB node performs an **ETL** to another node it is _not_ replicating the data, it is _writing_ it.  
      In other words, we always _overwrite_ whatever exists on the other side, there is no [conflicts handling](../../../todo-update-me-later).  

    * The source database for the ETL process is the owner of the data.  
      This means that any modifications done to the ETL’ed data on the destination database side are lost when overwriting  occurs.  

    * So, if you need to modify the ETL'ed data in the destination side, you should create a companion document on the destination database instead of modifying the ETL’ed data directly.  
      The rule is: For ETL’ed data, you can look but not touch...  

    * On the other hand, Data that is replicated with RavenDB's [External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) does _not_ overwrite existing documents.  
      Conflicts are created and handled according to the destination database policy defined.  
      This means that you _can_ change the replicated data on the destination database and conflicts will be solved.  

2. **Data content**:  

    * With replication Task, _all_ documents contained in the database are replicated to the destination database _without_ any content modification.  

    * Whereas in ETL, the document content sent can be filtered and modified with the supplied transformation script.  
      In addition, partial data can be sent as specific collections can be selected.  
{PANEL/}

## Related Articles

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)
- [SQL ETL Task](../../../../server/ongoing-tasks/etl/sql)
- [Define RavenDB ETL Task in Studio](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
- [Define SQL ETL Task in Studio](../../../todo-update-me-later)
