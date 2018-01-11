# Distinct

`Distinct` method allows user to remove duplicates from result. Items are compared based on fields listed in `select` section of the query. 


{CODE-TABS}
{CODE-TAB:csharp:Query distinct_1_0@Indexes\Querying\Distinct.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Orders 
order by ShipTo.Country 
select distinct ShipTo.Country 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Paging 

Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results). This kind of paging is required when using distinct keyword.



## Related Articles

- [Indexing : Querying : Paging](../../indexes/querying/paging)
