# Storing data in index

After the [tokenization and analysis](../indexes/using-analyzers) process is complete, the resulting tokens are stored in an index, which is now ready to be search with. Only fields in the final index projection could be used for searches, and the actual tokens stored for each depend on how the selected Analyzer processed the original text.

Lucene allows storing the original token text for fields, and RavenDB exposes this feature in the index definition object via `Stores`.

By default, tokens are saved to the index as _Indexed and Analyzed_ but not _Stored_ - that is: they can be searched on using a specific Analyzer (or the default one), but their original value is unavailable after indexing. Enabling field storage causes values to be available for retrieval via the [ProjectFromIndexFieldsInto](../client-api/session/querying/how-to-perform-projection#projectfromindexfieldsinto), [transformers](../transformers/what-are-transformers), or [other](../client-api/session/querying/how-to-perform-projection) projection functions. It is done like this:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask storing_1@Indexes\Storing.cs /}
{CODE-TAB:csharp:Commands storing_2@Indexes\Storing.cs /}
{CODE-TABS/}

## Remarks

{INFO Default value in `Stores` for each field is `FieldStorage.No`. Keep in mind that storing fields will increase disk space required by an index. /}

{INFO If the **projection function requires only the fields that are stored**, then the document will not be loaded from storage and all data will come directly from the index. This can increase query performance (at the cost of disk space used) in the frequent situations when the whole document is not needed. /}

## Related articles

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
