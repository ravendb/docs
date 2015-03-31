#What are commands?

Commands are a set of low level operations that can be used to manipulate data on the RavenFS instance.

They are accessible from the `IFilesStore` object by using `AsyncFilesCommands` property:

{CODE commands_access@FileSystem\ClientApi\Commands\WhatAreCommands.cs /}

Note that in contrast to [RavenDB commands](../../../client-api/commands/what-are-commands), RavenFS client exposes only the async methods.

##Working with multiple file systems

By default file commands are executed against the `DefaultFileSystem` configured in the `FilesStore`. However, you can perform the command actions on another existing file system. Note that this file system may requite different credentials. Take a look at the example:

{CODE commands_different_fs@FileSystem\ClientApi\Commands\WhatAreCommands.cs /}

