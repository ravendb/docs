# Storing

After the tokenization and analysis process is complete, the resulting tokens are stored in an index, which is now ready to be search with. As we have seen before, only fields in the final index projection could be used for searched, and the actual tokens stored for each depends on how the selected Analyzer processed the original text.

Lucene allows storing the original token text for fields, and RavenDB exposes this feature in the index definition object via `Stores`.

By default, tokens are saved to the index as Indexed and Analyzed but not Stored - that is: they can be searched on using a specific Analyzer (or the default one), but their original value is unavailable after indexing. Enabling field storage causes values to be available to be retrieved via `IDocumentQuery<T>.SelectFields<TProjection>(...)`, and is done like so:

{CODE stores1@ClientApi\Querying\StaticIndexes\ConfiguringIndexOptions.cs /}

The default values for each field are `FieldStorage.No` in Stores and `FieldIndexing.Default` in Indexes.

Setting `FieldIndexing.No` causes values to not be available in where clauses when querying (similarly to not being present in the original projection). `FieldIndexing.NotAnalyzed` causes whole properties to be treated as a single token and matches must be exact, similarly to using a KeywordAnalyzer on this field. The latter is useful for product Ids, for example. `FieldIndexing.Analyzed` allows to perform full text search operations against the field. `FieldIndexing.Default` will index the field as a single term, in lower case.
