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

[The structure of the RavenDB directories](directory-structure) cannot be changed. An exception is locations of temporary files for [documents](../../server/configuration/storage-configuration#storage.temppath) and [indexes](../../server/configuration/indexing-configuration#indexing.temppath), that can be changed by setting appropriate configuration options.  

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

## Related Articles

### Storage

- [Directory Structure](directory-structure)
- [Storage Engine](../../server/storage/storage-engine)

### Installation

- [Deployment Considerations](../../start/installation/deployment-considerations)
