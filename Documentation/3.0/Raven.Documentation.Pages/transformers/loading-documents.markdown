# Loading documents

Sometimes, in our result set, we want to put data from referenced documents. To do so, the ability to load external documents was introduced. To use it just invoke `LoadDocument` method with appropriate parameters. This method contains various overloads that can be used for loading single document by its Id or multiple documents from an array containing Ids:

{CODE transformers_1@Transformers/Loading.cs /}

{CODE transformers_2@Transformers/Loading.cs /}

## Related articles

- [Basic transformations](../transformers/basic-transformations)
- [Including documents](../transformers/including-documents)
- [Passing parameters](../transformers/passing-parameters)
