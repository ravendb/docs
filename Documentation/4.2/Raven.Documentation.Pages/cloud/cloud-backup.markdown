#Cloud: Backup
---

{NOTE: }

* Your cloud nodes, clusters and data are regularly and automatically backed up.  
* You can run your own ongoing backup tasks, as you would off-cloud.  
* In this page:  
  * [Automatic Backup](../cloud/cloud-backup#automatic-backup)  
  * [Charging For Your Backup storage](../cloud/cloud-backup#charging-for-your-backup-storage)  
  * [Backup Encryption](../cloud/cloud-backup#backup-encryption)  
  * [Custom Backup](../cloud/cloud-backup#custom-backup)  
{NOTE/}

---

{PANEL: }

### Automatic Backup  

Your cloud instance automatically backs up your data to ensure it is safe. This process cannot be disabled.  

* A full backup is created every 24 hours.  
* An incremental backup is created every 15 minutes.  
* Backup files are saved for a minimum of 14 days, as per our retention policy.  
  You can [contact Support](../cloud/cloud-control-panel#the-support-tab) to extend this retention period, but it **cannot 
be reduced to less than 14 days.**  

---

### Charging For Your Backup storage  

Backup storage up to 1 GB is free. Your backup storage usage will be measured once a day. Each month you will be charged 
based on the *average* amount by which you exceed the 1 GB limit.

{NOTE: } 
Backup files are compressed.  
{NOTE/}

{NOTE: } 
To ensure that your data is safe and can be restored if necessary, **we do not delete backup files before 14 days have passed.**
{NOTE/}

---

### Backup Encryption  

Backup files are always encrypted.

* If the database being backed up is NOT encrypted:  
  We will encrypt it using an encryption key that **we** manage, unique to your account.  
* If your database IS encrypted:  
  **Your own database encryption key** will be used to encrypt the backup as well.  
  
  {WARNING: }
  **Be aware** that RavenDB does NOT keep or manage your own database encryption keys.  
  If you lose them we will NOT be able to help you decrypt your backup files or database.  
  Keep your encryption keys safe!  
  {WARNING/}

---

### Custom Backup  

You can create your own [ongoing backup tasks](https://ravendb.net/docs/article-page/4.2/Csharp/studio/database/tasks/ongoing-tasks/backup-task) 
on your RavenDB cloud instance as you would off-cloud.  

{PANEL/}

