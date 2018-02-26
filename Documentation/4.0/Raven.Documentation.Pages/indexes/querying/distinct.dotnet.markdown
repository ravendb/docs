# Querying : Distinct

The `Distinct` method allows you to remove duplicates from the result. Items are compared based on fields listed in the `select` section of the query. 

{CODE-TABS}
{CODE-TAB:csharp:Query distinct_1_0@Indexes\Querying\Distinct.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
select distinct ShipTo.Country 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Paging 

Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results). This kind of paging is required when using a distinct keyword.

## Count

{WARNING:Performance}
RavenDB supports returning counts when distinct operation is used, but please keep in mind that this operation might not be efficient for large sets of data due to the need to scan all of the index results in order to find all the unique values.
{WARNING/}

{CODE-TABS}
{CODE-TAB:csharp:Query distinct_1_1@Indexes\Querying\Distinct.cs /}
{CODE-TABS/}

## Related Articles

- [Indexing : Querying : Paging](../../indexes/querying/paging)
