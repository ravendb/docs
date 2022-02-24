# Querying: Plain Filtering
---

{NOTE: }

* **Plain Filtering** is an additional layer of filtering that can be applied 
  to Collection or Indexed query results.  

* To perform plain filtering use the `filter` Javascript method.  

* Where filtering query results using the `where` command uses a Lucene index or, 
  in the absence of one, creates the missing index and runs it, plain filtering 
  does not create or use an index but filters the retrieved results dataset.  
  Hence, applying plain filtering to a collection query would cause a full 
  scan of the given collection.  

* In this page:  
   * [filter](../../indexes/querying/external-filtering#filter)  
   * [`filter` properties]()  

{NOTE/}

---

{PANEL: `filter`}

`filter` can be placed after a collection query, forcing a full scan of the retrieved collection.  
{CODE-TABS}
{CODE-TAB:csharp:Query plain-filter_1.1@Indexes\Querying\PlainFiltering.cs /}
{CODE-TAB:csharp:DocumentQuery plain-filter_1.2@Indexes\Querying\PlainFiltering.cs /}
{CODE-TAB:csharp:RawQuery plain-filter_1.3@Indexes\Querying\PlainFiltering.cs /}
{CODE-TABS/}
  
`filter` can also be placed after a `where` clause, to filter the reults retrieved by an index query.  
{CODE-TABS}
{CODE-TAB:csharp:Query plain-filter_2.1@Indexes\Querying\PlainFiltering.cs /}
{CODE-TAB:csharp:DocumentQuery plain-filter_2.2@Indexes\Querying\PlainFiltering.cs /}
{CODE-TAB:csharp:RawQuery plain-filter_2.3@Indexes\Querying\PlainFiltering.cs /}
{CODE-TABS/}

The results returned from plain filtering can be projected using `select`.  
{CODE-BLOCK:csharp}
from Employees as e
where e.ReportsTo != null
filter e.FirstName == 'Anne' or e.Extension == 5467
select { FullName: e.FirstName +  " " + e.LastName }
{CODE-BLOCK/}

{PANEL/}

## Where - Numeric Property

## Where - Nested Property

## Where + Any

## Where + In

## Where + ContainsAny

## Where + ContainsAll

## Where - StartsWith

## Where - EndsWith

## Where - Identifier Property

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Basics](../../indexes/querying/basics)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
