# Manage Your Server : Global Configuration
Global Configuration feature allows you to define behavior for all server databases in one place. 
This behavior is inherited automatically but also can be overridden by a user per database basis.

You can configure global behavior for the following bundles:      
`Periodic export` - read more about periodic export configuration [here](../overview/settings/periodic-export).   
`Replication` - read more about replication configuration [here](../overview/settings/replication).   
`SQL Replication` - read more about SQL replication configuration [here](../overview/settings/sql-replication).   
`Quotas` - read more about quotas configuration [here](../overview/settings/quotas).   
`Custom functions` -read more about custom functions configuration [here](../overview/settings/custom-functions).   
`Versioning` - read more about versioning configuration [here](../overview/settings/versioning).   
   
![Figure 1. Manage Your Server. Global Configuration.](images/manage_your_server-global-configuration.png)

{NOTE:Note}

Global Configuration is used extensively by a [Clustering feature](../../server/scaling-out/clustering/clustering-overview) 
and will allow you to spread configuration across all nodes.

![Figure 2. Manage Your Server. Global Configuration Cluster.](images/manage_your_server-global-configuration-2.png)
{NOTE/}
