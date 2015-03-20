#Getting file systems stats

{PANEL: GetNamesAsync}

This method returns names of all existing file system in the server.

### Syntax

{CODE get_names_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Return Value | |
| ------------- | ------------- |
| **Task&lt;string[]&gt;** | A task that represents the asynchronous restore operation. The task result is the array containing file names |

### Example

{CODE get_names_2@FileSystem\ClientApi\Commands\Admin.cs /}

{PANEL/}

{PANEL: GetStatisticsAsync}

This method returns statistics of currently loaded file systems.

### Syntax

{CODE get_stats_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileSystemStats[]&gt;** | A task that represents the asynchronous restore operation. The task result is the array containing file names |

### Example

{CODE get_stats_2@FileSystem\ClientApi\Commands\Admin.cs /}

{PANEL/}