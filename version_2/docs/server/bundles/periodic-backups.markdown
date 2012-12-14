#Periodic Backups bundle

RavenDB comes with support of doing periodic backups of documents and attachments to [Amazon AWS](http://aws.amazon.com/) services. In order to periodic backups to work you must activate `PeriodicBackups` bundle, by activating this bundle globally and turning it on/off per database, or activating it per database only.

##How it works

Periodic backups are leveraging the concept of incremental backups available in RavenDB and to take advantage of that, we are storing an information about last successful ETag of the documents and attachments that were send to backup destination.

##Configuration

###Activating bundle

To activate bundle globally just add `PeriodicBackups` to the `Raven/ActiveBundles`. More about setting up configuration can be found [here](../Administration/configuration).

If you wish to setup periodic backups per database, then add `PeriodicBackups` to the list of database active bundles or use [the Studio](../../studio/bundles/periodicbackup).

Bundle can also be activated during database creation process.

{CODE periodic_backups_1@Server\Bundles\PeriodicBackups.cs /}

###Configuring backup destination

Two steps need to be taken to setup backup destination properly.

First we need to add our AWS access and secret key to database settings. For example if we want to create new database with bundle already activated and keys setup, then we can execute following code:

{CODE periodic_backups_2@Server\Bundles\PeriodicBackups.cs /}

In next step we need to create a backup setup document under `Raven/Backup/Periodic/Setup` where we will store our backup destination configuration. This document will be created automatically when you will use Studio to setup periodic backups, but it can be created almost as easily using the API.

{CODE periodic_backups_3@Server\Bundles\PeriodicBackups.cs /}

`GlacierVaultName` and `S3BucketName` values **exclude** each other in favor to the `GlacierVaultName` so if you will specify both, then RavenDB will only use `GlacierVaultName`. 

{NOTE More information about Amazon Simple Storage Service (Amazon S3) can be found [here](http://aws.amazon.com/s3/) and if you are interested in Amazon Glacier then visit [this](http://aws.amazon.com/glacier/) page. /}

