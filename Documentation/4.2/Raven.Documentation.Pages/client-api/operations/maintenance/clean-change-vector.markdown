# Clean Change Vector

---

{NOTE: }

* A database's [change vector](../../../server/clustering/replication/change-vector) contains entries from each instance of the database 
in the database group. However, even when an instance no longer exists (because it was removed or replaced) its entry will remain in the 
database change vector. These entries can build up over time, leading to longer change vectors that take up unnecessary space.  

* **`UpdateUnusedDatabasesOperation`** lets you specify the IDs of database instances that no longer exist so that their entries can be 
removed from the database change vector.  

* This operation does not affect any documents' _current_ change vectors, but from now on when documents are modified or created their 
change vector will not include the obsolete entries.  

{NOTE/}

---

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\CleanChangeVector.cs /}

| Parameter | Type | Description |
| ------------- | ----- | ---- |
| **database** | `string` | Name of the database |
| **unusedDatabaseIds** | `HashSet<string>` | The database IDs to be removed from the change vector |

{PANEL/}

{PANEL: Example}

In the 'General Stats' view in the [management studio](../../../studio/overview), you can see your database's current change vector (it's 
the same as the change vector of the database's most recently updated/created document).  

Below we see the change vector of an [example database](../../../start/about-examples) "NorthWind". It includes three entries: one of the 
NorthWind instance currently housed on cluster node A (whose ID begins with `N79J...`), and two of instances that were also previously 
housed on node A but which no longer exist.  

![Fig. 1](images/clean-change-vector.png "Database change vector displayed in the General Stats view")

This code removes the obsolete entries specified by their database instance IDs:  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_sync@ClientApi\Operations\Maintenance\CleanChangeVector.cs /}
{CODE-TAB:csharp:Async example_async@ClientApi\Operations\Maintenance\CleanChangeVector.cs /}
{CODE-TABS/}
<br/>

Next time a document is modified, you will see that the database change vector has been cleaned.  

![Fig. 2](images/clean-change-vector-after.png "Database change vector after the operation")

{PANEL/}

## Related Articles

### Getting Started
- [About Examples](../../../start/about-examples)

### Server
- [Change Vector Overview](../../../server/clustering/replication/change-vector)

### Studio
- [Studio Overview](../../../studio/overview)
