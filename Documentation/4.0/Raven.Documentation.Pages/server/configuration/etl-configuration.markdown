# Configuration : ETL Options

{PANEL:ETL.SQL.CommandTimeoutInSec}

Number of seconds after which SQL command will timeout.

- **Type**: `int`
- **Default**: `null` (use provider default)
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:ETL.ExtractAndTransformTimeoutInSec}

Number of seconds after which extraction and transformation will end and loading will start.

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:ETL.MaxNumberOfExtractedDocuments}

Max number of extracted documents in ETL batch.

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide or per database

If value is not set, or set to null - number of extracted documents is infinite per ETL batch 

{PANEL/}
