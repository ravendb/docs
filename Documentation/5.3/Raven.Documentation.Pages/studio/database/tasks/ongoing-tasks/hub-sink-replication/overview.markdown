# Hub/Sink Replication: Overview

---

{NOTE: }

Hub/Sink replication is used to maintain a live replica of a database 
or a chosen part of it, through a secure connection between ongoing Hub 
and Sink replication tasks.  

* A Hub can be used by many Sinks.  

* A database can define multiple Sinks in it, each for a different hub.  

* The connection is always initiated by the Sink, but you can choose 
  whether to replicate data from *Hub to Sink* and/or from *Sink to Hub*.  

* Replication can be [filtered](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication) 
  by both the Hub and the Sink, and for both *incoming* and *outgoing* documents, using wildcards and document IDs, to select the 
  documents that would be replicated.  

* The connection between Hub and Sink tasks is [secure](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#accesses-and-certificates), 
  requiring a secure RavenDB cluster setup.  
  A secure cluster can be raised using RavenDB's [setup wizard](../../../../../start/installation/setup-wizard#secure-setup-with-a-let).  

* In this page:
    * [What is Hub/Sink Replication for?](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#what-is-hub/sink-replication-for?)  
    * [What is and is not replicated?](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#what-is-and-is-not-replicated?)  
    * [Accesses and Certificates](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#accesses-and-certificates)  
    * [Filtered Replication](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication)  
    * [What does the replication include?](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#what-does-the-replication-include?)  
    * [Failover](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#failover)  
    * [Backward Compatibility](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#backward-compatibility)  
{NOTE/}

---

{PANEL: What is Hub/Sink Replication for?}

* **Flexible Connectivity**  
  The connection between Hub and Sink doesn't have to be continuous 
  and can be initiated by the Sink at will.  
  This can be very helpful when the Sink instance tends to go offline, e.g. -  

   * Using a cellular network that may not always have connectivity  
   * Using a laptop in a location with no WiFi
   * Onboard ships that sail offshore and offline  
   * in a security installation that limits its communication time 
     with the outside world  
   * In a remote settlement that surfaces online every now and then  
   
     in the ship's case, for example, the Sink can initiate a connection 
     with an onshore Hub to replicate data, whenever it returns online.  

* **Many to One**  
  The Hub can serve many Sinks using the same port, **simplify 
  security protocols** and **save server resources**.  

    This can be useful whenever multiple devices (e.g. an array of computers 
    onboard a fleet of trucks) collect their records locally and replicate them 
    periodically to a central database.  

{PANEL/}


{PANEL: What is and is not replicated?}

**What is being replicated:**  

  * All database documents and related data:  
    * [Attachments](../../../../../document-extensions/attachments/what-are-attachments)  
    * [Revisions](../../../../../document-extensions/revisions/overview)  
    * [Counters](../../../../../document-extensions/counters/overview)  
    * [Time Series](../../../../../document-extensions/timeseries/overview)  

**What is _not_ being replicated:**  

  * Server and cluster level features:  
    * [Indexes](../../../../../indexes/creating-and-deploying)  
    * [Conflict resolver definitions](../../../../../server/clustering/replication/replication-conflicts#conflict-resolution-script)  
    * [Compare-Exchange](../../../../../client-api/operations/compare-exchange/overview)  
    * [Subscriptions](../../../../../client-api/data-subscriptions/what-are-data-subscriptions)
    * [Identities](../../../../../server/kb/document-identifier-generation#strategy--3)  
    * Ongoing tasks
      * [ETL](../../../../../server/ongoing-tasks/etl/basics)
      * [Backup](../../../../../studio/database/tasks/backup-task)
      * [Hub/Sink Replication](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)

{NOTE: Why are some cluster-level features not replicated?}
To provide for architecture that prevents conflicts between clusters, especially when ACID transactions are important, 
RavenDB is designed so that data ownership is at the cluster level.  
To learn more, see [Data Ownership in a Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).

It is also best to ensure that each cluster defines policies, configurations, and ongoing tasks that are relevant for it.  
{NOTE/}

{PANEL/}


{PANEL: Accesses and Certificates}

The connection between Hub and Sink tasks is validated using public-key cryptography.  

To access the Hub or Sink Task Studio interface:  

a. Open the **Databases** view in the source server.  
b. Select the database where the task will be active.  
c. Click **Tasks** tab.  
d. Select **Ongoing Tasks**  
e. Click **Add a database task**  
f. Click **Replication Hub** or **Replication Sink**.  
g. Toggle desired configurations and click **Add Access** to view the following interface:

![Certificates](images/certificates-hub-sink-replication.png "Certificates")

Select existing access or define a new sink/hub, then click **Save** to create a new replication access with the certificate interface.  


* **Creating Accesses**  
  When you create or edit a Hub task, you can add it [Accesses](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task#access-configuration).  
  Each Access can be used by a Sink task to connect the Hub.  
  You are required to issue a certificate for each Access, that would 
  identify Sinks that connect the Hub via this Access.  

* **Issuing Certificates**  
  * You can issue a certificate from a few sources:  
     * **Create a new certificate** in the Hub task creation page.  
     * **Import and reuse** the certificate that is already used 
       by the server to validate client access.  
     * **Provide a certificate** from any other source.  
  
* **Export from Hub, Import by Sink**  
  After using the Hub task creation page to issue the certificate, 
  you need to export the certificate to a file.  
  When you create a Sink task, you are given the option to load 
  the file and provide the Sink with the certificate it would access 
  the hub with.  

{PANEL/}

{PANEL: Filtered Replication}

Filtered Replication allows you to choose the documents that would 
be replicated. Replicating only a selected part of your database 
can be useful in numerous cases, e.g. -  

* **To keep documents confidentiality**  
  When an organization whose main database serves multiple departments, 
  facilities or branches use Filtered Replication to keep documents 
  confidentiality.  
  The central database of a healthcare network, for example, can protect 
  patients' privacy by updating each clinic's database instance only with 
  records of patients treated by this clinic.  

* **As an additional security measure**  
  A central database that an array of traffic enforcement cameras replicate
  speed tickets to, for example, can use Filtered Replication to restrict
  each camera's replication to just tickets generated by this camera.  
  This will prevent a potential hacker from replicating a stolen camera's
  speed tickets as if they were collected by another camera (e.g. to overrun
  a ticket collected by any chosen camera, with a blank one).  

To access the Hub or Sink Task Studio interface:  

a. Open the **Databases** view in the source server.  
b. Select the database where the task will be active.  
c. Click **Tasks** tab.  
d. Select **Ongoing Tasks**  
e. Click **Add a database task**  
f. Click **Replication Hub** or **Replication Sink**.  
g. Toggle desired configurations and click **Add Access** to view the following interface:

![Filtered Replication](images/filtering-hub-sink-replication.png "Filtered Replication")

Select existing access or define a new sink/hub, then click **Save** to create a new replication access where filtration is defined.  


{INFO: }

* **Both the Hub and the Sink can filter data**  
   Only documents matching the filters defined by both will be replicated.  
* **Documents are selected by path**  
  Paths can include wildcards (`companies/*`) and 
  exact document IDs (`companies/88-A`).  
* ***Read* and *Write* filtering**  
  You can further increase filtering resolution, by 
  defining separate lists of allowed paths for **incoming** 
  and **outgoing** documents.  

{INFO/}

{PANEL/}

{PANEL: What does the replication include?}

Documents are replicated along with all their properties, including 
[Time Series](../../../../../document-extensions/timeseries/overview), 
[counters](../../../../../document-extensions/counters/overview), 
[attachments](../../../../../document-extensions/attachments/what-are-attachments) 
and [revisions](../../../../../document-extensions/revisions/overview). 

{PANEL/}

{PANEL: Failover}
Read about replication tasks Failover [Here](../../../../../server/ongoing-tasks/hub-sink-replication#failover).  
{PANEL/}

{PANEL: Backward-Compatibility}
Read about Hub/Sink Replication backward compatibility with Pull Replication [Here](../../../../../server/ongoing-tasks/hub-sink-replication#backward-compatibility).  
{PANEL/}

## Related Articles

**Studio Articles**:   

- [Replication Hub Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
- [Replication Sink Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

**Server Articles**:  

- [Hub/Sink Replication (includes code samples)](../../../../../server/ongoing-tasks/hub-sink-replication)  
- [External Replication](../../../../../server/ongoing-tasks/external-replication)  
- [Client Certificate Usage](../../../../../server/security/authentication/client-certificate-usage)  

**Client Articles**:

- [Adding a Connection String](../../../../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  


