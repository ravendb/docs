# Lazy Transactions 
##### End Point : /databases/*/admin/lazy-transaction?mode= < true > | < false >
#### Description
RavenDB, when Lazy Transaction mode set to `true`, uses temporary mapped files for journals.  This means transactions are written to non persistent memory instead of writing them on the disk (in journal files).
Only when journal is full (by default 256MB) or when datafile (Raven.voron) is flushed with the journal content - the data is synced from memory onto the physical drive. 
Therefor, if RavenDB is shutdown abnoramally during Lazy Transaction mode, the last unsynced journal transactions will be lost.  The benfite is higher writing speed to journals on slow IO machines.  At anycase when RavenDB starts up again and perform journal recovery, the previous lazy transaction mode session should not affect / corrupt the already synced data.
Setting mode to `false` will write the data in memory to the disk (in the journal file). 

#####Example
```
http://127.0.0.1:8080/databases/myDB/admin/lazy-transaction?mode=true
```
