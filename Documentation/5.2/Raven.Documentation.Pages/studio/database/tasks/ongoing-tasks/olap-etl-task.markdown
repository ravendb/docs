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
      * [Custom Partition Value](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#custom-partition-value)
      * [Run Frequency](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#run-frequency)
      * [OLAP Connection String](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#olap-connection-string)
      * [OLAP ETL Destinations](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#olap-etl-destinations)
      * [Transform Script](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#transform-script)

{NOTE/}

---

{PANEL: Navigate to the OLAP ETL View}

!["Ongoing task view"](images/olap-etl-1.png "Ongoing task view")

{WARNING: }
To begin creating your OLAP ETL task:  

1. Navigate to `Tasks > Ongoing Tasks`  
2. Click on "Add a Database Task"  
{WARNING/}

!["Task selection view"](images/olap-etl-2.png "Task selection view")

{WARNING: }
3. Select "OLAP ETL"  
{WARNING/}

{PANEL/}

{PANEL: Define an OLAP ETL Task}

!["New OLAP ETL task"](images/olap-etl-3_1.png "New OLAP ETL task view")

{WARNING: }

1. The name of this ETL task (optional).  
2. Choose which of the cluster nodes will run this task (optional).  
3. Set a custom partition value which can be referenced in the transform script. See next section.  

{WARNING/}
<br/>
### Custom Partition Value

!["Custom partition value"](images/olap-etl-7.png "Custom partition value")

The custom partition value can be referenced in the transform script as 
`$customPartitionValue`. This setting gives you another way to distinguish 
data from different ETL tasks that use the same transform script.  

Learn more in [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap#the-custom-partition-value).  
<br/>
### Run Frequency

!["Task run frequency"](images/olap-etl-3.png "Task run frequency")

* Select the exact timing and frequency at which this task should run from the dropdown menu.  
* The maximum frequency is once every minute.  
* Select 'custom' from the dropdown menu to schedule the task using your own customized 
[cron expression](https://docs.oracle.com/cd/E12058_01/doc/doc.1014/e12030/cron_expressions.htm).  
<br/>
### OLAP Connection String

![](images/olap-etl-4.png)

If you chose to create a new connection string for this OLAP task, you can input 
its name and the destination here. Multiple destinations can be defined.  
<br/>
### OLAP ETL Destinations

!["OLAP ETL destinations"](images/olap-etl-3_2.png "OLAP ETL destinations")

Select one or more destinations from this list. Clicking each toggle reveals further 
fields and configuration options for each destination.  
<br/>
### Transform Script

!["Transform script"](images/olap-etl-6.png "Transform script")

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

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  

### Client API

- [Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
