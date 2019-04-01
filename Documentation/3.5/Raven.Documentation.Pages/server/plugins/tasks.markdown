# Plugins: Tasks

Another type of plugins gives us the ability to perform various actions during server/database startup process or enables us to perform actions periodically. For these instances we have introduced two interfaces and one abstract class.

{CODE plugins_4_0@Server\Plugins.cs /}

{CODE plugins_4_1@Server\Plugins.cs /}

where:   
* `IStartupTask` can be used to implement a task which will be started during database initialization.   
* `IServerStartupTask` can be used to implement a task which will be started during server initialization.    
* `AbstractBackgroundTask ` is a base for all periodic tasks.    

## Example - Send email when server is starting

{CODE plugins_4_2@Server\Plugins.cs /}

## Example - Perform a cleanup task during database initialization

{CODE plugins_4_3@Server\Plugins.cs /}

## Example - Perform a cleanup task every six hours

{CODE plugins_4_4@Server\Plugins.cs /}
