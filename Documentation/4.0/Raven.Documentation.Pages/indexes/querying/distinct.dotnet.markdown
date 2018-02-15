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

## Related Articles

- [Indexing : Querying : Paging](../../indexes/querying/paging)
