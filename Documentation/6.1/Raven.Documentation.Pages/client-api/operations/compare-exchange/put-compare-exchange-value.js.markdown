# Put Compare Exchange Operation
---

{NOTE: }


* Use the `PutCompareExchangeValueOperation` operation to either:
  * **Create** a new compare-exchange item  
  * **Update** the value or metadata of an existing compare-exchange item

* Compare-exchange items can also be managed via [advanced session](../../../client-api/session/cluster-transaction/compare-exchange) methods 
  or from the [Studio](../../../studio/database/documents/compare-exchange-view).
  
* In this page:  
  * [Create new cmpXchg item](../../../client-api/operations/compare-exchange/put-compare-exchange-value#create-new-cmpxchg-item)  
  * [Create new cmpXchg item with metadata](../../../client-api/operations/compare-exchange/put-compare-exchange-value#create-new-cmpxchg-item-with-metadata)  
  * [Update existing cmpXchg item](../../../client-api/operations/compare-exchange/put-compare-exchange-value#update-existing-cmpxchg-item)
  * [Syntax](../../../client-api/operations/compare-exchange/put-compare-exchange-value#syntax)

{NOTE/}

---

{PANEL: Create new cmpXchg item}

{CODE:nodejs put_1@client-api\operations\compareExchange\putCompareExchange.js /}

* Note:  
  Using `0` with a key that already exists will Not modify existing compare-exchange item.

{PANEL/}

{PANEL: Create new cmpXchg item with metadata}

{CODE:nodejs put_2@client-api\operations\compareExchange\putCompareExchange.js /}

* Find more examples of adding metadata to a compare-exchange item in [compare-exchange metadata](../../../client-api/operations/compare-exchange/compare-exchange-metadata).

{PANEL/}

{PANEL: Update existing cmpXchg item}

* When calling `PutCompareExchangeValueOperation`,  
  the index from the request is compared to the index that is currently stored in the server for the specified _key_.
* The compare-exchange item is updated only if the two are equal.

{CODE:nodejs put_3@client-api\operations\compareExchange\putCompareExchange.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\compareExchange\putCompareExchange.js /}

| Parameter     | Type        | Description                                                                                                                                 |
|---------------|-------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| **key**       | `string`    | <ul><li>A unique identifier in the database scope.</li><li>Can be up to 512 bytes.</li><ul>                                                 |
| **value**     | `object`    | <ul><li>A value to be saved for the specified _key_.</li><li>Can be any object (number, string, array, or any valid JSON object).</li></ul> |
| **index**     | `number`    | <ul><li>Pass `0` to create a new key.</li><li>When updating an existing key, pass the current number for concurrency control.</li><ul>      |
| **metadata**  | `object`    | <ul><li>Metadata to be saved for the specified _key_.</li><li>Must be a valid JSON object.</li></ul>                                        |

{CODE:nodejs syntax_2@client-api\operations\compareExchange\putCompareExchange.js /}

| Return Value   | Type      | Description                                                                                                                                                                                                                                                                                                     |
|----------------|-----------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **successful** | `boolean` | <ul><li>`true` if the put operation has completed successfully.</li><li>`false` if the put operation has failed.</li></ul>                                                                                                                                                                                      |
| **value**      | `object`  | <ul><li>Upon success - the value of the compare-exchange item that was saved.</li><li>Upon failure - the existing value on the server.</li></ul>                                                                                                                                                                |
| **index**      | `number`  | <ul><li>The compare-exchange item's version.</li><li>This number increases with each successful modification of the `value` or `metadata`.</li><li>Upon success - the updated version of the compare-exchange item that was saved.</li><li>Upon failure - the existing version number in the server.</li></ul> |

{PANEL/}

## Related Articles

### Compare Exchange

- [Overview](../../../client-api/operations/compare-exchange/overview)
- [Get compare-exchange value](../../../client-api/operations/compare-exchange/get-compare-exchange-value)
- [Get compare-exchange values](../../../client-api/operations/compare-exchange/get-compare-exchange-values)
- [Delete compare-exchange](../../../client-api/operations/compare-exchange/delete-compare-exchange-value)
