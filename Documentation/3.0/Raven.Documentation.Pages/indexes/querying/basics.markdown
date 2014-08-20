# Basics

TODO
You probably read quite a few times by now, that indexes are used by RavenDB to satisfy queries, but what does that mean?

## Query-flow

1. First step, when query is issued, is to locate appropriate index. If our query specifies that index, the task is simple - use this index. Otherwise the query analysis is taking place and auto-index is created (if it does not exist already).
2. When we have our index, we scan it for records that match query predicate.
3. From each record server extracts appropriate fields. It always extracts `__document_id` field ([stored]() by default). Additionaly when [projection]() is taking place, then fields defined in projection are extracted from index (if stored).
4. Next, if query is not a projection query, then we download a document from storage. Otherwise, if we stored all requested fields in index, then we use them and continue, if not, then document is loaded from storage and missing fields are fetched from it.
5. In next step, our potential query results must pass [read triggers](). 
6. _(Optional)_ If query indicates that [transformer]() should be used, then all results that were not filtered out are processed by its projection function.
7. Return results.
