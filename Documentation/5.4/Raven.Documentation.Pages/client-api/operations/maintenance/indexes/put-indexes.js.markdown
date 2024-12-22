# Put Indexes Operation

 ---

{NOTE: }

* There are a few ways to create and deploy indexes in a database.  

* This page describes deploying a **static-index** using the `PutIndexesOperation` Operation.  
  For a general description of Operations see [what are operations](../../../../client-api/operations/what-are-operations).  

* In this page:
    * [Ways to deploy indexes - short summary](../../../../client-api/operations/maintenance/indexes/put-indexes#ways-to-deploy-indexes---short-summary)
    * [Put indexes operation with IndexDefinition](../../../../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/put-indexes#syntax)

{NOTE/}

---

{PANEL: Ways to deploy indexes - short summary }

{NOTE: }

##### Static-indexes:

There are a few ways to deploy a static-index from the Client API:

* The following methods are explained in section [Deploy a static-index](../../../../indexes/creating-and-deploying#deploy-a-static-index):
    * Call `execute()` on a specific index instance.
    * Call `executeIndex()` or `executeIndexes()` on your _DocumentStore_ object.
    * Call `IndexCreation.createIndexes()`.

* Alternatively, you can execute the `PutIndexesOperation` maintenance operation on the _DocumentStore_, **as explained below**.

{NOTE/}
{NOTE: }

##### Auto-indexes:

* An auto-index is created by the server when making a filtering query that doesn't specify which index to use.
  Learn more in [Creating auto indexes](../../../../indexes/creating-and-deploying#auto-indexes).

{NOTE/}
{PANEL/}

{PANEL: Put indexes operation with IndexDefinition }

Using `PutIndexesOperation` with **IndexDefinition** allows you to:

  * Choose any name for the index.  
    This string-based name is specified when querying the index.
  * Set low-level properties available in _IndexDefinition_.

{CODE-TABS}
{CODE-TAB:nodejs:LINQ-index put_1@client-api\operations\maintenance\indexes\put.js /}
{CODE-TAB:nodejs:JavaScript-index put_1_JS@client-api\operations\maintenance\indexes\put.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE:nodejs syntax@client-api\operations\maintenance\indexes\put.js /}

| Parameter        | Type                   | Description                      |
|------------------|------------------------|----------------------------------|
| **indexesToAdd** | `...IndexDefinition[]` | Definitions of indexes to deploy |

<a id="indexDefinition" />

| `IndexDefinition` parameter                  | Type                     | Description                                                                                                                                 |
|----------------------------------------------|--------------------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| name                                         | `string`                 | Name of the index, a unique identifier                                                                                                      |
| maps                                         | `Set<string>`            | All the map functions for the index                                                                                                         |
| reduce                                       | `string`                 | The index reduce function                                                                                                                   |
| deploymentMode                               | `object`                 | Deployment mode<br>(Parallel, Rolling)                                                                                                      |
| state                                        | `object`                 | State of index<br>(Normal, Disabled, Idle, Error)                                                                                           |
| priority                                     | `object`                 | Priority of index<br>(Low, Normal, High)                                                                                                    |
| lockMode                                     | `object`                 | Lock mode of index<br>(Unlock, LockedIgnore, LockedError)                                                                                   |
| fields                                       | `Record<string, object>` | _IndexFieldOptions_ per index field                                                                                                         |
| additionalSources                            | `Record<string, string>` | Additional code files to be compiled with this index                                                                                        |
| additionalAssemblies                         | `object[]`               | Additional assemblies that are referenced                                                                                                   |
| configuration                                | `object`                 | Can override [indexing configuration](../../../../server/configuration/indexing-configuration) by setting this Record&lt;string, string&gt; |
| outputReduceToCollection                     | `string`                 | A collection name for saving the reduce results as documents                                                                                |
| reduceOutputIndex                            | `number`                 | This number will be part of the reduce results documents IDs                                                                                |
| patternForOutputReduceToCollectionReferences | `string`                 | Pattern for documents IDs which reference IDs of reduce results documents                                                                   |
| patternReferencesCollectionName              | `string`                 | A collection name for the reference documents created based on provided pattern                                                             |

| `store.maintenance.send(putIndexesOp)` return value  | Description                |
|------------------------------------------------------|----------------------------|
| `object[]`                                           | operation result per index |

| Operation result per index  | Type     | Description                             |
|-----------------------------|----------|-----------------------------------------|
| index                       | `string` | Name of the index that was added        |
| raftCommandIndex            | `long`   | Index of raft command that was executed |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [How to Get Indexes](../../../../client-api/operations/maintenance/indexes/get-indexes)
- [How to Delete Index](../../../../client-api/operations/maintenance/indexes/delete-index)
- [How to Reset Index](../../../../client-api/operations/maintenance/indexes/reset-index)
