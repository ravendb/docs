#Export and import file system

Export and Import tabs enable fast data moving between file systems.

##Export

![Figure 1. Studio. Tasks. Export](images/export-1.png)  

The easiest way to export a file system is by clicking the `Export Filesystem` button. Dialog for saving file will appear and you can save the export file.

By default the name of the export file is `Dump of [FILE_SYSTEM_NAME], [YYYY-MM-DD HH-MM].ravenfsdump`, you can change it by selecting `Override file name`.

##Import

![Figure 1. Studio. Tasks. Export](images/import-1.png)  

The import follows the same procedure as the export. You can click `Choose file` button to choose a file you want and the import will start automatically. Note that this operation will overwrite existing files.

{NOTE:Note}

Since version 3.5, you can disable versioning bundle during import.

{NOTE/}

{NOTE:Disk space verification}

Verification of free disk space happens before importing data.

{NOTE/}