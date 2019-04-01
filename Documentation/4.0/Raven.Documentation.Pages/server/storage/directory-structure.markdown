# Storage: Directory Structure

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

## Related Articles

### Storage

- [Customize Data Location](../../server/storage/customizing-raven-data-files-locations)
- [Storage Engine](../../server/storage/storage-engine)
- [Transaction Mode](../../server/storage/transaction-mode)
