#Synchronization configurations

RavenFS uses configuration items during the file synchronization process. This article describes their meaning and format.

##Raven/Synchronization/VersionHilo

Used by HiLo algorithm to store "Hi" value. HiLo method is used to generate file versions stored in metadata.

{CODE-BLOCK:json}
{
    "Value": 1
}
{CODE-BLOCK/}

##Raven/Synchronization/Config

Used to keep synchronization options like:

* default built-in conflict resolver,
* max number of concurrent synchronizations performed to a single destination fs,
* locking timeout during the synchronization.

{CODE-BLOCK:json}
{
    "FileConflictResolution": "None",
    "MaxNumberOfSynchronizationsPerDestination": 5,
    "SynchronizationLockTimeout": "00:10:00"
}
{CODE-BLOCK/}

##Raven/Synchronization/Destinations

The list of destination file servers.

{CODE-BLOCK:json}
{
    "Destinations": 
	[
		{
			"ServerUrl": "http://localhost:8080",
			"Username": null,
			"Password": null,
			"Domain": null,
			"ApiKey": null,
			"FileSystem": "Northwind",
			"Enabled": true
		}
	]
}
{CODE-BLOCK/}

##Raven/Synchronization/Sources/[SOURCE-FS-ID]

The configuration stored on a destination file system after a successful file synchronization. It is used to keep track already synchronized docs by using
storing `LastSourceFileEtag` which is an `Etag` of the last synchronized file from a given file system. There is one configuration item per a source file system.

{CODE-BLOCK:json}
{
    "LastSourceFileEtag": "00000000-0000-0100-0000-000000000004",
    "SourceServerUrl": "http://localhost:8080/fs/Northwind",
    "DestinationServerId": "79d30df8-f226-4479-9039-922a1c747d3b"
}
{CODE-BLOCK/}

##SyncingLock/[FILENAME]

Its existence indicates that a file is currently being locked and it is inaccessible to modify. It exists only if the file is being synchronized.

{CODE-BLOCK:json}
{
    "SourceServer": 
    {
        "FileSystemUrl": "http://localhost:8080/fs/Northwind",
        "Id": "8e4541e5-87f4-4943-a263-e49c5bc4ed06"
    },
    "FileLockedAt": "2015-02-17T14:25:22.1876544Z"

}
{CODE-BLOCK/}

##Syncing/[DESTINATION-FS-URL]/[FILENAME]

This configuration is stored on a source file system for every already synchronized file. It is removed if the destination confirms that the synchronization succeeded.

{CODE-BLOCK:json}
{
    "FileName": "/git.exe",
    "FileETag": "00000000-0000-0100-0000-00000000000e",
    "DestinationUrl": "http://localhost:8080/fs/Northwind",
    "Type": "ContentUpdate"
}
{CODE-BLOCK/}

##SyncResult/[FILENAME]

This configuration represents a result of the synchronization of a file. It is stored by a destination file system. If any exception is thrown during synchronization, it will be stored in Exception property.
BytesTransfered, BytesCopied and NeedListLength are filled up only if the file content has changed.

{CODE-BLOCK:json}
{
    "FileName": "/git.exe",
    "FileETag": "00000000-0000-0100-0000-00000000000e",
    "BytesTransfered": "1073741824",
    "BytesCopied": "357913941",
    "NeedListLength": "5",
    "Exception": "",
    "Type": "ContentUpdate"
}
{CODE-BLOCK/}

##Conflicted/[FILENAME]

A conflict item is stored as a configuration. It contains histories of both versions of a conflicted file, a file name and a url of a source file system. 
The conflict is always created on a destination node.

{CODE-BLOCK:json}
{
    "RemoteHistory": 
	[
		{
			"Version": 1,
			"ServerId": "de406356-c86f-4cc3-8f12-49c64042c5e1"
		}
	],
	"CurrentHistory":
	[
		{
			"Version": 1,
			"ServerId": "a785653d-841a-4604-a789-025b48f88f03"
		}
	],
    "FileName": "/git.exe",
    "RemoteServerUrl": "http://localhost:8080/fs/Northwind"
}
{CODE-BLOCK/}
