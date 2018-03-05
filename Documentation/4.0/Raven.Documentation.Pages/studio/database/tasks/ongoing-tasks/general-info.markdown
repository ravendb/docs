# Ongoing Tasks - General Info
---

{NOTE: }

* Ongoing tasks are ***work tasks*** defined for the database.  
* Once defined, these tasks are ***ongoing*** , meaning that they will do the defined work for any data change in the database.  
* Each task has a responsible node from the Database Group nodes, this node will actually perform the defined task work.  
* The available ongoing tasks are:  
  * [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
      * Replicate the database documents to another RavenDB database that is not on this Database Group  
      * A live copy of the data, on the local cluster or another cluster  
  * [RavenDB ETL](../../../todo-update-me-later)  
      * Write all database documents, or just part of it, to another RavenDB database  
      * Data can be mutated with transformation scripts  
  * [SQL ETL](../../../todo-update-me-later)  
      * Write the database data to a relational database  
      * Data can be mutated with transformation scripts  
  * [Backup](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
      * Schedule a backup or a snapshot of the database at a specified point in time  
  * [Subscription](../../../todo-update-me-later)  
      * Sending batches of documents that match a pre-defined query for processing on a client  
      * Data can be mutated with transformation scripts  

* In this page:  
  * [Ongoing Tasks - View](../../../../studio/database/tasks/ongoing-tasks/general-info#ongoing-tasks---view)  
  * [Ongoing Tasks - Actions](../../../../studio/database/tasks/ongoing-tasks/general-info#ongoing-tasks---actions)  
  * [Ongoing Tasks - Add New Task](../../../../studio/database/tasks/ongoing-tasks/general-info#ongoing-tasks---add-new-task)  
{NOTE/}

---

{PANEL: Ongoing Tasks - View}

![Figure 1. Ongoing Tasks View](images/ongoing-tasks-general-1.png "Ongoing Tasks List for databases DB1")

1. The list of the current tasks on the database  

2. Task name & state:  
   * _Active_ - Task is active and will do its job when there is a change to the database  
   * _Not-Active_ - Task is defined but has been disabled  
   * _Not on Node_ - The node in the currently viewed tab is not responsible for performing the task  
   * _Reconnect_ - Destination is unavailable, the task is active and trying to reconnect  

3. The node that is currently responsible for doing the task  
  * If not specified by the user, the cluster decides which node will actually be responsible for the task
  * If a node is down, the cluster will reassign the work to another node for the duration  

4. Tasks graph view  

{PANEL/}

{PANEL: Ongoing Tasks - Actions}

![Figure 2. Ongoing Tasks Actions](images/ongoing-tasks-general-2.png "Ongoing Tasks - Actions")

1. **Add Task** - Create a new task for the database - see below  
2. **Details** - Click for a short task details summary in this view  
3. **Enable / Disable** the task  
4. **Edit** - Click to edit the task  
5. **Delete** the task  
6. **Full screen** - Click to see the graph in a full-screen mode  

{PANEL/}

{PANEL: Ongoing Tasks - Add New Task}

![Figure 3. Ongoing Tasks New Task](images/ongoing-tasks-general-3.png "Add Ongoing Task")

* Select the preferred task to add  
  1. See [adding Extrenal Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) task  
  2. See [adding RavenDB ETL](../../../todo-update-me-later) task  
  3. See [adding SQL ETL](../../../todo-update-me-later) task  
  4. See [adding Backup](../../../../studio/database/tasks/ongoing-tasks/backup-task) task  
  5. See [adding Subscription](../../../todo-update-me-later) task  

* Once the task is defined and saved, it will be written to the [Database Record](../../../todo-update-me-later)  

* The cluster will decide which node will actually be responsible for doing the task - see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties)  
{PANEL/}
