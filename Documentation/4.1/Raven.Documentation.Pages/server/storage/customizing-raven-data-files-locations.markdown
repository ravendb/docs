# Customizing Data Files Locations

---

{NOTE: }

* The structure of [RavenDB directories](../../server/storage/directory-structure) **cannot** be changed. However:  
    * Path for temporary files can be customized.
    * Data files can be stored in different locations,  
      by defining junction points (Windows) or mount points (Linux).  
    * A script can be used to automate the different location definitions.  

* In this page:
    * [Why store data on different devices](../../server/storage/customizing-raven-data-files-locations#why-store-data-on-different-devices)
    * [Configuring temp files location](../../server/storage/customizing-raven-data-files-locations#configuring-temp-files-location)
    * [Configuring data files location](../../server/storage/customizing-raven-data-files-locations#configuring-data-files-location)
    * [Automate storage definitions](../../server/storage/customizing-raven-data-files-locations#automate-storage-definitions)
        * [Script example - Basic usage](../../server/storage/customizing-raven-data-files-locations#script-example---basic-usage)
        * [Script example - Point Indexes to different location](../../server/storage/customizing-raven-data-files-locations#script-example---point-indexes-to-different-location)

{NOTE/}

---

{PANEL: Why store data on different devices}

* A file or directory can be redirected to a different storage location according to its speed, durability, etc.  
* This allows organizing the files based on their usage pattern and the performance of the different devices you own.  
  i.e. slow devices may cause a slow down when reading from _Raven.voron_.  
* [Voron components](../../server/storage/directory-structure#voron-storage-environment) operate concurrently.  
  Storing them to different locations can help avoid traffic jams and achieve better concurrency.  

{PANEL/}

{PANEL: Configuring temp files location}

* **Databases temporary files**  
  By default, all databases' temporary files are written to the `Temp` folder under each Database directory.  
  Customize the files path by setting configuration option [Storage.TempPath](../../server/configuration/storage-configuration#storage.temppath) in your _settings.json_ file.  

* **Indexes temporary files**  
  By default, all indexes' temporary files are written to the `Temp` folder under each Index directory.  
  Customize the files path by setting configuration option [Indexing.TempPath](../../server/configuration/indexing-configuration#indexing.temppath) in your _settings.json_ file.  

* **Backup temporary files**  
  By default, backup temporary files are written under the `Database` directory or under `Storage.TempPath` if defined.  
  Customize the files path by setting configuration option [Backup.TempPath](../../server/configuration/backup-configuration#backup.temppath) in your _settings.json_ file.  

{PANEL/}

{PANEL: Configuring data files location}

* You can store RavenDB data files in any directory of your choice.  
  This is done by defining __junction points__ (Windows) or __mount points__ (Linux).

* If data already exists in the directory, and you want to define the junction / mount point,  
  you need to create a backup of the data first and copy it back into the directory after executing the command.

* The database must be offline when moving the database folder itself to a new point.  


{NOTE: }
__Example - Moving Journals__

A common practice is to store the journals on a very fast drive to achieve better write performance.
The following command will point the `Journals` directory of the _Northwind_ database to a path on a different drive.

{CODE-BLOCK:powershell}
# Windows:
C:\RavenDB\Server\RavenData\Databases\Northwind>mklink /J Journals E:\Journals\Northwind
{CODE-BLOCK/}

{CODE-BLOCK:bash}
# Linux:
ln -s /mnt/FastDrive/Databases/Northwind/Journals ~/RavenDB/Server/RavenData/Databases/Northwind/Journals
{CODE-BLOCK/}

{NOTE/}

{NOTE: }
__Example - Moving Indexes__

To store the data of all indexes of the _Northwind_ database in the custom location,
you can use the following command to to point the `Indexes` directory to a new location:

{CODE-BLOCK: powershell}
# Windows:
C:\RavenDB\Server\RavenData\Databases\Northwind>mklink /J Indexes D:\Indexes\Northwind
{CODE-BLOCK/}

{CODE-BLOCK: bash}
# Linux:
ln -s /mnt/FastDrive/Databases/Northwind/Indexes ~/RavenDB/Server/RavenData/Databases/Northwind/Indexes 
{CODE-BLOCK/}

{NOTE/}
{PANEL/}

{PANEL: Automate storage definitions}

* To help automate the process, we have added the [on directory initialize](../../server/configuration/storage-configuration#storage.ondirectoryinitialize.exec) configuration option.  
  Whenever RavenDB __creates a directory__, it will invoke the process that is defined within that configuration.

* The process is called just before the directory is created.  
  This allows you to create a script with your own logic, defining junction/mount points as needed.  

---

* RavenDB will invoke the process with the following params:

    * Params passed by the user: 
        * User arguments - optional params, set in the [optional user arguments](../../server/configuration/storage-configuration#storage.ondirectoryinitialize.exec.arguments) configuration option
  
    * Params passed by RavenDB:
        * The environment type (System, Database, Index, Configuration, Compaction)
        * The database name
        * Path of the `DataDir` directory
        * Path of the `Temp` directory
        * Path of the `Journals` directory

---

{NOTE: }
#### Script example - Basic usage

The following is a very simple PowerShell script example, here only to show basic usage.  
The script is Not modifying any file location, it will only print out the value of the script params into a text file.  

{CODE-BLOCK:powershell}
# script.ps1

param([string]$userArg ,[string]$type, [string]$name, [string]$dataPath, [string]$tempPath, [string]$journalPath)
Add-Content $userArg "$type $name $dataPath $tempPath $journalPath\r\n"
exit 0
{CODE-BLOCK/}

The output file _outFile.txt_ is supplied as a user argument.  
Add the script path and its user arguments to the _settings.json_ file as follows:

{CODE-BLOCK:json}
{
    "Setup.Mode": "None",
    "ServerUrl": "http://127.0.0.1:8080",
    "License.Eula.Accepted": true,
    "Storage.OnDirectoryInitialize.Exec" :"powershell",
    "Storage.OnDirectoryInitialize.Exec.Arguments" :"c:\\scripts\\script.ps1 c:\\scripts\\outFile.txt"
}
{CODE-BLOCK/}
  
When launching the server and creating the Northwind database with the Northwind sample data, the script is invoked every time a directory is created.  
Each line in _outFile.txt_ shows the values passed to the script when it was called.

{CODE-BLOCK:plain}
{
System System C:\RavenDB\Server\System C:\RavenDB\Server\System\Temp C:\RavenDB\Server\System\Journals\r\n
Configuration Northwind C:\RavenDB\Server\Databases\Northwind\Configuration C:\RavenDB\Server\Databases\Northwind\Configuration\Temp C:\RavenDB\Server\Databases\Northwind\Configuration\Journals\r\n
Database Northwind C:\RavenDB\Server\Databases\Northwind C:\RavenDB\Server\Databases\Northwind\Temp C:\RavenDB\Server\Databases\Northwind\Journals\r\n
Index Northwind C:\RavenDB\Server\Databases\Northwind\Indexes\Orders_ByCompany C:\RavenDB\Server\Databases\Northwind\Indexes\Orders_ByCompany\Temp C:\RavenDB\Server\Databases\Northwind\Indexes\Orders_ByCompany\Journals\r\n
Index Northwind C:\RavenDB\Server\Databases\Northwind\Indexes\Product_Search C:\RavenDB\Server\Databases\Northwind\Indexes\Product_Search\Temp C:\RavenDB\Server\Databases\Northwind\Indexes\Product_Search\Journals\r\n
// ... + more lines per index folder created
}
{CODE-BLOCK/}

{NOTE/}

{NOTE: }
#### Script example - Point Indexes to different location

The following bash script example will point the Indexes data to a new location.  

If the environment type is other than 'Database' the script will exit.  
Else, the script will create a soft link for the `Indexes` directory.  

After the script is run, the Indexes link will reside under the Database directory (See [Directory Structure](../../server/storage/directory-structure)).  
The link will point to the new location set by variable $INDEXES_TARGET_DIR_NAME where all indexes' data will be written.  

{CODE-BLOCK:bash}
#!/bin/bash
# bash ./your-script USER_ARGS Database DB_NAME BASE_PATH TEMP_PATH JOURNALS_PATH

# Use directory names as defined on your machine
RDB_DATA_DIR="/var/lib/ravendb/data"
INDEXES_TARGET_DIR="/mnt/ravendb-indexes"

DIR_TYPE="$1"
DB_NAME="$2"

if [ "$DIR_TYPE" != 'Database' ]; then
exit 0
fi

INDEXES_SOURCE_DIR_NAME="${RDB_DATA_DIR}/Databases/${DB_NAME}/Indexes"
INDEXES_TARGET_DIR_NAME="${INDEXES_TARGET_DIR}/${DB_NAME}/Indexes"


if [ -d "$INDEXES_SOURCE_DIR_NAME" ] && [ ! -L "$INDEXES_SOURCE_DIR_NAME" ]; then
# If Indexes directory exists then exit - need manual handling
echo "FATAL: Directory $INDEXES_SOURCE_DIR_NAME already exists."
exit 1
fi

if [ -L "$INDEXES_SOURCE_DIR_NAME" ]; then
exit 0
fi

mkdir -p "$INDEXES_TARGET_DIR_NAME"

ln -s "$INDEXES_TARGET_DIR_NAME" "$INDEXES_SOURCE_DIR_NAME"
{CODE-BLOCK/}

Add the script to the _settings.json_ file as follows:  

{CODE-BLOCK:json}
{
"Setup.Mode": "None",
"ServerUrl": "http://127.0.0.1:8080",
"License.Eula.Accepted": true,
"Storage.OnDirectoryInitialize.Exec" :"bash",
"Storage.OnDirectoryInitialize.Exec.Arguments" :"/scripts/your-script.sh"
}
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

## Related Articles

### Storage

- [Directory Structure](directory-structure)
- [Storage Engine](../../server/storage/storage-engine)

### Installation

- [Deployment Considerations](../../start/installation/deployment-considerations)
