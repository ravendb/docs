# RavenDB on the Cloud: Backup And Restore
---

{NOTE: }

* RavenDB cloud instances of the [Free](../cloud/cloud-instances#a-free-cloud-node) and 
  [Production](../cloud/cloud-instances#a-production-cloud-cluster) tiers are regularly and automatically backed up.  

* You can also define your own backup tasks, as you would with an on-premises RavenDB server.  

* [Development](../cloud/cloud-instances#a-development-cloud-server) products do **not** offer backup (neither automatic nor custom).  

* In this page:  
  * [The Mandatory-Backup Routine](../cloud/cloud-backup-and-restore#the-mandatory-backup-routine)  
  * [Backup storage](../cloud/cloud-backup-and-restore#backup-storage)  
  * [Charging For Backup storage](../cloud/cloud-backup-and-restore#charging-for-backup-storage)  
  * [Backup Encryption](../cloud/cloud-backup-and-restore#backup-encryption)  
  * [Creating a Custom Backup](../cloud/cloud-backup-and-restore#creating-a-custom-backup)  
  * [Restore Backup Files](../cloud/cloud-backup-and-restore#restore-backup-files)  
      - [Restore Mandatory-Backup Files](../cloud/cloud-backup-and-restore#restore-mandatory-backup-files)  
      - [Restore Custom-Backup Files](../cloud/cloud-backup-and-restore#restore-custom-backup-files)  
{NOTE/}

---

{PANEL: The Mandatory-Backup Routine}

Each database you create using a RavenDB cloud product, is assigned a mandatory-backup task that 
stores a [full backup](../server/ongoing-tasks/backup-overview#backup-scope-full-or-incremental) 
**every 24 hours** and an **incremental backup every 15 minutes** with all the changes that took 
place since the last backup.  

{INFO: }
The mandatory backup process cannot be disabled.  
{INFO/}

{INFO: }
We keep your backup files for no less than 14 days.  
You can [contact support](../cloud/portal/cloud-portal-support-tab) to prolong this period, but not reduce it.  
{INFO/}

---

#### The Backup Task

To view or activate the mandatory backup task, open your product's management studio and -  

* Choose the database whose backup task you want to view  
* Click **Manage Ongoing Tasks**  
* Click the "Server Wide Backup" task.  

!["Backup Task"](images\backup-and-restore-001-backup-task.png "Backup Task")  

---
{PANEL/}

{PANEL: Backup storage}

####Mandatory-Backup Storage
Backups created by the [mandatory backup routine](../cloud/cloud-backup-and-restore#the-mandatory-backup-routine) are stored in a RavenDB 
cloud you have no direct access to.  
You can [view and restore](../cloud/cloud-backup-and-restore#restore-mandatory-backup-files) them using your portal's Backups tab and the 
management Studio.  

---

####Custom-Backup Storage
[Custom-backup](../cloud/cloud-backup-and-restore#creating-a-custom-backup) files can be kept where you choose to keep them.  
We recommend that you use a backup method **local to your host cloud**. If your product is hosted by Amazon AWS for example, 
you can store your backup files on an Amazon S3 bucket.  

---

{INFO: }
All Backup files, mandatory and custom, are **compressed** and **encrypted**.  
{INFO/}

{PANEL/}

{PANEL: Charging For Backup storage}

####Mandatory-Backup charge
Mandatory-backup files are kept in RavenDB's own cloud.  

We give backup storage of up to 1 GB per product **for free**. 

Your backup storage usage is measured once a day, and you'll be charged each month based on your average daily usage.  

Free-tier users that overuse backup storage may trim their database to fit the limitation, pay for the extra storage, 
or stop the product.  

---

####Custom-Backup charge
Your custom backups are kept in a storage location of your choosing, and charges for it are unrelated to us.  

**Make sure**, however, that you're aware of **all possible expanses** related to your backups, including -  

* Any external storage service you may be charged for (e.g. an S3 bucket)  
* File transfer costs you may be charged for (e.g. passing files over FTP)  


{PANEL/}

{PANEL: Backup Encryption}

Your backup files are encrypted.

* If your database **is encrypted**:  
  **Your own database encryption key** will be used to encrypt the backup as well.  
  
    {WARNING: }
    Be aware that RavenDB does NOT keep or manage *your own* database encryption keys.  
    If you lose a database encryption key we will NOT be able to help you decrypt the database itself nor its backup files.  
    **KEEP YOUR ENCRYPTION KEYS SAFE!**  
    {WARNING/}

* If your database is **not encrypted**:  
  We will encrypt its backup files using an encryption key that **we** manage and is unique to your account.  

{PANEL/}

{PANEL: Creating a Custom Backup}

You can create your own [ongoing backup tasks](https://ravendb.net/docs/article-page/4.2/Csharp/studio/database/tasks/ongoing-tasks/backup-task) 
on your RavenDB cloud instance, as you would off-cloud.  

---

Use your cloud instance's management Studio to create a new task.  
!["Create Task"](images\backup-001-add-backup-task.png "Create Task")  

---

Add a Backup task  
!["Backup Task"](images\backup-002-add-backup-task.png "Backup Task")  

---

You can store custom-backup files on an unrelated cloud service like an S3 bucket, a Glacier vault, or an Azure platform.  

Activate and configure your unrelated cloud storage service, e.g. the bucket in which your backup files would be kept.  
!["Set S3 Bucket"](images\backup-003-set-s3-bucket.png "Set S3 Bucket")  

{PANEL/}

{PANEL: Restore Backup Files}

##Restore Mandatory-Backup Files

####View The mandatory Backups List  
Backup files that have already been created, are listed in the backups tab.  

* Open your portal's Backups tab, and choose the product whose database you want to restore.  
  Its backups will be shown, listed by the databases they've been created for.  
* Click "Generate Backup Link" for the database you want to restore.  
  The backup link window will open.

  !["Backups List"](images\backup-and-restore-002-mandatory-backups-tab-list.png "Backups List")  

---

####Restore The Database  
Clicking the **Generate Backup Link** button will show you a simple procedure. Follow it to restore your database.  

!["Backup Link"](images\backup-and-restore-003-backup-link-window.png "Backup Link")  

* **A.** Create a **New Database From Backup**  
  !["New DB From Backup"](images\backup-and-restore-004-new-database-from-backup.png "New DB From Backup")  
* **B.** Choose **Cloud** as your Source  
  !["Source Is Cloud"](images\backup-and-restore-005-source-is-cloud.png "Source Is Cloud")  
* **C.** Copy the link you've been given in the Backups Tab, to the **Backup Link** box here.  
  !["Paste Backup Link Here"](images\backup-and-restore-006-paste-backup-link.png "Paste Backup Link Here")  
* **D.** Choose a **Restore Point**  
  !["Choose Restore Point"](images\backup-and-restore-007-restore-point.png "Choose Restore Point")  
* **E.** Encryption Key for an **Encrypted** Database  
  If your original database **is** encrypted, its backup has been encrypted with the same key. Find it and paste it here.  
  !["Encryption Key: Encrypted Database"](images\backup-and-restore-008-encryption-key-1.png "Encryption Key: Encrypted Database")  
* **F.**  Encryption Key for an **Unencrypted** Database  
  If your original database is **not** encrypted, we used our own key to encrypt your backup file.  
  Copy it from the right Backups Tab box.  
  !["Encryption Key: Unencrypted Database"](images\backup-and-restore-009-encryption-key-2.png "Encryption Key: Unencrypted Database")  

##Restore Custom-Backup Files

To restore a custom-backup file:  

* Load your backup file from your custom location (e.g. an S3 bucket) to your local disk.  
* Restore it from this location.  
  !["Restore from Local Location"](images\backup-and-restore-010-restore-from-local-location.png "Restore from Local Location")  

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Migration](cloud-migration)  
[Security](cloud-security)  
  
[Portal](../cloud/portal/cloud-portal)  
[Backup Tab](../cloud/portal/cloud-portal-backups-tab)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  

**Client-API**  
[Backup](../client-api/operations/maintenance/backup/backup)  
[Restore](../client-api/operations/maintenance/backup/restore)  

**Server**  
[Backup Overview](../server/ongoing-tasks/backup-overview)  

**Studio**  
[Backup Task](../studio/database/tasks/ongoing-tasks/backup-task)  
