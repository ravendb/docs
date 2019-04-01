# Operations: How to Get Index Terms

The **GetTermsOperation** will retrieve stored terms for a field of an index.

## Syntax

{CODE:java get_terms1@ClientApi\Operations\IndexTerms.java /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to get terms for |
| **field** | String | Name of field to get terms for |
| **fromValue** | String | Starting point for a query, used for paging |
| **pageSize** | Integer | Maximum number of terms that will be returned |

| Return Value | |
| ------------- | ----- |
| String[] | Array of index terms. |

## Example

{CODE:java get_terms2@ClientApi\Operations\IndexTerms.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)
