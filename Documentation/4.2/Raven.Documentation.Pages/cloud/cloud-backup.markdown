#Cloud: Backup
---

{NOTE: }

* RavenDB cloud instances of the [Free](../cloud/cloud-instances#a-free-cloud-node) and 
  [Production](../cloud/cloud-instances#a-production-cloud-cluster) tiers are regularly and automatically backed up.  
* You can also define your own backup tasks as you would on-premises.  

* In this page:  
  * [Automatic Backup](../cloud/cloud-backup#automatic-backup)  
  * [Backup storage](../cloud/cloud-backup#backup-storage)  
  * [Charging For Backup storage](../cloud/cloud-backup#charging-for-backup-storage)  
  * [Backup Encryption](../cloud/cloud-backup#backup-encryption)  
  * [Custom Backup](../cloud/cloud-backup#custom-backup)  
{NOTE/}

---

{PANEL: Automatic Backup}

Your cloud instance automatically backs up your data to ensure it is safe. This process cannot be disabled.  

* A [full](../server/ongoing-tasks/backup-overview#backup-scope-full-or-incremental) backup is created every 24 hours.  
* An [incremental](../server/ongoing-tasks/backup-overview#backup-scope-full-or-incremental) backup is created every 15 minutes.  
* Backup files are saved for a minimum of 14 days, as per our retention policy.  
  You can [contact support](../cloud/portal/cloud-portal-support-tab) to extend this retention period, but it **cannot 
be reduced to less than 14 days.**  

{PANEL/}

{PANEL: Backup storage}

Backup files are compressed, to minimize storage usage.  
To ensure that your data is safe and can always be restored if necessary, **we do not delete backup files before 14 days have passed.**

{PANEL/}

{PANEL: Charging For Backup storage}

Backup storage up to 1 GB is **free**.  
Your backup storage usage is measured once a day.  
You are charged each month based on the *average* amount of storage you used per day.

{PANEL/}

{PANEL: Backup Encryption}

The automated regular backup files are always encrypted.

* If your database is NOT encrypted:  
  We will encrypt its backup files using an encryption key that **we** manage and which is unique to your account.  
* If your database IS encrypted:  
  **Your own database encryption key** will be used to encrypt the backup as well.  
  
  {WARNING: }
  Be aware that RavenDB does NOT keep or manage *your own* database encryption keys.  
  If you lose a database's encryption key we will NOT be able to help you decrypt the database itself nor its backup files.  
  Keep your encryption keys safe!  
  {WARNING/}

{PANEL/}

{PANEL: Custom Backup}

You can create your own [ongoing backup tasks](https://ravendb.net/docs/article-page/4.2/Csharp/studio/database/tasks/ongoing-tasks/backup-task) 
on your RavenDB cloud instance, as you would off-cloud.  

However, unlike an on-premises instance of RavenDB, backup files **cannot be saved on your instance's cloud host**, due to storage limitations. 
Cloud providers therefore send your backup files out of the cloud, and charge you for it.  

If you have access to another service by the same cloud provider (such as an S3 Bucket if your cloud provider is AWS), you can save your backup 
files there free of charge.  

Free-product users should be aware that backup files' transfer is charged for and may drain their budget.  

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
