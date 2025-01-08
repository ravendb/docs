# Storage: Directory Structure

---

{NOTE: }

* RavenDB keeps all data under the location specified by the [DataDir](../../server/configuration/core-configuration#datadir) configuration value.

* In this page:
    * [On-disk Data Structure](../../server/storage/directory-structure#on-disk-data-structure)
    * [Voron Storage Environment](../../server/storage/directory-structure#voron-storage-environment)
{NOTE/}

---

{PANEL: On-disk Data Structure}

* The on-disk structure of RavenDB data directories is as follows:

{NOTE: }

* __<data-dir>__
  * `Databases`
      * __<database-name>__
          * `Confguration`
              * `Journals`
              * `Temp`
              * Raven.voron
          * `Indexes`
              * __<index-name>__
                  * `Journals`
                  * `Temp`
                  * Raven.voron
              * __...more indexes__
          * `Journals`
          * `Temp`
          * Raven.voron
      * __...more databases__
  * `System`
      * `Journals`
      * `Temp`
      * Raven.voron

{NOTE/}

* The main <data-dir> contains:

  *  **Databases folder**  
     Contains subdirectories with data per database.  
  *  **System folder**  
     Stores cluster data & server-wide data such as shared resources needed by all cluster nodes,  
     e.g. the Database Record.  

* The System folder, the Database folders, and their inner folders (the Configuration folder and each Index folder) are each a separate **Voron storage environment**.

{PANEL/}

{PANEL: Voron Storage Environment}

* Each Voron storage environment is composed of:  

{NOTE: }
__Temp Folder__

* Holds temporary scratch & compression <em>*.buffers</em> files.  
* These are small memory-mapped files that keep separate data versions for concurrent running transactions.  
* Data modified by a transaction is copied into the scratch space - modifications are made on a copy of the data.  
* Compression files are used to compress the transaction data just before writing it to the journal.  
* When a transaction is written from these files to the journal file it is considered committed.
{NOTE /}

{NOTE: }
__Journals Folder__  

* Contains Write-Ahead Journal files (WAJ) that are used in the hot path of a transaction commit.  
* From the journals, transactions will be flushed to the Voron data file where they are persisted.  

---

* Entries written to these files use unbuffered sequential writes and direct I/O to ensure a direct write that bypasses all caches.  
* Writes to the journals happen immediately, have high priority, and take precedence over writes to the Voron data file.  

---

* Data is cleared from the journals once it is successfully stored in the Voron data file.  
* The journal's transactions take part in database recovery - info is used to recover up to the same point you were at before failure.  
{NOTE /}

{NOTE: }
__Raven.voron file__

* This file contains the persisted data on disk.  
* It is a memory-mapped data file using buffered writes with random reads and writes.
{NOTE /}
 
{PANEL/}

## Related Articles

### Storage

- [Customize Data Location](../../server/storage/customizing-raven-data-files-locations)
- [Storage Engine](../../server/storage/storage-engine)
