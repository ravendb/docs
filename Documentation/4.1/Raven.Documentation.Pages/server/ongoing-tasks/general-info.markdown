# Ongoing Tasks - General Info
---

{NOTE: }

* Ongoing tasks are ***work tasks*** defined for the database.  
* Once defined, these tasks are ***ongoing*** , meaning that they will do the defined work for any data change in the database.  
* Each task has a responsible node from the Database Group nodes, this node will actually perform the defined task work.  
* The available ongoing tasks are:  
  * [External Replication](../../server/ongoing-tasks/external-replication)  
      * Replicate the database documents to another RavenDB database that is not on this Database Group  
      * A live copy of the data, on the local cluster or another cluster  
  * [RavenDB ETL](../../server/ongoing-tasks/etl/raven)  
      * Write all database documents, or just part of it, to another RavenDB database  
      * Data can be filtered and modified with transformation scripts  
  * [SQL ETL](../../server/ongoing-tasks/etl/sql)  
      * Write the database data to a relational database  
      * Data can be mutated with transformation scripts  
  * [Backup](../../server/ongoing-tasks/backup-overview)  
      * Schedule a backup or a snapshot of the database at a specified point in time  
  * [Subscription](../../client-api/data-subscriptions/what-are-data-subscriptions)
      * Sending batches of documents that match a pre-defined query for processing on a client  
      * Data can be mutated with transformation scripts  
{NOTE/}

{PANEL: Ongoing Tasks - General Maintenance Operation}

### Delete Ongoing Task Operation

{CODE-BLOCK: csharp}
    public DeleteOngoingTaskOperation(long taskId, OngoingTaskType taskType)
{CODE-BLOCK/}

| Parameters | | |
| ------------- | ----- | ---- |
| **taskId** | long | task id |
| **taskType** | OngoingTaskType | Task type :  Replication, RavenEtl, SqlEtl, Backup, Subscription|

### Get Ongoing Task Info Operation

{CODE-BLOCK: csharp}
   public GetOngoingTaskInfoOperation(long taskId, OngoingTaskType type)
{CODE-BLOCK/}

| Parameters | | |
| ------------- | ----- | ---- |
| **taskId** | long | task id |
| **taskType** | OngoingTaskType | Task type :  Replication, RavenEtl, SqlEtl, Backup, Subscription|

{CODE-BLOCK: csharp}
    public GetOngoingTaskInfoOperation(string taskName, OngoingTaskType type)
{CODE-BLOCK/}

| Parameters | | |
| ------------- | ----- | ---- |
| **taskName** | string | task name |
| **taskType** | OngoingTaskType | Task type :  Replication, RavenEtl, SqlEtl, Backup, Subscription|

{PANEL/}
