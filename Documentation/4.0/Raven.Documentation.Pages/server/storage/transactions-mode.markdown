# Transaction Mode
##### End Point : /databases/*/admin/transactions-mode&mode=< Safe | Lazy | Danger >
#### Description
Transactions in RavenDB are written first to journal files, and only afterwards copied to data file. The journals, functioning as transactions log, are being write while preserving transactions ACID properties. In the event of non graceful server shutdowns, the journals are the source of transactions recovery on next startup.
Each transaction, written to the journal in a way it can be recovering from there immediately if necessary, and only then the client get notification on successful transaction commit.

The described above are the default behaviour, and named as _Safe_.  There are different modes which can be changed per database : _Lazy_ and _Danger_.  When set - if not changed within 24 hours, RavenDB will automatically change the mode to _Safe_. See https://ravendb.net/docs/article-page/4.0/csharp/server/configuration/storage-configuration#storage.transactionsmodedurationinmin

_Lazy_ mode tells RavenDB to store transactions in a temporary memory mapped journal file, which means the data is not written to the disk all the time. The time when it happens is when the 256MB of journal is filled (32MB in 32 bit OS) and a new journal needs to be opened.  The previous one is flushed into the datadfile.
By storing journals transaction directly into memory, we gain speed over a chance to lose few latest transactions.

_Danger_ mode as it sounds is dangerous to use. It means data will be written to the disk without bypassing the machine and disk memory buffers. In the event of non graceful shutdown, the journal file might be corrupted. The data in the journal might enter inconsistent non recoverable state.

The benefit is, therefore, speed over data integration in case of _Danger_ mode and speed with the price of possible transactions lose in _Lazy_ mode.

#####Example
```
curl http://127.0.0.1:8080/databases/myDB/admin/transactions-mode?mode=Safe
```
