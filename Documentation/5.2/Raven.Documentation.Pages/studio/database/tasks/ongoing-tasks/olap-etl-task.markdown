# OLAP ETL Task
---

{NOTE: }

* The **OLAP ETL task** is an ETL process that converts RavenDB data to the 
[_Apache Parquet_](https://parquet.apache.org/documentation/latest/) file format, and sends 
it to one of these destinations:  
  * [Amazon Web Services S3](https://aws.amazon.com/s3/)  
  * [Microsoft Azure](https://azure.microsoft.com/)  
  * [Google Cloud Platform](https://cloud.google.com/)  
  * Local storage  

* This page explains how to create an OLAP ETL task using the studio. To 
learn more about OLAP ETL tasks, and how to create one using the client API, 
see [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap).

* In this page:  
  * [Navigating to the OLAP ETL View](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#navigating-to-the-olap-etl-view)
  * [Defining an OLAP ETL Task](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#defining-an-olap-etl-task)

{NOTE/}

---

{PANEL: Navigating to the OLAP ETL View}

![](images/olap-etl-0.png)
![](images/olap-etl-1.png)

{WARNING: }
To begin creating your OLAP ETL task:  

1. Navigate to `Tasks > Ongoing Tasks`  
2. Press "Add a Database Task"  
3. Select "OLAP ETL"  
{WARNING/}

{PANEL/}

{PANEL: Defining an OLAP ETL Task}

![](images/olap-etl-5.png)

{WARNING: }

1. The name of this ETL task (optional).  
2. Choose which of the cluster nodes will run this task (optional).  
3. Set a prefix that will form part of the name of the folder that will contain the data at 
the destination.  
4. Schedule when this task will run. See more details [below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#run-frequency).  
5. Toggle whether to create a new connection string or use an existing one.  
6. Select one of the existing OLAP connection strings.  
7. To create a new table, enter the name of a collection that one of the ETL scripts 
operates on.  
8. Choose the name of the column that contains the document ID (optional).  
9. Add a transformation script. See [below](../../../../studio/database/tasks/ongoing-tasks/olap-etl-task#transform-script).  

{WARNING/}

{INFO: }
The list of existing transform scripts.  
{INFO/}

### Transform Script

![](images/olap-etl-6.png)

{WARNING: }

1. The transform script. Learn more about these scripts [here](../../../../server/ongoing-tasks/etl/raven).  
2. Select a collection on which this script will operate.  
3. If this option is selected, the script will operate on all existing documents in 
the specified collections the first time the task runs. If not, the script only 
operates on new documents.  

{WARNING/}

{INFO: }

1. The 'name' of a script is always "Script #[order of script creation]".  
2. The list of collections on which this script operates.  
{INFO/}

### Run Frequency

![](images/olap-etl-3.png)

Choose the exact timing and frequency at which this task should run. The maximum 
frequency is once every minute. If you choose `custom` from the dropdown menu, you can 
schedule the task using a [cron expression](https://docs.oracle.com/cd/E12058_01/doc/doc.1014/e12030/cron_expressions.htm).  

### OLAP Connection String

![](images/olap-etl-4.png)

If you chose to create a new connection string for this OLAP task, you can input 
its name and the destination here.  

{PANEL/}

## Related Articles

- [ETL Basics](../../../../server/ongoing-tasks/etl/raven)  
- [Ongoing Tasks: OLAP ETL](../../../../server/ongoing-tasks/etl/olap)  
