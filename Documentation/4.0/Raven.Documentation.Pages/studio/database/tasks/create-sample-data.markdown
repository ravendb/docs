# Creating Sample Data
---

{NOTE: }

* Sample data can be created in playground databases to enable developers to experiment with features before making changes to their actual database.  
  It is populated with various collections, .json documents, indexes, and document extensions.  

* Many of the examples in the RavenDB documentation are based on this sample data.  
  Creating it will enable you to play with the data as you use the documentation to familiarize yourself with RavenDB features.  

* In this page:  
  * [Create Sample Data](../../../studio/database/tasks/create-sample-data#create-sample-data)  
  * [The Data Generated](../../../studio/database/tasks/create-sample-data#the-data-generated)  
  * [The C# Entities](../../../studio/database/tasks/create-sample-data#the-c#-entities)  
{NOTE/}

---

{PANEL: Create Sample Data}

* Sample data can be added only on an empty database.  To create sample data, you can either empty your playground database or [create a new database](../../../studio/database/create-new-database/general-flow).  
    * If you plan to use [multiple nodes](../../../studio/server/databases/create-new-database/general-flow#3.-configure-replication) 
      and/or [encryption](../../../studio/server/databases/create-new-database/encrypted) in production, 
      we recommend installing your playground database in a similar environment.  

* Navigate to `Tasks` options in `Settings`, then click the **'Create'** button to generate the Northwind data.  


---
![Figure 1. Create Sample Data](images/create-sample-data-1.png "Create sample data")

{PANEL/}

{PANEL: The Data Generated }


![Figure 2. The generated sample documents](images/create-sample-data-2.png "Sample documents generated")

<br/>

![Figure 3. The generated sample indexes](images/create-sample-data-3.png "Sample indexes generated")

{PANEL/}

{PANEL: The C# Entities}

* The **C# classes** used for appropriate documents entities can also be viewed  
<br/>

![Figure 4. The C# classes used for the entities](images/create-sample-data-4.png "C# Classes")

{PANEL/}
