#Background tasks

#Async deletes and renames

When a file is removed or renamed then relevant configuration items are created in a file system: `DeleteOp-[filename]` and `RenameOp-[filename]`.
They are basically markers to indicate that an operation was initiated for a given file. A configuration will be deleted only if a related file operation finishes successfully.

Note that these actions applied to really large files can take a while. Based on the prefixed configuration items the file system is able 
to resume a deletion of a file or its rename if a server was restarted in the middle. There are two background tasks which detect if any operation
needs to be retried. They run periodically - every 15 minutes.

#Synchronization

Another background work is the synchronization of files to destination nodes. It is performed periodically to ensure that all modifications are 
propagated to destinations, even though one of them was down for a long time. If a destination server wakes up then our file system will 
resend all missing file updates. The synchronization task is run every 10 minutes.
