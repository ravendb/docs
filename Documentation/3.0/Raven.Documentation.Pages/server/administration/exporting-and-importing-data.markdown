# Administration : Exporting and Importing data

In order to export or import data from a RavenDB server, you can use the Raven.Smuggler utility (aka `Smuggler`).

Raven.Smuggler is distributed in both the:

- RavenDB [distribution package](http://ravendb.net/download). It is located under the `/Smuggler` folder.
- RavenDB.Server [nuget package](https://nuget.org/packages/RavenDB.Server). It is located under the `/tools` folder.

Using the Smuggler utility is necessary when trying to move a RavenDB Data folder between servers.

{WARNING Copying `Data` folder between servers or even within a single server instance is not supported and can result in various server errors. /}

## Exporting to file

To Export data, use this command:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080/ dump.ravendump --database=Northwind
{CODE-BLOCK/}
or
{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080/databases/Northwind dump.ravendump
{CODE-BLOCK/}

This command will export all indexes, transformers, documents and attachments from `Northwind` database to a file named `dump.ravendump`.

The dump file will also include documents that were added during the export process, so you can make changes while the export is executing.

### Exporting and Replication

If **Replication is turned on in your source database**, it is **recommended** that you filter out the `Raven/Replication/Destinations` document, using the following command:
 
{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 dump.ravendump --databases=Northwind --negative-metadata-filter:@id=Raven/Replication/Destinations
{CODE-BLOCK/}

## Importing from file

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 dump.ravendump --database=NewNorthwind
{CODE-BLOCK/}

This command will import all the indexes, transformers, documents and attachments from the file to the `NewNorthwind` database.

{NOTE Import does **not support** database creation. If destination database does not exist error message will be shown. /}

{NOTE Import will **overwrite** any existing documents in the destination database. /}

You can continue using that RavenDB instance while data is being imported to it.

### Importing and Replication

Note that if you have either the replication bundle or the periodic backup bunlde active on the database, it is recommened that you'll filter out the following documents when doing an import: `Raven/Replication/Destinations`, `Raven/Replication/VersionHilo`, `Raven/Backup/Periodic/Setup`, `Raven/Backup/Periodic/Status`.  
This can be done using the following command: 

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 dump.ravendump --database=NewNorthwind --negative-metadata-filter:@id=Raven/Replication/Destinations --negative-metadata-filter:@id=Raven/Backup/Periodic/Setup --negative-metadata-filter:@id=Raven/Backup/Periodic/Status --negative-metadata-filter:@id=Raven/Replication/VersionHilo
{CODE-BLOCK/}

## Incremental Export and Import

With the incremental export operation we can use in order to backup the database incrementally, on each export, we will only take the export the documents create or updated
since the last export.

To export data with incremental we can use 2 options.  
If it is the first run and the folder does not exist yet use (you can continue to use this command every time):

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 --database=Northwind folder_location --incremental
{CODE-BLOCK/}

If you ran the command before or you created the folder earlier you can use:

{CODE-BLOCK:plain}
    Raven.Smuggler out http://localhost:8080 --database=Northwind folder_location
{CODE-BLOCK/}

In order to import date that was exported with incremental operation, you can use either of the following:

{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 --database=NewNorthwind folder_location --incremental
{CODE-BLOCK/}
or
{CODE-BLOCK:plain}
    Raven.Smuggler in http://localhost:8080 --database=NewNorthwind folder_location
{CODE-BLOCK/}

## Moving data between two databases

To move data directly between two instances (or different databases in same instance) use `between` option in following manner:

{CODE-BLOCK:plain}
    Raven.Smuggler between http://localhost:8080 http://localhost:8080 --database=Northwind --database2=NewNorthwind folder_location
{CODE-BLOCK/}

## Command line options

You can tweak the export/import process with the following parameters:

 - operate-on-types: Specify the types to export/import. Usage example: `--operate-on-types=Indexes,Documents,Attachments,Transformers`.
 - filter: Filter documents by a document property. Usage example: `--filter=Property-Name=Value`.
 - negative-filter: Filter documents NOT matching a document property. Usage example: `--negative-filter=Property-Name=Value`.   
 - metadata-filter: Filter documents by a metadata property. Usage example: `--metadata-filter=Raven-Entity-Name=Posts`.
 - negative-metadata-filter: Filter documents NOT matching a metadata property. Usage example: `--negative-metadata-filter=Raven-Entity-Name=Posts`.
 - transform: Transform documents using a given script (import only).   
 - transform-file: Transform documents using a given script file (import only).   
 - max-steps-for-transform-script: Maximum number of steps that transform script can have (import only).
 - timeout: The timeout (in milliseconds) to use for requests.
 - batch-size: The batch size for requests.
 - chunk-size: The number of documents to import before new connection will be opened.
 - database: The database to operate on. If no specified, the operations will be on the default database.
 - username: The username to use when the database requires the client to authenticate.
 - password: The password to use when the database requires the client to authenticate.
 - domain: The domain to use when the database requires the client to authenticate.
 - api-key: The API-key to use, when using OAuth.
 - incremental: States usage of incremental operations.
 - wait-for-indexing: Wait until all indexing activity has been completed (import only).
 - excludeexpired: Excludes expired documents created by the [expiration bundle](../extending/bundles/expiration).    
 - limit: Reads at most VALUE documents/attachments.
 - help: You can use the help option in order to print the built-in options documentation.

## Filtering

To filter out documents we introduced few filtering options that can be used during import or export process.

1. `filter` is used to filter documents based on a property. E.g. if we want to export all documents with property `Name` and value `John` then we must apply command as follows: `--filter=Name=John` .   
2. `negative-filter` is an opposite to `filter` and will filter documents that does NOT match the given property.  
3. `metadata-filter` is similar to `filter`, but works on document metadata properties.   
4. `negative-metadata-filter` filters out documents that does NOT match given metadata property.   

## Transforms

Transforms can be used to modify or filter out documents, but only work during the import process. The scripts must use JavaScript syntax and be in following format:   

{CODE-BLOCK:json}
function(doc) {
	// custom code here
}
{CODE-BLOCK/}

where `doc` will contain our document with it's metadata under `@metadata` property.

#### Change scripts:   

E.g. To change document property `Name` value to the new one, the following script can be used:   

{CODE-BLOCK:json}
function(doc) {
	doc['Name'] = 'NewValue';
	return doc;
}
{CODE-BLOCK/}

#### Filter scripts:    

If we return `null` then the document will be filtered out.   

{CODE-BLOCK:json}
function(doc) {
	var id = doc['@metadata']['@id'];
	if(id === 'orders/999')
		return null;
	return doc;
}
{CODE-BLOCK/}

## SmugglerApi

Alternatively, if you prefer to do export/import from code rather than from the console utility, you can use the `SmugglerApi` class (in order to use this class you need to reference the `Raven.Smuggler.exe`).

### Exporting

{CODE smuggler_api_1@Server\Administration\ExportImport.cs /}

### Importing

{CODE smuggler_api_2@Server\Administration\ExportImport.cs /}

### Moving data between two databases

{CODE smuggler_api_3@Server\Administration\ExportImport.cs /}

## DataDumper

Smuggler communicates with server using the HTTP protocol, which means that it can't communicate with Embedded instance as long as `UseEmbeddedHttpServer` is set to `false` (which is a default value). If embedded http server can't be started then `DataDumper` (found in `Raven.Database.Smuggler` namespace in `Raven.Database.dll`) can be used to import or export data.

{CODE smuggler_api_4@Server\Administration\ExportImport.cs /}

## Remarks

{INFO During **export** Smuggler is using [document streaming](../../client-api/commands/documents/stream). To maintain backward compatibility, Smuggler will detect from what version it exports the documents and adjust behavior accordingly. /}

{INFO During **import** Smuggler is using [bulk insert operation](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation). /}

#### Related articles

TODO
