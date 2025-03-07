# Get Counters Operation

This operation is used to get counters' values for a specific document.  
It can be used to get the value of a single counter, multiple counters' values, or all counters' values.

{PANEL: Syntax}

#### Get Single Counter

{CODE:java get_single_counter@ClientApi\Operations\Counters\Counters.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | String | The ID of the document that holds the counters |
| **counter** | String | The name of the counter to get |
| **returnFullResults** | boolean | A flag which indicates if the operation should include a dictionary of counter values per database node in the result  |

{INFO: }

**Return Full Results flag**:  

If RavenDB is running in a distributed cluster, and the database resides on several nodes,
a counter can have a different *local* value on each database node, and the total counter value is the
sum of all the local values of this counter from each node.  
In order to get the counter values per database node, set the `returnFullResults` flag to `true`

{INFO/}

---

#### Get Multiple Counters 

{CODE:java get_multiple_counters@ClientApi\Operations\Counters\Counters.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | String | The ID of the document that holds the counters |
| **counters** | String[] | The names of the counters to get |
| **returnFullResults** | boolean | A flag which indicates if the operation should include a dictionary of counter values per database node in the result  |

---

#### Get All Counters of a Document 

{CODE:java get_all_counters@ClientApi\Operations\Counters\Counters.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | String | The ID of the document that holds the counters |
| **returnFullResults** | boolean | A flag which indicates if the operation should include a dictionary of counter values per database node in the result  |

{PANEL/}

{PANEL: Return Value}

The operation returns a `CountersDetail` object, which holds a list of `CounterDetail` objects

{CODE:java counters_detail@ClientApi\Operations\Counters\Counters.java /}

{CODE:java counter_detail@ClientApi\Operations\Counters\Counters.java /}

{PANEL/}

{PANEL: Examples}

Assume we have a `users/1` document that holds 3 counters:  
`likes`, `dislikes` and `downloads` -  with values 10, 20 and 30 (respectively)

### Example #1 : Get single counter

{CODE:java get_counters1@ClientApi\Operations\Counters\Counters.java /}

#### Result:

{CODE-BLOCK:json}
{
	"Counters": 
    [
		{
			"DocumentId" : "users/1",
			"CounterName" : "likes",
			"TotalValue" : 10,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #2 : Get multiple counters 

{CODE:java get_counters2@ClientApi\Operations\Counters\Counters.java /}

#### Result:

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
			"CounterName" : "dislikes",
			"TotalValue" : 20,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #3 : Get all counters 

{CODE:java get_counters3@ClientApi\Operations\Counters\Counters.java /}

#### Result:

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
			"CounterName" : "dislikes",
			"TotalValue" : 20,
			"CounterValues" : null
		},
        {
			"DocumentId" : "users/1",
			"CounterName" : "downloads",
			"TotalValue" : 30,
			"CounterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #4 : Include full values in the result

{CODE:java get_counters4@ClientApi\Operations\Counters\Counters.java /}

#### Result:

Assuming a 3-node cluster, the distribution of the counter's value to nodes A, B, and C could be as follows:

{CODE-BLOCK:json}
{
	"Counters": 
    [
		{
			"DocumentId" : "users/1",
			"CounterName" : "likes",
			"TotalValue" : 10,
			"CounterValues" : 
            {
                "A:35-UuCp420vs0u+URADcGVURA" : 5,
                "B:83-SeCFU29daUOxfjUcAlLiJw" : 3,
                "C:27-7i7GP8bOOkGYLNflO/rSeg" : 2,
            }
		}
	]
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Counter Batch Operation](../../../client-api/operations/counters/counter-batch)
