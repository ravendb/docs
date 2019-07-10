#Cloud: Backup
---

{NOTE: }

* RavenDB cloud instances of the [free](cloud-instances#cloud-free-node) and [production](cloud-instances#cloud-production-cluster) tiers 
are regularly and automatically backed up.  
* You can also define your own backup tasks as you would off cloud.  
* In this page:  
  * [Automatic Backup](../cloud/cloud-backup#automatic-backup)  
  * [Charging For Your Backup storage](../cloud/cloud-backup#charging-for-your-backup-storage)  
  * [Backup Encryption](../cloud/cloud-backup#backup-encryption)  
  * [Custom Backup](../cloud/cloud-backup#custom-backup)  
{NOTE/}

---

{PANEL: Automatic Backup}

Your cloud instance automatically backs up your data to ensure it is safe. This process cannot be disabled.  

* A [full](../server/ongoing-tasks/backup-overview#backup-scope-full-or-incremental) backup is created every 24 hours.  
* An [incremental](../server/ongoing-tasks/backup-overview#backup-scope-full-or-incremental) backup is created every 15 minutes.  
* Backup files are saved for a minimum of 14 days, as per our retention policy.  
  You can [contact support](../cloud/cloud-portal/cloud-portal#the-support-tab) to extend this retention period, but it **cannot 
be reduced to less than 14 days.**  

{PANEL/}

{PANEL: Charging For Your Backup storage}

Backup storage up to 1 GB is free. Your backup storage usage will be measured once a day. Each month you will be charged 
based on the *average* amount of storage you used per day.

{NOTE: } 
Backup files are compressed.  
To ensure that your data is safe and can always be restored if necessary, **we do not delete backup files before 14 days have passed.**
{NOTE/}

{PANEL/}

{PANEL: Backup Encryption}

Backup files are always encrypted.

* If your database is NOT encrypted:  
  We will encrypt its backup files using an encryption key that **we** manage and which is unique to your account.  
* If your database IS encrypted:  
  **Your own database encryption key** will be used to encrypt the backup as well.  
  
  {WARNING: }
  Be aware that RavenDB does NOT keep or manage *your own* database encryption keys.  
  If you lose them we will NOT be able to help you decrypt your encrypted database or their backup files.  
  Keep your encryption keys safe!  
  {WARNING/}

{PANEL/}

{PANEL: Custom Backup}

You can create your own [ongoing backup tasks](https://ravendb.net/docs/article-page/4.2/Csharp/studio/database/tasks/ongoing-tasks/backup-task) 
on your RavenDB cloud instance as you would off-cloud.  

However, unlike an on-premises instance of RavenDB, backup files **cannot be saved locally** due to storage limitations. Cloud 
providers charge for sending data out of the cloud, therfore saving backup files externally is limited to the [development](cloud-instances#cloud-development-node) 
and [production](cloud-instances#cloud-production-cluster) tiers. If you have access to another service by the same cloud provider
(such as an S3 Bucket if your cloud provider is AWS), you can save your backup files there free of charge.  

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Migration](cloud-migration)  
[Security](cloud-security)  

**Client-API**  
[Backup](../client-api/operations/maintenance/backup/backup)  
[Restore](../client-api/operations/maintenance/backup/restore)  

**Server**  
[Backup Overview](../server/ongoing-tasks/backup-overview)  

**Studio**  
[Backup Task](../studio/database/tasks/ongoing-tasks/backup-task)  
