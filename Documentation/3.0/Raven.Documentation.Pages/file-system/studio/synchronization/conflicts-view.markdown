#Conflicts view

If two file system are configured to synchronize files to each other (*master/master*) then [conflicts](../synchronization/conflicts) may occur.
The conflicts view presents the list of conflicted files and allows to resolve them.

![Figure 1. Studio. Conflicts view](images/conflicts-view.png)  

If there is a conflict you need to choose which version of a file you want to take and resolve the conflict by using one of the 
two possible strategies:

* `Resolve with Local` if you want to resolve the conflict in favor of local version,
* `Resolve with Remote` to allow the synchronization source file system to synchronize its version.

{INFO: Remote strategy}
Don't be surprised if resolving the conflict by using the remove version strategy will not cause the conflict to disappear. The conflict
will exist until the source server retries the synchronization of the conflicted file and notices that the applied strategy lets him to push its version.
{INFO/}