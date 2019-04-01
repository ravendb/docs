# Glossary: ReplicationDestination

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Url** | string | the URL of the replication destination |
| **Username** | string | The replication server username to use |
| **Password** | string | The replication server password to use |
| **Domain** | string | The replication server domain to use |
| **ApiKey** | string | The replication server api key to use |
| **Database** | string | The database to use |
| **TransitiveReplicationBehavior** | [TransitiveReplicationOptions](../glossary/replication-destination#transitivereplicationoptions-enum) |  How should the replication bundle behave with respect to replicated documents. If a document was replicated to us from another node, should we replicate that to this destination, or should we replicate only documents that were locally modified. |
| **IgnoredClient** | bool | Controls if the replication will ignore this destination in the client |
| **Disabled** | bool | Controls if replication to this destination is disabled in both client and server. |
| **ClientVisibleUrl** | string | The Client URL of the replication destination |

<hr />

# TransitiveReplicationOptions (enum)

### Members

| Name | Description |
| ---- | ----- |
| **None** |  Don't replicate replicated documents |
| **MapCompleted** | Replicate replicated documents |

