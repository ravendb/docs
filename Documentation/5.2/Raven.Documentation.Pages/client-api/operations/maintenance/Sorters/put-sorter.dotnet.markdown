# Put Custom Sorter Operation

 ---

{NOTE: }

* The Lucene indexing engine allows you to create your own __Custom Sorters__   
  where you can define how query results will be ordered based on your specific requirements.

* Use `PutSortersOperation` to deploy a custom sorter to the RavenDB server.  
  Once deployed, you can use it to sort query results for all queries made on the __database__  
  that is scoped to your [Document Store](../../../../client-api/setting-up-default-database).  

* To deploy a custom sorter that will apply cluster-wide, to all databases, see [put server-wide sorter](../../../../todo..).

* A custom sorter can also be uploaded to the server from the [Studio](../../../../todo....).

* In this page:
    * [Put custom sorter](../../../../client-api/operations/maintenance/sorters/put-sorter#put-custom-sorter)
    * [Syntax](../../../../client-api/operations/maintenance/sorters/put-sorter#syntax)

{NOTE/}

---

{PANEL: Put custom sorter}

* First, create your own sorter class that inherits from the Lucene class [Lucene.Net.Search.FieldComparator](https://lucenenet.apache.org/docs/3.0.3/df/d91/class_lucene_1_1_net_1_1_search_1_1_field_comparator.html).

* Then, send the custom sorter to the server using the `PutSortersOperation`.

{CODE-TABS}
{CODE-TAB:csharp:Sync put_sorter@ClientApi\Operations\Maintenance\Sorters\PutSorter.cs /}
{CODE-TAB:csharp:Async put_sorter_async@ClientApi\Operations\Maintenance\Sorters\PutSorter.cs /}
{CODE-TABS/}

{NOTE: }

You can now order your query results using the custom sorter.  
A query example is available [here](../../../../client-api/session/querying/sort-query-results#custom-sorters).

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp syntax_1@ClientApi\Operations\Maintenance\Sorters\PutSorter.cs /}

| Parameter         | Type                 | Description                                          |
|-------------------|----------------------|------------------------------------------------------|
| __sortersToAdd__  | `SorterDefinition[]` | One or more Sorter Definitions to send to the server |


{CODE syntax_2@ClientApi\Operations\Maintenance\Sorters\PutSorter.cs /}

{PANEL/}

## Related Articles

### Client API

- [Sort query results](../../../../client-api/session/querying/sort-query-results)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [Put server-wide sorter](../../../../todo...)
