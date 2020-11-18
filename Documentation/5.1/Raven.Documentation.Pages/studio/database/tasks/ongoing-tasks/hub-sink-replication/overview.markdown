# Hub/Sink Replication: Overview

---

{NOTE: }

Hub/Sink replication is used to maintain a live replica of a database 
or a chosen part of it, through a secure connection between ongoing Hub 
and Sink replication tasks.  

* Each Sink can access a single Hub.  
  A Hub can access many Sinks.  

* The connection is always initiated by the Sink, but you can choose 
  whether to replicate data from *Hub to Sink* and/or from *Sink to Hub*.  

* Replication can be [filtered](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication) 
  by both Hub and Sink, using wildcards and document IDs, to select the 
  documents that would be replicated.  

* The connection between Hub and Sink tasks is [secure](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#certificates), 
  requiring a secure RavenDB cluster setup.  
  A secure cluster can be raised easily using RavenDB's [setup wizard](../../../../../start/installation/setup-wizard#secure-setup-with-a-let).  

* In this page:
    * [What is Hub/Sink Replication for?](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#what-is-hub/sink-replication-for?)  
    * [Filtered Replication](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication)  
    * [Accesses and Certificates](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#accesses-and-certificates)  
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

   * Onboard ships that sail offshore and offline  
   * in a security installation that limits its communication time 
     with the outside world  
   * In a remote settlement that surfaces online every now and then  
   
     in the ship's case, for example, the Sink can initiate a connection 
     with an onshore Hub to replicate data, whenever it returns online.  

* **Many to One**  
  The Hub can serve and be served by many Sinks using the same 
  port, **simplify security protocols** and **save server resources**.  

    This can be useful whenever multiple devices (e.g. an array of 
    security cameras) collect their records locally and replicate 
    them to a central database periodically.  

{PANEL/}

{PANEL: Filtered Replication}

Filtered Replication allows you to choose the documents that would 
be replicated. Replicating only a selected part of your database 
can be useful in numerous cases, e.g. -  

* **To keep documents confidentiality**  
  When an organization whose main database serves multiple departments, 
  facilities or branches use Filtered Replication to keep documents 
  confidentiality.  
  The central database of a health-care network, for example, can protect 
  patients privacy by updating each clinic's database instance only with 
  records of patients treated by this clinic.  

* **As an additional security measure**  
  A central database that an array of traffic enforcement cameras replicate 
  speed tickets to, for example, can use Filtered Replication to restrict 
  each camera's replication to a single collection.  
  This will prevent a potential hacker from replicating a stolen camera's 
  speed tickets to any other collection (e.g. to overrun existing tickets 
  with blank ones).  

{INFO: }

* **Both Hub and Sink tasks can filter data.**  
   Only documents defined by both tasks will be replicated.  
* **Documents are selected by path.**  
   * A path can include wildcards ('companies/*') and 
     document IDs ('companies/88-A').  
   * Each task (Hub and Sink) can define separate paths 
     for incoming and outgoing replication.  
{INFO/}

{PANEL/}

{PANEL: Accesses and Certificates}

The connection between Hub and Sink tasks is validated using public-key cryptography.  

* **Creating Accesses**  
  When you create or edit a Hub task, you can add it [Accesses](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task#access-configuration).  
  Each Access can be used by Sink tasks to connect the Hub.  
  You are required to issue a certificate for each Access, that contains the 
  Hub's Public key and a Private key for Sink tasks that wish to connect the 
  Hub via this Access.  

* **Issuing Certificates**  
  * You can issue a certificate from a few sources:  
     * **Create a new certificate** in the Hub task creation page.  
     * **Import and reuse** the certificate that is already used 
       by the server to validate client access.  
     * **Provide a certificate** from any other source.  
  
* **Export from Hub, Import by Sink**  
  After using the Hub task creation page to issue the certificate, 
  you need to export the certificate to a file. Any Sink task that 
  wishes to connect this Hub and Access, will import the file and 
  keep the Hub's public key and its own private key.  
  After the export, the Hub task will keep only its public key.  

{PANEL/}

{PANEL: What does the replication include?}

Documents are replicated along with all their properties, including 
[Time Series](../../../../../document-extensions/timeseries/overview), 
[counters](../../../../../document-extensions/counters/overview), 
[attachments](../../../../../document-extensions/attachments/what-are-attachments) 
and [revisions](../../../../../server/extensions/revisions).  

{PANEL/}

{PANEL: Failover}
Read about replication tasks Failover [Here](../../../../../server/ongoing-tasks/hub-sink-replication#failover).  
{PANEL/}

{PANEL: Backward-Compatibility}
Read about Hub/Sink Replication backward compatibility with Pull Replication [Here](../../../../../server/ongoing-tasks/hub-sink-replication#backward-compatibility).  
{PANEL/}

## Related Articles

**Studio Articles**:   
[Replication Hub Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
[Replication Sink Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

**Server Articles**:  
[External Replication](../../../../../server/ongoing-tasks/external-replication)  
[Client Certificate Usage](../../../../../server/security/authentication/client-certificate-usage)  

**Client Articles**:  
[Adding a Connection String](../../../../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  


