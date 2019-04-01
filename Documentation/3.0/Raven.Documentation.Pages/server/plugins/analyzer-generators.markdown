# Plugins: Analyzer Generators

To add a custom analyzer, one must implement the `AbstractAnalyzerGenerator` class and provide logic when your custom analyzer should be used.

{CODE plugins_7_0@Server\Plugins.cs /}

where:   
* **GenerateAnalyzerForIndexing** returns an analyzer that will be used while performing indexing operation.   
* **GenerateAnalyzerForQuerying** returns an analyzer that will be used while performing querying.    

## Example - Using different analyzer for specific index

{CODE plugins_7_1@Server\Plugins.cs /}

## Related articles

- [Indexes : Analyzers](../../indexes/using-analyzers)
