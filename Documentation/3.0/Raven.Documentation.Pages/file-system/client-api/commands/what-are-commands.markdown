#What are commands?

Commands are a set of low level operations that can be used to manipulate data on RavenFS instance.

They are accessible from `IFilesStore` object by using `AsyncFilesCommands` property:

{CODE commands_access@FileSystem\ClientApi\Commands\WhatAreCommands.cs /}

Note that in contrast to [RavenDB commands](../../../client-api/commands/what-are-commands), RavenFS client exposes only async methods.

##Working with multiple file systems

By default file commands are executed against `DefaultFileSystem` configured in `FilesStore`. However you can perform command actions on
an another existing file system. Note that this file system may requite different credentials. Take a look at the example:

{CODE commands_different_fs@FileSystem\ClientApi\Commands\WhatAreCommands.cs /}

