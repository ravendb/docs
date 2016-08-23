# Monitoring : How to setup Zabbix monitoring

RavenDB 3.5 has built-in support for [SNNP](./snmp), which means that with a few quick steps you can monitor your server using Zabbix.

### Zabbix installation

Before you setup Zabbix to monitor RavenDb make sure you have it up and running. If you haven't done so already, you should [read about Zabbix](https://www.zabbix.com/documentation/3.0/start), 
[install it](https://www.zabbix.com/documentation/3.0/manual/installation/getting_zabbix) and [create your own user](https://www.zabbix.com/documentation/3.0/manual/quickstart/login). 

Once installed, login to the frontend (the web interface provided with Zabbix). You should see the Zabbix dashboard.

![Figure 1. Monitoring : How to setup Zabbix monitoring: Dashboard](images/monitoring-zabbix-dashboard.PNG) 

### Importing the RavenDB template

Navigate to `Configuration`->`Templates` and click the `Import` button on the top right corner.   
Import the RavenDB template which is located in:   
`<RavenDB-Directory>\Raven.Database\Plugins\Builtins\Monitoring\Snmp\Templates\zabbix_ravendb_template.xml`   

### Adding a host

Navigate to `Configuration`->`Hosts` and click the `Create Host` button on the top right corner.
This is where we define what host we will monitor, in our case it's the server which runs the RavenDB instance.   
Name your host and choose the `Database servers` group.
Remove the default `Agent interface` and instead add an `SNMP interface`, using either an IP address or a DNS name and a port. 
Also, make sure you click the appropriate IP/DNS button.    

![Figure 2. Monitoring : How to setup Zabbix monitoring: Add host](images/monitoring-zabbix-add-host.PNG) 

Still under `Configuration`->`Hosts`, go to the next tab: `Templates` and add a link to the template we imported earlier.
 
![Figure 3. Monitoring : How to setup Zabbix monitoring: Link to template](images/monitoring-zabbix-link-template.PNG) 

Still under `Configuration`->`Hosts`, go to the tab: `Macros` and add the {$SNMP_COMMUNITY} macro. Click `Save` when you're done.

![Figure 4. Monitoring : How to setup Zabbix monitoring: Community macro](images/monitoring-zabbix-cummunity-macro.PNG) 

That's it! We've added our host and can start exploring.    
Navigate to `Monitoring`->`Overview` and you should now see the different metrics RavenDB exposes.   

### Configuring a trigger

Let's see an example of what you can do with all these metrics.   
We will create a trigger and action that will notify us when the server is up/down.   
Navigate to `Configuration`->`Hosts` and click the host name. Then, in the top navigation bar click on `Triggers`.
Click on `Create trigger` on the top right corner.

Now let's assume you named your host "RavenDB Amazing Server v7.0". Name the trigger "Server is down" and enter the following expression into the text box:   
{CODE-BLOCK:plain}
    {RavenDB Amazing Server v7.0:serverUpTime.nodata(1800)}=1
{CODE-BLOCK/}

You may add a description and severity level and make sure to check the `Enabled` check-box. Click `Save`.
We have created a trigger that will fire when the serverUpTime metric has no data for 1800 seconds.
Learn more about trigger expressions [here](https://www.zabbix.com/documentation/3.0/manual/config/triggers/expression).

![Figure 5. Monitoring : How to setup Zabbix monitoring: Create trigger](images/monitoring-zabbix-create-trigger.PNG) 

Now we will define an action that will be executed whenever the trigger is fired.
Navigate to `Configuration`->`Actions` and click on the `Create action` button in the top right corner.   
Name your action and enter the default subject and/or message, you can also define a Recovery message for when the trigger condition is no longer true.

If you [configure e-mail as the delivery channel for messages](https://www.zabbix.com/documentation/3.0/manual/config/notifications/media/email), 
the message you define in an action will be sent to your e-mail address every time the trigger is fired. 

![Figure 6. Monitoring : How to setup Zabbix monitoring: Create action](images/monitoring-zabbix-create-action.PNG) 

### Setting up RavenDB for monitoring

RavenDB 3.5 is already configured to support SNMP and all you have to do is enable it and restart the server. This is done by adding the following key to your app.config/web.config:
{CODE-BLOCK:plain}
<configuration>
    <appSettings>
        <add key="Raven/Monitoring/Snmp/Enabled" value="true" />
{CODE-BLOCK/}

The default community string is "ravendb" and the default port is 161. You can change those with the following configuration keys:
{CODE-BLOCK:plain}
<add key="Raven/Monitoring/Snmp/Port" value="12345" />
<add key="Raven/Monitoring/Snmp/Community" value="YourString" />
{CODE-BLOCK/}

### The Metrics

For your convenience we've added the list of metrics, which are defined in:   
`<RavenDB-Directory>\Raven.Database\Plugins\Builtins\Monitoring\Snmp\Templates\RAVENDB-MIB.txt`
The Zabbix template comes ready with these metrics. However, you should also be able to access those directly using the SNMP agent.   
Each metric has a unique identifier (OID) and can be accessed individually.

For example, using the SNMP agent you could run the command:
{CODE-BLOCK:plain}
snmpget -v 2c -c ravendb live-test.ravendb.net 1.3.6.1.4.1.45751.1.1.1.2
{CODE-BLOCK/}

Where "ravendb" is the community string and "live-test.ravendb.net" is the host. This command gets the server up time.

![Figure 7. Monitoring : How to setup Zabbix monitoring: snmpget result](images/monitoring-zabbix-snmpget.PNG) 
  
--rootOid 1.3.6.1.4.1.45751.1.1.

--1. Server   
--1.1. Server name   
--1.2. Server up time   
--1.3. Server build version   
--1.4. Server product version   
--1.5. Server PID   
--1.6.1. Server concurrent requests   
--1.6.2. Server total requests   
--1.7. Server CPU   
--1.8.1 Server total memory   
--1.9. Server url   
--1.10 Server indexing errors (global)   

--5. Resources   
--5.1.1 Database total count   
--5.1.2 Database loaded count   

--5.2.X Database   
--5.2.X.1. Database statistics   
--5.2.X.1.1 Database name   
--5.2.X.1.2 Database count of indexes   
--5.2.X.1.3 Database stale count   
--5.2.X.1.4 Database count of transformers   
--5.2.X.1.5 Database approximate task count   
--5.2.X.1.6 Database count of documents   
--5.2.X.1.7 Database count of attachments   
--5.2.X.1.8 Database CurrentNumberOfItemsToIndexInSingleBatch   
--5.2.X.1.9 Database CurrentNumberOfItemsToReduceInSingleBatch   
--5.2.X.1.10 Database errors (count)   
--5.2.X.1.11 Database id   
--5.2.X.1.12 Database active bundles   
--5.2.X.1.13 Database loaded   
--5.2.X.2. Database storage statistics   
--5.2.X.2.1. Database transactional storage allocated size   
--5.2.X.2.2. Database transactional storage used size   
--5.2.X.2.3. Database index storage size   
--5.2.X.3.4. Database total size   
--5.2.X.2.5. Database transactional storage drive remaining space   
--5.2.X.2.6. Database index storage drive remaining space   
--5.2.X.3. Database metrics   
--5.2.X.3.1. Database docs write per second   
--5.2.X.3.2. Database indexed per second   
--5.2.X.3.3. Database reduced per second   
--5.2.X.3.4. Database requests   
--5.2.X.3.4.1. Database requests per second   
--5.2.X.3.4.2. Database requests duration   
--5.2.X.3.4.2.1. Database requests duration last minute avg   
--5.2.X.3.4.2.2. Database requests duration last minute max   
--5.2.X.3.4.2.3. Database requests duration last minute min   

--5.2.X.4. Database indexes   
--5.2.X.4.Y. Index   
--5.2.X.4.Y.1. Index exists   
--5.2.X.4.Y.2. Index name   
--5.2.X.4.Y.3. Index id   
--5.2.X.4.Y.4. Index priority   
--5.2.X.4.Y.5. Indexing attempts   
--5.2.X.4.Y.6. Indexing successes   
--5.2.X.4.Y.7. Indexing errors   
--5.2.X.4.Y.8. Reduce indexing attempts   
--5.2.X.4.Y.9. Reduce indexing successes   
--5.2.X.4.Y.10. Reduce indexing errors   
--5.2.X.4.Y.11. Time since last query   
--5.2.X.5. Database index statistics   
--5.2.X.5.1. Number of indexes   
--5.2.X.5.2. Number of static indexes   
--5.2.X.5.3. Number of auto indexes   
--5.2.X.5.4. Number of idle indexes   
--5.2.X.5.5. Number of abandoned indexes   
--5.2.X.5.6. Number of disabled indexes   
--5.2.X.5.7. Number of error indexes   

--5.2.X.6. Database bundles   
--5.2.X.6.1. Replication bundle   
--5.2.X.6.1.1. Replication active   
--5.2.X.6.1.2.Y. Replication destinations   
--5.2.X.6.1.2.Y.1. Replication destination enabled   
--5.2.X.6.1.2.Y.2. Replication destination url   
--5.2.X.6.1.2.Y.3. Time since last replication   

## Related articles

- [Monitoring: SNMP support](./snmp)