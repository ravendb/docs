# Configuration: Tombstone Options

{PANEL:Tombstones.CleanupIntervalInMin}

Time (in minutes) between tombstone cleanups.

- **Type**: `TimeUnit.Minutes`
- **Default**: `5`
- **Scope**: Server-wide or per database
{PANEL/}

{PANEL: Tombstones.RetentionTimeWithReplicationHubInHrs}

Time (in hours) to save tombsones from deletion if this server is defined 
as a replication hub.  

- **Type**: `TimeUnit.Hours`
- **Default**: `336` (14 days)
- **Scope**: Server-wide or per database
{PANEL/}

{PANEL: Tombstones.CleanupIntervalWithReplicationHubInMin}

Time (in minutes) between tombstone cleanups if this server is defined as 
a replication hub.  

- **Type**: `TimeUnit.Minutes`
- **Default**: `1440` (1 day)
- **Scope**: Server-wide or per database
{PANEL/}
