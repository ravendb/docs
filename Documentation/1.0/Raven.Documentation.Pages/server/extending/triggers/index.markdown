# Triggers

Raven allows you to use triggers to add custom behavior for the database. Those triggers are wired together using [MEF](https://github.com/MicrosoftArchive/mef), by default, Raven will search for triggers in a directory called Plugins under the application base directory.

Raven supports the following triggers:

* [PUT triggers](../../../csharp/server/extending/triggers/put)
* [DELETE triggers](../../../csharp/server/extending/triggers/delete?version=1.0)
* [Read triggers](../../../csharp/server/extending/triggers/read?version=1.0)
* [Index update triggers](../../../csharp/server/extending/triggers/indexing?version=1.0)

While not precisely a trigger, the document codec does fall into the same category:

![Figure 1: Triggers](images\triggers_docs.png)

This allows you to control the actual byte storage format on the disk. This is useful if you want to encrypt the data on the disk.