# Glossary : BulkInsertOptions

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **OverwriteExisting** | bool | Indicates if existing documents should be overwritten. If not, exception will be thrown. Default: `false`. |
| **CheckReferencesInIndexes** | bool | Enables reference checking. Default: `false`. |
| **SkipOverwriteIfUnchanged** | bool | Determines whether should skip to overwrite a document when it is updated by exactly the same document (by comparing a content and as well as metadata). Default: `false`. |
| **BatchSize** | int | Used batch size. Default: `512`. |
| **WriteTimeoutMilliseconds** | int |Maximum 'quiet period' in milliseconds. If there will be no writes during that period operation will end with `TimeoutException`. Default: `15000`.  |
| **Format** | BulkInsertFormat | Specify which type of format would be send to the bulk insert request. Default: `BulkInsertFormat.Bson`. |
| **Compression** | BulkInsertCompression | Specify which compression format would be use (can also be disabled). Default: `BulkInsertCompression.GZip`. |