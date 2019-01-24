#Counters
---

{NOTE: }

* Counters are numeric data variables that can be added to a document.  
  Use a Counter to count anything that needs counting, such as:
   * Products sales  
   * Voting results  
   * Any event related to the document  

* Create and manage Counters using [API methods](../../../../../client-api/session/counters/overview) or through the Studio.  

* In this page:  
  * [Viewing Counters Data](../../../../../studio/database/documents/document-view/additional-features/counters#viewing-counters-data)  
  * [Managing Counters](../../../../../studio/database/documents/document-view/additional-features/counters#managing-counters)  
  * [Counters and ongoing-tasks](../../../../../studio/database/documents/document-view/additional-features/counters#counters-and-ongoing-tasks)  
  * [Counters Export and Import](../../../../../studio/database/documents/document-view/additional-features/counters#counters-export-and-import)  
{NOTE/}

---

{PANEL:Viewing Counters Data}

**To view counters data, open the document they belong to.**

**1.** Choose a database from the databases list:  
{NOTE: }
![Figure 1. Choose a database](images/counters-1-DBs-list.png)
{NOTE/}

**2.** The Counters Flag indicates which documents currently have counters.  
{NOTE: }
![Figure 2. A documents' Counters flag](images/counters-2-documents-list-counters-flag.png)
{NOTE/}

**3.** Open a document.  
{NOTE: }
![Figure 3. Choose a document whose counters you want to review or manage](images/counters-3-documents-list.png)
{NOTE/}

**4.** Counters' details are located in the Counters Tab.  
{NOTE: }
####    

![Figure 4. A document's Counters information](images/counters-4-document-view.png)
<br/>

1. `HasCounters` **Flag**  
    This flag in the document's metadata, is **automatically** set or removed by the server,  
    indicating whether a document has Counters.  
2. **Counters-Tab Header**  
   Click the header to open the Counters tab.  
   The current number of Counters is stated on the header.  
3. **Counter Name**  
   You can use whatever characters you choose, including Unicode symbols.  
4. **Counter Value**  
{NOTE/}
{PANEL/}

{PANEL:Managing Counters}

{NOTE: }

**Add, Modify or Remove Counters from the Counters Tab.**

![Figure 5. Counter management controls](images/counters-5-management-controls.png)
{NOTE/}

{NOTE: 1. Click "Add Counter" to create a new Counter.}

![Figure 6. Add a new Counter](images/counters-6-add-new-counter.png)  
{NOTE/}

{NOTE: 2. Click the Edit icon to modify a Counter's value.}

![Figure 7. Modify Counter value](images/counters-7-modify-counter-value.png)  

* 
  **A.** Counter's name - Note: Once set, the name cannot be modified.  
  **B.** Current value - The **accumulated value from all nodes**.  
  **C.** Enter a new value here. This value is the acculated value, see note below.  
  **D.** Confirm to carry out the update or Cancel.  
  **E.** The Counter's value **per cluster node**.  
  **F.** The node's e-tag.  


* **Note**:  
  Modifying a Counter's value sets its **accumulated value**.  
  For example, in the above image, the Counter's value is 348 in Node A, 310 in node B, and (-25) in node C,  
  making its accumulated value **633**.  
  Setting the new value to 700 from node A, will change its **accumulated value** to 700 by setting its value in node A to **415**.  

* A Counter value can be negative.  

{NOTE/}

{NOTE: 3. Click the Delete icon to remove a counter.}
![Figure 8. Delete a Counter](images/counters-8-delete-counter.png)
{NOTE/}
{PANEL/}

{PANEL:Counters and Ongoing Tasks}

 * There are [Ongoing task](../../../../../studio/database/tasks/ongoing-tasks/general-info) that [relate to Counters](../../../../../client-api/session/counters/counters-and-other-features#counters-and-ongoing-tasks) and can be configured using the Studio:  
   * [Backup](../../../../../studio/database/tasks/ongoing-tasks/backup-task)  
     * All data, including Counters and their values, is stored and restored automatically.  
       There's no need to handle Counters in particular.  
     * **Trigger**: Time Schedule  
                    Full/Partial backup is invoked by a pre-set time routine.  
     * **Counter value**: Distributed.  
                          Counters are stored and restored along with their values on all nodes. 

   * [External Replication](../../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
     * The ongoing External Replication task replicates all data automatically, including Counters.  
       **Triggers**: Document Change,
                     Counter Value Modification  
       **Counter value**: Distributed.
                          Counters are replicated along with their values on all nodes. 

   * [RavenDB ETL](../../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
     Use [ETL](../../../server/ongoing-tasks/etl/basics) to export data from a Raven database to an external (either Raven or SQL) database.  
     * [SQL ETL](../../../server/ongoing-tasks/etl/sql) - **Not supported**  
                Counters are not exported to SQL databases over SQL ETL.  
     * [RavenDB ETL](../../../server/ongoing-tasks/etl/raven) - **Supported**  
         * Counters [are](../../../server/ongoing-tasks/etl/raven#counters) exported over RavenDB ETL.  
         * Default behavior: When an ETL script is not provided, Counters **are** exported.  
         * **Triggers**: Document Change,
                         Counter Value Modification  
         * **Counter value**: Distributed.
                              Counters are exported along with their values on all nodes. 

   * [Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscriptions)  
       * **Trigger**: Document Change  
                      Data Subscriptions are **not** triggered by Counter Value modifications.  

{PANEL/}

{PANEL:Counters Export and Import}

* Use the "Export" option to export selected database components into a file, for backup or transit.  
  Use the "Import" option to restore exported components when required.  
   * Please note that a Counter can be imported only if the Counter's document exists.  
       * E.g., a Counter that's been created for a document named "products/1-C" and then exported to a file, can be imported only if "products/1-C" still exist.  

{NOTE: }

**Export Database with Counters.**

![Figure 9. Export DB with Counters](images/counters-9-export.png)

* 
  **1.** Click the Settings gear.  
  **2.** Click Export Database.  
  **3.** Use the default name or enter your own.  
  **4.** Select the components you wish to export. Make sure Counters are included.  
  **5.** Click to export the database.  

{NOTE/}

{NOTE: }

**Import Database with Counters.**

![Figure 10. Import DB with Counters](images/counters-10-import.png)

* 
  **1.** Click the Settings gear.  
  **2.** Click Import Data.  
  **3.** Select a file you wish to import from, or another source.  
  **4.** Select the components you wish to import. Make sure Counters are included.  
  **5.** Click to import the database.  

{NOTE/}

{PANEL/}


## Related articles
**Client API Articles**:  
[Counters Overview (Session Usage)](../../../../../client-api/session/counters/overview)  
[Counters Operations](../../../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
