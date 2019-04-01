# Storage: Customizing RavenDB Data Files Locations

##  Storing RavenDB data files in different devices, by customizing their locations.

{PANEL: Overview}

{NOTE: Motivation:}

* Avoiding traffic jams.

* Better concurrency.

* Directing each file or directory (e.g. `Raven.voron` file, `Journals`, etc.) to a data storage according to its speed, durability, etc.

{NOTE/}

{NOTE: Main components:}

* **Journals** - files for unbuffered, sequential writes, typically write only (except for recovery operations).  
`Journals` are also in the critical path for commit operations, since a commit is not confirmed until the write is done.

* **Raven.voron files** - memory mapped, buffered files with random reads and writes.  
Write operations are async, but slow devices may cause slow downs (especially on reads).

{NOTE/}

Each database includes a `Journals`, a `Temp` and an `Indexes` folder, and a `Raven.voron` data file.  
Each index folder has its own `Journals` and `Temp` folders, and a `Raven.voron` file.  
All these components (`Journals`, `Raven.voron`, etc.) operate concurrently with each other and can be stored on different devices.  
It allows you to organize the files based on their usage pattern and the performance of the different devices you own. 

{PANEL/}

{PANEL: Practice}

[The structure of the RavenDB directories](directory-structure) cannot be changed. An exception is locations of temporary files for [documents](../../server/configuration/storage-configuration#storage.temppath) and [indexes](../../server/configuration/indexing-configuration#indexing.temppath), that can be changed by setting the [Storage.TempPath](../../server/configuration/storage-configuration#storage.temppath) configuration option.  

However, you can store any RavenDB data files or directories in whatever location you choose, by defining junction points (Windows) or mount points (Linux).

{NOTE: Example - Moving Journals}

A common practice is to store the journals on a very fast drive to achieve better write performance.
The following command will point the `Journals` directory of the _Northwind_ database to a path on a different drive.

#### Windows

{CODE-BLOCK:powershell}
C:\RavenDB\Server\RavenData\Databases\Northwind>mklink /J Journals E:\Journals\Northwind
{CODE-BLOCK/}

#### Linux

{CODE-BLOCK:bash}
 ln -s ~/RavenDB/Server/RavenData/Databases/Northwind/Journals /mnt/FastDrive/Databases/Northwind/Journals
 {CODE-BLOCK/}

{NOTE/}

{NOTE: Example - Moving Indexes}

If you want to store the data of _all_ indexes of the _Northwind_ database in the custom location, you can use the following command:

#### Windows

{CODE-BLOCK:powershell}
C:\RavenDB\Server\RavenData\Databases\Northwind>mklink /J Indexes D:\Indexes\Northwind
{CODE-BLOCK/}

{INFO Creation of junction / mount points requires the database to be offline /}

{INFO If data already exists in the directory, and you want to define the junction (Windows) or mount (Linux) point, you need to create a backup of the data first, and copy it back into the directory after executing the command. /}

#### Linux

{CODE-BLOCK:bash}
ln -s ~/RavenDB/Server/RavenData/Databases/Northwind/Indexes /mnt/FastDrive/Databases/Northwind/Indexes 
{CODE-BLOCK/}

{INFO Start RavenDB server _after_ creating soft link to a faster drive mount point: /}

{NOTE/}

{PANEL/}

{PANEL: Automation}

To help you automate the process, we have added the [Storage.OnDirectoryInitialize](../../server/configuration/storage-configuration#storage.ondirectoryinitialize.exec) extension point.
Whenever RavenDB creates or opens a directory, it will invoke a process of your choice.
It allows you to create a script with your own logic, defining juction points as needed.

RavenDB will invoke the process with [optional user arguments](../../server/configuration/storage-configuration#storage.ondirectoryinitialize.exec.arguments) followed by:  

* The environment type (System, Database, Index, Configuration, Compaction)
* The database name
* Path of the `DataDir` directory
* Path of the `Temp` directory
* Path of the `Journals` directory

Let's look at an example which demonstrates how the mechanism works.  
Here is a very simple powershell script which will append a line to a text file every time it is called. The path of the output text file is supplied as a user argument.

{CODE-BLOCK:powershell}
param([string]$userArg ,[string]$type, [string]$name, [string]$dataPath, [string]$tempPath, [string]$journalPath)
Add-Content $userArg "$type $name $dataPath $tempPath $journalPath\r\n"
exit 0
{CODE-BLOCK/}

We supply this script to RavenDB via the [Storage.OnDirectoryInitialize](../../server/configuration/storage-configuration#storage.ondirectoryinitialize.exec) configuration option:

{CODE-BLOCK:json}
{
    "Setup.Mode": "None",
    "ServerUrl": "http://127.0.0.1:8080",
    "License.Eula.Accepted": true,
    "Storage.OnDirectoryInitializeExec" :"powershell",
    "Storage.OnDirectoryInitializeExecArguments" :"c:\\example\\script.ps1 c:\\example\\outFile.txt"
}
{CODE-BLOCK/}

When launching the RavenDB server and creating the `Northwind` sample data, the script is invoked 6 times.  
Following the example above, the content of outFile.txt will be:

{CODE-BLOCK:plain}
{
System System C:\Raven4\Server\System C:\Raven4\Server\System\Temp C:\Raven4\Server\System\Journals\r\n
Configuration Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Configuration C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Configuration\Temp C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Configuration\Journals\r\n
Database Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Temp C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Journals\r\n
Index Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_ByCompany C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_ByCompany\Temp C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_ByCompany\Journals\r\n
Index Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Product_Search C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Product_Search\Temp C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Product_Search\Journals\r\n
Index Northwind C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_Totals C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_Totals\Temp C:\Raven4\Server\System\Temp C:\Raven4\Server\Databases\Northwind\Indexes\Orders_Totals\Journals\r\n
}
{CODE-BLOCK/}



{PANEL/}

## Related Articles

### Storage

- [Directory Structure](directory-structure)
- [Storage Engine](../../server/storage/storage-engine)
- [Transaction Mode](../../server/storage/transaction-mode)

### Installation

- [Deployment Considerations](../../start/installation/deployment-considerations)
