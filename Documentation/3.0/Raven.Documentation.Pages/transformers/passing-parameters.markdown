# Passing parameters

Parameters can be passed to alter transformations by:

- using `AddTransformerParameter` in session [Query](../client-api/session/querying/how-to-use-transformers-in-queries)
- using `SetTransformerParameters` in session [DocumentQuery](../client-api/session/querying/lucene/how-to-use-lucene-in-queries)
- filling `TransformerParameters` in `IndexQuery` that can be used in commands [Query](../client-api/commands/transformers/how-to/transform-query-results)
- passing parameters directly in various [Get](../client-api/commands/documents/get) methods from commands

To access passed parameters from within a transformer use on of the two available methods: `Parameter` or `ParameterOrDefault`. 
The difference between those two methods is that `Parameter` will throw if the parameter is not supplied, and `ParameterOrDefault` will use the 
default value.

{CODE transformers_1@Transformers/Parameters.cs /}

{CODE transformers_2@Transformers/Parameters.cs /}

## Related articles

- [Basic transformations](../transformers/basic-transformations)
- [Loading documents](../transformers/loading-documents)
- [Including documents](../transformers/including-documents)
- [Nesting transformers](../transformers/nesting-transformers)
- [Transforming hierarchical data](../transformers/transforming-hierarchical-data)
