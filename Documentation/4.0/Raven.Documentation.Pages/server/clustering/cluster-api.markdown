# Cluster : Cluster API

The easiest way to manage a RavenDB cluster is through the Management Studio's [Cluster View](../../studio/server/cluster/cluster-view).  

All cluster operations are available through the GUI. However, sometimes you'd want to automate things. In this section, we will give examples of how to perform certain cluster operations programmatically.

## Add node to the cluster

Adding a node to the cluster can be done using an HTTP PUT request to the `/admin/cluster/node` endpoint with the following arguments:

| Argument | Description | Required | Default |
| - | - | - | - |
| new-node-url | The address of the new node we want to add to the cluster | true | -
| new-node-tag | 1-4 uppercase unicode letters | false | 'A' - 'Z' assigned by order of addition
| is-watcher | Add the new node as a [watcher](../../studio/server/cluster/cluster-view#cluster-nodes-types) | false | false
| assigned-cores | The number of cores to assign to the new node | false | number of processors on the machine or the license limit (smallest)

### C# 

{CODE add_node_with_args@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Put -URI "http://<server-url>/admin/cluster/node?url=<new-node-url>&tag=<new-node-tag>&watcher=<is-watcher>&assignedCores=<assigned-cores>"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X PUT http://<server-url>/admin/cluster/node?url=<new-node-url>&tag=<node-tag>&watcher=<is-watcher>&assignedCores=<assigned-cores>
{CODE-BLOCK/}

## Delete node from the cluster

Deleting a node from the cluster can be done using an HTTP DELETE request to the `/admin/cluster/node` endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to delete | true | -

### C# 

{CODE delete_node@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Delete -URI "http://<server-url>/admin/cluster/node?nodeTag=<node-tag>"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X Delete http://<server-url>/admin/cluster/node?nodeTag=<node-tag>
{CODE-BLOCK/}
