# Set Index Priority Operation

---

{NOTE: }

* In RavenDB, each index has its own dedicated thread for all indexing work.  
  By default, RavenDB prioritizes processing requests over indexing,  
  so indexing threads start with a lower priority than request-processing threads.  

* Use `SetIndexesPriorityOperation` to raise or lower the index thread priority.  

* **Indexes scope**:  
  Index priority can be set for both static and auto indexes.  

* **Nodes scope**:  
  The priority will be updated on all nodes in the database group.

* Setting the priority can also be done from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) in the Studio.  

* In this page:
    * [Index priority](../../../../client-api/operations/maintenance/indexes/set-index-priority#index-priority)
    * [Set priority - single index](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---single-index)
    * [Set priority - multiple indexes](../../../../client-api/operations/maintenance/indexes/set-index-priority#set-priority---multiple-indexes)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/set-index-priority#syntax)

{NOTE/}

---

{PANEL: Index priority}

Setting the priority will affect the indexing thread priority at the operating system level:  

| Priority value | Indexing thread priority<br> at OS level | |
| - | - | - |
| **Low** | Lowest | <ul><li>Having the `Lowest` priority at the OS level, indexes will run only when there's a capacity for them, when the system is not occupied with higher-priority tasks.</li><li>Requests to the database will complete faster.<br>Use when querying the server is more important to you than indexing.</li></ul> |
| **Normal** (default) | Below normal | <ul><li>Requests to the database are still preferred over the indexing process.</li><li>The indexing thread priority at the OS level is `Below normal`<br>while Requests processes have a `Normal` priority.</li></ul> |
| **High** | Normal | <ul><li>Requests and Indexing will have the same priority at the OS level.</li></ul> |

{PANEL/}

{PANEL: Set priority - single index}

{CODE:nodejs set_priority_single@client-api\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

{PANEL: Set priority - multiple indexes}

{CODE:nodejs set_priority_multiple@client-api\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\Operations\Maintenance\Indexes\setPriority.js /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | `string` | Index name for which to change priority |
| **priority** | `"Low"` /<br> `"Normal"` /<br> `"High"` | Priority to set |
| **parameters** | parameters object | List of indexes + Priority to set.<br>An exception is thrown if any of the specified indexes doesn't exist. |

{CODE:nodejs syntax_2@client-api\Operations\Maintenance\Indexes\setPriority.js /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Change Index Lock Mode](../../../../client-api/operations/maintenance/indexes/set-index-lock)
