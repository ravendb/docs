# Monitoring : SNMP Support

Simple Network Management Protocol (SNMP) is an Internet-standard protocol for collecting and organizing information 
about managed devices on IP networks, and is used primarily for monitoring network services. SNMP exposes management 
data in the form of variables which describe the system status and configuration. These variables can then be 
remotely queried (and, in some circumstances, manipulated) by managing applications.

In RavenDB 3.5 we added support for SNMP. Using SNMP allows monitoring tools like [Zabbix](./setup-zabbix), OpenView and MS MOM direct 
access to the internal details of RavenDB. We expose a long list of metrics: the loaded databases, the number 
of indexed items per second, the ingest rate, the number of queries or how much storage space each database takes and 
so on.

You can still monitor what is going on with RavenDB directly from the studio, or by using one of our monitoring tools 
but using SNMP might be easier in some cases. As RavenDB users start running large numbers of RavenDB instances, it 
becomes unpractical to deal with each of them individually and using a monitoring system that can watch many servers 
is preferable.

## Related articles

- [Monitoring: How to setup Zabbix monitoring](./setup-zabbix)