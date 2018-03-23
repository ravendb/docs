#Recovering from Voron Errors 
---

Voron errors described in this article are indicators that something had horribly gone wrong.  
If such errors happen, they need to be reported as soon as possible to [RavenDB support](mailto:support@ravendb.net).  

{PANEL: Symptoms}  

* RavenDB info level logs will contain the following exceptions:
  * **VoronUnrecoverableErrorException**: `Index points to a non leaf page`  
  * **VoronUnrecoverableErrorException**: `Was unable to retrieve the correct node. Data corruption possible`  
  * **VoronUnrecoverableErrorException**: `Error syncing the data file. The last sync tx is...`  

{PANEL/} 

{PANEL: Possible Causes}  

* Such exceptions are caused by the corruption of a Voron _data file_, which could occur due to a few reasons:  
  * Hardware failure of hard-drive or memory  
  * In compliance‏ with RavenDB [hardware and OS requeirements](#ravendb-hardware-and-os-requirements)  
  * Critical bug in `Voron`

{PANEL/}

{PANEL: RavenDB Hardware And OS Requirements}

* A _filesystem_ and _hard-drive_ backing up a RavenDB server should have the following properties:
  * support for `fsync`  
  * support for `write-through` on windows and `O_DIRECT` on Linux based OS.  
 
 Read more about `fsync` across platforms [here](https://www.humboldt.co.uk/fsync-across-platforms/)
  
{PANEL/}

{PANEL: Resolution}

For the best results, simply restore a new database from a backup.  
For more information, read this article about [backup configuration](../server/configuration/backup-configuration).  

If there is no recent/relevant backup available, it is possible to use the [Voron Recovery Tool](../server/troubleshooting/voron-recovery-tool).  
This tool can be used to recover intact data from the corrupted file and import it to a newly created database.  
  
If the corruption has affected an index, it should simply be reset in order to restore normal functionality.  

{NOTE If the corruption is caused by a hardware failure, identify and replace the faulty disk. /}

{PANEL/}
