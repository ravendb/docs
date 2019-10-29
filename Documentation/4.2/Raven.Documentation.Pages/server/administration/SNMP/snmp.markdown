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

The most basic SNMP commands are `snmpget`, `snmpset` and `snmptrapd` ([Read more](http://net-snmp.sourceforge.net/tutorial/tutorial-5/commands/)). 
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

![](images/monitoring-zabbix-snmpget.PNG)  

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

RavenDB's **root OID** is: **1.3.6.1.4.1.45751.1.1.**

| OID | Metric |
| --- | ------ |
| 1.1. | Server |
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
| 1.6.1 | Server allocated memory in MB |
| 1.7.1 | Number of concurrent requests |
| 1.7.2 | Total number of requests since server startup |
| 1.7.3 | Number of requests per second (one minute rate) |
| 1.8 | Server last request time |
| 1.9.1 | Server license type |
| 1.9.2 | Server license expiration date |
| 1.9.3 | Server license expiration left |
| 3.1.1 | Current node tag |
| 3.1.2 | Current node state |
| 3.2.1 | Cluster term |
| 3.2.2 | Cluster index |
| 3.2.3 | Cluster ID |
| 5.1.1 | Number of all databases |
| 5.1.2 | Number of loaded databases |
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
| 5.2.X.1.16 | Number of indexes errors per database |
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
| 5.2.X.5.1 | Number of indexes |
| 5.2.X.5.2 | Number of static indexes |
| 5.2.X.5.3 | Number of auto indexes |
| 5.2.X.5.4 | Number of idle indexes |
| 5.2.X.5.5 | Number of disabled indexes |
| 5.2.X.5.6 | Number of error indexes |
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

### Templates

For easier setup we have prepared a few templates for monitoring tools which can be found [here](https://github.com/ravendb/ravendb/tree/v4.0/src/Raven.Server/Monitoring/Snmp/Templates).   
These templates include the metrics and their associated OIDs.
{PANEL/}

## Related Articles

- [Monitoring: How to setup Zabbix monitoring](./setup-zabbix)
