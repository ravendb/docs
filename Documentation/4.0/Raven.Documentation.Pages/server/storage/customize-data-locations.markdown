# Storage : Storing Data in Custom Locations and Seperate Devices

{PANEL: Theory}

{NOTE: The motivation to separate devices by customizing data locations, may be:}

* Avoiding traffic jams.

* Better concurrency.

* To focos specific performence (e.g. speed / durability) to specific data (jurnal/voron of system/database/index).

{NOTE/}

{NOTE: The main players are:}

* **Journals** - unbuffered single writing that typically only written to (Except for recovery). They also detain commit operations, since commit will not be confirmed until the writing is done.

* **Raven.voron files** - buffered random writing. They do not detain any operation, but slow devices may cause to memory load.

{NOTE/}

Each database includes journal, Raven.voron file and indexs. Each index includes journal and Raven.voron file.

All that parts operate concurrecy with the others.

Every part can be seperate to differnt device and the exact arrangement depends upon the specific server instance.

{PANEL/}

{PANEL: Practice}

[The structure of RavenDB directories](directory-structure) cannot be changed except locations of temporary files for [documents](../../server/configuration/storage-configuration#storage.temppath) and [indexes](../../server/configuration/indexing-configuration#indexing.temppath) by setting [appropriate configuration options](../configuration/storage-configuration).

However, you can store any RavenDB data in different locations by defining junction points (Windows) or mount points (Linux).

{NOTE: Example - Moving Journals}

A common practice is to store the journals on a very fast drive to achieve better write performance.
The following command will point the `Journals` directory of _Northwind_ database to path on a different drive.

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

If you want to store the data of _all_ indexes of _Northwind_ database in the custom location, you can use the following command:

#### Windows

{CODE-BLOCK:powershell}
C:\RavenDB\Server\RavenData\Databases\Northwind>mklink /J Indexes D:\Indexes\Northwind
{CODE-BLOCK/}

{INFO Creation of junction / mount points requires a database to be offline /}

{INFO If data already exists in the directory, you want to define the junction / mount point for you need to backup it first and copy back after executing the command. /}

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
- [Storage Engine](../../storage/storage-engine)
- [Transaction Mode](../../server/storage/transaction-mode)
