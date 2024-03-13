# Counters Batch Operation

*CounterBatchOperation* allows you to operate on multiple counters (`Increment`, `Get`, `Delete`) of different documents in a **single request**.

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\counters\countersBatch.js /}

| Parameter        |                |                                                                                                                                                              |
|------------------|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **counterBatch** | `CounterBatch` | An object that holds a list of `DocumentCountersOperation`.<br>Each element in the list describes the counter operations to perform for a specific document |

{CODE:nodejs syntax_2@client-api\operations\counters\countersBatch.js /}

{CODE:nodejs syntax_3@client-api\operations\counters\countersBatch.js /}

{CODE:nodejs syntax_4@client-api\operations\counters\countersBatch.js /}

{INFO: Document updates as a result of a counter operation }
A document that has counters holds all its counter names in the `metadata`.  
Therefore, when creating a new counter, the parent document is modified, as the counter's name needs to be added to the metadata.  
Deleting a counter also modifies the parent document, as the counter's name needs to be removed from the metadata.  
Incrementing an existing counter will not modify the parent document.

Even if a `DocumentCountersOperation` contains several `CounterOperation` items that affect the document's metadata (create, delete),
the parent document will be modified **only once**, after all the `CounterOperation` items in this `DocumentCountersOperation` have been processed.  
If `DocumentCountersOperation` doesn't contain any `CounterOperation` that affects the metadata, the parent document won't be modified.

{INFO/}

{PANEL/}

{PANEL: Return Value}

* *CounterBatchOperation* returns a `CountersDetail` object, which holds a list of `CounterDetail` objects.

* If the type is `Increment` or `Get`, a `CounterDetail` object will be added to the result.  
  `Delete` operations will Not be included in the result.

{CODE:nodejs syntax_5@client-api\operations\counters\countersBatch.js /}

{CODE:nodejs syntax_6@client-api\operations\counters\countersBatch.js /}

{PANEL/}

{PANEL: Examples}

Assume we have two documents, `users/1` and `users/2`, that hold 3 counters each -  
_"Likes"_, _"Dislikes"_ and _"Downloads"_ -  with values 10, 20 and 30 (respectively)

---

### Example #1 : Increment Multiple Counters in a Batch

{CODE:nodejs example_1@client-api\operations\counters\countersBatch.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 15,
			"counterValues" : null
		},
        {
			"documentId"    : "users/1",
			"counterName"   : "Dislikes",
			"totalValue"    : 20,
			"counterValues" : null
		},
        {
			"documentId"    : "users/2",
			"counterName"   : "Likes",
			"totalValue"    : 110,
			"counterValues" : null
		},
        {
			"documentId"    : "users/2",
			"counterName"   : "score",
			"totalValue"    : 50,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

---

### Example #2 : Get Multiple Counters in a Batch

{CODE:nodejs example_2@client-api\operations\counters\countersBatch.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 15,
			"counterValues" : null
		},
        {
			"documentId"    : "users/1",
			"counterName"   : "Downloads",
			"totalValue"    : 30,
			"counterValues" : null
		},
        {
			"documentId"    : "users/2",
			"counterName"   : "Likes",
			"totalValue"    : 110,
			"counterValues" : null
		},
        {
			"documentId"    : "users/2",
			"counterName"   : "Score",
			"totalValue"    : 50,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

---

### Example #3 : Delete Multiple Counters in a Batch

{CODE:nodejs example_3@client-api\operations\counters\countersBatch.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": []
}
{CODE-BLOCK/}

---

### Example #4 : Mix Different Types of CounterOperations in a Batch

{CODE:nodejs example_4@client-api\operations\counters\countersBatch.js /}

#### Result:

* Note: The `Delete` operations are Not included in the result.

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 30,
			"counterValues" : null
		},
        null,
        {
			"documentId"    : "users/2",
			"counterName"   : "Likes",
			"totalValue"    : 110,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Get Counters Operation](../../../client-api/operations/counters/get-counters)


