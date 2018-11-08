# Replication : Using Embedded instance

## Overview
[Replication](../../../server/clustering/replication/replication) works by using long-living TCP connections between cluster nodes, or in case of [External Replication](../../../server/ongoing-tasks/external-replication) instances external to the cluster. 
Esentially, Embedded RavenDB is the same as non-embedded, their only difference is what process is a host. [Here](../../../server/embedded) you can read more in-depth about Embedded RavenDB functionality.

## Configuring replication between embedded instance and a cluster
One possibility would be to configure it through [the studio](../../../studio/server/cluster/add-node-to-cluster#add-another-node-to-the-cluster), by adding the Embedded instance to the cluster, then adding the database replicated to relevant [database group](../../../server/clustering/distribution/distributed-database).  
Studio of the Embedde instance can be opened in the following way:
{CODE Embedded_OpenStudio@Server\ReplicationEmbedded.cs /}

Another possibility would be to configure it programmatically.

{CODE Embedded_Replication_Setup@Server\ReplicationEmbedded.cs /}


## Related articles
  * [Running RavenDB Embedded Instance](../../../server/embedded)
  * [Add Another Node to the Cluster](../../../studio/server/cluster/add-node-to-cluster#add-another-node-to-the-cluster)  
  * [Database Topology](../../../server/clustering/distribution/distributed-database#database-topology)  
  * [Manage Database Group](../../../studio/database/settings/manage-database-group)  
