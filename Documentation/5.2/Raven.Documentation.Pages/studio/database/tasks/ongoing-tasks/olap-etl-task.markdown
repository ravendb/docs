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
      * [Override ID column](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#override-id-column)

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
3. Set a custom partition value which can be referenced in the transform script. [See below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#custom-partition-value).  

{WARNING/}
<br/>
### Custom Partition Value

!["Custom partition value"](images/olap-etl-7.png "Custom partition value")

* A custom partition can be defined to differentiate parquet file locations when 
using the same connection string and transform script in multiple OLAP ETL tasks.  
* The custom partition **name** is defined inside the transformation script.  
* The custom partition **value** is defined in the input box above.  
* The custom partition value can be referenced in the transform script as 
`$customPartitionValue`.  
* A parquet file path with custom partition will have the following format: `{RemoteFolderName}/{CollectionName}/{customPartitionName=$customPartitionValue}`  
* Learn more in [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap#the-custom-partition-value).  
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

* Select an existing connection string from the available dropdown or create a new one.  
* If you choose to create a new connection string you can enter its name and destination here.  
* Multiple destinations can be defined.  
<br/>
### OLAP ETL Destinations

!["OLAP ETL destinations"](images/olap-etl-3_2.png "OLAP ETL destinations")

Select one or more destinations from this list. Clicking each toggle reveals further 
fields and configuration options for each destination.  
<br/>
### Transform Script

!["List of transform scripts"](images/olap-etl-9.png "List of transform scripts")

{WARNING: }

1. List of existing transform scripts.  
2. Add a new transform script.  
2. Edit an existing transform script.  

{WARNING/}

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

{INFO: }

Every parquet table that is created by a transform script includes two columns that 
aren't specified in the script:  

* `_id`: contains the document ID. Its default name is `_id`, but this name can be 
overriden in the task definition - see more [below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#override-id-column).  
* `_lastModifiedTime`: the value of the `last-modified` field in a document's 
metadata. Represented in unix time.  

{INFO/}
<br/>
### Override ID Column

!["Override ID column"](images/olap-etl-8.png "Override ID column")

These settings allow you to specify a different column name for the document ID column 
in a parquet file. The default ID column name is `_id`.  

{WARNING: }

1. Add a new setting.  
2. Select the name of the parquet table for which you want to override the ID column.  
3. Select the name for the table's ID column.  
4. Save the setting.  
5. Edit an existing setting.  

{WARNING/}

{PANEL/}

## Related Articles

### Server

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  

### Client API

- [Add a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  
- [Get a Connection String](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)  
