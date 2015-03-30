#Exporting and importing files

In order to export or import files from RavenFS you can use the same tool like for exporting and importing databases - [Raven.Smuggler](../../server/administration/exporting-and-importing-data).

{PANEL:**Exporting to a file**}

To export data from RavenFS use this command:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080/ dump.ravenfs --filesystem=NorthwindFS
{CODE-BLOCK/}

This command will export `NorthwindFS` file system to a file named `dump.ravenfs`.

{PANEL/}

{PANEL:**Importing from a file**}

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080/ dump.ravenfs --filesystem=NewNorthwindFS
{CODE-BLOCK/}

This command will import data from `dump.ravenfs` file to `NewNorthwindFS` file system.

{INFO:Information}

- Import does **not support** file system creation. If a destination file system does not exist, an error message will appear.
- Import will **overwrite** any existing files in a destination file system.
{INFO/}

{PANEL/}

{PANEL:**Moving data between two file systems**}

To move files directly between two instances (or different file systems in the same instance) use  the `between` option in following manner:

{CODE-BLOCK:plain}
    Raven.Smuggler between http://localhost:8080/ http://localhost:8081/ --filesystem=NorthwindFS --filesystem2=NewNorthwindFS
{CODE-BLOCK/}

{PANEL/}

{PANEL:**Command line options**}

You can tweak the export/import process with the following parameters:

 - timeout: The timeout (in milliseconds) to use for requests.
 - batch-size: The batch size for requests.
 - filesystem: The file system to operate on.
 - username: A username to use when a file system requires client authentication.
 - password: A password to use when a file system requires client authentication.
 - domain: A domain to use when a file system requires client authentication.
 - api-key: An API-key to use, when using OAuth.
 - incremental: States usage of incremental operations.
 - strip-replication-information: Remove all synchronization information from metadata (import only). 
 - disable-versioning-during-import: Disables versioning for the duration of the import.
 - help: You can use the help option in order to print the built-in options documentation.

{PANEL/}

{PANEL:**SmugglerFilesApi**}

Alternatively, if you prefer to do export/import from code rather than from the console utility, you can use the `SmugglerFilesApi` class (in order to use this class you need to reference the `Raven.Smuggler.exe`).

### Exporting

{CODE smuggler_api_1@FileSystem\Server\ExportImport.cs /}

### Importing

{CODE smuggler_api_2@FileSystem\Server\ExportImport.cs /}

### Moving data between two databases

{CODE smuggler_api_3@FileSystem\Server\ExportImport.cs /}

{PANEL/}