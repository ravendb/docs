# Import data from existing RavenDB server

Importing from a live URL is another option to import your data from other server, and it is backward compatible between RavenDB versions. In order to import a database
from existing RavenDB server, we need an existing database on live server. The process is straightforward and it consists of a few steps:

1. Setting up a certificates (only when importing from secured 4.x server).
2. Providing server address and credentials.
3. Customizing import options.

{PANEL: Step #1: Preparing secured 4.x servers to import process (optional)}

If your Raven 4.x is exposed over HTTPS then in order to be able to import data from it you need to register a certificate of a new destination server (also running on HTTPS) as a client certificate.
This way your new RavenDB instance will be able access existing database and fetch data from it.

In order to accomplish it please go to the RavenDB Studio where you want to import data and perform the following actions:

1. Go to [Certificate Management](../../../../server/security/authentication/certificate-management) view (`Server Management -> Certificates`)   
2. Choose `Export cluster certificates` option from `Cluster certificate` dropdown:

![Figure 1. Export server certificate](images/import-from-raven-export-server-certificate.png "Export server certificate")

Then on a source RavenDB server you need to register that certificate:

1. Go to [Certificate Management](../../../../server/security/authentication/certificate-management) view (`Server Management -> Certificates`)
2. Upload the exported certificate as the client certificate by choosing `Upload client certificate`:

    ![Figure 2. Import exported certificate as client certificate](images/import-from-raven-upload-server-cert-as-client-cert.png "Import exported certificate as client certificate")

3. Provide a meaningful name for that certificate. Security clearance `User` will provide necessary access for migration purposes. Please remember to add `Read/Write` permission to 
a database you want to import:

    ![Figure 3. Import certificate details](images/import-from-raven-upload-server-cert-as-client-cert-details.png "Import certificate details")

4. Upload it.

{PANEL/}

{PANEL: Step #2: Accessing Import From RavenDB view}

1. Select a `database` that you want to import you data into.
    ![Figure 1. Databases List](images/import-from-ravendb-db-list.png "Databases List")

2. Select `Setting` and select `Import Data` under `Tasks` submenu.
    ![Figure 2. Import Data](images/import-from-ravendb-import-data.png "Import Data")

3. Select `From RavenDB`.
    ![Figure 3. From RavenD](images/import-from-ravendb-from-ravendb.png "From RavenDB")

{PANEL/}

{PANEL: Step #3: Import configuration}

We need to specify the URL and select the database we want to import the data from.

![Figure 4. Import configuration](images/import-from-ravendb-configuration.png "Import Configuration")

1. **Server URL** - URL to server you want to import from, you can specify URL to either v4 server or v3 server.
2. **Server Version** - Version of server that you want to import from.
3. **Database Name** - The name of the Database that you want to migrate your data from.

{PANEL/}

{PANEL: Import options}

Here we can filter the data we want to import, select configuration and apply a transform script on your documents.

![Figure 5. Import Options](images/import-from-ravendb-options.png "Import Options")

- **Include Documents:** Determines whether or not documents should be imported or not, if disabled attachments and counters will automatically be disabled too. 
    - **Include Attachments:** Determines whether or not attachments should be imported. 
    - **Include Revisions:** Determines whether or not Revisions should be imported.
    - **Include Conflicts:** Determines whether or not Conflicts should be imported.
- **Include Indexes:** Determines whether or not Indexes should be imported. 
    - **Remove Analyzers:** Determines whether or not Analyzers used by indexes should be stripted or not. 
- **Include Identities:** Determines whether or not Identities should be imported.
- **Include Compare Exchange:** Determines whether or not Compare Exchange values should be imported.
- **Include Subscriptions:** Determines whether or not Subscriptions should be imported.
- **Include Configuration and OngoingTasks:** Determines whether or not [server configurations and ongoing tasks](#customize-configuration-and-ongoing-tasks) should be imported.
{NOTE:Importing item that doesn’t exist}
If any of the options is set but the other database doesn't contain any items of that type, the type will be skipped.
{NOTE/}

### Advanced import options

#### Transform Script

![Figure 6. Advanced Import Options - Transform Script](images/import-from-ravendb-advanced-transform-script.png "Advanced Import Options - Transform Script")

- Use Transform Script: when enabled will allow to supply a transform javascript script to be operated on each document contained by the file

{CODE-BLOCK:javascript}
delete this['@metadata']['@change-vector']
// The script above will delete the change-vector from imported documents
// and will generate new change vectors during import. 
// This is very helpfull if the data is imported from a diffrent database group
// and you want to avoid adding old change vector entries to a new environment. 
{CODE-BLOCK/}

{PANEL/}

### Customize Configuration and Ongoing Tasks}

![Figure 6. Advanced Import Options - Customize Configuration and Ongoing Tasks](images/import-from-ravendb-advanced-configuration-ongoing-tasks.png "Advanced Import Options - Customize Configuration and Ongoing Tasks")

**Ongoing tasks:**

- **Periodic Backups:** Determines whether or not Periodic Backups tasks configuration should be imported or not. 
- **External replications:** Determines whether or not External replications tasks configuration should be imported. 
- **RavenDB ETLs:** Determines whether or not RavenDB ETLs tasks configuration should be imported.
- **SQL ETLs:** Determines whether or not SQL ETLs tasks configuration should be imported.
- **Pull Replication Sinks:** Determines whether or not Pull Replication Sinks tasks configuration should be imported. 
- **Pull Replication Hubs:** Determines whether or not Pull Replication Hubs tasks configuration used by indexes should be stripted or not. 

**Other:**

- **Settings:** Determines whether or not Settings should be imported.
- **Conflict Solver Configuration:** Determines whether or not Conflict Solver Configuration should be imported.
- **Revisions Configuration:** Determines whether or not Revisions Configuration should be imported.
- **Document Expiration:** Determines whether or not Document Expiration settings should be imported.
- **Client Configuration:** Determines whether or not Client Configuration should be imported. 
- **Custom Sorters:** Determines whether or not Custom Sorters should be stripted or not. 

**Connection Strings:**

- **RavenDB Connection Strings:** Determines whether or not RavenDB Connection Strings should be imported.
- **SQL Connection Strings:** Determines whether or not SQL Connection Strings values should be imported.
