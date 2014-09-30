# Basics

Following article will introduce you into the querying. You probably read quite a few times by now, that indexes are used by RavenDB to satisfy queries, but what does that mean?

## Query-flow

Each query in RavenDB must be done against index and the query-flow is as follows:

1. First step, when query is issued, is to locate appropriate index. If our query specifies that index, the task is simple - use this index. Otherwise the query analysis is taking place and auto-index is created (if it does not exist already).
2. When we have our index, we scan it for records that match query predicate.
3. From each record server extracts appropriate fields. It always extracts `__document_id` field ([stored]() by default). Additionaly when [projection]() is taking place, then fields defined in projection are extracted from index (if stored).
4. Next, if query is not a projection query, then we download a document from storage. Otherwise, if we stored all requested fields in index, then we use them and continue, if not, then document is loaded from storage and missing fields are fetched from it.
5. In next step, our potential query results must pass [read triggers](). 
6. _(Optional)_ If query indicates that [transformer]() should be used, then all results that were not filtered out are processed by its projection function.
7. Return results.

## Querying using LINQ

RavenDB Client supports querying using LINQ, this functionality can be accessed using session `Query` method and is the most common and basic method for querying the database.

### Example I

Let's execute our first query and return all employees from Northwind database. To do that, we need to have a [document store]() and [opened session]() and specify a [collection]() type that we want to query (in our case `Employees`) by passing `Employee` as a generic parameter to `Query` method:

{CODE basics_0_0@Indexes\Querying\Basics.cs /}

Notice that by specifying `Employee` as a type parameter, we are also defining a result type.

{SAFE By default, if page size is not specified, the value will be set to `128`. /}

### Example II - filtering

To filter out results use suitable LINQ method e.g. `Where`:

{CODE basics_0_1@Indexes\Querying\Basics.cs /}

You can read more about filtering [here]().

### Example III - paging

Paging is very simple, methods `Take` and `Skip` can be used:

{CODE basics_0_2@Indexes\Querying\Basics.cs /}

You can read more about paging [here]().

### Example IV - querying specified index

In above examples we **did not** specify an index that we want to query, in that case RavenDB will try to locate an appropriate index or create a new one. You can read more about creating indexes [here]().

In order to specify a index, we need to pass it as a second generic parameter to `Query` method or pass index name as a parameter.

{CODE basics_0_3@Indexes\Querying\Basics.cs /}

{CODE basics_0_4@Indexes\Querying\Basics.cs /}

{INFO:Remember}
If you are filtering by fields that are not present in index, exception will be thrown.
{INFO/}

## Low-level query access

To take a full control over your queries, we introduced a `DocumentQuery` method that is available in advanced session operations. Basically it is a low-level access to the querying mechanism that user can take advantage of to shape queries according to needs.

### Example

{CODE basics_1_0@Indexes\Querying\Basics.cs /}

### Remarks

{INFO You can check the API reference for the `DocumentQuery` [here](). /}

{INFO There are some differences between `Query` and `DocumentQuery` and they are described in [this article](). /}

## Related articles

TODO
