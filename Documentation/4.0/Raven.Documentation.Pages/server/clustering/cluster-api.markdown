# Cluster : Cluster API

The easiest way to manage a RavenDB cluster is through the Management Studio's [Cluster View](../../studio/server/cluster/cluster-view).  

All cluster operations are available through the GUI. However, sometimes you'd want to automate things. In this section, 
we will give examples of how to perform certain cluster operations programmatically:

- [Add node to the cluster](../../server/clustering/cluster-api#add-node-to-the-cluster)
- [Delete node from the cluster](../../server/clustering/cluster-api#delete-node-from-the-cluster)
- [Promote a watcher node](../../server/clustering/cluster-api#promote-a-watcher-node)
- [Demote a watcher node](../../server/clustering/cluster-api#demote-a-watcher-node)
- [Force elections](../../server/clustering/cluster-api#force-elections)
- [Force timeout](../../server/clustering/cluster-api#force-timeout)
- [Bootstrap the cluster](../../server/clustering/cluster-api#bootstrap-cluster)

{NOTE: }
If authentication is turned on, a client certificate with `ClusterAdmin` or `Operator` Security Clearance must be supplied according to the required SecurityClearance. Read more about [Client Certificate Usage](../../server/security/authentication/client-certificate-usage).
{NOTE/}


## Add node to the cluster

Adding a node to the cluster can be done using an HTTP PUT request to the `/admin/cluster/node` endpoint with the following arguments:

| Argument | Description | Required | Default |
| - | - | - | - |
| new-node-url | The address of the new node we want to add to the cluster | true | -
| new-node-tag | 1-4 uppercase unicode letters | false | 'A' - 'Z' assigned by order of addition
| is-watcher | Add the new node as a [watcher](../../studio/server/cluster/cluster-view#cluster-nodes-types) | false | false
| assigned-cores | The number of cores to assign to the new node | false | number of processors on the machine or the license limit (smallest)

SecurityClearance: `ClusterAdmin`

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

SecurityClearance: `ClusterAdmin`

### C# 

{CODE delete_node@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Delete -URI "http://<server-url>/admin/cluster/node?nodeTag=<node-tag>"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X DELETE http://<server-url>/admin/cluster/node?nodeTag=<node-tag>
{CODE-BLOCK/}

## Promote a Watcher Node

[Promoting a node](../../server/clustering/rachis/cluster-topology#nodes-states-and-types) can be done using an HTTP POST request to the `/admin/cluster/promote` 
endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to promote | true | -

The POST request body should be empty.

SecurityClearance: `Operator`

### C# 

{CODE promote_node@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Post -URI "http://<server-url>/admin/cluster/demote?nodeTag=<node-tag>"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X POST http://<server-url>/admin/cluster/demote?nodeTag=<node-tag>
{CODE-BLOCK/}

## Demote a Watcher Node

[Demoting a node](../../server/clustering/rachis/cluster-topology#nodes-states-and-types) can be done using an HTTP POST request to the `/admin/cluster/demote` endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to demote | true | -

The POST request body should be empty.

SecurityClearance: `Operator`

### C# 

{CODE demote_node@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Post -URI "http://<server-url>/admin/cluster/demote?nodeTag=<node-tag>"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X POST http://<server-url>/admin/cluster/demote?nodeTag=<node-tag>
{CODE-BLOCK/}

## Force Elections

Forcing an election can be done using an empty HTTP POST request to the `/admin/cluster/reelect` endpoint:

SecurityClearance: `Operator`

### C# 

{CODE force_election@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Post -URI "http://<server-url>/admin/cluster/reelect"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X POST http://<server-url>/admin/cluster/reelect
{CODE-BLOCK/}

## Force Timeout

Forcing a timeout can be done using an empty HTTP POST request to the `/admin/cluster/timeout` endpoint:

SecurityClearance: `Operator`

### C# 

{CODE force_timeout@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Post -URI "http://<server-url>/admin/cluster/timeout"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X POST http://<server-url>/admin/cluster/timeout
{CODE-BLOCK/}

## Bootstrap Cluster

Bootstrapping the cluster can be done using an empty HTTP POST request to the `/admin/cluster/bootstrap` endpoint:
Note: This option is only available when the server is in the Passive state. 

SecurityClearance: `ClusterAdmin`

### C# 

{CODE bootstrap@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
Invoke-WebRequest -Method Post -URI "http://<server-url>/admin/cluster/bootstrap"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK:plain}
curl -X POST http://<server-url>/admin/cluster/bootstrap
{CODE-BLOCK/}
