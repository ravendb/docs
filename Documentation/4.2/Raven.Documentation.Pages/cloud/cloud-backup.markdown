# RavenDB on the Cloud: Instance Backup
---

{NOTE: }

* Your cloud instance is automatically backed up.  
* You can create backups of your own.  

* In this page:  
  * [Automatic Backup](../cloud/cloud-backup#automatic-backup)  
  * [Custom Backup](../cloud/cloud-backup#custom-backup)  
  * [Charging for your backups](../cloud/cloud-backup#charging-for-your-backups)  
  * [Backup Encryption](../cloud/cloud-backup#backup-encryption)  
{NOTE/}

---

{PANEL: Backup}

### Automatic Backup  

Your cloud instance automatically backs your documents up for you.  
This automatic backup is mandatory, and cannot be turned off.  

* A full backup is created every 24 hours.  
* An incremental backup is created every 15 minutes.  
* Documents up to 14-days-old are backed up.  
  You can contact Support to enhance this period if you want. It cannot be decreased under 14 days.  
* Backup files are kept for 14 days.  

### Custom Backup  

You can create ongoing backup tasks using your RavenDB cloud instance as you would off-cloud, with no limitations.  
Do note that backup storage is taken from whatever storage space you have left, and keep an eye on your storage costs.  

### Charging for your backups  

Your first GB of backup is free.  
If you exceed the 1 GB limit, we'll charge you 1$ per month for every additional GB.  

* If your database size is 200MB for example, the full daily backup would bring your storage usage 
  to 1.4 GB in a week, leaving 0.4 GB to be paid for.

{NOTE: }
We keep your backup files, but we do not manage or delete them for you.  
If you want to delete them to save storage space and money, please remember to do so.  
{NOTE/}

{NOTE: }
Your backup files use your storage space as any file and document would.  

* When your instance storage space is 90% used, we **automatically enhance it** to make sure no data is lost.  
  As you'll be charged for a bigger storage space, be aware of this process and delete your files if you need to.  
* Your storage space's new size will remain. It will not shrink back to its former size if you delete your documents.  

{NOTE/}


### Backup Encryption  

Backup files are always encrypted.

* When your database is NOT encrypted:  
  **We use our own account-wide backup encryption key** for the backup.  
  You will need this key in order to restore your documents.  
  To get the key, enter your account and open the Account Details page.
* When your database IS encrypted:  
  **Your own database key** will be used to encrypt your backup.  
  {NOTE: }
  **Be Aware** that RavenDB does NOT keep or manage your own encryption keys, 
  and if you ever lose them - will not be able to help you decrypt your backup or database.  
  Keep your encryption keys safe!  
  {NOTE/}

{PANEL/}


## Related articles
**Studio Articles**:  
[xxx](../../../xxx)  

**Client-API Articles**:  
[xxx](../../../xxx)  
