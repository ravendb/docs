# Compare Exchange in Cluster-Wide Session

---

{NOTE: }

* Compare-Exchange items can be created and managed on the advanced session (`session.advanced`).  
  Other options are listed in this [compare-exchange overview](../../../client-api/operations/compare-exchange/overview#how-to-create-and-manage-compare-exchange-items).

* When working with compare-exchange items from the session,  
  the session **must be opened as a [cluster-wide session](../../../client-api/session/cluster-transaction/overview#open-a-cluster-transaction)**.

* In this page:
    * [Create compare-exchange](../../../client-api/session/cluster-transaction/compare-exchange#create-compare-exchange)
    * [Get compare-exchange](../../../client-api/session/cluster-transaction/compare-exchange#get-compare-exchange)
    * [Delete compare-exchange](../../../client-api/session/cluster-transaction/compare-exchange#delete-compare-exchange)
{NOTE/}

---

{PANEL: Create compare-exchange}

{NOTE: }
#### Example

{CODE:php new_compare_exchange_sync@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

* `saveChanges()` throws a `ConcurrencyException` if the key already exists.
* An `InvalidOperationException` exception is thrown if the session was Not opened as **cluster-wide**.

{NOTE/}

{NOTE: }
#### Syntax

{CODE:php methods_3_sync@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

| Parameters   | Type     | Description                                                        |
|--------------|----------|--------------------------------------------------------------------|
| **key**      | `string` | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `T`      | The associated value to store for the key                          |

| Return Value              | Description                               |
|---------------------------|-------------------------------------------|
| `CompareExchangeValue<T>` | The new compare-exchange item is returned |
{NOTE/}

{NOTE: }
#### The CompareExchangeValue

| Parameters   | Type     | Description                                                        |
|--------------|----------|--------------------------------------------------------------------|
| **key**      | `string` | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `T`      | The value associated with the key                                  |
| **index**    | `long`   | Index for concurrency control                                      |

{NOTE/}
{PANEL/}

{PANEL: Get compare-exchange}

{NOTE: }
#### Get single value

{CODE:php methods_1_sync@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

| Parameters   | Type     | Description         |
|--------------|----------|---------------------|
| **key**      | `string` | The key to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `CompareExchangeValue<T>`| The compare exchange value, or `null` if it doesn't exist |

{NOTE/}

{NOTE: }
#### Get multiple values

{CODE:php methods_2_sync@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

| Parameters   | Type       | Description               |
|--------------|------------|---------------------------|
| **keys**     | `string[]` | Array of keys to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `Dictionary<string, CompareExchangeValue<T>>` | If a key doesn't exists the associate value will be `null` |
{NOTE/}

{NOTE: }
#### Get compare-exchange lazily

{CODE:php methods_sync_lazy_1@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

| Parameters  | Type       | Description               |
|-------------|------------|---------------------------|
| **key**     | `string`   | The key to retrieve       |
| **keys**    | `string[]` | Array of keys to retrieve |

| Return Value | Description |
| ------------- | ----- |
| `Lazy<CompareExchangeValue<T>>`| If the key doesn't exist it will return `null` |
| `Lazy<Dictionary<string, CompareExchangeValue<T>>>` | If a key doesn't exists the associate value will be `null` |
{NOTE/}
{PANEL/}

{PANEL: Delete compare-exchange}

{NOTE: }

{CODE:php methods_4_sync@ClientApi\Session\ClusterTransaction\CompareExchange.php /}

| Parameters | Type                      | Description                                    |
|------------|---------------------------|------------------------------------------------|
| **key**    | `string`                  | The key of the compare-exchange item to delete |
| **index**  | `long`                    | The index of this compare-exchange item        |
| **item**   | `CompareExchangeValue<T>` | The compare-exchange item to delete            |

{NOTE/}
{PANEL/}

## Related Articles

### Compare-Exchange Operations

- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
