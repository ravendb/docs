# Nesting transformers

When you have more complex transformations and you do not want to repeat code in each transformer to do the exact same transformation, you can use transformer nesting feature by invoking `TransfromWith` in your projection function.

{CODE transformers_1@Transformers/Nesting.cs /}

{CODE transformers_2@Transformers/Nesting.cs /}

## Related articles

- [Basic transformations](../transformers/basic-transformations)
- [Loading documents](../transformers/loading-documents)
- [Including documents](../transformers/including-documents)
- [Passing parameters](../transformers/passing-parameters)
- [Transforming hierarchical data](../transformers/transforming-hierarchical-data)
