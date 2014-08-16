# Bundle : Periodic Export

RavenDB comes with support of doing periodic exports of documents and attachments to file system folder, [Amazon AWS](http://aws.amazon.com/) services or [Microsoft Azure](http://azure.microsoft.com/) storage.  

## How it works

Periodic exports are leveraging the concept of incremental exports available in RavenDB and to take advantage of that, we are storing an information about last successful ETag of the documents and attachments that were send to backup destination.

## Activating bundle

To activate bundle globally just add `PeriodicExport` to the `Raven/ActiveBundles` in appropriate configuration file. More about setting up configuration can be found [here](../Administration/configuration).

If you wish to setup periodic export per database, then add `PeriodicExport` to the list of database active bundles or use the [Studio](../../studio/bundles/periodicbackup).

Bundle can also be activated during database creation process.

{CODE periodic_backups_1@Server\Bundles\PeriodicExport.cs /}

## Configuring file system folder

In order to export database to specified folder available locally, you need to create a `Raven/Backup/Periodic/Setup` with `LocalFolderName` property filled with designated path.

{CODE periodic_backups_4@Server\Bundles\PeriodicExport.cs /}

## Configuring Amazon AWS

Two steps need to be taken to setup export destination properly.

First we need to add our AWS access and secret key to database settings. For example if we want to create new database with bundle already activated and keys setup, then we can execute following code:

{CODE periodic_backups_2@Server\Bundles\PeriodicExport.cs /}

In next step we need to create a backup setup document under `Raven/Backup/Periodic/Setup` where we will store our backup destination configuration. This document will be created automatically when you will use Studio to setup periodic export, but it can be created almost as easily using the API.

{CODE periodic_backups_3@Server\Bundles\PeriodicExport.cs /}

`GlacierVaultName` and `S3BucketName` values **exclude** each other in favor of the `GlacierVaultName` so if you will specify both, then RavenDB will only use `GlacierVaultName`. 

{INFO More information about Amazon Simple Storage Service (Amazon S3) can be found [here](http://aws.amazon.com/s3/) and if you are interested in Amazon Glacier then visit [this](http://aws.amazon.com/glacier/) page. /}

## Configuring Azure Storage

Configuring Azure is almost identical as Amazon AWS process. First you need to store in database configuration your account name and access key. For example if we want to create new database with bundle already activated and keys setup, then we can execute following code:

{CODE periodic_backups_5@Server\Bundles\PeriodicExport.cs /}

In next step we need to create a backup setup document under `Raven/Backup/Periodic/Setup` where we will store our backup destination configuration. This document will be created automatically when you will use Studio to setup periodic export, but it can be created almost as easily using the API.

{CODE periodic_backups_6@Server\Bundles\PeriodicExport.cs /}

{INFO More information about Microsoft Azure Storage can be found [here](http://azure.microsoft.com/en-us/services/storage/) /}

## Remarks

Amazon AWS and Azure Storage related properties in `PeriodicExportSetup` document **exclude** each other. Server will always upload export to one location and location will be picked in following order: `GlacierVaultName`, `S3BucketName`, `AzureStorageContainer`.

#### Related articles

TODO