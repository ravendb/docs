# Cluster: Cluster API

The easiest way to manage a RavenDB cluster is through the Management Studio's [Cluster View](../../studio/cluster/cluster-view).  
All cluster operations are available through the GUI. However, sometimes you'd want to automate things. 

In this section we will demonstrate of how to perform certain cluster operations programmatically using the REST API.
In the first example we will show how to make a secure call using PowerShell, cURL and the RavenDB C# Client. The rest of the examples are cURL only.

- [Add node to the cluster](../../server/clustering/cluster-api#add-node-to-the-cluster)
- [Delete node from the cluster](../../server/clustering/cluster-api#delete-node-from-the-cluster)
- [Promote a watcher node](../../server/clustering/cluster-api#promote-a-node)
- [Demote a watcher node](../../server/clustering/cluster-api#demote-a-node)
- [Force elections](../../server/clustering/cluster-api#force-elections)
- [Force timeout](../../server/clustering/cluster-api#force-timeout)
- [Bootstrap the cluster](../../server/clustering/cluster-api#bootstrap-cluster)

{NOTE: }
If authentication is turned on, a client certificate with either `Cluster Admin` or `Operator` [Security Clearance](../../server/security/authorization/security-clearance-and-permissions) must be supplied, depending on the endpoint.
{NOTE/}

{PANEL: Add node to the cluster}

Adding a node to the cluster can be done using an HTTP PUT request to the `/admin/cluster/node` endpoint with the following arguments:

| Argument | Description | Required | Default |
| - | - | - | - |
| new-node-url | The address of the new node we want to add to the cluster | true | - |
| new-node-tag | 1-4 uppercase unicode letters | false | 'A' - 'Z' assigned by order of addition |
| is-watcher | Add the new node as a watcher | false | `false` |
| max-utilized-cores | The maximum number of cores that can be assigned to the new node | false | Number of processors on the machine or the license limit (smallest) |

SecurityClearance: `Cluster Admin`

### C# Client

* To make a secure call, the Document Store must be supplied with the client certificate ([example](../../client-api/setting-up-authentication-and-authorization#example---initializing-document-store-with-a-client-certificate)).

{CODE add_node_with_args@ClusterAPI.cs /}

### Powershell

{CODE-BLOCK:powershell}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$clientCert = Get-PfxCertificate -FilePath <path-to-pfx-cert>
Invoke-WebRequest -Method Put -URI "https://<server-url>/admin/cluster/node?url=<new-node-url>&tag=<new-node-tag>&watcher=<is-watcher>&maxUtilizedCores=<max-utilized-cores> -Certificate $cert"
{CODE-BLOCK/}

### cURL

{CODE-BLOCK: bash}
curl -X PUT https://<server-url>/admin/cluster/node?url=<new-node-url>&tag=<node-tag>&watcher=<is-watcher>&maxUtilizedCores=<max-utilized-cores> --cert <path-to-pem-cert>
{CODE-BLOCK/}

{PANEL/}

{PANEL: Delete node from the cluster}

Deleting a node from the cluster can be done using an HTTP DELETE request to the `/admin/cluster/node` endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to delete | true | -

SecurityClearance: `Cluster Admin`

### Example

{CODE-BLOCK: bash}
curl -X DELETE https://<server-url>/admin/cluster/node?nodeTag=<node-tag>
{CODE-BLOCK/}

{PANEL/}

{PANEL: Promote a Node}

Promoting a node can be done using an HTTP POST request to the `/admin/cluster/promote` 
endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to promote | true | -

The POST request body should be empty.

SecurityClearance: `ClusterAdmin`

### Example

{CODE-BLOCK: bash}
curl -X POST https://<server-url>/admin/cluster/promote?nodeTag=<node-tag> -d ''
{CODE-BLOCK/}

{PANEL/}

{PANEL: Demote a Node}

Demoting a node can be done using an HTTP POST request to the `/admin/cluster/demote` endpoint with the following argument:

| Argument | Description | Required | Default |
| - | - | - | - |
| node-tag | The tag of the node to demote | true | -

The POST request body should be empty.

SecurityClearance: `ClusterAdmin`

### Example

{CODE-BLOCK: bash}
curl -X POST https://<server-url>/admin/cluster/demote?nodeTag=<node-tag> -d ''
{CODE-BLOCK/}

{PANEL/}

{PANEL: Force Elections}

Forcing an election can be done using an empty HTTP POST request to the `/admin/cluster/reelect` endpoint:

SecurityClearance: `Operator`

### Example

{CODE-BLOCK: bash}
curl -X POST https://<server-url>/admin/cluster/reelect -d ''
{CODE-BLOCK/}

{PANEL/}

{PANEL: Force Timeout}

Forcing a timeout can be done using an empty HTTP POST request to the `/admin/cluster/timeout` endpoint:

SecurityClearance: `Operator`

### Example

{CODE-BLOCK: bash}
curl -X POST https://<server-url>/admin/cluster/timeout -d ''
{CODE-BLOCK/}

{PANEL/}

{PANEL: Bootstrap Cluster}

Bootstrapping the cluster can be done using an empty HTTP POST request to the `/admin/cluster/bootstrap` endpoint:
Note: This option is only available when the server is in the Passive state. 

SecurityClearance: `Cluster Admin`

### Example

{CODE-BLOCK: bash}
curl -X POST https://<server-url>/admin/cluster/bootstrap -d ''
{CODE-BLOCK/}

 {PANEL/}
