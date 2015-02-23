#What are commands?

Commands are a set of low level operations that can be used to manipulate data on RavenFS instance.

They are accessible from `IFilesStore` object by using `AsyncFilesCommands` property:

{CODE commands_access@FileSystem\ClientApi\Commands\WhatAreCommands.cs /}

Note that in contrast to [RavenDB commands](../../../client-api/commands/what-are-commands), RavenFS client exposes only async methods.

##File commands

The following commands can be used to manipulate files:

TODO arek

##Configuration commands

The following commands can be used to manage [configuration items](../../configurations):

TODO arek

##Synchronization commands

The following commands can be used to manually perform synchronization actions:

TODO arek

##Storage commands

The following commands can be used to force storage background tasks to run:

TODO arek

##Admin commands

The following commands can be used to execute admin actions:

TODO arek