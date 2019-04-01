# Configuration: Server Options

{PANEL:Server.MaxTimeForTaskToWaitForDatabaseToLoadInSec}

This setting is indicating how many seconds a task (e.g. request) will wait for the database to load (when it is unloaded - e.g. after server restart).

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Server.ProcessAffinityMask}

EXPERT: The process affinity mask.

- **Type**: `long`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Server.IndexingAffinityMask}

EXPERT: The affinity mask to be used for indexing. Overrides the Server.NumberOfUnusedCoresByIndexes value. Should only be used if you also set `Server.ProcessAffinityMask`.

- **Type**: `long`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Server.NumberOfUnusedCoresByIndexes}

EXPERT: The numbers of cores that will be NOT running indexing. Defaults to 1 core that is kept for all other tasks and will not be used for indexing.

- **Type**: `int`
- **Default**: `1`
- **Scope**: Server-wide only

{PANEL/}
