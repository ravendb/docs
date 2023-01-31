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

* __Operation scope__:  
  The priority will be updated on all nodes in the database group.

* Setting the priority can also be done from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) in the Studio.  

* In this page:
    * [Set priority - single index](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---single-index)
    * [Set priority - multiple indexes](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---multiple-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/set-index-priority#syntax)

{NOTE/}

---

{PANEL: Set priority - single index}

{CODE:nodejs set_priority_single@ClientApi\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

{PANEL: Set priority - multiple indexes}

{CODE:nodejs set_priority_multiple@ClientApi\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Maintenance\Indexes\setPriority.js /}

| Parameters | | |
| - | - | - |
| **indexName** | string | Index name for which to change priority |
| **priority** | `"Low"` / `"Normal"` / `"High"` | Priority to set |
| **parameters** | parameters object | List of indexes + Priority to set |

{CODE:nodejs syntax_2@ClientApi\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Change Index Lock Mode](../../../../client-api/operations/maintenance/indexes/set-index-lock)
