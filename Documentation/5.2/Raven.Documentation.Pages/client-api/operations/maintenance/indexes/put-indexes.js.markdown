# Put Indexes Operation

 ---

{NOTE: }

* There are a few ways to create and deploy indexes in a database.  

* This page describes deploying a __static-index__ using the `PutIndexesOperation` Operation.  
  For a general description of Operations see [what are operations](../../../../client-api/operations/what-are-operations).  

* In this page:
    * [Ways to deploy indexes - short summary](../../../../client-api/operations/maintenance/indexes/put-indexes#ways-to-deploy-indexes---short-summary)
    * [Put indexes operation with IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/put-indexes#syntax)

{NOTE/}

---

{PANEL: Ways to deploy indexes - short summary }
{NOTE: }

__Static index__:

There are a few ways to deploy a static-index from the Client API:  

  * Call `execute()` on a specific index instance
  * Call `IndexCreation.createIndexes()` to deploy multiple indexes
  * Execute `PutIndexesOperation` maintenance operation on the Document Store - see below
  * Learn more in [static indexes](../../../../indexes/creating-and-deploying#static-indexes)

{NOTE/}
{NOTE: }

__Auto index__:  

  * An auto-index is created by the server when making a filtering query that doesn't specify which index to use
  * Learn more in [auto indexes](../../../../indexes/creating-and-deploying#auto-indexes)

{NOTE/}  
{PANEL/}

{PANEL: Put indexes operation with IndexDefinition }
{NOTE: }

Using `PutIndexesOperation` with __IndexDefinition__ allows the following:  

  * Choosing any name for the index
  * Setting low-level properties available in _IndexDefinition_
{NOTE/}

{CODE-TABS}
{CODE-TAB:nodejs:LINQ-index put_1@ClientApi\Operations\Maintenance\Indexes\put.js /}
{CODE-TAB:nodejs:JavaScript-index put_1_JS@ClientApi\Operations\Maintenance\Indexes\put.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\put.js /}

| Parameters | Type | Description |
| - |- | - |
| **indexesToAdd** | `...IndexDefinition[]` | Definitions of indexes to deploy |

<a id="indexDefinition" />

| `IndexDefinition` | | |
| - | - | - |
| name | string | Name of the index, a unique identifier |
| maps | Set&lt;string&gt; | All the map functions for the index |
| reduce | string | The index reduce function |
| deploymentMode | object | Deployment mode<br>(Parallel, Rolling) |
| state | object | State of index<br>(Normal, Disabled, Idle, Error) |
| priority | object | Priority of index<br>(Low, Normal, High) |
| lockMode | object | Lock mode of index<br>(Unlock, LockedIgnore, LockedError) |
| fields | Record&lt;string, object&gt; | _IndexFieldOptions_ per index field |
| additionalSources | Record&lt;string, string&gt; | Additional code files to be compiled with this index |
| additionalAssemblies | object[] | Additional assemblies that are referenced |
| configuration | object | Can override [indexing configuration](../../../../server/configuration/indexing-configuration) by setting this Record&lt;string, string&gt; |
| outputReduceToCollection | string | A collection name for saving the reduce results as documents |
| reduceOutputIndex | number | This number will be part of the reduce results documents IDs |
| patternForOutputReduceToCollectionReferences | string | Pattern for documents IDs which reference IDs of reduce results documents |
| patternReferencesCollectionName | string | A collection name for the reference documents created based on provided pattern |

| Return value of `store.maintenance.send(putIndexesOp)` | |
| - | - |
| `object[]` | operation result per index |

| Operation result per index | | |
| - | - | - |
| index | string | Name of the index that was added |
| raftCommandIndex | long | Index of raft command that was executed |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
