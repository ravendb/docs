# Triggers

Raven allows you to use triggers to add custom behavior for the database. Those triggers are wired together using [MEF](http://mef.codeplex.com/), by default, Raven will search for triggers in a directory called Plugins under the application base directory.

Raven supports the following triggers:

* [PUT triggers](/docs/1.0/server/extending/triggers/put)
* [DELETE triggers](/docs/1.0/server/extending/triggers/delete)
* [Read triggers](/docs/1.0/server/extending/triggers/read)
* [Index update triggers](/docs/1.0/server/extending/triggers/indexing)

While not precisely a trigger, the document codec does fall into the same category:

![Figure 1: Triggers](images\triggers_docs.png)

This allows you to control the actual byte storage format on the disk. This is useful if you want to encrypt the data on the disk.