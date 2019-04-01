# Operations: Counters: Batch

*CounterBatchOperation* allows you to operate on multiple counters (`INCREMENT`, `GET`, `DELETE`) of different documents in a **single request**.

{PANEL:Syntax}

{CODE:java counter_batch_op@ClientApi\Operations\Counters.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **counterBatch** | `CounterBatch` | An object that holds a list of `DocumentCountersOperation`. Each element in the list describes the counter operations to perform for a specific document |

{CODE:java counter_batch@ClientApi\Operations\Counters.java /}

####DocumentCountersOperation 

{CODE:java document_counters_op@ClientApi\Operations\Counters.java /}

####CounterOperation 

{CODE:java counter_operation@ClientApi\Operations\Counters.java /}

####CounterOperationType

{CODE:java counter_operation_type@ClientApi\Operations\Counters.java /}

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

{PANEL:Return Value}

*CounterBatchOperation* returns a `CountersDetail` object, which holds a list of `CounterDetail` objects

{CODE:java counters_detail@ClientApi\Operations\Counters.java /}

{CODE:java counter_detail@ClientApi\Operations\Counters.java /}

If a `CounterOperationType` is `INCREMENT` or `GET`, a `CounterDetail` object will be added to the result.  
`DELETE` operations will not be included in the result.

{PANEL/}

{PANEL:Examples}

Assume we have two documents, *"users/1"* and *"users/2"*, that hold 3 counters each -  
*"likes"*, *"dislikes"* and *"downloads"* -  with values 10, 20 and 30 (respectively)

###Example #1 : Increment Multiple Counters in a Batch

{CODE:java counter_batch_exmpl1@ClientApi\Operations\Counters.java /}

####Result:
{CODE-BLOCK:json}
{
	"Counters": 
    [
		{
			"DocumentId" : "users/1",
			"CounterName" : "likes",
			"TotalValue" : 15,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/1",
			"CounterName" : "dislikes",
			"TotalValue" : 21,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/2",
			"CounterName" : "likes",
			"TotalValue" : 110,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/2",
			"CounterName" : "score",
			"TotalValue" : 50,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}

###Example #2 : Get Multiple Counters in a Batch

{CODE:java counter_batch_exmpl2@ClientApi\Operations\Counters.java /}

####Result:
{CODE-BLOCK:json}
{
	"Counters": 
    [
		{
			"DocumentId" : "users/1",
			"CounterName" : "likes",
			"TotalValue" : 10,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/1",
			"CounterName" : "downloads",
			"TotalValue" : 30,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/2",
			"CounterName" : "likes",
			"TotalValue" : 10,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/2",
			"CounterName" : "dislikes",
			"TotalValue" : 20,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}

###Example #3 : Delete Multiple Counters in a Batch

{CODE:java counter_batch_exmpl3@ClientApi\Operations\Counters.java /}

####Result:

{CODE-BLOCK:json}
{
	"Counters": []
}
{CODE-BLOCK/}

###Example #4 : Mix Different Types of CounterOperations in a Batch

{CODE:java counter_batch_exmpl4@ClientApi\Operations\Counters.java /}

####Result:
{CODE-BLOCK:json}
{
	"Counters": 
    [
		{
			"DocumentId" : "users/1",
			"CounterName" : "likes",
			"TotalValue" : 20,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/1",
			"CounterName" : "dislikes",
			"TotalValue" : 20,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/2",
			"CounterName" : "likes",
			"TotalValue" : 10,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}
{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Get Counters Operation](../../../client-api/operations/counters/get-counters)


