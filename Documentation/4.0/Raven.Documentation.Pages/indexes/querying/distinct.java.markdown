# Querying: Distinct

The `distinct` method allows you to remove duplicates from the result. Items are compared based on the fields listed in the `select` section of the query. 

{CODE-TABS}
{CODE-TAB:java:Java distinct_1_1@Indexes\Querying\Distinct.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
select distinct ShipTo.Country 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Paging 

Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results). This kind of paging is required when using a distinct keyword.

## Count

RavenDB supports returning counts when the distinct operation is used.

{CODE:java distinct_2_1@Indexes\Querying\Distinct.java /}

{INFO:Performance}

Please keep in mind that this operation might not be efficient for large sets of data due to the need to scan all of the index results in order to find all the unique values.

The same result might be achieved by creating a [Map-Reduce](../../indexes/map-reduce-indexes) index that aggregates data by the field where you want a distinct value of. e.g.

{CODE:java distinct_3_1@Indexes\Querying\Distinct.java /}

{CODE:java distinct_3_2@Indexes\Querying\Distinct.java /}

{INFO/}

## Related Articles

### Querying

- [Paging](../../indexes/querying/paging)
