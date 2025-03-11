# Get Counters Operation

This operation is used to get counters' values for a specific document.  
It can be used to get the value of a single counter, multiple counters' values, or all counters' values.

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\counters\getCounters.js /}

{CODE:nodejs syntax_2@client-api\operations\counters\getCounters.js /}

{CODE:nodejs syntax_3@client-api\operations\counters\getCounters.js /}

| Parameter             | Type     | Description                                                                                                           |
|-----------------------|----------|-----------------------------------------------------------------------------------------------------------------------|
| **docId**             | string   | The ID of the document that holds the counters                                                                        |
| **counter**           | string   | The name of the counter to get                                                                                        |
| **counters**          | string[] | The list of counter names to get                                                                                     |
| **returnFullResults** | boolean  | A flag which indicates if the operation should include a dictionary of counter values per database node in the result |

{INFO: }

**The full results flag:**  

If RavenDB is running in a distributed cluster, and the database resides on several nodes,  
then a counter can have a different *local* value on each database node.  
The total counter value is the sum of all the local values of this counter from each node.  
To get the counter values per database node, set the `returnFullResults` flag to `true`.

{INFO/}

{PANEL/}

{PANEL: Return Value}

The operation returns a `CountersDetail` object, which holds a list of `CounterDetail` objects

{CODE:nodejs syntax_4@client-api\operations\counters\getCounters.js /}

{CODE:nodejs syntax_5@client-api\operations\counters\getCounters.js /}

{PANEL/}

{PANEL: Examples}

Assume we have a `users/1` document that holds 3 counters:  
`Likes`, `Dislikes` and `Downloads` -  with values 10, 20 and 30 (respectively)

---

### Example #1 : Get single counter

{CODE:nodejs example_1@client-api\operations\counters\getCounters.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 10,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #2 : Get multiple counters 

{CODE:nodejs example_2@client-api\operations\counters\getCounters.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 10,
			"counterValues" : null
		},
        {
			"documentId"    : "users/1",
			"counterName"   : "Dislikes",
			"totalValue"    : 20,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #3 : Get all counters 

{CODE:nodejs example_3@client-api\operations\counters\getCounters.js /}

#### Result:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 10,
			"counterValues" : null
		},
        {
			"documentId"    : "users/1",
			"counterName"   : "Dislikes",
			"totalValue"    : 20,
			"counterValues" : null
		},
        {
			"documentId"    : "users/1",
			"counterName"   : "Downloads",
			"totalValue"    : 30,
			"counterValues" : null
		}
	]
}
{CODE-BLOCK/}

### Example #4 : Include full values in the result

{CODE:nodejs example_4@client-api\operations\counters\getCounters.js /}

#### Result:

Assuming a 3-node cluster, the distribution of the counter's value to nodes A, B, and C could be as follows:

{CODE-BLOCK:json}
{
	"counters": 
    [
		{
			"documentId"    : "users/1",
			"counterName"   : "Likes",
			"totalValue"    : 10,
			"counterValues" : 
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
