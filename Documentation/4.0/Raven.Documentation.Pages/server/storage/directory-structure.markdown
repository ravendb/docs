# Storage : Directory Structure

{PANEL:RavenDB Data}

RavenDB keeps all data in a location specified in [`DataDir`](../../server/configuration/core-configuration#datadir) setting. 
The structure of RavenDB data directories are as follows:

* _{DataDir}_
  * `Databases`
      * _<database-name>_
          * `Confguration`
              * `Journals`
              * `Temp`
          * `Indexes`
              * _<index-name>_
                  * `Journals`
                  * `Temp`
              * _...more indexes..._
          * `Journals`
          * `Temp`
      * _...more databases..._
  * `System`
      * `Journals`
      * `Temp`

The main directory has a `Databases` folder, which contains subdirectories per each database, and a `System` folder where server-wide data are stored (e.g. database records, cluster data).

The database is composed of such data items as documents, indexes, and configuration. Each of them is a separate [Voron](../../server/storage/storage-engine) storage environment.
The data is persisted in a `Raven.voron` file and `.journal` files which are located in the `Journals` directory. In addition, temporary files are put into the `Temp` folder.

{PANEL/}

{PANEL:Storing Data in Custom Locations}

The structure of RavenDB directories cannot be changed except locations of temporary files for [documents](../../server/configuration/storage-configuration#storage.temppath) and [indexes](../../server/configuration/indexing-configuration#indexing.temppath) by setting appropriate configuration options.

However, you can store any RavenDB data in different locations by defining junction points (Windows) or mount points (Linux).

### Example - Moving Journals

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

### Example - Moving Indexes

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

{PANEL/}
