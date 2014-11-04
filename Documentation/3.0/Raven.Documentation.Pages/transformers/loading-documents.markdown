# Loading documents

Sometimes, in our results set, we want to include data from referenced documents. To do so, the ability to load external documents was introduced. To use it just invoke the `LoadDocument` method with appropriate parameters. This method contains various overloads that can be used to load a single document by its Id or to load multiple documents from an array containing Ids:

{CODE transformers_1@Transformers/Loading.cs /}

{CODE transformers_2@Transformers/Loading.cs /}

## Related articles

- [Basic transformations](../transformers/basic-transformations)
- [Including documents](../transformers/including-documents)
- [Passing parameters](../transformers/passing-parameters)
