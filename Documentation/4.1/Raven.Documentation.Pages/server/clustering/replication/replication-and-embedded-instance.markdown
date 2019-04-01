# Replication: Using Embedded Instance

## Overview
[Replication](../../../server/clustering/replication/replication) works by using long-living TCP connections between cluster nodes, or in case of [External Replication](../../../server/ongoing-tasks/external-replication) instances external to the cluster. 
Essentially, Embedded RavenDB is the same as non-embedded, their only difference is what process is a host. [Here](../../../server/embedded) you can read more in-depth about Embedded RavenDB functionality.

## Configuring Replication between Embedded Instance and a Cluster
{PANEL:Examples in this page }
  * [Configuring cluster membership of embedded instance through the Studio](../../../server/clustering/replication/replication-and-embedded-instance#configuring-cluster-membership-of-embedded-instance-through-the-studio)  
  * [Programmatically configuring cluster membership of embedded instance](../../../server/clustering/replication/replication-and-embedded-instance#programmatically-configuring-cluster-membership-of-embedded-instance)  
  * [Configuring embedded instance as External Replication destination through the Studio](../../../server/clustering/replication/replication-and-embedded-instance#configuring-embedded-instance-as-external-replication-destination-through-the-studio)  
  * [Programmatically embedded instance as External Replication destination](../../../server/clustering/replication/replication-and-embedded-instance#programmatically-embedded-instance-as-external-replication-destination)  
{PANEL/}


## Configuring Cluster Membership of Embedded Instance through the Studio
One possibility would be to configure it through [the studio](../../../studio/server/cluster/add-node-to-cluster#add-another-node-to-the-cluster), by adding the Embedded instance to the cluster, then adding the database replicated to relevant [database group](../../../server/clustering/distribution/distributed-database).  
Studio of the Embedded instance can be opened in the following way:
{CODE Embedded_OpenStudio@Server\ReplicationEmbedded.cs /}

## Programmatically Configuring Cluster Membership of Embedded Instance
Another possibility would be to configure it programmatically.

{CODE Embedded_Replication_Setup@Server\ReplicationEmbedded.cs /}

## Configuring Embedded Instance as External Replication Destination through the Studio
Another possibility to configure the embedded instance as an [external replication](../../../server/ongoing-tasks/external-replication) source or a destination.  
This can be done in [RavenDB Studio](../../../studio/database/tasks/ongoing-tasks/external-replication-task).
{NOTE: External Replication is configured only one way, so in order to create two-way external replication, we would need to configure External Replication Tasks at both RavenDB instances. /}

## Programmatically Embedded Instance as External Replication Destination
It is also possible to configure [external replication](../../../server/ongoing-tasks/external-replication) programmatically, so the embedded instance can serve as a source, destination or both.
{CODE External_Replication_And_Embedded@Server\ReplicationEmbedded.cs /}

## Related Articles
  * [Running RavenDB Embedded Instance](../../../server/embedded)
  * [Add Another Node to the Cluster](../../../studio/server/cluster/add-node-to-cluster#add-another-node-to-the-cluster)  
  * [Database Topology](../../../server/clustering/distribution/distributed-database#database-topology)  
  * [Manage Database Group](../../../studio/database/settings/manage-database-group)  
  * [External Replication](../../../server/ongoing-tasks/external-replication)
