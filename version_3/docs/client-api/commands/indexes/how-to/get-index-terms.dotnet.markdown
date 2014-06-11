# Client API : Indexes : How to get index terms?

**GetTerms** will retrieve all stored terms for a field of an index.

## Syntax

{CODE get_terms_1@ClientApi\Commands\Indexes\HowTo\GetTerms.cs /}

**Parameters**   

- index - name of an index   
- field - index field   
- fromValue - starting point for a query, used for paging    
- pageSize - maximum number of terms that will be returned      

**Return Value**

This methods returns an array of index terms as a result.

## Example

{CODE get_terms_2@ClientApi\Commands\Indexes\HowTo\GetTerms.cs /}

#### Related articles

- [How to **reset index**?](../../../../client-api/commands/indexes/how-to/reset-index)   
- [How to **get index merge suggestions**?](../../../../client-api/commands/indexes/how-to/get-index-merge-suggestions)   