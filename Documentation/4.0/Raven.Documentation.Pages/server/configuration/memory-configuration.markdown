# Configuration: Memory Options

{PANEL:Memory.LowMemoryLimitInMb}

The minimum amount of available memory RavenDB will attempt to achieve (free memory lower than this value will trigger low memory behavior). Value is in MB.

- **Type**: `int`
- **Default**: minimum of either `10% of total physical memory` or `2048`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Memory.LowMemoryCommitLimitInMb}

The minimum amount of available commited memory RavenDB will attempt to achieve (free commited memory lower than this value will trigger low memory behavior). Value is in MB.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Memory.MinimumFreeCommittedMemoryPercentage}

EXPERT: The minimum amount of committed memory that RavenDB will attempt to ensure remains available. Reducing this value too much may cause RavenDB to fail if there is not enough memory available for the operation system to handle operations.

- **Type**: `float`
- **Default**: `0.05f`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Memory.MaxFreeCommittedMemoryToKeepInMb}

EXPERT: The maximum amount of committed memory that RavenDB will attempt to ensure remains available. Reducing this value too much may cause RavenDB to fail if there is not enough memory available for the operation system to handle operations. Value is in MB.

- **Type**: `int`
- **Default**: `128`
- **Scope**: Server-wide only

{PANEL/}
