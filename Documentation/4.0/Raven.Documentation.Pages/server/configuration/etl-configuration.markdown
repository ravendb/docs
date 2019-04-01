# Configuration: ETL Options

{PANEL:ETL.SQL.CommandTimeoutInSec}

Number of seconds after which the SQL command will timeout.

- **Type**: `int`
- **Default**: `null` (use provider default)
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:ETL.ExtractAndTransformTimeoutInSec}

Number of seconds after which extraction and transformation will end and loading will start.

- **Type**: `int`
- **Default**: `60`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:ETL.MaxNumberOfExtractedDocuments}

Max number of extracted documents in an ETL batch.

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide or per database

If value is not set, or set to null, the number of extracted documents is infinite per ETL batch 

{PANEL/}

{PANEL:ETL.MaxFallbackTimeInSec}

Maximum number of seconds the ETL process will be in a fallback mode after a load connection failure to a destination. The fallback mode means suspending the process.

- **Type**: `int`
- **Default**: `900`
- **Scope**: Server-wide or per database

{PANEL/}
