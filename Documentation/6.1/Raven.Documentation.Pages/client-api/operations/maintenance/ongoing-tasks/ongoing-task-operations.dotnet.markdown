# Ongoing Task Operations
---

{NOTE: }

* Once an ongoing task is created, it can be managed via the Client API [Operations](../../../../client-api/operations/what-are-operations).   
  You can get task info, toggle the task state (enable, disable), or delete the task. 

* Ongoing tasks can also be managed via the [Tasks list view](../../../../studio/database/tasks/ongoing-tasks/general-info#ongoing-tasks---view) in the Studio.

* In this page:
  * [Get ongoing task info](../../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations#get-ongoing-task-info)
  * [Delete ongoing task](../../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations#delete-ongoing-task)
  * [Toggle ongoing task state](../../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations#toggle-ongoing-task-state)
  * [Syntax](../../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations#syntax)

{NOTE/}

{PANEL: Get ongoing task info}

For the examples in this article, let's create a simple external replication ongoing task:

{CODE:csharp create_task@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

---

Use `GetOngoingTaskInfoOperation` to get information about an ongoing task.

{CODE-TABS}
{CODE-TAB:csharp:Sync get@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TAB:csharp:Async get_async@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Delete ongoing task}

Use `DeleteOngoingTaskOperation` to remove an ongoing task from the list of tasks assigned to the database.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TAB:csharp:Async delete_async@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Toggle ongoing task state}

Use `ToggleOngoingTaskStateOperation` to enable/disable the task state.

{CODE-TABS}
{CODE-TAB:csharp:Sync toggle@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TAB:csharp:Async toggle_async@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

{CODE syntax_2@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

{CODE syntax_3@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

| Parameter    | Type              | Description                                            |
|--------------|-------------------|--------------------------------------------------------|
| **taskId**   | `long`            | Task ID                                                |
| **taskName** | `string`          | Task name                                              |
| **taskType** | `OngoingTaskType` | Task type                                              |
| **disable**  | `bool`            | `true` - disable the task<br>`false` - enable the task |

{CODE syntax_4@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

<br> 

| Return value of `store.Maintenance.Send(GetOngoingTaskInfoOperation)`   |                                        |
|-------------------------------------------------------------------------|----------------------------------------|
| `OngoingTaskReplication`                                                | Object with information about the task |

{CODE syntax_5@ClientApi\Operations\Maintenance\OngoingTasks\OngoingTaskOperations.cs /}

{PANEL/}
