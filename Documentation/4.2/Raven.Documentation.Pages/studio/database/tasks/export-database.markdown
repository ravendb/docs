# Export Database to a .ravendbdump file

A `.ravendbdump` file is the RavenDB format for exporting/importing a database. It is backwards compatible between RavenDB versions. 
In order to export a `.ravendbdump` file we need an existing database. Let's select a database and navigate to `Settings`.  

![Figure 1. Settings](images/export-database-select.png "Databases List")

Select `Export Data` under `Tasks` submenu.

![Figure 2. Export Database](images/export-database-export-database.png)

## Export options 

Here you can filter the data you want to export, add encryption, select collections, configurations and apply a transform script on your documents.

![Figure 3. Export Options](images/export-database-options.png "Export Options")

- **Include Documents:** Determines whether or not documents contained in the database should be exported or not, if disabled _Attachments_ and _Counters_ will automatically be disabled too. 
    - **Include Attachments:** Determines whether or not attachments contained in the database should be exported. 
    - **Include Counters:** Determines whether or not Counters contained in the database should be exported. 
    - **Include Revisions:** Determines whether or not Revisions contained in the database should be exported.
    - **Include Conflicts:** Determines whether or not Conflicts contained in the database should be exported.
- **Include Indexes:** Determines whether or not Indexes contained in the database should be exported. 
    - **Remove Analyzers:** Determines whether or not Analyzers used by indexes contained in the file should be stripted or not. 
- **Include Identities:** Determines whether or not Identities contained in the database should be exported.
- **Include Compare Exchange:** Determines whether or not Compare Exchange values contained in the database should be exported.
- **Include Subscriptions:** Determines whether or not Subscriptions contained in the database should be exported.
- **Include Configuration and OngoingTasks:** Determines whether or not [server configurations and ongoing tasks](#customize-configuration-and-ongoing-tasks) should be exported.

#### Encrypt exported file

Used to add an ecryption key when exporting database to encrypted file.

## Advanced export options

### Export all collections

![Figure 4. Advanced Export Options - Export all collections](images/export-database-advanced-collections.png "Advanced export options - Export all collections")

- **Export all collections:** Determines whether or not All database collections should be exported.
    - If _Export all collections_ is disabled list of all database collections will be displayed with the ability to filter collections by name.

---

### Transform Script

![Figure 5. Advanced Export Options - Transform Script](images/export-database-advanced-transfrom-script.png "Advanced export options - Transform Script")

- Use Transform Script: when enabled will allow to supply a transform javascript script to be operated on each document contained by the file

{CODE-BLOCK:javascript}
var id = doc['@metadata']['@id'];
if (id === 'orders/999')
    throw 'skip'; // filter-out
{CODE-BLOCK/}

---

### Customize Configuration and Ongoing Tasks

![Figure 6. Advanced Export Options - Customize Configuration and Ongoing Tasks](images/export-database-advanced-configuration.png "Advanced export options - Customize Configuration and Ongoing Tasks")

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

---

### Copy command as PowerShell

- Generates the commands to run the exporting logic from PowerShell.
