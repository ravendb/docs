# Import from Other Databases
---

{NOTE: }

* You can import your existing NoSQL Databases to an [empty](../../../../studio/database/create-new-database/general-flow) RavenDB database.  

* Currently supported:  
   * **MongoDB**  
   * **CosmosDB**  

* You can import all collections, or select specific collections to import.  

* In this page:

  * [Download Raven.Migrator Tool](../../../../studio/database/tasks/import-data/import-from-other#download-raven.migrator-tool)  
  * [Import from MongoDB](../../../../studio/database/tasks/import-data/import-from-other#import-from-mongodb)  
  * [Import from CosmosDB](../../../../studio/database/tasks/import-data/import-from-other#import-from-cosmosdb)  

{NOTE/}



{PANEL: Download Raven.Migrator Tool}

To migrate data from your existing NoSQL database, you need to download an external tool called `Raven.Migrator`.  
The application is available at [ravendb.net/download](https://ravendb.net/download), in the `TOOLS` package (see image below).  
The `Raven.Migrator` tool can be found in the downloaded `Tools` folder. 

![Raven.Migrator Tool Download](images/raven-migrator-tool-download.png "Raven.Migrator Tool Download")

{PANEL/}

---

{PANEL: Import from MongoDB}
In the RavenDB Studio client on the left side, select `Tasks` -> `Import Data`.

![Figure 1.](images/mongodb-1.png "Import from MongoDB")

1. **Migrator Path**  
   * Find `Raven.Migrator.exe` in the [tools package](../../../../studio/database/tasks/import-data/import-from-other#download-raven.migrator-tool).  
   * Provide a path to `Raven.Migrator.exe`'s folder.  

2. **Database Source**  
   * Select `MongoDB`  
   
3. **Connection String**  
   * Provide the connection string to your MongoDB instance.  
   
4. **Migrate GridFS** 
   * Choosing this option will import GridFS attachments and save them as documents with attachments in the `@files` collection.  
   
5. **Database Name**
   * Provide the name of the **source** database.  
   
6. **Migrate all collections**  
   * You can either import all collections or select the collections you'd like to import.  
   * Optionally, you can rename the imported collections.  
   
7. **Use Transform script**  
   * Use a JavaScript to filter / modify imported documents.  

{PANEL/}


{PANEL: Import from CosmosDB}
In the RavenDB Studio client on the left side, select `Tasks` -> `Import Data`.

![Figure 2.](images/cosmosdb-1.png "Import from CosmosDB")


1. **Migrator Path**  
   * Find `Raven.Migrator.exe` in the [tools package](../../../../studio/database/tasks/import-data/import-from-other#download-raven.migrator-tool).  
   * Provide a path to `Raven.Migrator.exe`'s folder.  

2. **Database Source**  
   * Select `CosmosDB`  
   
3. **Azure Endpoint URL**  
   * Provide a URL to CosmosDB database  
   
4. **Primary Key**  
   * Provide the Primary key for CosmosDB  
   
5. **Database Name**  
   * Provide the source database name  
   
6. **Migrate all collections**  
   * You can either import all collections or select the collections you'd like to import.  
   * Optionally, you can rename the imported collections.  
   
7. **Use Transform script**  
   * Use a JavaScript to filter / modify imported documents.  

{PANEL/}
