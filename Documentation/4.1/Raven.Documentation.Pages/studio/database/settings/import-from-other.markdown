# Import from MongoDB/CosmosDB
---

{NOTE: }

* Import _From other_ allows you to import your existing NoSQL Database into RavenDB.

* Currently **MongoDB** and **CosmosDB** are supported.

* You can import all collections or select individual collections to import.

* In this page:
  * [Import from MongoDB](#import-from-mongodb)
  * [Import from CosmosDB](#import-from-cosmosdb)
{NOTE/}


{INFO:Before you start}

You need to download an external tool used to migrate data from your existing NoSQL database called `Raven.Migrator`. 
This application is available in the `tools` package which you can download from [ravendb.net/downloads](https://ravendb.net/downloads). 

{INFO/}

---

{PANEL: Import from MongoDB}

![Figure 1.](images/mongodb-1.png "Import from MongoDB")

1. **Migrator Path** 
   * `Raven.Migrator.exe` can be found in the tools package on [ravendb.net/downloads](https://ravendb.net/downloads)
   * Input the path to the folder which contains `Raven.Migrator.exe`

2. **Database Source**
   * Select `MongoDB`
   
3. **Connection String** 
   * Input the connection string to your MongoDB instance 
   
4. **Migrate GridFS** 
   * Choosing this option will import GridFS attachments and save them as documents with attachments in the `@files` collection
   
5. **Database Name**
   * Input the name of the source database
   
6. **Migrate all collections**
   * You can either import all collections or select specific collections  
   * Optionally you can rename the imported collections  
   
7. **Use transform script** 
   * A script for filtering / modifying imported documents (written in JavaScript)  

{PANEL/}


{PANEL: Import from CosmosDB}

![Figure 2.](images/cosmosdb-1.png "Import from CosmosDB")


1. **Migrator Path** 
   * `Raven.Migrator.exe` can be found in the tools package on [ravendb.net/downloads](https://ravendb.net/downloads)
   * Input the path to the folder which contains `Raven.Migrator.exe`.

2. **Database Source**
   * Select `CosmosDB`
   
3. **Azure Endpoint URL** 
   * URL for CosmosDB database 
   
4. **Primary Key** 
   * Primary key for CosmosDB
   
5. **Database Name**
   * Input the name of the source database
   
6. **Migrate all collections**
   * You can either import all collections or select specific collections  
   * Optionally you can rename the collections  
   
7. **Use transform script** 
   * A JavaScript script to filter / modify imported documents.  

{PANEL/}
