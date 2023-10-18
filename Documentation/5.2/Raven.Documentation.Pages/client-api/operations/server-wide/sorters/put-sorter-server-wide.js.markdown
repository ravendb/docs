# Put Custom Sorter (Server-Wide) Operation  

 ---

{NOTE: }

* The Lucene indexing engine allows you to create your own __Custom Sorters__   
  where you can define how query results will be ordered based on your specific requirements.

* Use `PutServerWideSortersOperation` to deploy a custom sorter to the RavenDB server.  
  Once deployed, you can use it to sort query results for all queries made on __all databases__ in your cluster.  

* To deploy a custom sorter that will apply only to the database scoped to your [Document Store](../../../../client-api/setting-up-default-database),  
  see [put custom sorter](../../../../client-api/operations/maintenance/sorters/put-sorter).

* A custom sorter can also be uploaded server-wide from the [Studio](../../../../studio/database/settings/custom-sorters).

* In this page:
    * [Put custom sorter server-wide](../../../../client-api/operations/server-wide/sorters/put-sorter-server-wide#put-custom-sorter-server-wide)
    * [Syntax](../../../../client-api/operations/server-wide/sorters/put-sorter-server-wide#syntax)

{NOTE/}

---

{PANEL: Put custom sorter server-wide}

* First, create your own sorter class that inherits from the Lucene class [Lucene.Net.Search.FieldComparator](https://lucenenet.apache.org/docs/3.0.3/df/d91/class_lucene_1_1_net_1_1_search_1_1_field_comparator.html).

* Then, send the custom sorter to the server using the `PutServerWideSortersOperation`.

{CODE:nodejs put_sorter@ClientApi\Operations\Server\Sorters\putSorterServerWide.js /}

{NOTE: }

You can now order your query results using the custom sorter.  
A query example is available [here](../../../../client-api/session/querying/sort-query-results#custom-sorters).

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Server\Sorters\putSorterServerWide.js /}

| Parameter         | Type          | Description                                          |
|-------------------|---------------|------------------------------------------------------|
| __sortersToAdd__  | `...object[]` | One or more Sorter Definitions to send to the server |


{CODE:nodejs syntax_2@ClientApi\Operations\Server\Sorters\putSorterServerWide.js /}

{PANEL/}

## Related Articles

### Client API

- [Sort query results](../../../../client-api/session/querying/sort-query-results)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [Put custom sorter](../../../../client-api/operations/maintenance/sorters/put-sorter)

### Studio

- [Custom sorters view](../../../../studio/database/settings/custom-sorters)
