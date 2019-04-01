# Tasks: Import & Export Database

Import and Export Tabs enable fast data moving between databases. For data export, the following tools can be used:

- [Smuggler](../../../server/administration/exporting-and-importing-data),
- Export Database Tab from Studio (which uses Smuggler underneath)

## Export

![Figure 1. Tasks. Import and Export Database Tab.](images/tasks-import_and_export_database_tab-1.png)

The easiest way to export a database is by clicking the `Export Database` button. Dialog for saving file will appear and default export options will be applied (documents, indexes and transformer included).

To change default options you need to choose desired options in `General` tab or, if necessary, change **batch size**, toggle if export should **include or exclude expired documents**, add **filters**, or **transform script** in the `Advanced` tab. You can read more about these functionalities [here](../../../server/administration/exporting-and-importing-data#filtering).

![Figure 2. Tasks. Export Database Tab. General Tab.](images/tasks-export_database_tab-general-2.png)

![Figure 3. Tasks. Export Database Tab. Advanced Tab.](images/tasks-export_database_tab-advanced-3.png)

{NOTE:Information}

When exporting database the equivalent Smuggler command will be shown in the bottom of the view.

{NOTE/}

<hr />

## Import

Import follows the same procedure as export. You can click the `Choose file` button to choose a file you need and import will start automatically, with default options. Import options can be changed in `General` or `Advanced` tab and this must be done prior choosing the file for import.

![Figure 4. Tasks. Import Database Tab. General Tab.](images/tasks-import_database_tab-general-4.png)

![Figure 5. Tasks. Import Database Tab. Advanced Tab.](images/tasks-import_database_tab-advanced-5.png)

{NOTE:Note}

Since version 3.5, you can disable versioning bundle and strip synchronization information from files metadata during import.

{NOTE/}

{NOTE:Disk space verification}

Verification of free disk space happens before importing data.

{NOTE/}

{DANGER Importing will overwrite any existing documents, indexes and transformers. /} 
