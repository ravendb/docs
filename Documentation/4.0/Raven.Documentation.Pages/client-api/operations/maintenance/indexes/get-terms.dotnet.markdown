# Operations: How to Get Index Terms

The **GetTermsOperation** will retrieve stored terms for a field of an index.

## Syntax

{CODE get_terms1@ClientApi\Operations\IndexTerms.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to get terms for |
| **field** | string | Name of field to get terms for |
| **fromValue** | string | Starting point for a query, used for paging |
| **pageSize** | int? | Maximum number of terms that will be returned |

| Return Value | |
| ------------- | ----- |
| string[] | Array of index terms. |

## Example

{CODE get_terms2@ClientApi\Operations\IndexTerms.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)
