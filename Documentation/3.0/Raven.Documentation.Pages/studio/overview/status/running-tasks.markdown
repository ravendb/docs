# Status: Running Tasks

Here you can view the details of currently running tasks and filter them according to their types.
Types of the task available are as follows: 

-  `SuggestionQuery`,
-  `BulkInsert`,
-  `IndexBulkOperation`,
-  `IndexDeleteOperation`,
-  `ImportDatabase`,
-  `RestoreDatabase`,
-  `RestoreFilesystem`,
-  `CompactDatabase`,
-  `CompactFilesystem`,
-  `IoTest`

 Each filter will appear if at least one type of this task is currently running. You can refresh the list of running tasks by clicking on the `Refresh` button and search for the specific tasks using `Search` option.

## Killing tasks

If a task of a certain type (namely `BulkInsert`, `IndexBulkOperation`, `ImportDatabase`, or `IoTest`) is currently running, you can `Kill` it using the appropriate button.

![Figure 1. Studio. Status. Killing Tasks.](images/status_running_tasks-1.png) 


{INFO Remember that server has an automatic purging mechanism for completed tasks, because of that completed tasks might not show up on this list. /}