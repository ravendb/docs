# OLAP ETL Task
---

{NOTE: }

* The **OLAP ETL task** is an ETL process that converts RavenDB data to the 
[_Apache Parquet_](https://parquet.apache.org/documentation/latest/) file format, and sends 
it to one or more of these destinations:  
  * [Amazon S3](https://aws.amazon.com/s3/)
  * [Amazon Glacier](https://aws.amazon.com/glacier/)
  * [Microsoft Azure](https://azure.microsoft.com/)
  * [Google Cloud Platform](https://cloud.google.com/)
  * File Transfer Protocol
  * Local storage

* This page explains how to create an OLAP ETL task using the studio. To 
learn more about OLAP ETL tasks, and how to create one using the client API, 
see [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap).

* In this page:  
  * [Navigate to the OLAP ETL View](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#navigate-to-the-olap-etl-view)
  * [Define an OLAP ETL Task](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#define-an-olap-etl-task)
      * [Transform Script](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#transform-script)
      * [Run Frequency](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#run-frequency)
      * [OLAP Connection String](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#olap-connection-string)

{NOTE/}

---

{PANEL: Navigate to the OLAP ETL View}

![](images/olap-etl-1.png)
![](images/olap-etl-2.png)

{WARNING: }
To begin creating your OLAP ETL task:  

1. Navigate to `Tasks > Ongoing Tasks`  
2. Press "Add a Database Task"  
3. Select "OLAP ETL"  
{WARNING/}

{PANEL/}

{PANEL: Define an OLAP ETL Task}

![](images/olap-etl-3_1.png)

{WARNING: }

1. The name of this ETL task (optional).  
2. Choose which of the cluster nodes will run this task (optional).  
3. Set a prefix that will form part of the name of the folder that will contain the data at 
the destination.  
4. Schedule when this task will run. See more details [below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#run-frequency).  
5. Toggle whether to create a new connection string or use an existing one.  
6. Select one of the existing OLAP connection strings.  

{WARNING/}

![](images/olap-etl-3_2.png)

{WARNING: }

1. Add a transform script. See [below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#transform-script).  
2. Edit a transform script.  
3. Create a setting to override the ID column name for a particular parquet table.  
4. Enter the name of a collection that one of the ETL scripts operates on.  
5. Choose the name of the column that contains the document ID.  

{WARNING/}

{INFO: }

1. Select a destination for the OLAP ETL task.  
2. Add and edit transform scripts.  
3. Create settings to override the ID column names for particular parquet tables.  

{INFO/}

### Transform Script

![](images/olap-etl-6.png)

{WARNING: }

1. The script name is generated once the 'Add' button is clicked. The name of a script 
is always in the format: "Script #[order of script creation]".  
2. The transform script. Learn more about these scripts [here](../../../../server/ongoing-tasks/etl/raven#transformation-script-options).  
3. Select a collection (or enter a new collection name) on which this script will operate.  
4. The selected collection names on which the script operates.  
5. If this option is checked, the script will operate on all existing documents in the 
specified collections the first time the task runs. When the option is unchecked, the 
script operates only on new documents.  

{WARNING/}

### Run Frequency

![](images/olap-etl-3.png)

Select the exact timing and frequency at which this task should run from the dropdown menu. 
The maximum frequency is once every minute. Select 'custom' from the dropdown menu to 
schedule the task using your own customized [cron expression](https://docs.oracle.com/cd/E12058_01/doc/doc.1014/e12030/cron_expressions.htm).  

### OLAP Connection String

![](images/olap-etl-4.png)

{INFO: }

If you chose to create a new connection string for this OLAP task, you can input 
its name and the destination here. Multiple destinations can be defined.  

{INFO/}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  

### Client API

- [Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
