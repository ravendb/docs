# Exporting and Importing data

To export or import data from a RavenDB server use the Raven.Smuggler utility that comes with the distrubution or nuget packages. It is located under the `/Tools` folder.

Using the Smuggler utility is necessary when trying to move a RavenDB Data folder around between servers. Simply copying it is not supported and can result in server errors.

## Exporting

To Export data, use this command:

    Raven.Smuggler out http://localhost:8080 dump.raven

This command will export all indexes and documents from the local RavenDB instance to a file named `dump.raven`.

The dump file will also include documents that were added during the export process, so you can make changes while the export is executing.

Specifying `--only-indexes` in the command line will export only the index definitions. To include attachments as well (might result in larger export file), specify `--include-attachments`.

## Importing

    Raven.Smuggler in http://localhost:8080 dump.raven

This command will import all the indexes and documents from the file to the local instance. 

{NOTE This will _overwrite_ any existing document on the local instance. /}

You can continue using that RavenDB instance while data is being imported to it.