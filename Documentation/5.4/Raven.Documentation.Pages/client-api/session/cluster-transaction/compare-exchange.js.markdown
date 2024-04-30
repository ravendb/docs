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

{CODE:nodejs create_compare_exchange_example@client-api\session\ClusterTransaction\CompareExchange.js /}

* An `InvalidOperationException` exception is thrown when:
  * The session was Not opened as **cluster-wide**.
  * The key already exists in the database.

{NOTE/}

{NOTE: }
#### Syntax

{CODE:nodejs create_compare_exchange@client-api\session\ClusterTransaction\CompareExchange.js /}

| Parameters   | Type      | Description                                                        |
|--------------|-----------|--------------------------------------------------------------------|
| **key**      | `string`  | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `object`  | The associated value to store for the key                          |

| Return value | Description                               |
|---------------|-------------------------------------------|
| `object`      | The new compare-exchange item is returned |
{NOTE/}

{NOTE: }
#### The compare exchange object returned

| Parameters   | Type     | Description                                                        |
|--------------|----------|--------------------------------------------------------------------|
| **key**      | `string` | The compare-exchange item key. This string can be up to 512 bytes. |
| **value**    | `object` | The value associated with the key                                  |
| **index**    | `number` | Index for concurrency control                                      |

{NOTE/}
{PANEL/}

{PANEL: Get compare-exchange}

{NOTE: }
#### Get single value

{CODE:nodejs get_compare_exchange_1@client-api\session\ClusterTransaction\CompareExchange.js /}

| Parameters   | Type     | Description                    |
|--------------|----------|--------------------------------|
| **key**      | `string` | The key to retrieve            |

| Return value | Description                                                                     |
|---------------|---------------------------------------------------------------------------------|
| `object`      | The compare-exchange item is returned.<br> Returns `null` if key doesn't exist. |

[key: string]: CompareExchangeValue<T>;

{NOTE/}

{NOTE: }
#### Get multiple values

{CODE:nodejs get_compare_exchange_2@client-api\session\ClusterTransaction\CompareExchange.js /}

| Parameters  | Type        | Description               |
|-------------|-------------|---------------------------|
| **keys**    | `string[]`  | Array of keys to retrieve |

| Return value            | Description                                                |
|--------------------------|------------------------------------------------------------|
| `Record<string, object>` | If a key doesn't exists the associate value will be `null` |
{NOTE/}

{NOTE: }
#### Get compare-exchange lazily

{CODE:nodejs get_compare_exchange_3@client-api\session\ClusterTransaction\CompareExchange.js /}

| Parameters   | Type       | Description               |
|--------------|------------|---------------------------|
| **key**      | `string`   | The key to retrieve       |
| **keys**     | `string[]` | Array of keys to retrieve |

| Return value - after calling `getValue` | Description                                                                        |
|-----------------------------------------|------------------------------------------------------------------------------------|
| `object`                                | For single item:<br>If the key doesn't exist it will return `null`                 |
| `Record<string, object>`                | For multiple items:<br>If a key doesn't exists the associate value will be `null`  |
{NOTE/}
{PANEL/}

{PANEL: Delete compare-exchange}

{NOTE: }

{CODE:nodejs delete_compare_exchange@client-api\session\ClusterTransaction\CompareExchange.js /}

| Parameters | Type     | Description                                    |
|------------|----------|------------------------------------------------|
| **key**    | `string` | The key of the compare-exchange item to delete |
| **index**  | `number` | The index of this compare-exchange item        |
| **item**   | `object` | The compare-exchange item to delete            |

{NOTE/}
{PANEL/}

## Related Articles

### Compare-Exchange Operations

- [Compare Exchange: Overview](../../../client-api/operations/compare-exchange/overview)
- [Get a Compare-Exchange Value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get Compare-Exchange Values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Put a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
- [Delete a Compare-Exchange Value](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
