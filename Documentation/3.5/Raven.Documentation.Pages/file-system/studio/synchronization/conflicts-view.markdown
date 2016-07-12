#Conflicts view

If two file system are configured to synchronize files to each other (*master/master*) then [conflicts](../../synchronization/conflicts) may occur.
The conflicts view presents the list of conflicted files and allows to resolve them.

![Figure 1. Studio. Conflicts view](images/conflicts-view.png)  

If there is a conflict you need to choose which version of a file you want to take and resolve the conflict by using one of the 
two possible strategies:

* `Resolve with Local` if you want to resolve the conflict in favor of local version,
* `Resolve with Remote` to allow the synchronization source file system to synchronize its version.

{INFO: Remote strategy}
![Figure 2. Studio. Mark Conflicts](images/mark-conflicts.png)      

After marking all the conflicts files we want to resolve. we can choose the option to resolve those conflicts.  
 
![Figure 3. Studio. Resolve with Remote](images/after-remote-resolve.png)   

With Resolve with Remote we can see the status of those conflicts files changed from `Not resolve` to `Scheduled resolution using remote version`.
Using Resolve with Remote strategy will not cause the conflict to disappear. The conflict will exist until the source server retries the synchronization 
of the conflicted file and notices that the applied strategy lets him to push its version.
{INFO/}

{NOTE: Note}
With Resolve with Local the file will disappear immediately from the conflicts page.
![Figure 4. Studio. Mark Conflicts With Arrow](images/mark-conflicts-arrow.png)   
![Figure 5. Studio. No Conflicts](images/no-conflicts.png) 
{NOTE/}