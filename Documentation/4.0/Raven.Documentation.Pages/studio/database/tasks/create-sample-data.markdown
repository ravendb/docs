# Creating Sample Data
---

{NOTE: }

* Sample data can be generated only on an empty database.  

* Most of the examples in the RavenDB documentation are based on this sample data.  
  Thus, creating it will enable you to play with the data as you use the documentation to become more familiar with various RavenDB functionalities.  

* In this page:  
  * [Create Sample Data](../../../studio/database/tasks/create-sample-data#create-sample-data)  
  * [The Data Generated](../../../studio/database/tasks/create-sample-data#the-data-generated)  
  * [The C# Entities](../../../studio/database/tasks/create-sample-data#the-c#-entities)  
{NOTE/}

---

{PANEL: Create Sample Data}

* Because you need an empty database, [create a new database](../../studio/server/databases/create-new-database/general-flow) and name it "Northwind" to sync with the documentation examples.  
    *If you will want to use [multiple nodes](../../studio/server/databases/create-new-database/general-flow#3.-configure-replication) and/or [encryption](../../studio/server/databases/create-new-database/encrypted), we recommend creating your playground database accordingly.  

* Navigate to `Tasks` options in `Settings`, then click the **'Create'** button to generate the Northwind data.  

* Various collections, their documents and sample indexes will be created in your empty database.  
<br/>

![Figure 1. Create Sample Data](images/create-sample-data-1.png "Create sample data")

{PANEL/}

{PANEL: The Data Generated }

* The Generated database contains **1,059 documents** organized in **8 collections** and **3 indexes**.  
<br/>

![Figure 2. The generated sample documents](images/create-sample-data-2.png "Sample documents generated")

<br/>

![Figure 3. The generated sample indexes](images/create-sample-data-3.png "Sample indexes generated")

{PANEL/}

{PANEL: The C# Entities}

* The **C# classes** used for appropriate documents entities can also be viewed  
<br/>

![Figure 4. The C# classes used for the entities](images/create-sample-data-4.png "C# Classes")

{PANEL/}
