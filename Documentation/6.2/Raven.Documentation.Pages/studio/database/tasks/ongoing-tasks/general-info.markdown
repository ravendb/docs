# Ongoing Tasks - Overview
---

{NOTE: }

* Ongoing tasks are **work tasks** defined for the database.
 
* Each task is assigned a responsible node from the [Database Group nodes](../../../../studio/database/settings/manage-database-group) to handle the work.  
    * If not specified by the user, the cluster decides which node will be responsible for the task. See [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties).  
    * If a node is down, the cluster will reassign the work to another node for the duration.  

* Once enabled, an **ongoing task** runs in the background,  
  and its responsible node executes the defined task work whenever relevant data changes occur.

* In this page:
    * [The ongoing tasks](../../../../studio/database/tasks/ongoing-tasks/general-info#the-ongoing-tasks)
    * [The ongoing tasks list - View](../../../../studio/database/tasks/ongoing-tasks/general-info#the-ongoing-tasks-list---view)
    * [The ongoing tasks list - Actions](../../../../studio/database/tasks/ongoing-tasks/general-info#the-ongoing-tasks-list---actions)

{NOTE/}

---

{PANEL: The ongoing tasks}

The available ongoing tasks are:

![Figure 3. Ongoing Tasks New Task](images/task-list-1.png "Add ongoing task")

**Replication:**

* **[External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)**  
    Create a live replica of your database in another RavenDB database in another cluster.  
    This replication is initiated by the source database.  
* **[Hub/Sink Replication](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)**  
    Create a live replica of your database, or a part of it, in another RavenDB database.  
    The replication is initiated by the *Sink* task.  
    The replication can be *bidirectional* or limited to a *single direction*.  
    The replication can be *filtered* to allow the delivery of selected documents.  

**Backups & Subscriptions:**

* **[Backup](../../../../studio/database/tasks/backup-task)**  
    Schedule a backup or a snapshot of the database at a specified point in time.  
* **[Subscription](../../../../client-api/data-subscriptions/what-are-data-subscriptions)**  
    Send batches of documents that match a pre-defined query for processing on a client.

**ETL (RavenDB => Target):**

* **[RavenDB ETL](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)**  
    Write all or chosen database documents to another RavenDB database.  
    Data can be filtered and modified with transformation scripts.  
* **[SQL ETL](../../../../server/ongoing-tasks/etl/sql)**  
    Write the database data to a relational database.  
    Data can be filtered and modified with transformation scripts.  
* **[OLAP ETL](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task)**  
    Convert database data to the _Parquet_ file format for OLAP purposes.  
    Data can be filtered and modified with transformation scripts.  
* **[Elasticsearch ETL](../../../../studio/database/tasks/ongoing-tasks/elasticsearch-etl-task)**  
    Write all or chosen database documents to an Elasticsearch destination.  
    Data can be filtered and modified with transformation scripts.  
* **[Kafka ETL](../../../../studio/database/tasks/ongoing-tasks/kafka-etl-task)**  
    Write all or chosen database documents to topics of a Kafka broker.  
    Data can be filtered and modified with transformation scripts.  
* **[RabbitMQ ETL](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-etl-task)**  
    Write all or chosen database documents to a RabbitMQ exchange.  
    Data can be filtered and modified with transformation scripts.  
* **[Azure Queue Storage ETL](../../../../studio/database/tasks/ongoing-tasks/azure-queue-storage-etl)**  
    Write all or chosen database documents to Azure Queue Storage.  
    Data can be filtered and modified with transformation scripts.  

**Sink (Source => RavendB)**

* **[Kafka Sink](../../../../studio/database/tasks/ongoing-tasks/kafka-queue-sink)**  
    Consume and process incoming messages from Kafka topics.  
    Add scripts to Load, Put, or Delete documents in RavenDB based on the incoming messages.  
* **[RabbitMQ Sink](../../../../studio/database/tasks/ongoing-tasks/rabbitmq-queue-sink)**  
    Consume and process incoming messages from RabbitMQ queues.  
    Add scripts to Load, Put, or Delete documents in RavenDB based on the incoming messages.  

{PANEL/}

{PANEL: The ongoing tasks list - View}

![Figure 1. Ongoing Tasks View](images/task-list-2.png "Ongoing tasks list for database DB1")

1. Navigate to **Tasks > Ongoing Tasks**

2. The list of the current tasks defined for the database.  

3. The task name. 

4. The node that is currently responsible for executing the task.

{PANEL/}

{PANEL: The ongoing tasks list - Actions}

![Figure 2. Ongoing Tasks Actions](images/task-list-3.png "Ongoing tasks - actions")

1. **Add Task** - Create a new task for the database.
2. **Enable / Disable** the task.
3. **Details** - Click to see a short task details summary in this view.
4. **Edit** - Click to edit the task.
5. **Delete** the task.

The ongoing tasks can also be managed via the Client API. See [Ongoing tasks operations](../../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations).

{PANEL/}
