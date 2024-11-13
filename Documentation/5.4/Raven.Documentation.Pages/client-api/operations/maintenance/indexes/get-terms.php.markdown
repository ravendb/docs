# Get Index Terms Operation

---

{NOTE: }

* Use `GetTermsOperation` to retrieve the **terms of an index-field**.  

* In this page:
    * [Get Terms example](../../../../client-api/operations/maintenance/indexes/get-terms#get-terms-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-terms#syntax)

{NOTE/}

---

{PANEL: Get Terms example}

{CODE:php get_index_terms@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php get_index_terms_syntax@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.php /}

| Parameters | Type | Description |
| - | - | - |
| **$indexName** | `?string` | Name of an index to get terms for |
| **$field** | `?string` | Name of index-field to get terms for |
| **$fromValue** | `?string` | The starting term to return results from.<br>This term is not included in the results.<br>`None` - start from first term. |
| **$pageSize** | `?int` | Number of terms to get.<br>`None` - return all terms.  |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)
