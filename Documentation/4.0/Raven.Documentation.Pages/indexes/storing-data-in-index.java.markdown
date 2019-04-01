# Indexes: Storing Data in Index

Once the [tokenization and analysis](../indexes/using-analyzers) process is completed, the resulting tokens, created according to the used analyzer, are stored in the index.
By default, tokens saved in the index are available for searching. but their original field values aren't stored.

Lucene allows you to store the original field text (before it is analyzed) as well. This feature is exposed in the index definition object as the `Storage` property of the `IndexFieldOptions`.

Enabling field storing causes original values will be available for retrieval when doing [projections](../indexes/querying/projections).

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask storing_1@Indexes\Storing.java /}
{CODE-TAB:java:Operation storing_2@Indexes\Storing.java /}
{CODE-TABS/}

## Remarks

{INFO Default `Storage` value for each field is `FieldStorage.NO`. Keep in mind that storing fields will increase disk space usage. /}

{INFO:Info}
If **the projection requires only the fields that are stored**, then the document will not be loaded from the storage and the query results will come directly from the index. This can increase query performance at the cost of disk space used.
{INFO/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Querying

- [Projections and Stored Fields](../indexes/querying/projections#projections-and-stored-fields)
