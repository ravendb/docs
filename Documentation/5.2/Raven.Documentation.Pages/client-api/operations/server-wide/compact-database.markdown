# Operations: Server: How to Compact a Database

* Use the CompactDatabaseOperation to compact a database.  
* You can choose what should be compacted: documents and/or listed indexes.  

{WARNING: }
The compacting operation is executed **asynchronously**, 
and during this operation **the database will be offline**.  
{WARNING/}


## Syntax

{CODE compact_1@ClientApi\Operations\Server\Compact.cs /}

{CODE compact_2@ClientApi\Operations\Server\Compact.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Name of a database to compact |
| **Documents** | bool | Indicates if documents should be compacted |
| **Indexes** | string[] | List of index names to compact |

## Example I

{CODE compact_3@ClientApi\Operations\Server\Compact.cs /}


## Example II

{CODE compact_4@ClientApi\Operations\Server\Compact.cs /}


## Example III

* CompactDatabaseOperation automatically runs **on the store's database**.  
  If we try to compact **a different database**, the process will succeed only if the database 
  resides **on the cluster's first online node**.  
  Trying to compact a non-default database on a different node will fail with an error such as -  
  **_"500 Internal Server Error : 
  System.InvalidOperationException , 
  Cannot compact database 'name' on node A, because it doesn't reside on this node."_**  
  
* To solve this, we can explicitly identify the database we want to compact by providing 
  its name to CompactDatabaseOperation as in the following example.  
  {CODE compact_5@ClientApi\Operations\Server\Compact.cs /}

## Related Articles

- [How to **create** database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database **statistics**?](../../../client-api/operations/maintenance/get-statistics)
- [How to start **restore** operation?](../../../client-api/operations/server-wide/restore-backup)

