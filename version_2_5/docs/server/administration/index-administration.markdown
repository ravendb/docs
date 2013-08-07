### Index administration

RavenDB indexes can be administrated easily from the consumer end using either code or the studio.

#### Resetting an index

An index usually need to be reset because it has reached its error quota and been disabled. Resetting an index means forcing RavenDB to re-index all documents matched by the index definition, which can be a very lengthy process.

You can reset an index in one of the following ways:

* Using the client API, by calling: 
{CODE reset_index@Server\Administration\IndexAdministration.cs /}
* Using the HTTP API, by issuing a HTTP call to the index with RESET as the method name: 
{CODE-START:plain /}
    > curl -X RESET http://localhost:8080/databases/DB1/indexes/indexName
{CODE-END /}

#### Deleting an index

You can delete an index by calling:
{CODE delete_index@Server\Administration\IndexAdministration.cs /}
from the client API. Alternatively you can make a HTTP request:
{CODE-START:plain /}
    > curl -X DELETE http://localhost:8080/databases/DB1/indexes/indexName
{CODE-END /}


#### Index locking

This feature allows you to change an index definition on the production server. You can lock an index for changes, either in such a way that gives you the ability ignore changes to this index
or by raising an error when someone tries to modify the index. You can update the index definition on the server, next update it on the codebase and deploy the application to match them. While the index is locked
at any time when `IndexCreation.CreateIndexes()` on start up is called will not revert the change that you did.

It is important to note that this is not a security feature, you can unlock the index at any time.

To lock the index you need to create a HTTP call:
{CODE-START:plain /}
    > curl -X POST http://localhost:8080/databases/DB1/indexes/indexName?op=lockModeChange&mode=LockedIgnore
{CODE-END /}

The available modes are:

* Unlock
* LockedIgnore
* LockedError

#### Using The Studio

The reset and delete operations can be easily performed from the studio. You can right click on a index name and select an action:

![Figure 1: Reset and delete index options in the studio](images/index-reset-delete-from-ui.png)

In the same tab you can also lock/unlock the index:

![Figure 2: Index lock / unlock](images/index-lock-unlock-ui.png)
