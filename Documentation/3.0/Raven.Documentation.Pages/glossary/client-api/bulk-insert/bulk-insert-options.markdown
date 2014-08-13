# Glossary : BulkInsertOptions

{CODE bulk_insert_options@Glossary\Glossary.cs /}

OverwriteExisting
:   Type: bool   
Indicates if existing documents should be overwritten. If not, exception will be thrown. Default: `false`.

CheckReferencesInIndexes
:   Type: bool   
Enables reference checking. Default: `false`.

BatchSize
:   Type: bool   
Used batch size. Default: `512`.

WriteTimeoutMilliseconds
:   Type: int   
Maximum 'quiet period' in milliseconds. If there will be no writes during that period operation will end with `TimeoutException`. Default: `15000`.
