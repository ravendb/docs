# Glossary: ReplicationConflictNotification

### General

This class extends `EventArgs`.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **ItemType** | [ReplicationConflictTypes](../glossary/replication-conflict-notification#replicationconflicttypes-enum-flags) | Replication conflict type (described below) |
| **Id** | string | Identifier of conflicted document |
| **Etag** | Etag | Etag of conflict |
| **OperationType** | [ReplicationOperationTypes](../glossary/replication-conflict-notification#replicationoperationtypes-enum-flags) | Operation Type (described below) |
| **Conflicts** | string[] | Identifier of conflicted versions |

<hr />

# ReplicationConflictTypes (enum flags)

### Members

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **DocumentReplicationConflict** | `1` |
| **AttachmentReplicationConflict** | `2` |

<hr />

# ReplicationOperationTypes (enum flags)

### Members

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Put** | `1` |
| **Delete** | `2` |
