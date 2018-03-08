#Recovering from Voron errors 

Voron errors which are described in this article are indicators that something had horribly gone wrong.
Thus, if such errors happen, they need to be reported as soon as possible to [RavenDB support](mailto:support@ravendb.net).

##Symptoms: 
RavenDB info level logs will contain the following exceptions:  
  1. VoronUnrecoverableErrorException: `Index points to a non leaf page`  
  2. VoronUnrecoverableErrorException: `Was unable to retrieve the correct node. Data corruption possible`  
  3. VoronUnrecoverableErrorException: `Error syncing the data file. The last sync tx is...`  

##Possible Causes:
Such exceptions are caued by the corruption of a Voron data file.
First, it is possible that the data file is corrupted due to critical bug.
Also, data file corruption is likely to happen due to a hardware failure,if the file system doesn't support `fsync` functionality, or the storage hardware does not respect the `fsync` commands.

{NOTE In Unix based OS it is a `fsync` command, in Windows OS, it is a matter of creating a file with a 'Write-Through' flag. /}

For more information about this see:  
  1. For Unix based systems, see [this article](http://www.tutorialspoint.com/unix_system_calls/fsync.htm).  
  2. For Windows systems, see [this article](https://msdn.microsoft.com/en-us/library/windows/desktop/aa364218(v=vs.85).aspx).  

##Resolution:
{NOTE If the corruption is caused by a hardware failure, identify and replace the faulty disk. /}

For the best results, simply restore a new database from a backup. For more information, see an article about [backup configuration](../server/configuration/backup-configuration).  

If there is no recent/relevant backup available, it is possible to use the [Voron Recovery Tool](../glossary/voron-recovery-tool).  
This tool can be used to recover intact data from the corrupted file and import it to a newly created database.  
  
If the corruption has affected an index, it should be reset in order to restore normal functionality.
