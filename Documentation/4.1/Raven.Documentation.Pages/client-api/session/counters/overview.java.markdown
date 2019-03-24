# Counters: Overview
---

{NOTE: }

* RavenDB's distributed counters, **Counters** for short, are numeric data variables that can be added to documents.  
  Use a Counter to count anything that needs counting, like:
   * Sold products  
   * Voting results  
   * Any event related to the document  

* Create and manage Counters using API methods, or through the [Studio](../../../studio/database/documents/document-view/additional-features/counters).  

* In this page:  
  * [Why use Counters?](../../../client-api/session/counters/overview#why-use-counters?)  
  * [Overview](../../../client-api/session/counters/overview#overview)  
  * [Managing Counters](../../../client-api/session/counters/overview#managing-counters)  
      * [Enabling the Counters Feature](../../../client-api/session/counters/overview#enabling-the-counters-feature)  
      * [Counter Methods and the `countersFor` object](../../../client-api/session/counters/overview#counter-methods-and-the--object)  
      * [Managing Counters using `operations`](../../../client-api/session/counters/overview#managing-counters-using-)
{NOTE/}

---

{PANEL: Why use Counters?}

* **Convenient counting mechanism**  
Counters are very easy to manage, using simple API methods or through the Studio.  
E.g. Use counters when you want to -  
  - Keep track of the number of times a document has been viewed or rated.  
  - Count how many visitors from certain countries or regions read a document.  
  - Continuously record the number of visitors on an event page.  
  - Avoid having to update the whole document for just a numeric value change.  
  - Have a need for a high-throughput counter (also see **Distributed Values** below).  

* **Distributed Values**  
A Counter's value is [distributed between cluster nodes](../../../client-api/session/counters/counters-in-clusters).  
Among the advantages of this:  

 * The cluster **remains available** even when nodes crash.  
 * Any node can provide or modify a Counter's value immediately, without checking or coordinating this with other nodes.  

* **High performance, Low resources**  
A document includes the Counter's _name_, while the Counter's actual _value_ is kept in a separate location.  
Modifying a Counter's value doesn't require the modification of the document itself.  
This results in a performant and uncostly operation.

* **High-frequency counting**  
Counters are especially useful when a very large number of counting operations is required,  
because of their speed and low resources usage.  
For example:  
  - Use Counters for an online election page, to continuously update a Number-Of-Votes Counter for each candidate.  
  - Continuously update Counters with the number of visitors in different sections of a big online store.  
{PANEL/}

{PANEL: Overview}

* **Design**  
  A document's metadata contains only the ***Counters' names-list*** for this document.  
  ***Counter Values*** are not kept in the document's metadata, but in a separate location.  
  * Therefore, changes like adding a new counter or deleting an existing counter trigger a document change,  
    while simply modifying the Counter Value does not.  

* **Cumulative Counter Actions**  
   - Counter value-modification actions are cumulative, the order in which they are executed doesn't matter.  
     E.g., It doesn't matter if a Counter has been incremented by 2 and then by 7, or by 7 first and then by 2.  
   - When a Counter is deleted, the sequence of Counter actions becomes non-cumulative and may require [special attention](../../../client-api/session/counters/counters-in-clusters#concurrent-delete-and-increment).  

* **Counters and conflicts**  
  * Counter actions (for either name or value) do not cause conflicts.  
      - Counter actions can be executed concurrently or in any order, without causing a conflict.  
      - You can successfully modify Counters while their document is being modified by a different client.  
  * Counters actions can still be performed when their related documents are in a conflicted state.  

* **Counters cost**  
  * Counters are designated to lower the cost of counting, but do come with a price.  
     * **All the names** of a document's Counters are added to its content, increasing its size.  
     * **Counter values** occupy storage space.  
  * Be aware that the negligible amount of resources required by a few Counters, 
    may become significant when there are many.  
    A single document with thousands of Counters is probably an indication of a modeling mistake, 
    for example.

---

* **Counter Naming Convention**  
    * Valid characters: All visible characters, [including Unicode symbols](../../../studio/database/documents/document-view/additional-features/counters#section)  
    * Length: Up to 512 bytes  
    * Encoding: UTF-8  

* **Counter Value**  
    * Valid range: Signed 64-bit integer (-9223372036854775808 to 9223372036854775807)  
    * Only integer additions are supported (no floats or other mathematical operations).

* **Number of Counters per document**  
    * RavenDB doesn't limit the number of Counters you can create.  
    * Note that the Counter names are stored in the document metadata and impact the size of the document.  

* **`HasCounters` Flag**  
    * When a Counter is added to a document, RavenDB automatically sets a `HasCounters` Flag in the document's metadata.  
    * When all Counters are removed from a document, the server automatically removes this flag.  
{PANEL/}

{PANEL: Managing Counters}

###Enabling the Counters Feature

* Counters management is currently an **experimental feature** of RavenDB, and is disabled by default.  

*  To enable this feature, follow these steps:  
  - Open the RavenDB server folder, e.g. `C:\Users\Dave\Downloads\RavenDB-4.1.1-windows-x64\Server`  
  - Open settings.json for editing.  
  - Enable the Experimental Features -  
    Verify that the json file contains the following line: **"Features.Availability": "Experimental"**  
  - Save settings.json, and restart RavenDB Server.  

---

###Counter Methods and the `countersFor` object

Managing Counters is performed using the `countersFor` Session object.  

*  **Counter methods**:  
  - `countersFor.increment`: Increment the value of an existing Counter, or create a new Counter if it doesn't exist.  
  - `countersFor.delete`: Delete a Counter.  
  - `countersFor.get`: Get the current value of a Counter.  
  - `countersFor.getAll`: Get _all_ the Counters of a document and their values.  

*  **Usage Flow**:  
  * Open a session.  
  * Create an instance of `countersFor`.  
      * Either pass `countersFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.load](../../../client-api/session/loading-entities#load).  
  * Use Counter methods to manage the document's Counters.  
  * If you execute [increment](../../../client-api/session/counters/create-or-modify) or [delete](../../../client-api/session/counters/delete), call `session.saveChanges` for the action to take effect on the server.  

*  **Success and Failure**:  
  - As long as the document exists, counter actions (increment, get, delete etc.) always succeed.
  - When a transaction that includes a Counter modification fails for any reason (e.g. a document concurrency conflict), 
    the Counter modification is reverted.

* **`countersFor` Usage Samples**  
  You can use `countersFor` by **explicitly passing it a document ID** (without pre-loading the document).  
  You can also use `countersFor` by passing it **the document object**.  
  {CODE-TABS}
  {CODE-TAB:java:Pass-CountersFor-Document-ID counters_region_CountersFor_without_document_load@ClientApi\Session\Counters\Counters.java /}
  {CODE-TAB:java:Pass-CountersFor-Document-Object counters_region_CountersFor_with_document_load@ClientApi\Session\Counters\Counters.java /}
  {CODE-TABS/}

---

### Managing Counters using `operations`

* In addition to working with the high-level Session, you can manage Counters using the low-level [Operations](../../../client-api/operations/what-are-operations).  

* [CounterBatchOperation](../../../client-api/operations/counters/counter-batch) 
can operate on a set of Counters of different documents in a single request.
{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

**Client-API - Session Articles**:  
[Creating and Modifying Counters](../../../client-api/session/counters/create-or-modify)  
[Deleting a Counter](../../../client-api/session/counters/delete)  
[Retrieving Counter Values](../../../client-api/session/counters/retrieve-counter-values)  
[Counters and other features](../../../client-api/session/counters/counters-and-other-features)  
[Counters In Clusters](../../../client-api/session/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
