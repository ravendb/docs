# Glossary : BulkInsertOptions

{CODE:java bulk_insert_options@Glossary\Glossary.java /}

OverwriteExisting
:   Type: bool   
Indicates if existing documents should be overwritten. If not, exception will be thrown. Default: `false`.

SkipOverwriteIfUnchanged
:   Type: bool   
Determines whether should skip to overwrite a document when it is updated by exactly the same document (by comparing a content and as well as metadata). Default: `false`.

CheckReferencesInIndexes
:   Type: bool   
Enables reference checking. Default: `false`.

BatchSize
:   Type: bool   
Used batch size. Default: `512`.

WriteTimeoutMilliseconds
:   Type: int   
Maximum 'quiet period' in milliseconds. If there will be no writes during that period operation will end with `TimeoutException`. Default: `15000`.
