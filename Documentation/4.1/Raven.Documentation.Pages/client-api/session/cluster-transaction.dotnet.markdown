# Cluster Transaction - Overview

## Open Cluster-Wide Session

To open cluster transaction session you have to set the `TransactionMode` to `TransactionMode.ClusterWide`.

{CODE-TABS}
{CODE-TAB:csharp:Sync open_cluster_session@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async open_cluster_session_async@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

You can store, delete and edit document and the session will track them as usual.

## Working with Compare Exchange


### Get Compare Exchange

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_1@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async methods_async_1@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | The key to retrieve |

| Return Value | |
| ------------- | ----- |
| `CompareExchangeValue<T>`| If the key doesn't exists it will return `null` |

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_2@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async methods_async_2@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keys** | string[] | Array of keys to retrieve |

| Return Value | |
| ------------- | ----- |
| `Dictionary<string, CompareExchangeValue<T>>` | If a key doesn't exists the associate value will be `null` |

### Create Compare Exchange

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_3@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async methods_async_3@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | The key to save with the associate value |
| **value** | `T` | The value to store |

If the value is already exists `SaveChanges()` will throw a `ConcurrencyException`.

{CODE-TABS}
{CODE-TAB:csharp:Sync new_compare_exchange@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async new_compare_exchange_async@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

### Update Compare Exchange

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_5@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async methods_async_5@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **item** | `CompareExchangeValue<T>` | The item to update |

If the value was changed by someone else the `SaveChanges()` will throw a `ConcurrencyException`.

{CODE-TABS}
{CODE-TAB:csharp:Sync update_compare_exchange@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async update_compare_exchange_async@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

### Delete Compare Exchange

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_4@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TAB:csharp:Async methods_async_4@ClientApi\Session\ClusterTransactions\ClusterTransactions.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | The key to save with the associate value |
| **index** | long | Index for concurrency control |

#### CompareExchangeValue

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | Key of the item to store |
| **index** | long | Index for concurrency control |
| **value** | `T` | The actual value to keep |