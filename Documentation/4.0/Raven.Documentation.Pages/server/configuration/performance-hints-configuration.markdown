# Configuration: Performance Hints Options

{PANEL:PerformanceHints.Documents.HugeDocumentSizeInMb}

The size of a document after which it will get into the huge documents collection. Value is in MB.

- **Type**: `int`
- **Default**: `5`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:PerformanceHints.Documents.HugeDocumentsCollectionSize}

The maximum size of the huge documents collection.

- **Type**: `int`
- **Default**: `100`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:PerformanceHints.Indexing.MaxIndexOutputsPerDocument}

The maximum amount of index outputs per document after which we send a performance hint.

- **Type**: `int`
- **Default**: `1024`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:PerformanceHints.MaxNumberOfResults}

The maximum amount of results after which we will create a performance hint.

- **Type**: `int`
- **Default**: `2048`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:PerformanceHints.TooLongRequestThresholdInSec}

Request latency threshold before the server would issue a performance hint. Value is in seconds.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}


