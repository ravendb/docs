# Creating Sample Data
---

{NOTE: }

* Sample data can be generated only on an empty database.  

* Most of the examples in the RavenDB documentation are based on this sample data.  
  Thus, creating it will enable you to play with the data as you use the documentation to become more familiar with various RavenDB functionalities.  

* In this page:  
  * [Create Sample Data](../../../studio/database/tasks/create-sample-data#create-sample-data)  
  * [The Data Generated - Documents View](../../../studio/database/tasks/create-sample-data#the-data-generated---documents-view)  
  * [The C# Entities](../../../studio/database/tasks/create-sample-data#the-c#-entities)  
{NOTE/}

---

{PANEL: Create Sample Data}

* Because you need an empty database, [create a new database](../../studio/server/databases/create-new-database/general-flow) and name it "Northwind" to sync with the documentation examples.  
    *If you will want to use [multiple nodes](../../studio/server/databases/create-new-database/general-flow#3.-configure-replication) and/or [encryption](../../studio/server/databases/create-new-database/encrypted), we recommend creating your playground database accordingly.  

* Various collections, their documents and sample indexes will be created in your empty database.  
<br/>

1. In `Tasks` tab  
2. Select `Create Sample Data`  
3. Click the `Create` button to generate the Northwind data.  
    ![Figure 1. Create Sample Data](images/Create-Sample-Data.png "Create Sample Data")

{PANEL/}

{PANEL: The Data Generated - Documents View}

* The Generated database contains **1,059 documents** organized in **8 collections** and **3 indexes**.  
<br/>
    ![Figure 2. The generated sample documents](images/Northwind-Documents-View.png "Sample documents generated")
    1. Documents tab  
    2. Database collections - When writing `from` in a [query](../../../indexes/querying/basics#query-flow) it refers to these collections.  
    3. The document IDs.  
    4. Which collection each document is in  
    5. Document Extensions each document contains  
        * [Attachments](../../../document-extensions/attachments/what-are-attachments)
        * [Revisions](../../../server/extensions/revisions)
        * [Counters](../../../document-extensions/counters/overview)  
        * [Time Series](../../../document-extensions/timeseries/overview)  
{PANEL/}

{PANEL: The Indexes Generated - Indexes View} 
    ![Figure 3. The generated sample indexes](images/Northwind-Indexes-View.png "Sample indexes generated")
    1. Index tab  
    2. Select List of Indexes  
    3. Indexes that you can modify  
    4. Query this index  


{PANEL/}

{PANEL: The C# Entities}

* The **C# classes** used for appropriate documents entities can also be viewed  
    * In `Tasks` tab, select `Create Sample Data`.  
    * Under the `Create` button, select `View C# classes`.
    ![Figure 4. The C# classes used for the entities](images/View-C#-Classes.png "C# Classes")

{PANEL/}
