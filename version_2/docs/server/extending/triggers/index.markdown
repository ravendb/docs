# Triggers

Raven allows you to use triggers to add custom behavior for the database. Those triggers are wired together using [MEF](http://mef.codeplex.com/), by default, Raven will search for triggers in a directory called Plugins under the application base directory.

Raven supports the following triggers:

* [PUT triggers](/docs/server/extending/triggers/put?version=2.0)
* [DELETE triggers](/docs/server/extending/triggers/delete?version=2.0)
* [Read triggers](/docs/server/extending/triggers/read?version=2.0)
* [Index update triggers](/docs/server/extending/triggers/indexing?version=2.0)

While not precisely a trigger, the document codec does fall into the same category:

![Figure 1: Triggers](images\triggers_docs.png)

This allows you to control the actual byte storage format on the disk. This is useful if you want to encrypt the data on the disk.