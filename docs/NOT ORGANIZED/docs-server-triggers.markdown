#Raven - Triggers

Raven allows you to use triggers to add custom behavior for the database. Those triggers are wired together using [MEF](http://mef.codeplex.com/), by default, Raven will search for triggers in a directory called Plugins under the application base directory.

Raven supports the following triggers:

* [PUT triggers]()//"TODO Link"
* [DELETE triggers]()//"TODO Link"
* [Read triggers]()//"TODO Link"
* [Index update triggers]()//"TODO Link"

While not precisely a trigger, the document codec does fall into the same category:

![Figure 1: Triggers](images\triggers_docs.png)

This allows you to control the actual byte storage format on the disk. This is useful if you want to encrypt the data on the disk.