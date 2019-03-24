# Import from MongoDB/CosmosDB
---

{NOTE: }

* Import from other allows you to import your existing NoSQL Database into RavenDB.

* Currently **MongoDB** and **CosmosDB** are supported.

* You can import all collections or choose collections to import.

* In this page:
  * [Import from MongoDB](../../../../studio/database/tasks/import-data/import-from-other#import-from-mongodb)
  * [Import from CosmosDB](../../../../studio/database/tasks/import-data/import-from-other#import-from-cosmosdb)
{NOTE/}


{INFO:Before you start}

You need to download external tool used to migrate data from your existing NoSQL database called `Raven.Migrator`. 
This application is available in `tools` package, which you can obtain from [ravendb.net](https://ravendb.net/downloads) page. 

{INFO/}

---

{PANEL: Import from MongoDB}

![Figure 1.](images/mongodb-1.png "Import from MongoDB")

1. **Migrator Path** 
   * `Raven.Migrator.exe` can be found in tools package on [ravendb.net](https://ravendb.net/downloads) website
   * Provide path to folder which contains `Raven.Migrator.exe` file.

2. **Database Source**
   * Choose `MongoDB`
   
3. **Connection String** 
   * Provide connection string to your mongodb instance 
   
4. **Migrate GridFS** 
   * Choosing this option will save GridFS attachments as documents with attachments in `@files` collection.
   
5. **Database Name**
   * Provide name of source database
   
6. **Migrate all collections**
   * You can either import all collections or choose which collections should be imported. 
   * Optionally you can rename collection names. 
   
7. **Use transform script** 
   * Transform script written in JavaScript allow to filter / modify imported documents.   

{PANEL/}


{PANEL: Import from CosmosDB}

![Figure 2.](images/cosmosdb-1.png "Import from CosmosDB")


1. **Migrator Path** 
   * `Raven.Migrator.exe` can be found in tools package on [ravendb.net](https://ravendb.net/downloads) website
   * Provide path to folder which contains `Raven.Migrator.exe` file.

2. **Database Source**
   * Choose `CosmosDB`
   
3. **Azure Endpoint URL** 
   * URL for CosmosDB database 
   
4. **Primary Key** 
   * Primary key for CosmosDB
   
5. **Database Name**
   * Provide name of source database
   
6. **Migrate all collections**
   * You can either import all collections or choose which collections should be imported. 
   * Optionally you can rename collection names. 
   
7. **Use transform script** 
   * Transform script written in JavaScript allow to filter / modify imported documents.   

{PANEL/}
