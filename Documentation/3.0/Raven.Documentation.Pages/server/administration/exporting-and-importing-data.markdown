# Administration: Exporting and Importing data

In order to export or import data from a RavenDB server, you can use the Raven.Smuggler utility (aka `Smuggler`).

Raven.Smuggler is distributed in both the:

- RavenDB [distribution package](https://ravendb.net/download). It is located under the `/Smuggler` folder.
- RavenDB.Server [nuget package](https://www.nuget.org/packages/RavenDB.Server). It is located under the `/tools` folder.

Using the Smuggler utility is necessary when trying to move a RavenDB Data folder between servers.

{DANGER Copying `Data` folder between servers or even within a single server instance is not supported and can result in various server errors. /}

{PANEL:**Exporting to file**}

To Export data, use this command:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind
{CODE-BLOCK/}
or
{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080/databases/Northwind dump.ravendump
{CODE-BLOCK/}

This command will export all indexes, transformers, documents and attachments from `Northwind` database to a file named `dump.ravendump`.

The dump file will also include documents that were added during the export process, so you can make changes while the export is being executed.

### Exporting and Replication

If **Replication is turned on in your source database**, it is **recommended** that you filter out the `Raven/Replication/Destinations` document, using the following command:
 
{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 dump.ravendump --databases=Northwind --negative-metadata-filter:@id=Raven/Replication/Destinations
{CODE-BLOCK/}

{PANEL/}

{PANEL:**Importing from file**}

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 dump.ravendump --database=NewNorthwind
{CODE-BLOCK/}

This command will import all the indexes, transformers, documents, and attachments from the file to the `NewNorthwind` database.

{INFO:Information}

- Import does **not support** database creation. If a destination database does not exist, an error message will appear.
- Import will **overwrite** any existing documents in a destination database.

{INFO/}

You can continue using this RavenDB instance while data is being imported to it.

### Importing and Replication

Note that if you have either the replication bundle or the periodic export bundle active on a database, it is recommended that you'll filter out the following documents when doing an import: `Raven/Replication/Destinations`, `Raven/Replication/VersionHilo`, `Raven/Backup/Periodic/Setup`, `Raven/Backup/Periodic/Status`.  
This can be done using the following command: 

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 dump.ravendump --database=NewNorthwind --negative-metadata-filter:@id=Raven/Replication/Destinations --negative-metadata-filter:@id=Raven/Backup/Periodic/Setup --negative-metadata-filter:@id=Raven/Backup/Periodic/Status --negative-metadata-filter:@id=Raven/Replication/VersionHilo
{CODE-BLOCK/}

{PANEL/}

{PANEL:**Incremental Export and Import**}

With the incremental export operation we can use in order to backup the database incrementally, on each export, we will get to export only the documents created or updated
since the last export.

To export data incrementally we can use two options:

- If it is the first run and the folder does not exist yet, use:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 folder_location --database=Northwind --incremental
{CODE-BLOCK/}

Note that this cammand can be used every time.

- If you ran the command before or you created the folder earlier, you can use:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 folder_location --database=Northwind
{CODE-BLOCK/}

To import data that was exported incrementally, you can use either of the following:

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080  folder_location --database=NewNorthwind --incremental
{CODE-BLOCK/}

or

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 folder_location --database=NewNorthwind
{CODE-BLOCK/}

### Incremental exports and deletions

{WARNING `Raven.Smuggler` does not support exporting deletions for incremental backups. If you want to backup whole database use [Raven.Backup](../../server/administration/backup-and-restore) utility or [Periodic Export](../../server/bundles/periodic-export) bundle. /}

{PANEL/}

{PANEL:**Moving data between two databases**}

To move data directly between two instances (or different databases in the same instance) use  the `between` option in following manner:

{CODE-BLOCK:plain}
    Raven.Smuggler between http://localhost:8080 http://localhost:8080 folder_location --database=Northwind --database2=NewNorthwind
{CODE-BLOCK/}

{PANEL/}

{PANEL:**Command line options**}

You can tweak the export/import process with the following parameters:

 - database: The database to operate on. If not specified, the operations will be performed on a default database.
 - database2: The database to operate on. If not specified, the operations will be performed on a default database (used only in the between operation).
 - filesystem: The filesystem to operate on.
 - filesystem2: The filesystem to operate on (used only in the between operation).
 - operate-on-types: Specify the types to export/import. Usage example: `--operate-on-types=Indexes,Documents,Attachments,Transformers`.
 - filter: Filter documents by a document property. Usage example: `--filter=Property-Name=Value`.
 - negative-filter: Filter documents NOT matching a document property. Usage example: `--negative-filter=Property-Name=Value`.   
 - metadata-filter: Filter documents by a metadata property. Usage example: `--metadata-filter=Raven-Entity-Name=Posts`.
 - negative-metadata-filter: Filter documents NOT matching a metadata property. Usage example: `--negative-metadata-filter=Raven-Entity-Name=Posts`.
 - ignore-errors-and-continue: If this option is enabled, smuggler will not halt its operation on errors. Errors still will be displayed to the user.
 - transform: Transform documents using a given script.   
 - transform-file: Transform documents using a given script file.   
 - max-steps-for-transform-script: Maximum number of steps that a transform script can have.
 - timeout: The timeout (in milliseconds) to use for requests.
 - batch-size: The batch size for requests.
 - chunk-size: The number of documents to import before new connection will be opened.
 - username: A username to use when a database requires client authentication.
 - username2: A username to use when a database requires client authentication (used only in the between operation).
 - password: A password to use when a database requires client authentication.
 - password2: A password to use when a database requires client authentication (used only in the between operation).
 - domain: A domain to use when a database requires client authentication.
 - domain2: A domain to use when a database requires client authentication (used only in the between operation).
 - api-key: An API-key to use, when using OAuth.
 - api-key2: An API-key to use, when using OAuth (used only in the between operation).
 - incremental: States usage of incremental operations.
 - wait-for-indexing: Wait until all indexing activities have been completed (import only).
 - excludeexpired: Excludes expired documents created by the [expiration bundle](../../server/bundles/expiration).    
 - limit: Reads at most VALUE documents/attachments.
 - strip-replication-information: Remove all replication information from metadata (import only). 
 - disable-versioning-during-import: Disables versioning for the duration of the import.
 - continuation-token: Activates the usage of a continuation token in case of unreliable connections or huge imports.
 - skip-conflicted: The database will issue and error when conflicted documents are put. The default is to alert the user, this allows to skip them to continue.
 - help: You can use the help option in order to print the built-in options documentation.

{PANEL/}

{PANEL:**Filtering**}

To filter out documents, we introduced a few filtering options that can be used during an import or export process.

1. `filter` is used to filter documents according to their properties. E.g. if we want to export all documents with a property `Name` and value `John`, we have to apply command as follows: `--filter=Name=John` .   
2. `negative-filter` is a countertype to `filter` and will filter documents that do NOT match the given property.  
3. `metadata-filter` is similar to `filter` but works on the document's metadata properties.   
4. `negative-metadata-filter` filters out documents that do NOT match given metadata property.   

{INFO:Multiple value support}

Comma has been introduced to support multiple values e.g. `--filter=Name=John,Greg` will export all documents with a `Name` equal to `John` or `Greg`. 

If you want to use comma in your filter, wrap the value in `'` e.g. `--filter=Name='John, the Second',Greg` will export all documents with a `Name` equal to `John, the Second` and `Greg`.

{INFO/}

#### Example I - basics

To export all documents containing property FirstName with value `Robert` we need to execute following command:

{CODE-BLOCK:plain}
	Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind --filter=FirstName=Robert --operate-on-types=Documents
{CODE-BLOCK/}

#### Example II - basics

To export all documents from `Employees` collection and HiLo document for this collection (if exists) following command needs to be executed:

{CODE-BLOCK:plain}
	Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind --metadata-filter=Raven-Entity-Name=Employees --operate-on-types=Documents
{CODE-BLOCK/}

#### Example III - combining filters

To export all documents from `Employees` collection that contain property FirstName with value `Robert` and HiLo document for this collection (if exists) following command needs to be executed:

{CODE-BLOCK:plain}
	Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind --filter=FirstName=Robert --metadata-filter=Raven-Entity-Name=Employees --operate-on-types=Documents
{CODE-BLOCK/}

#### Example IV - multiple filters

Same filter can be used multiple times, but one must remember that document must match all the filters (logical AND), so to export all documents that contain property FirstName with value `Robert` and LastName with value `King` then following command must be executed:

{CODE-BLOCK:plain}
	Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind --filter=FirstName=Robert --filter=LastName=King --operate-on-types=Documents
{CODE-BLOCK/}

#### Example V - importing

Filter can be also used during import and they work the same way as in export. E.g. if we have an export containing whole `Northwind` database and we want to only import documents from `Employees` collection then we must execute following command:

{CODE-BLOCK:plain}
	Raven.Smuggler in http://localhost:8080/ dump.ravendump --database=NewNorthwind --metadata-filter=Raven-Entity-Name=Employees --operate-on-types=Documents
{CODE-BLOCK/}

{PANEL/}

{PANEL:**Transforms**}

Transforms can be used to modify or filter out documents, but they work only during the import process. The scripts have to use JavaScript syntax and be in following format:   

{CODE-BLOCK:json}
function(doc) {
	// custom code here
}
{CODE-BLOCK/}

where `doc` will contain our document with its metadata under the `@metadata` property.

#### Change scripts:   

E.g. To change document's property `Name` value to a new one, the following script can be used:   

{CODE-BLOCK:json}
function(doc) {
	doc['Name'] = 'NewValue';
	return doc;
}
{CODE-BLOCK/}

#### Filter scripts:    

If we return `null`, the document will be filtered out.   

{CODE-BLOCK:json}
function(doc) {
	var id = doc['@metadata']['@id'];
	if(id === 'orders/999')
		return null;
	return doc;
}
{CODE-BLOCK/}

{INFO: Transforms support for export }

Since version 3.0.3836-Unstable, the change and filter scripts are also allowed to be used during the export process. 

{INFO/}

{PANEL/}

{PANEL:**SmugglerDatabaseApi**}

Alternatively, if you prefer to do export/import from code rather than from the console utility, you can use the `SmugglerDatabaseApi` class (in order to use this class you need to reference the `Raven.Smuggler.exe`).

### Exporting

{CODE smuggler_api_1@Server\Administration\ExportImport.cs /}

### Importing

{CODE smuggler_api_2@Server\Administration\ExportImport.cs /}

### Moving data between two databases

{CODE smuggler_api_3@Server\Administration\ExportImport.cs /}

{PANEL/}

{PANEL:**DatabaseDataDumper**}

A smuggler communicates with a server using the HTTP protocol, meaning that it cannot communicate with Embedded instance as long as the `UseEmbeddedHttpServer` is set to `false` (which is a default value). If embedded http server cannot be started, the `DatabaseDataDumper` (found in `Raven.Database.Smuggler` namespace in `Raven.Database.dll`) can be used to import or export data.

{CODE smuggler_api_4@Server\Administration\ExportImport.cs /}

{PANEL/}

## Remarks

{INFO:Information}

- During **export** the Smuggler is using [document streaming](../../client-api/commands/documents/stream). To maintain backward compatibility, the Smuggler will detect from what version it exports the documents, and adjust its behavior accordingly.
- During **import**  the Smuggler is using [bulk insert operation](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation).
- The usage of **disable-versioning-during-import** option disables versioning bundle of the target database for imported docs. In particular that will allow to import existing revisions (without creating new ones) if an export file contains such (the source database had versioning enabled too). 

{INFO/}

## Related articles

- [Studio : Tasks : Import & Export Database](../../studio/overview/tasks/import-export-database)
