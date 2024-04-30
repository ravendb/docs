# Compare Exchange in Cluster-Wide Session

---

{NOTE: }

* Compare-Exchange items can be created and managed on the advanced session (`session.advanced`)  
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

{CODE:python new_compare_exchange_sync@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

* `save_changes()` throws a `ConcurrencyException` if the key already exists.
* A `RuntimeError` exception is thrown if the session was Not opened as **cluster-wide**.

{NOTE/}

{NOTE: }
#### Syntax

{CODE:python methods_3_sync@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

| Parameters   | Type     | Description                                                |
|--------------|----------|------------------------------------------------------------|
| **key**      | `str`    | The key for the compare-exchange item to be created<br>This string can be up to 512 bytes |
| **value**    | `T`      | The value to associate with this key                       |

| Return Value              | Description                               |
|---------------------------|-------------------------------------------|
| `CompareExchangeValue[T]` | The new compare-exchange item is returned |
{NOTE/}

{NOTE: }
#### The CompareExchangeValue

| Parameters   | Type     | Description                                                         |
|--------------|----------|---------------------------------------------------------------------|
| **key**      | `str`    | The compare-exchange item key<br>This string can be up to 512 bytes |
| **value**    | `T`      | The value associated with the key                                   |
| **index**    | `int`    | Index for concurrency control                                       |

{NOTE/}
{PANEL/}

{PANEL: Get compare-exchange}

{NOTE: }
#### Get single value

{CODE:python methods_1_sync@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

| Parameters   | Type     | Description         |
|--------------|----------|---------------------|
| **key**      | `str`    | The key for a compare-exchange item whose value is requested |

| Return Value | Description |
| ------------- | ----- |
| `CompareExchangeValue[T]`| The requested value<br>If the key doesn't exist the value associated with it will be `None` |

{NOTE/}

{NOTE: }
#### Get multiple values

{CODE:python methods_2_sync@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

| Parameters   | Type        | Description               |
|--------------|-------------|---------------------------|
| **keys**     | `List[str]` | An array of compare-exchange keys whose values are requested |

| Return Value | Description |
| ------------- | ----- |
| `Dict[str, CompareExchangeValue[T]]` | A dictionary of requested values<br>If a key doesn't exist the value associated with it will be `None` |
{NOTE/}

{NOTE: }
#### Get compare-exchange lazily

{CODE:python methods_sync_lazy_1@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

| Parameters  | Type        | Description               |
|-------------|-------------|---------------------------|
| **key**     | `str`       | The key for a compare-exchange item whose value is requested |
| **keys**    | `List[str]` | An array of compare-exchange keys whose values are requested |

| Return Value | Description |
| ------------- | ----- |
| `Lazy[CompareExchangeValue[T]]`| The requested value<br>If the key doesn't exist the value associated with it will be `None` |
| `Lazy[Dict[str, CompareExchangeValue[T]]]` | A dictionary of requested values<br>If a key doesn't exist the value associated with it will be `None` |
{NOTE/}
{PANEL/}

{PANEL: Delete compare-exchange}

To delete a compare exchange item, use either of the following methods.  

{NOTE: }

{CODE:python methods_4_sync@ClientApi\Session\ClusterTransaction\CompareExchange.py /}

| Parameters | Type                      | Description                                     |
|------------|---------------------------|-------------------------------------------------|
| **key**    | `str`                     | The key for the compare-exchange item to delete |
| **index**  | `int`                     | The index for the compare-exchange item to delete   |
| **item**   | `CompareExchangeValue[T]` | The compare-exchange item to delete             |

{NOTE/}
{PANEL/}

## Related Articles

### Compare-Exchange Operations

- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
