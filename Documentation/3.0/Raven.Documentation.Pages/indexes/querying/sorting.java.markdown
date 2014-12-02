# Sorting

{NOTE This article focuses only on querying side of the sorting. If you are interested in reading how to create indexes and change default sorting behavior go [here](../../indexes/customizing-results-order). /}

## Basics

By default, all index values are sorted lexicographically, this can be changed in index definition, but sorting is **not applied** until you request it by using appropriate methods, so following queries will not return sorted results, even if we define in our index appropriate sorting option:

{CODE-TABS}
{CODE-TAB:java:Query sorting_1_1@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:DocumentQuery sorting_1_2@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Commands sorting_1_3@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Index sorting_1_4@Indexes\Querying\Sorting.java /}
{CODE-TABS/}

So, the start sorting, we need to request to order by some specified index field. In our case we will order by `UnitsInStock` in descending order:

{CODE-TABS}
{CODE-TAB:java:Query sorting_2_1@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:DocumentQuery sorting_2_2@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Commands sorting_2_3@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Index sorting_1_4@Indexes\Querying\Sorting.java /}
{CODE-TABS/}

{INFO:Convention}
You probably noticed that we used `-` in a name of a field passed to `SortedField` that was used in **commands**, which means that we want to sort our results in a descending order. Using `+` symbol or no prefix means that ascending sorting is requested. 

Of course you can change order and field name in `SortedField` later since all properties have public access.
{INFO/}

## Ordering by score

When query is issued, each index entry is scored by Lucene (you can read more about Lucene scoring [here](http://lucene.apache.org/core/3_3_0/scoring.html)) and this value is available in metadata information of a document under `Temp-Index-Score` (the higher the value, the better the match). To order by this value you can use `orderByScore` or `orderByScoreDescending` methods:

{CODE-TABS}
{CODE-TAB:java:Query sorting_4_1@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:DocumentQuery sorting_4_2@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Commands sorting_4_3@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Index sorting_1_4@Indexes\Querying\Sorting.java /}
{CODE-TABS/}

## Random ordering

If you want to randomize the order of your results each time the query is executed you can use `randomOrdering` method (API reference [here](../../client-api/session/querying/how-to-customize-query#randomordering)):

{CODE-TABS}
{CODE-TAB:java:Query sorting_3_1@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:DocumentQuery sorting_3_2@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Commands sorting_3_3@Indexes\Querying\Sorting.java /}
{CODE-TAB:java:Index sorting_1_4@Indexes\Querying\Sorting.java /}
{CODE-TABS/}

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
- [Querying : Filtering](../../indexes/querying/filtering)
- [Querying : Paging](../../indexes/querying/paging)
