# Get Index Terms Operation

---

{NOTE: }

* Use `GetTermsOperation` to retrieve the __terms of an index-field__.

* In this page:
    * [Get index terms example](../../../../client-api/operations/maintenance/indexes/get-terms#get-index-terms-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/get-terms#syntax)

{NOTE/}

---

{PANEL: Get index terms example}

{CODE:nodejs get_index_terms@ClientApi\Operations\Maintenance\Indexes\getIndexTerms.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs get_index_terms_syntax@ClientApi\Operations\Maintenance\Indexes\getIndexTerms.js /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | string | Name of an index to get terms for |
| **field** | string | Name of index-field to get terms for |
| **fromValue** | string | The starting term from which to return results.<br>This term is not included in the results.<br>`null` - start from first term. |
| **pageSize** | number | Number of terms to get.<br>`undefined/null` - return all terms.  |

| Return value of `store.maintenance.send(getTermsOp)` | |
| - |- |
| string[] | List of terms for the requested index-field. <br> Alphabetically ordered. |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)
