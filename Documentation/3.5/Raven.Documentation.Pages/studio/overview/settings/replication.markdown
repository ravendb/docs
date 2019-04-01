# Settings: Replication

Here you can edit the following [replication](../../../server/scaling-out/replication/how-replication-works) settings:

{PANEL:Client failover behavior}

Client can decide which failover behavior to use by default (`Let client decide`) or the failover behavior can be enforced by a server, with the following options:

- `Allow reads from secondaries`,
- `Allow reads from secondaries when request time SLA threshold is reached`,
- `Allow reads from secondaries and writes to secondaries`,
- `Fail immediately`,
- `Read from all servers`,
- `Read from all servers but switch when request time SLA thresold is reached`,
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

{NOTE:Note}

You can replicate, skip replication or force replication for all indexes and transformers.

{NOTE/}

{PANEL/}

{PANEL:Conflict Resolution}

You can resolve all existing conflict acording to the current conflict resolution

![Figure 4. Settings. Replication. Conflict Resolution.](images/settings_replication_conflict.png)

{PANEL/}

{PANEL:Server HiLo prefix}

Please refer to following [article](../../../client-api/bundles/how-client-integrates-with-replication-bundle#custom-document-id-generation).

![Figure 5. Settings. Replication. Server Hilo Prefix.](images/settings_replication-4.png)

{PANEL/}

{NOTE:Note}

Since version 3.5, enabling the replication bundle is possible for an existing databases. In that case, `Raven/ConflictDocuments` and `Raven/ConflictDocumentsTransformer` are automatically deployed.

![Figure 6. Settings. Replication. Enable Replication Bundle.](images/settings_replication-5.png)

{NOTE/}

{WARNING:Warning}

This is not recommended if your database already contain documents.   
In case you decided to do it anyway, there will be a side effect where replication info will be missing from the document's metadata (can cause conflicts)
until you "touch" the document again.    
For more information read about influence on metadata in [Advanced replication details](../../../server/kb/advanced-replication-details).

{WARNING/}



## Related articles

- [Explain replication](../../../studio/overview/status/debug/explain-replication)
- [Client API : How client integrates with replication bundle?](../../../client-api/bundles/how-client-integrates-with-replication-bundle)
