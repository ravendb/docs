# Creating Sample Data
---

{NOTE: }

* Sample data can be generated only on an empty database.  

* Most of the examples in the RavenDB documentation are based on this sample data.  
  Thus, creating it will enable you to play with the data as you use the documentation to familiarize yourself with RavenDB features.  

* In this page:  
  * [Create Sample Data](../../../studio/database/tasks/create-sample-data#create-sample-data)  
  * [The Data Generated - Documents View](../../../studio/database/tasks/create-sample-data#the-data-generated---documents-view)  
  * [The C# Entities](../../../studio/database/tasks/create-sample-data#the-c#-entities)  
{NOTE/}

---

{PANEL: Create Sample Data}

* Because you need an empty database, [create a new database](../../../studio/database/create-new-database/general-flow) and name it "Northwind" to sync with the documentation examples.  
    * If you will want to use [multiple nodes](../../../studio/server/databases/create-new-database/general-flow#3.-configure-replication) and/or [encryption](../../../studio/server/databases/create-new-database/encrypted) in production, we recommend creating your playground database with these features so that your playground database will match your actual database.  

* Various collections, their documents and sample indexes will be created in your database.  


 ![Figure 1. Create Sample Data](images/Create-Sample-Data.png "Create Sample Data")

 1. In **Tasks** tab  
 2. Select **Create Sample Data**  
 3. Click the **Create** button to generate the Northwind data.  

{PANEL/}

{PANEL: The Data Generated - Documents View}

![Figure 2. Documents View](images/Northwind-Documents-View.png "Documents View")

1. **Documents** tab  
2. **Database Collections**  
   This is the first part of the auto-generated document `Id` string.  
3. **Document Ids**  
   Unique [document identification keys](../../../client-api/document-identifiers/working-with-document-identifiers) are strings.
4. **Collection**  
   Which [collection](../../../studio/database/documents/documents-and-collections) each document is in  
5. **Document Extensions** each document contains  
    * [Attachments](../../../document-extensions/attachments/what-are-attachments)
    * [Revisions](../../../server/extensions/revisions)
    * [Counters](../../../document-extensions/counters/overview)  
    * [Time Series](../../../document-extensions/timeseries/overview)  

{PANEL/}

{PANEL: The Indexes Generated - Indexes View} 
    
![Figure 3. Indexes View](images/Northwind-Indexes-View.png "Indexes View")

1. Index tab  
2. List of **Indexes**  
3. Indexes that you can **modify via Studio**  
4. **Query** this index  


{PANEL/}

{PANEL: The C# Entities}

The **C# classes** used for appropriate documents entities can also be viewed  


 ![Figure 4. The C# classes](images/View-CS-Classes.png "C# Classes")

  1. In **Tasks** tab, select **Create Sample Data**.  
  2. Under the **Create** button, select **View C# classes**.
  3. These are the C# classes. You can scroll to see them all.  

{PANEL/}
