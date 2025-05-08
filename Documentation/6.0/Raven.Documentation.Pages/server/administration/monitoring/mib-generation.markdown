# Monitoring: MIB generation and usage

---

{NOTE: }

RavenDB allows you to generate a MIB (Management Information Base) file that contains 
a structured collection of SNMP OIDs.  

The created MIB can be used by monitoring tools to extract RavenDB metrics via SNMP.  

* In this page:  
   * [Generating a MIB](../../../server/administration/monitoring/mib-generation#generating-a-mib)  
      * [MIB generation endpoint](../../../server/administration/monitoring/mib-generation#mib-generation-endpoint)  
      * [Fine-tune the OIDs list](../../../server/administration/monitoring/mib-generation#fine-tune-the-oids-list)  

{NOTE/}

---

{PANEL: Generating a MIB}

### MIB generation endpoint

To generate a MIB, use RavenDB's HTTP `/monitoring/snmp/mib` GET endpoint.  
You can inspect this endpoint using your browser to download a text file with 
RavenDB's OIDs, or connect it with a monitoring too to utilize these OIDs.  

The endpoint's path is added to RavenDB's address, including its port number.  

- To connect a local RavenDB setup, for example, and generate the MIB, use:  
  `http://localhost:8080/monitoring/snmp/mib`  
- Or to generate a MIB for RavenDB's live test server, use:  
  [http://live-test.ravendb.net/monitoring/snmp/mib](http://live-test.ravendb.net/monitoring/snmp/mib)  

---

### Fine-tune the OIDs list

By default, the MIB includes **server** metrics OIDs. You can fine-tune 
it to include the OIDs range your are interested in. Available options are:  

* `includeServer` - Include or exclude OIDs with **server** metrics.  
* `includeCluster` - Include or exclude OIDs with **cluster** metrics.  
* `includeDatabases` - Include or exclude OIDs with **databases** metrics.  

#### Examples:
To include **databases** metrics OIDs, for example, you can use the `includeDatabases` flag this way:  
[http://live-test.ravendb.net/monitoring/snmp/mib?includeDatabases=true](http://live-test.ravendb.net/monitoring/snmp/mib?includeDatabases=true)  
Or to exclude **server** metrics OIDs use the **includeServer** flag like so:  
[http://live-test.ravendb.net/monitoring/snmp/mib?includeServer=false](http://live-test.ravendb.net/monitoring/snmp/mib?includeServer=false)  

{PANEL/}

## Related Articles

### Monitoring
- [Prometheus](../../../server/administration/monitoring/prometheus)  
- [Telegraf Plugin](../../../server/administration/monitoring/telegraf)  

### Administration
- [SNMP Administration](../../../server/administration/SNMP/snmp)  
- [Zabbix](../../../server/administration/SNMP/setup-zabbix)  

### Integrations
- [PostgreSQL Overview](../../../integrations/postgresql-protocol/overview)  
- [Power BI](../../../integrations/postgresql-protocol/power-bi)  
