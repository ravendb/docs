# Configuration: Replication Options

{PANEL:Replication.ActiveConnectionTimeoutInSec}

Threshold under which an incoming replication connection is considered active. If an incoming connection receives messages within this time-span, new connection coming from the same source would be rejected (as the existing connection is considered active). Value is in seconds.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Replication.ReplicationMinimalHeartbeatInSec}

Minimal time in milliseconds before sending another heartbeat. Value is in seconds.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Replication.RetryReplicateAfterInSec}

If the replication failed, we try to replicate again after the specified time elapsed. Value is in seconds.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Replication.MaxItemsCount}

Maximum number of items replication will send in single batch, `null` means we will not cut the batch by number of items.

- **Type**: `int`
- **Default**: `16 * 1024`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Replication.MaxSizeToSendInMb}

Maximum number of data size replication will send in single batch, `null` means we will not cut the batch by the size. Value is in MB.

- **Type**: `int`
- **Default**: `64`
- **Scope**: Server-wide or per database

{PANEL/}
