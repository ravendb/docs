# Import data from .ravendbdump file

A .ravendbdump file is RavenDB format for exporting a database and it is backward compatible between RavenDB versions.  
Inorder to import a `.ravendbdump` we need an existing database, lets create `Northwind` database and select `Setting`.
![Figure 1. Settings](images/setting.png "Settings")
Lets select `Import Data`
![Figure 2. Import Data](images/import-data.png "Import Data")
Now we are faced with multiple filters for importing data.
![Figure 3. Import Options](images/import-options.png "Import Options")

## Import options 

- Include Documents: Determines whether or not documents contained in the file should be imported or not, if disabled attachments and counters will automatically be disabled too. 
- Include Attachments: Determines whether or not attachments contained in the file should be imported. 
- Include Legacy Attachments: Determines whether or not legacy attachments contained in the file should be imported where legacy attachments refers to v2.x and v3.x attachments. 
- Include Counters: Determines whether or not Counters contained in the file should be imported. 
- Include Revisions: Determines whether or not Revisions contained in the file should be imported. 
- Include Conflicts: Determines whether or not Conflicts contained in the file should be imported. 
- Include Indexes: Determines whether or not Indexes contained in the file should be imported. 
- Remove Analyzers: Determines whether or not Analyzers used by indexes should be stripted or not. 
- Include Identities: Determines whether or not Identities contained in the file should be imported. 
- Include Configuration: Determines whether or not configurations of Revisions, Expiration and Client contained in the file should be imported. 
- Include Compare Exchange: Determines whether or not Compare Exchange values contained in the file should be imported. 

{NOTE:Note}
If any of the options is set but the file doesn't contain any items of that type it will not error. 
{NOTE/}

## Advanced import options
![Figure 1. Advanced import options](images/advanced-import-options.JPG "Advanced import options")

- Use Transform script: when enabled will allow to supply a transform javascript script to be operated on each document contained by the file

{CODE-BLOCK:javascript}
delete this['@metadata']['@change-vector']
{CODE-BLOCK/} 

The script above will delete the change-vector from imported documents and will generate new change vectors during import. 
This is very helpfull if the data is imported from a diffrent database group and you want to avoid adding old change vector entries to a new environment. 

- Copy command as PowerShell: generates the commands to run the importing logic from PowerShell.
