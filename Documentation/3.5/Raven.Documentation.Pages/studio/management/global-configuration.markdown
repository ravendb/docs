# Manage Your Server: Global Configuration

### Bundles

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

### Cluster-wide database settings

Cluster-wide database settings feature allow you to define databases behavior for all nodes in the cluster.
You can read more about all databases configuration [here](../../server/configuration/configuration-options).
You can change only databases configuration and not server configuration.

{NOTE:Important}

Remember to restart RavenDB server or reload database to apply cluster-aware database settings.

{NOTE/}

![Figure 2. Manage Your Server. Cluster Configuration.](images/manage_your_server-cluster-configuration.png)
