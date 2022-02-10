# Monitoring: SNMP Support

---

{NOTE: }
{INFO: }
SNMP support is available for enterprise licenses only.  
{INFO/}

* This page explains how to use SNMP to monitor RavenDB, and what metrics can be accessed.  

* In this page:  
  * [Overview](../../../server/administration/snmp/snmp#overview)
  * [Enabling SNMP in RavenDB](../../../server/administration/snmp/snmp#enabling-snmp-in-ravendb)
      * [Configuration options](../../../server/administration/snmp/snmp#configuration-options)
  * [The Metrics](../../../server/administration/snmp/snmp#the-metrics)
      * [List of OIDs](../../../server/administration/snmp/snmp#list-of-oids)
      * [Templates](../../../server/administration/snmp/snmp#templates)

{NOTE/}

---

{PANEL: Overview}

Simple Network Management Protocol (SNMP) is an Internet-standard protocol for collecting and organizing 
information about managed devices on IP networks. It is used primarily for monitoring network services. 
SNMP exposes management data in the form of variables (metrics) which describe the system status and 
configuration. These metrics can then be remotely queried (and, in some circumstances, manipulated) by 
managing applications.  

In RavenDB we have support for SNMP which allows monitoring tools like [Zabbix](https://www.zabbix.com), 
[PRTG](https://www.paessler.com/prtg) and [Datadog](https://www.datadoghq.com/) direct access to the 
internal details of RavenDB. We expose a long list of metrics: CPU and memory usage, server total requests, 
the loaded databases, and also database specific metrics like the number of indexed items per second, 
document writes per second, storage space each database takes, and so on.  

You can still monitor what is going on with RavenDB directly from the Studio, or by using one of our 
monitoring tools. However, using SNMP might be easier in some cases. As users start running large numbers 
of RavenDB instances, it becomes impractical to deal with each of them individually, and using a monitoring 
system that can watch many servers becomes advisable.  
{PANEL/}

{PANEL: Enabling SNMP in RavenDB}

RavenDB is already configured to support SNMP. All you have to do is enable it and restart the server. 
This is done by adding the following key to your [settings.json](../../configuration/configuration-options#json) 
file:  

{CODE-BLOCK:json}
{
    ...
    "Monitoring.Snmp.Enabled": true
    ...
}
{CODE-BLOCK/}

### Configuration options

There are several configurable SNMP properties in RavenDB.  
#### For SNMPv1:  

* The SNMP port - default: `161`  
* List of supported SNMP versions - default: `"V2C;V3"`  

#### For SNMPv2c:  

* The community string - default: `"ravendb"`  

The community string is used like a password. It is sent with each SNMP `GET` request and allows or 
denies access to the monitored device.  

#### For SNMPv3:  

* Authentication protocol - default: `"SHA1"`  
* The user for authentication - default: `"ravendb"`  
* The authentication password - default: `null`; if set to `null` the community string is used instead.  
* Privacy protocol - default: `None`  
* Privacy password - default: `"ravendb"`  

You can change these properties with the following configuration keys:  
{CODE-BLOCK:json}
{
    ...
    "Monitoring.Snmp.Port": 12345,  
    "Monitoring.Snmp.SupportedVersions": "V#",  
    "Monitoring.Snmp.Community": "yourString",  
    "Monitoring.Snmp.AuthenticationProtocol": "protocol",  
    "Monitoring.Snmp.AuthenticationUser": "yourUser",  
    "Monitoring.Snmp.AuthenticationPassword": "yourString",  
    "Monitoring.Snmp.PrivacyProtocol": "protocol",  
    "Monitoring.Snmp.PrivacyPassword": "yourString"  
    ...
}
{CODE-BLOCK/}
{PANEL/}

{PANEL: The Metrics}

It is usually easy to query the exposed metrics using a monitoring tool. ([Example](./setup-zabbix)). 
However, you should also be able to access those directly using any SNMP agent like [Net-SNMP](http://net-snmp.sourceforge.net/). 
Each metric has a unique identifier (OID) and can be accessed individually.  

The most basic SNMP commands are `snmpget`, `snmpset` and `snmptrapd`.  
For example, using the SNMP agent you could run the following snmpget commands which get the server 
up-time metric.  

#### For SNMPv2c:  
{CODE-BLOCK:plain}
snmpget -v 2c -c ravendb live-test.ravendb.net 1.3.6.1.4.1.45751.1.1.1.3
{CODE-BLOCK/}

Where **ravendb** is the community string (configured via `Monitoring.Snmp.Community` configuration 
option) and "live-test.ravendb.net" is the host.  

#### For SNMPv3
{CODE-BLOCK:plain}
snmpget -v 3 -l authNoPriv -u ravendb -a SHA -A ravendb live-test.ravendb.net 1.3.6.1.4.1.45751.1.1.1.3
{CODE-BLOCK/}

`-l authNoPriv` sets the security level to use authentication but no privacy. `-u ravendb` sets the 
user for authentication purposes to "ravendb", `-a SHA` sets the authentication protocol to SHA, and 
`-A ravendb` sets the authentication password to "ravendb".  

**Example request for server URL, and the response:**  

{CODE-BLOCK:plain}
ml054@MARCIN-WIN:~$ snmpget -v 2c -c ravendb live-test.ravendb.net 1.3.6.1.4.1.45751.1.1.1.1.1
iso.3.6.1.4.1.45751.1.1.1.1.1 = STRING: "http://bf7631445baf:8080"
{CODE-BLOCK/}

{NOTE:Accessing OID value via HTTP}
Individual OID values can be retrieved via HTTP `GET` endpoint `<serverUrl>/monitoring/snmp?oid=<oid>`  

Example cURL request for the server up-time metric:  
{CODE-BLOCK:bash}
curl -X GET http://live-test.ravendb.net/monitoring/snmp?oid=1.3.6.1.4.1.45751.1.1.1.3

{"Value":"4.21:32:56.0700000"}
{CODE-BLOCK/}
{NOTE/}
<br/>
### List of OIDs

{NOTE: }
You can get a list of all OIDs along with their descriptions via HTTP `GET` endpoint `<serverUrl>/monitoring/snmp/oids`  
{NOTE/}

{NOTE: }
RavenDB's **root OID** is: **1.3.6.1.4.1.45751.1.1.**
{NOTE/}

| OID | Metric |
| --- | ------ |
| 1.1.1 | Server URL |
| 1.1.2 | Server Public URL |
| 1.1.3 | Server TCP URL |
| 1.1.4 | Server Public TCP URL |
| 1.2.1 | Server version |
| 1.2.2 | Server full version |
| 1.3 | Server up-time |
| 1.4 | Server process ID |
| 1.5.1 | Process CPU usage in % |
| 1.5.2 | Machine CPU usage in % |
| 1.5.3.1 | CPU Credits Base |
| 1.5.3.2 | CPU Credits Max |
| 1.5.3.3 | CPU Credits Remaining |
| 1.5.3.4 | CPU Credits Gained Per Second |
| 1.5.3.5 | CPU Credits Background Tasks Alert Raised |
| 1.5.3.6 | CPU Credits Failover Alert Raised |
| 1.5.3.7 | CPU Credits Any Alert Raised |
| 1.5.4 | IO wait in % |
| 1.6.1 | Server allocated memory in MB |
| 1.6.2 | Server low memory flag value |
| 1.6.3 | Server total swap size in MB |
| 1.6.4 | Server total swap usage in MB |
| 1.6.5 | Server working set swap usage in MB |
| 1.6.6 | Dirty Memory that is used by the scratch buffers in MB |
| 1.6.7 | Server managed memory size in MB |
| 1.6.8 | Server unmanaged memory size in MB |
| 1.6.9 | Server encryption buffers memory being in use in MB |
| 1.6.10 | Server encryption buffers memory being in pool in MB |
| 1.6.11.X.1 | Specifies if this is a compacting GC or not. |
| 1.6.11.X.2 | Specifies if this is a concurrent GC or not. |
| 1.6.11.X.3 | Gets the number of objects ready for finalization this GC observed. |
| 1.6.11.X.4 | Gets the total fragmentation (in MB) when the last garbage collection occurred. |
| 1.6.11.X.5 | Gets the generation this GC collected. |
| 1.6.11.X.6 | Gets the total heap size (in MB) when the last garbage collection occurred. |
| 1.6.11.X.7 | Gets the high memory load threshold (in MB) when the last garbage collection occurred. |
| 1.6.11.X.8 | The index of this GC. |
| 1.6.11.X.9 | Gets the memory load (in MB) when the last garbage collection occurred. |
| 1.6.11.X.10.1 | Gets the pause durations. First item in the array. |
| 1.6.11.X.10.2 | Gets the pause durations. Second item in the array. |
| 1.6.11.X.11 | Gets the pause time percentage in the GC so far. |
| 1.6.11.X.12 | Gets the number of pinned objects this GC observed. |
| 1.6.11.X.13 | Gets the promoted MB for this GC. |
| 1.6.11.X.14 | Gets the total available memory (in MB) for the garbage collector to use when the last garbage collection occurred. |
| 1.6.11.X.15 | Gets the total committed MB of the managed heap. |
| 1.7.1 | Number of concurrent requests |
| 1.7.2 | Total number of requests since server startup |
| 1.7.3 | Number of requests per second (one minute rate) |
| 1.7.4 | Average request time in milliseconds |
| 1.8 | Server last request time |
| 1.8.1 | Server last authorized non cluster admin request time |
| 1.9.1 | Server license type |
| 1.9.2 | Server license expiration date |
| 1.9.3 | Server license expiration left |
| 1.9.4 | Server license utilized CPU cores |
| 1.9.5 | Server license max CPU cores |
| 1.10.1 | Server storage used size in MB |
| 1.10.2 | Server storage total size in MB |
| 1.10.3 | Remaining server storage disk space in MB |
| 1.10.4 | Remaining server storage disk space in % |
| 1.11.1 | Server certificate expiration date |
| 1.11.2 | Server certificate expiration left |
| 1.11.3 | List of well known admin certificate thumbprints |
| 1.12.1 | Number of processor on the machine |
| 1.12.2 | Number of assigned processors on the machine |
| 1.13.1 | Number of backups currently running |
| 1.13.2 | Max number of backups that can run concurrently |
| 1.14.1 | Number of available worker threads in the thread pool |
| 1.14.2 | Number of available completion port threads in the thread pool |
| 1.15.1 | Number of active TCP connections |
| 3.1.1 | Current node tag |
| 3.1.2 | Current node state |
| 3.2.1 | Cluster term |
| 3.2.2 | Cluster index |
| 3.2.3 | Cluster ID |
| 5.2.X.1.1 | Database name |
| 5.2.X.1.2 | Number of indexes |
| 5.2.X.1.3 | Number of stale indexes |
| 5.2.X.1.4 | Number of documents |
| 5.2.X.1.5 | Number of revision documents |
| 5.2.X.1.6 | Number of attachments |
| 5.2.X.1.7 | Number of unique attachments |
| 5.2.X.1.10 | Number of alerts |
| 5.2.X.1.11 | Database ID |
| 5.2.X.1.12 | Database up-time |
| 5.2.X.1.13 | Indicates if database is loaded |
| 5.2.X.1.14 | Number of rehabs |
| 5.2.X.1.15 | Number of performance hints |
| 5.2.X.1.16 | Number of indexing errors |
| 5.2.X.2.1 | Documents storage allocated size in MB |
| 5.2.X.2.2 | Documents storage used size in MB |
| 5.2.X.2.3 | Index storage allocated size in MB |
| 5.2.X.2.4 | Index storage used size in MB |
| 5.2.X.2.5 | Total storage size in MB |
| 5.2.X.2.6 | Remaining storage disk space in MB |
| 5.2.X.3.1 | Number of document puts per second (one minute rate) |
| 5.2.X.3.2 | Number of indexed documents per second for map indexes (one minute rate) |
| 5.2.X.3.3 | Number of maps per second for map-reduce indexes (one minute rate) |
| 5.2.X.3.4 | Number of reduces per second for map-reduce indexes (one minute rate) |
| 5.2.X.3.5 | Number of requests per second (one minute rate) |
| 5.2.X.3.6 | Number of requests from database start |
| 5.2.X.3.7 | Average request time in milliseconds |
| 5.2.X.5.1 | Number of indexes |
| 5.2.X.5.2 | Number of static indexes |
| 5.2.X.5.3 | Number of auto indexes |
| 5.2.X.5.4 | Number of idle indexes |
| 5.2.X.5.5 | Number of disabled indexes |
| 5.2.X.5.6 | Number of error indexes |
| 5.2.X.6.1 | Number of writes (documents, attachments, counters) |
| 5.2.X.6.2 | Number of bytes written (documents, attachments, counters) |
| 5.2.X.4.Y.1 | Indicates if index exists |
| 5.2.X.4.Y.2 | Index name |
| 5.2.X.4.Y.4 | Index priority |
| 5.2.X.4.Y.5 | Index state |
| 5.2.X.4.Y.6 | Number of index errors |
| 5.2.X.4.Y.7 | Last query time |
| 5.2.X.4.Y.8 | Index indexing time |
| 5.2.X.4.Y.9 | Time since last query |
| 5.2.X.4.Y.10 | Time since last indexing |
| 5.2.X.4.Y.11 | Index lock mode |
| 5.2.X.4.Y.12 | Indicates if index is invalid |
| 5.2.X.4.Y.13 | Index status |
| 5.2.X.4.Y.14 | Number of maps per second (one minute rate) |
| 5.2.X.4.Y.15 | Number of reduces per second (one minute rate) |
| 5.2.X.4.Y.16 | Index type |
| 5.1.1 | Number of all databases |
| 5.1.2 | Number of loaded databases |
| 5.1.3 | Time since oldest backup |
| 5.1.4 | Number of disabled databases |
| 5.1.5 | Number of encrypted databases |
| 5.1.6 | Number of databases for current node |
| 5.1.7.1 | Number of indexes in all loaded databases |
| 5.1.7.2 | Number of stale indexes in all loaded databases |
| 5.1.7.3 | Number of error indexes in all loaded databases |
| 5.1.8.1 | Number of indexed documents per second for map indexes (one minute rate) in all loaded databases |
| 5.1.8.2 | Number of maps per second for map-reduce indexes (one minute rate) in all loaded databases |
| 5.1.8.3 | Number of reduces per second for map-reduce indexes (one minute rate) in all loaded databases |
| 5.1.9.1 | Number of writes (documents, attachments, counters) in all loaded databases |
| 5.1.9.2 | Number of bytes written (documents, attachments, counters) in all loaded databases |
| 5.1.10 | Number of faulted databases |

### Templates

For easier setup we have prepared a few templates for monitoring tools which can be found [here](https://github.com/ravendb/ravendb/tree/v4.0/src/Raven.Server/Monitoring/Snmp/Templates).   
These templates include the metrics and their associated OIDs.
{PANEL/}

## Related Articles

- [Monitoring: How to setup Zabbix monitoring](./setup-zabbix)
