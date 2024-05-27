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

{CODE:python get_index_terms@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python get_index_terms_syntax@ClientApi\Operations\Maintenance\Indexes\GetIndexTerms.py /}

| Parameters | Type | Description |
| - | - | - |
| **index_name** | `str` | Name of an index to get terms for |
| **field** | `str` | Name of index-field to get terms for |
| **from_value** | `str` (optional) | The starting term from which to return results.<br>This term is not included in the results.<br>`None` - start from first term. |
| **page_size** | `int` | Number of terms to get.<br>`None` - return all terms.  |

| Return value of `store.maintenance.send(GetTermsOperation)` | Description |
| - |- |
| `List[str]` | List of terms for the requested index-field. <br> Alphabetically ordered. |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)
