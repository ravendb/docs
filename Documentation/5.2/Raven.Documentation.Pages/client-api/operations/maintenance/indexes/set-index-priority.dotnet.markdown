# Set Index Priority Operation

---

{NOTE: }

* In RavenDB, each index has its own dedicated thread for all indexing work.  
  By default, RavenDB prioritizes processing requests over indexing,  
  so indexing threads start with a lower priority than request-processing threads.  

* Use `SetIndexesPriorityOperation` to increase or lower the index thread priority.  
  Setting the priority will affect the indexing thread priority at the operating system level.

* __Indexes scope__:  
  Index priority can be set for both static and auto indexes.  

* __Nodes scope__:  
  The priority will be updated on all nodes in the database group.

* Setting the priority can also be done from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) in the Studio.  

* In this page:
    * [Set priority - single index](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---single-index)
    * [Set priority - multiple indexes](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---multiple-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/set-index-priority#syntax)

{NOTE/}

---

{PANEL: Set priority - single index}

{CODE-TABS}
{CODE-TAB:csharp:Sync set_priority_single@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}
{CODE-TAB:csharp:Async set_priority_single_async@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Set priority - multiple indexes}

{CODE-TABS}
{CODE-TAB:csharp:Sync set_priority_multiple@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}
{CODE-TAB:csharp:Async set_priority_multiple_async@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}

| Parameters | | |
| - | - | - |
| **indexName** | string | Index name for which to change priority |
| **priority** | `IndexingPriority` | Priority to set |
| **parameters** | `SetIndexesPriorityOperation.Parameters` | List of indexes + Priority to set.<br>An exception is thrown if any of the specified indexes do not exist. |

{CODE syntax_2@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}

{CODE syntax_3@ClientApi\Operations\Maintenance\Indexes\SetPriority.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Change Index Lock Mode](../../../../client-api/operations/maintenance/indexes/set-index-lock)
