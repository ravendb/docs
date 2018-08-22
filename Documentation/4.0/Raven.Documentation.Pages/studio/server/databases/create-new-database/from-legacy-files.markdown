# Create a Database : From Legacy Files
---

{NOTE: }

This database creation option is designed for importing database from data files from RavenDB v3.x.

* In this page:  
  * [1. New Database From Backup](../../../../studio/server/databases/create-new-database/from-legacy-files#1.-new-database)  
  * [2. Data Source Configuration](../../../../studio/server/databases/create-new-database/from-legacy-files#2.-source-configuration)  
 
 {NOTE/}

---

{PANEL: 1. Creating New Database From Legacy Files}

![Figure 1. Create New Database From Legacy Files](images/new-database-from-legacy-1.png "Create New Database From Legacy Files")

{NOTE: }
Open the down arrow and click `New database from legacy files`.
{NOTE/}
{PANEL/}

{PANEL: 2. Data Source Configuration}

![Figure 2. Create New Database From Legacy Files - Data Source Configuration](images/new-database-from-legacy-2.png "Data Source Configuration")

1. **Database Name**
    A database name can be any sequence of characters except for the following:  

    * A name cannot start or end with  ' . '  
    * A name cannot exceed 230 characters  
    * A name cannot contain any of the following:   /, \, :, *, ?, ", <, >, |  

2. **Resource type**
    * RavenFS files will be saved as documents with attachments in @files collection.

3. **Data directory**
    * Absolute path to data directory. 
    * This folder should contain file Data.jfm or Raven.voron.

4. **Advanced source properties**
    
    * Source bundles : Encryption   
        In v3.x by default, backup of an encrypted database contains the encryption information as a plain text in Database.Document file found in backup. 
        This is required to make RavenDB able to restore the backup on a different machine.
        You need to insert :

        * Encryption key : value of 'Raven/Encryption/Key'
            
        * Encryption algorithm : value of 'Raven/Encryption/Algorithm'

        * Encryption key size : value of 'Raven/Encryption/KeyBitsPreference'   
           
        ![Figure 3. Create New Database From Legacy Files - Encryption](images/new-database-from-legacy-3.png "Encryption")

5. **Data Exporter**
    * RavenDB 3.5 tool that can be found on [ravendb.net](http://ravendb.net/download) as a part of the tools package.
    
{PANEL/}



{NOTE: }
 Note: The backup will be restored only to the current node After restore, this database can be added to other nodes using the 'Manage group' button.
 Learn more about **Manage group** in : [Manage group](../../../database/settings/manage-database-group)  
{NOTE/}

## Related Articles

- [Create a Database : General Flow](general-flow)
- [Create a Database : Encrypted](encrypted)


