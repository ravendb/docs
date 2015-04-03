# Versioning

The versioning bundle will create snapshots for every file, upon every update received, or when it is deleted. It is useful when you need to track the history of files or when you need a full audit trail.

## Installation

To activate versioning simply add `Versioning` to `Raven/ActiveBundles` configuration in the global configuration file or setup a new file system with the versioning bundle turned on using API or the Studio.

Learn how to create a file system with versioning enabled using the Studio [here](../../studio/how-to/how-to-setup-versioning).

## Configuration

By default, the Versioning bundle will track history for all files and never purge old revisions. This may be easily configurable by changing appropriate configuration item:

{CODE versioning_1@FileSystem\Server\Bundles\Versioning.cs /}

Such a default configuration will setup the versioning bundle to version all files (`Exclude = false`) and keep only up to 10 revisions (`MaxRevisions = 10`).

It is possible to override the default behavior for files located in a particular directory. For example, let's say that we don't want to version files in `/documents/temp` folder.
To achieve that we need to set the following configuration:

{CODE versioning_2@FileSystem\Server\Bundles\Versioning.cs /}

This will cause that no file located under specified directory and its subfolders will be versioned because `Exclude = true`.

The configuration naming convention is `Raven/Versioning/[directory/path]`. You can create multiple configurations for different nesting levels, the versioning bundle will look for the most specific one.
For example the above versioning configuration will disable versioning for files under `/documents/temp/drafts` too. However you can also set the following configuration:

{CODE versioning_3@FileSystem\Server\Bundles\Versioning.cs /}

to enable versioning for this folder. There will be created no more than 5 revisions for files from this directory.

Apart from `MaxRevisions` there are a few more options:

* *ExcludeUnlessExplicit* set to `true` disables versioning for impacted files unless file metadata at the time of saving contains the key `Raven-Create-Version`.  This key is transient and it is removed from metadata before put. Default: `false`.
* *PurgeOnDelete* determines whether revisions should be deleted if a related file is deleted. Default: false.
* *ResetOnRename* indicates if the versioning should be reset on a file rename. Default: `true`, means that the last existing revision will become first revision while
the other ones will be deleted. If you set this option to `false` then revisions will be renamed according to the new name of the related file.

## Client integration

The versioning bundle also has a client side part, which you can access by adding `Raven.Client.FileSystem.Bundles.Versioning` namespace.

Then, you can access past revisions of a file using the following code:

{CODE versioning_4@FileSystem\Server\Bundles\Versioning.cs /}

## Related articles

- [Studio : How to enable and setup versioning?](../../studio/how-to/how-to-setup-versioning)