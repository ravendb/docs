# Creating Sample Data
---

{NOTE: }

* Sample data can be created in playground databases to enable developers to experiment with features before making changes to their actual database.  
  It is populated with various collections, .json documents, indexes, and document extensions.  

* Many of the examples in the RavenDB documentation are based on this sample data.  
  Creating it will enable you to play with the data as you use the documentation to familiarize yourself with RavenDB features.  

* In this page:  
  * [Create Sample Data](../../../studio/database/tasks/create-sample-data#create-sample-data)  
  * [The Data Generated - Documents View](../../../studio/database/tasks/create-sample-data#the-data-generated---documents-view)  
  * [The C# Entities](../../../studio/database/tasks/create-sample-data#the-c#-entities)  
{NOTE/}

---

{PANEL: Create Sample Data}

* Sample data can be added only on an empty database.  To create sample data, you can either empty your playground database or [create a new database](../../../studio/database/create-new-database/general-flow).  
    * If you plan to use [multiple nodes](../../../studio/server/databases/create-new-database/general-flow#3.-configure-replication) 
      and/or [encryption](../../../studio/server/databases/create-new-database/encrypted) in production, 
      we recommend installing your playground database in a similar environment.  




 ![Figure 1. Create Sample Data](images/Create-Sample-Data.png "Create Sample Data")

 1. Select the **Tasks** tab  
 2. Select **Create Sample Data**  
 3. Click the **Create** button to generate the Northwind sample data.  
    Various collections, their documents and sample indexes will be created in your database.  

{PANEL/}

{PANEL: The Data Generated - Documents View}

![Figure 2. Documents View](images/Northwind-Documents-View.png "Documents View")

1. **Documents** tab  
2. **Document Collections**  
   This is the first part of the auto-generated document `Id` string.  
3. **Document Ids**  
   Unique [document identification keys](../../../client-api/document-identifiers/working-with-document-identifiers).
4. **Collection**  
   Which [collection](../../../studio/database/documents/documents-and-collections) each document is in.  
5. **Document Extensions**  
    * [Attachments](../../../document-extensions/attachments/what-are-attachments)
    * [Revisions](../../../server/extensions/revisions)
    * [Counters](../../../document-extensions/counters/overview)  
    * [Time Series](../../../document-extensions/timeseries/overview)  

{PANEL/}

{PANEL: The Indexes Generate111d - Indexes View} 
    
![Figure 3. Indexes View](images/Northwind-Indexes-View.png "Indexes View")

1. **Index** tab  
   Click to view and modify current indexes.  
   * RavenDB's default [dynamic auto-indexes](../../../studio/database/indexes/indexes-overview#index-types) are managed 
     automatically in response to queries.  
   * [Static Indexes](../../../indexes/creating-and-deploying) can also be set up manually.  
2. **List of Indexes**  
   Select to see the list of current indexes.  
3. **Edit** this index.  
   Indexes can be **modified via Studio**.  
   Read [Modifying Index Definition](../../../studio/database/indexes/indexes-overview#modifying-index-definition) to learn more.  
4. **Query** this index.  
   A [general query](../../../studio/database/queries/query-view) automatically determines the best index to query and creates 
   new ones if a good index doesn't exist for that query.  
   Still, you can query specific indexes via the List of Indexes.  
   


{PANEL/}

{PANEL: The C# Entities}

The **C# classes** can also be viewed  


 ![Figure 4. The C# classes](images/View-CS-Classes.png "C# Classes")

  1. Select **Tasks** tab -> **Create Sample Data**.  
  2. Click to **View C# classes**.  
  3. C# classes based on the sample data.  
    Scroll to see more classes  

{PANEL/}
