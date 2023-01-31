# Get Index Terms Operation

The **GetTermsOperation** will retrieve stored terms for a field of an index.

## Syntax

{CODE:java get_terms1@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.java /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to get terms for |
| **field** | String | Name of field to get terms for |
| **fromValue** | String | The starting term from which to return results |
| **pageSize** | Integer | Number of terms to get |

| Return Value | |
| ------------- | ----- |
| String[] | List of terms for the requested index-field. <br> Alphabetically ordered. |

## Example

{CODE:java get_terms2@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)
