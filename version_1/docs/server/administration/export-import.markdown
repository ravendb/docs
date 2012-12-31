# Exporting and Importing data

In order to export or import data from a RavenDB server, you can use the Raven.Smuggler utility.

Raven.Smuggler is distributed both in the RavenDB [distrubution package](http://builds.hibernatingrhinos.com/Builds/RavenDB) and in any of the shipped [nuget packages](http://ravendb.net/docs/intro/quickstart/adding-ravendb-to-your-application?version=1.0#installing-using-nuget). It is located under the `/Tools` folder.

Using the Smuggler utility is necessary when trying to move a RavenDB Data folder around between servers. Simply copying it is not supported and can result in server errors.

## Exporting

To Export data, use this command:

    Raven.Smuggler out http://localhost:8080 dump.raven

This command will export all indexes, documents and attachments from the local RavenDB instance to a file named `dump.raven`.

The dump file will also include documents that were added during the export process, so you can make changes while the export is executing.

## Importing

    Raven.Smuggler in http://localhost:8080 dump.raven

This command will import all the indexes, documents and attachments from the file to the local instance. 

{NOTE This will _overwrite_ any existing document on the local instance. /}

You can continue using that RavenDB instance while data is being imported to it.

##Incremental Export and Import
With the incremental export operation we can use in order to backup the database incrementally, on each export, we will only take the export the documents create or updated
since the last export.

To export data with incremental we can use 2 options.  
If it is the first run and the folder does not exist yet use (you can continue to use this command every time):

    Raven.Smuggler out http://localhost:8080 folder_location --incremental

If you ran the command before or you created the folder you can use:

    Raven.Smuggler out http://localhost:8080 folder_location


In order to import date that was exported with incremental operation, you can use either of the following:

    1) Raven.Smuggler in http://localhost:8080 folder_location --incremental
    2) Raven.Smuggler in http://localhost:8080 folder_location

## Command line options

You can tweak the export/import process with the following parameters:

 - operate-on-types: Specify the types to export/import. Usage example: `--operate-on-types=Indexes,Documents,Attachments`.
 - filter: Filter documents by a document property. Usage example: `--filter=Property-Name=Value`.
 - metadata-filter: Filter documents by a metadata property. Usage example: `--metadata-filter=Raven-Entity-Name=Posts`.
 - database: The database to operate on. If no specified, the operations will be on the default database.
 - username: The username to use when the database requires the client to authenticate.
 - password: The password to use when the database requires the client to authenticate.
 - domain: The domain to use when the database requires the client to authenticate.
 - api-key: The API-key to use, when using OAuth.
 - help: You can use the help option in order to print the built-in options documentation.

## SmugglerApi

Alternatively, if you prefer to do export/import from code rather than from the console utility, you can use the SummglerApi class. In order to use this class you need to reference the Raven.Smuggler.exe.

Usage example:

{CODE smuggler-api@Server\Smuggler.cs /}

In the above code we exporting all of the data on the server, which is the documents, indexes and attachments, and than importing just the documents and the indexes. In this example the import would overwrite the existing documents, so if you want to import to another database you'll need to create another instance of the SmugllerApi with a different connection string options.