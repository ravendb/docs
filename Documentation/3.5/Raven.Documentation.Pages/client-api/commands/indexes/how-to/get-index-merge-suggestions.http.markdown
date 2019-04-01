# Commands: Indexes: How to get index merge suggestions?

**GetIndexMergeSuggestions** will retrieve all suggestions for an index merging.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/debug/suggest-index-merge \
	-X GET 
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | [IndexMergeResults](../../../../glossary/index-merge-results) |

<hr />

### Example I

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/debug/suggest-index-merge" 
< HTTP/1.1 200 OK
{
	"Unmergables":
	{
		"Raven/DocumentsByEntityName":"Cannot merge indexes that are using a let clause",
		"Orders/ByCompany":"Cannot merge map/reduce indexes",
		"Product/Sales":"Cannot merge map/reduce indexes"
	},
	"Suggestions":
	[
		{
			"CanMerge":["Orders/Total2s","Orders/Totals"],
			"Collection":"Orders",
			"MergedIndex":  {mergedIndexDefinition} 
		}
	]
}
{CODE-BLOCK/}



## Related articles

- [How to **reset index**?](../../../../client-api/commands/indexes/how-to/reset-index)   
- [How to **get index terms**?](../../../../client-api/commands/indexes/how-to/get-index-terms) 
