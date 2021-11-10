# TCP Compression
---

{NOTE: }

* With **TCP Compression** enabled, internal cluster data transfers 
  are compressed, including nodes' 
  [database replication](../server/clustering/replication/replication)
  and the data submitted by 
  [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions) 
  to their workers.  

* Especially on the cloud, the significant reduction in the amount of transferred 
  data translates to significant reduction of costs.  
  

* In this page:  
  * [TCP Compression](../server/tcp-compression#tcp-compression)  
  * [Compression Algorythm and Ratio](../server/tcp-compression#compression-algorythm-and-ratio)  
  * [Enabling TCP Compression](../server/tcp-compression#enabling-tcp-compression)  

{NOTE/}

---

{PANEL: TCP Compression}

RavenDB offers a variety of compression tools, including the compression 
of [stored documents](../server/storage/documents-compression) and 
[backups](../server/ongoing-tasks/backup-overview#compression).  

With the **TCP compression** feature enabled, data **in transit** is compressed as well.  

* RavenDB will compress data before [replicating](../server/clustering/replication/replication) 
  it from one cluster node to another.  
  Replication makes a large portion of a cluster's traffic, and compressing 
  replicated data will minimize the traffic volume and expedite data delivery.  

* [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions) 
  will also transfer compressed data to their workers.  
  Data subscriptions are used to automate documents processing, 
  and may transfer large quantities of documents on a regular basis. 
  Compressing the data they submit can reduce the traffic volume 
  and the costs of such automated routines considerably.  

## Compression Algorythm and Ratio

* TCP Compression uses the [Zstandard compression algorithm](https://en.wikipedia.org/wiki/Zstandard), 
  continuously learning your data model to create dictionaries that represent the redundant 
  structural data across transferred documents.  

* Compression ratio tends to assend as the size of the transferred data grows, 
  and may **top 85%** for big data transfers.  

## Enabling TCP Compression

Your server's [license](../studio/server/license-management) 
type determines whether TCP compression, among other features, 
is activated on it. No additional configuration is needed 
to enable this feature.  

![License (Studio View)](images/tcp-compression-license.png "License (Studio View)")

{PANEL/}

## Related Articles

### Studio

- [Documents Compression](../studio/database/settings/documents-compression)  
- [License](../studio/server/license-management)

### Server

- [Backup Compression](../server/ongoing-tasks/backup-overview#compression)  
- [Database Replication](../server/clustering/replication/replication)  

### Client API

- [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions)  
