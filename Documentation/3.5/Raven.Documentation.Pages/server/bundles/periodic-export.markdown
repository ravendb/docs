# Bundle: Periodic Export

RavenDB innately supports periodic exports of the documents and attachments (including deletions) to a file system folder, [Amazon AWS](https://aws.amazon.com/) services, or [Microsoft Azure](https://azure.microsoft.com/) storage.  

## How it works

Periodic exports leverage the concept of incremental exports available in RavenDB. To take advantage of that, we are storing information about last successful ETag of the documents and attachments that were sent to the backup destination.

{NOTE:Note}

Periodic export saves every full backup to its own time-stamped folder and every incremental backup goes into the current full backup folder until the next full backup is created.

{NOTE/}

## Activating bundle

To activate bundle globally, simply add `PeriodicExport` to the `Raven/ActiveBundles` in  an appropriate configuration file. More about configuration's setup can be found [here](../../server/configuration/configuration-options).

If you wish to set up periodic export per database, then add `PeriodicExport` to the list of database's active bundles or use the [Studio](../../studio/overview/settings/periodic-export).

Bundle can also be activated during a database creation process.

{CODE periodic_backups_1@Server\Bundles\PeriodicExport.cs /}

## Configuring file system folder

In order to export a database to a specified folder available locally, you need to create a `Raven/Backup/Periodic/Setup` with the `LocalFolderName` property filled with the designated path.

{CODE periodic_backups_4@Server\Bundles\PeriodicExport.cs /}

## Configuring Amazon AWS

Two steps need to be taken to set up export destination properly.

First, we need to add our AWS access and secret key to the database settings. For example, if we want to create new database with a bundle already activated and keys set up, we can execute the following code:

{CODE periodic_backups_2@Server\Bundles\PeriodicExport.cs /}

Next, we need to create a backup setup document under the `Raven/Backup/Periodic/Setup`, where our backup destination configuration will be stored. This document will be created automatically when you use the Studio to set up a periodic export, yet it can be created almost as easily using the API.

{CODE periodic_backups_3@Server\Bundles\PeriodicExport.cs /}

`GlacierVaultName` and `S3BucketName` values **exclude** each other in favor of the `GlacierVaultName`, so if you specify both, RavenDB will only use `GlacierVaultName`. 

{INFO More information about Amazon Simple Storage Service (Amazon S3) can be found [here](https://aws.amazon.com/s3/). If you are interested in Amazon Glacier, visit [this](https://aws.amazon.com/glacier/) page. /}

## Configuring Azure Storage

Configuring Azure is almost identical to Amazon AWS process. First, you need to store your account name and access key in a database configuration. For example, if we want to create a new database with bundle already activated and keys setup, we can execute the following code:

{CODE periodic_backups_5@Server\Bundles\PeriodicExport.cs /}

Next, we need to create a backup setup document under the `Raven/Backup/Periodic/Setup`, where our backup destination configuration will be stored. This document will be created automatically when you use the Studio to set up a periodic export, yet it can be created almost as easily using the API.

{CODE periodic_backups_6@Server\Bundles\PeriodicExport.cs /}

{INFO More information about Microsoft Azure Storage can be found [here](https://azure.microsoft.com/en-us/services/storage/) /}

## Remarks

Amazon AWS and Azure Storage related properties in the `PeriodicExportSetup` document **exclude** each other. Server will always upload export to **only** one location, and the location will be picked in the following order: `GlacierVaultName`, `S3BucketName`, `AzureStorageContainer`.
