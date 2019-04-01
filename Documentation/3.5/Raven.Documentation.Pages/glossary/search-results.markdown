# Glossary: SearchResults

RavenFS client API class representing search results.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Files** | List&lt;FileHeader&gt; | The list of matching files represented by [FileHeader](file-header) objects |
| **FileCount** | int | The total number of files matching search criteria |
| **Start** | int | The `start` parameter passed to a search method |
| **PageSize** | int | The `pageSize` parameter passed to a search method |
