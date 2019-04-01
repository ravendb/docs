# Glossary: IndexMergeResults

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Unmergables** | Dictionary&lt;string, string&gt; | Map of index name and reason |
| **Suggestions** | List&lt;[MergeSuggestions](../glossary/index-merge-results#mergesuggestions)&gt; | List of merge suggestions |

# MergeSuggestions

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **CanMerge** | List&lt;string&gt; | list of index names which can be merged |
| **Collection** | string | the collection that is being merged |
| **MergedIndex** | [IndexDefinition](../glossary/index-definition) | Proposition for new index with all it's properties |
