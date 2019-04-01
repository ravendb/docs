# Query: Query View

This view allows running queries for already existing indexes (click [here](../../../indexes/creating-and-deploying#static-indexes) to read about static indexes) and running dynamic queries, which can create a new dynamic index (click [here](../../../indexes/creating-and-deploying#auto-indexes) to read more about dynamic indexes)

## Action Bar

Action Bar consists of the following:

- `Execute` - runs the queries,
- `Add` - adds a transformer or sort option to the query,
- `Settings` - converts implicit operators, toggle field showing or index entries,
- `Edit` - takes you directly to [Index Edit View](../../../studio/overview/indexes/index-edit-view) where you can edit the index,
- `Choose columns` - allows you to choose columns which will be displayed in `Results`,
- `Export CSV` - exports results to CSV file,
- `Indexing performance statistics` - takes you to [Indexing Performance Chart](../../../studio/overview/status/indexing/indexing-performance) to view statistics,
- `Recent Queries` - contains a list of recently ran queries,
- `Terms` - navigates to index terms view,
- `Query Stats` - opens a window with statistics for the current query,
- `Delete` - deletes documents that match the query (only Map index)

![Figure 1. Studio. Query View.](images/query_view-1.png)

<hr />

## Querying Static Index

The first step while running queries is choosing an appropriate index from the list of available indexes.

![Figure 2. Studio. Querying Static Index.](images/query_view-quering_static_index-2.png)

Then, you will need to enter a query that uses Lucene-flavored syntax (more [here](../../../indexes/querying/full-query-syntax)) that matches appropriate fields defined in selected index,

![Figure 3. Studio. Querying Static Index.](images/query_view-quering_static_index-3.png)

After running the query using `Execute` from `Action Bar`, query results will be viewed in `Results` section.

![Figure 4. Studio. Querying Static Index.](images/query_view-quering_static_index-4.png)

<hr />

## Dynamic queries

To run a dynamic query, you need to choose a suitable collection from the list first (e.g. dynamic/Products).

![Figure 5. Studio. Dynamic queries.](images/query_view-dynamic_queries-5.png)

Then, as in the case of static querying, you will need to enter an appropriate query.

![Figure 6. Studio. Dynamic queries.](images/query_view-dynamic_queries-6.png)

Query results, as in the case of static querying, will be displayed in `Results` section.

![Figure 7. Studio. Dynamic queries.](images/query_view-dynamic_queries-7.png)

{WARNING Remember that running dynamic query may create a dynamic index, which can always be viewed using the [Indexes View](../../../studio/overview/indexes/indexes-view). /}

![Figure 8. Studio. Dynamic queries.](images/query_view-dynamic_queries-8.png)
