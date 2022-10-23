# Compare Exchange in Cluster-Wide Session

---

{NOTE: }

* Compare-Exchange items can be created and managed on the advanced session (`Session.Advanced`)  
  Other options are listed in this [compare-exchange overview](../../../client-api/operations/compare-exchange/overview#how-to-create-and-manage-compare-exchange-items).

* When working with compare-exchange items from the session,  
  the session __must be opened as a [cluster-wide session](../../../client-api/session/cluster-transaction/overview#open-a-cluster-transaction)__.

* In this page:
    * [Create compare-exchange](../../../client-api/session/cluster-transaction#compare-exchange#create-compare-exchange)
    * [Get compare-exchange](../../../client-api/session/cluster-transaction#compare-exchange#get-compare-exchange)
    * [Delete compare-exchange](../../../client-api/session/cluster-transaction#compare-exchange#delete-compare-exchange)
{NOTE/}

---

{PANEL: Create compare-exchange}

{NOTE: }
__Example__

{CODE-TABS}
{CODE-TAB:csharp:Sync new_compare_exchange_sync@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async new_compare_exchange_async@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

* `SaveChanges()` throws a `ConcurrencyException` if the key already exists.
* An `InvalidOperationException` exception is thrown if the session was Not opened as __cluster-wide__.

{NOTE/}

{NOTE: }
__Syntax__

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_3_sync@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async methods_async_3@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

| Parameters   | Type     | Description                                                        |
|--------------|----------|--------------------------------------------------------------------|
| **key**      | `string` | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `T`      | The associated value to store for the key                          |

| Return Value              | Description                               |
|---------------------------|-------------------------------------------|
| `CompareExchangeValue<T>` | The new compare-exchange item is returned |
{NOTE/}

{NOTE: }
__The CompareExchangeValue__

| Parameters   | Type     | Description                                                        |
|--------------|----------|--------------------------------------------------------------------|
| **key**      | `string` | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `T`      | The value associated with the key                                  |
| **index**    | `long`   | Index for concurrency control                                      |

{NOTE/}
{PANEL/}

{PANEL: Get compare-exchange}

{NOTE: }
__Get single value__

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_1_sync@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async methods_async_1@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **key** | `string` | The key to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `CompareExchangeValue<T>`| If the key doesn't exist it will return `null` |

{NOTE/}

{NOTE: }
__Get multiple values__

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_2_sync@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async methods_async_2@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **keys** | `string[]` | Array of keys to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `Dictionary<string, CompareExchangeValue<T>>` | If a key doesn't exists the associate value will be `null` |
{NOTE/}

{NOTE: }
__Get compare-exchange lazily__

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_sync_lazy_1@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async methods_async_lazy_1@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **key** | `string` | The key to retrieve |
| **keys** | `string[]` | Array of keys to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `Lazy<CompareExchangeValue<T>>`| If the key doesn't exist it will return `null` |
| `Lazy<Dictionary<string, CompareExchangeValue<T>>>` | If a key doesn't exists the associate value will be `null` |
{NOTE/}
{PANEL/}

{PANEL: Delete compare-exchange}

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Sync methods_4_sync@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TAB:csharp:Async methods_async_4@ClientApi\Session\ClusterTransaction\CompareExchange.cs /}
{CODE-TABS/}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **key** | `string` | The key to save with the associate value |
| **index** | `long` | Index for concurrency control |

{NOTE/}
{PANEL/}

## Related Articles

### Compare-Exchange Operations

- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
