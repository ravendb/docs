#Operations: Server: Toggle Dynamic Database Distribution

---

{NOTE: }

* In [dynamic database distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) mode, 
if a database node is down, another cluster node is added to the database group to compensate.  

* Use this operation to toggle dynamic distribution for a particular database group.  

* This can also be done [in the studio](../../../studio/database/settings/manage-database-group#database-group-topology---actions) under 
database group settings.  

{NOTE/}

---

{PANEL: }

###Syntax

{CODE syntax@ClientApi\Operations\ServerWide\Dynamic.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of database group |
| **allowDynamicDistribution** | bool | Set to `true` to activate dynamic distribution mode. |

---

###Example

{CODE operation@ClientApi\Operations\ServerWide\Dynamic.cs /}

{PANEL/}

## Related Articles

### Server

- [Distributed Database](../../../server/clustering/distribution/distributed-database)

### Studio

- [Manage Database Group](../../../studio/database/settings/manage-database-group)
