# Settings: Replication

Here you can edit the following [replication](../../../server/scaling-out/replication/how-replication-works) settings:

{PANEL:Client failover behavior}

Client can decide which failover behavior to use by default (`Let client decide`) or the failover behavior can be enforced by a server, with the following options:

- `Allow reads from secondaries`,
- `Allow reads from secondaries and writes to secondaries`,
- `Fail immediately`,
- `Read from all servers`,
- `Read from all servers and allow write to secondaries`

If you want to read more about failover behavior, please visit following [article](../../../client-api/bundles/how-client-integrates-with-replication-bundle#failover-behavior).

![Figure 1. Settings. Replication. Client failover behavior.](images/settings_replication-1.png)

{PANEL/}

{PANEL:Conflict resolution}

Server can automatically resolve any occured conflicts based on predefined strategies:

- `None` (default),
- `Resolve to local`,
- `Resolve to remove`,
- `Resolve to latest`

![Figure 2. Settings. Replication. Conflict Resolution.](images/settings_replication-2.png)

{PANEL/}

{PANEL:Replication Destinations}

List of all destinations to which a database replicates. Here you can choose the following:

- `Enabled` - toggles replication on and off,
- `Url` - url of the server to which a database replicates,
- `Database` - new databases on a target server,
- `Credentials` - credentials to use in server authentication,
- `Client Visible Url`,
- `Failover` - toggles if this destination should be ignored by client,
- `Transitive Replication` - marks what document types should be replicated:
	- Changed only - locally
	- Changed and replicated - from other sources
		
![Figure 3. Settings. Replication. Replication Destination.](images/settings_replication-3.png)

{PANEL/}

{PANEL:Server HiLo prefix}

Please refer to following [article](../../../client-api/bundles/how-client-integrates-with-replication-bundle#custom-document-id-generation).

![Figure 4. Settings. Replication. Server Hilo Prefix.](images/settings_replication-4.png)

{PANEL/}

## Related articles

- [Explain replication](../../../studio/overview/status/debug/explain-replication)
- [Client API : How client integrates with replication bundle?](../../../client-api/bundles/how-client-integrates-with-replication-bundle)
