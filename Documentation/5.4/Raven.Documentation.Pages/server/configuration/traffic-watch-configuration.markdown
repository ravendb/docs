# Configuration: Traffic Watch Options

{PANEL: TrafficWatch.Mode}

Traffic Watch logging mode.

- **Type**: `TrafficWatchMode`  
- **Default**: `Off`   
- **Scope**: Server-wide only  

Possible values:

- `Off`
- `ToLogFile`

{PANEL/}

{PANEL: TrafficWatch.Databases}

A semicolon-separated list of database names by which the Traffic Watch logging entities will be filtered.  
If not specified, Traffic Watch entities of all databases will be included.  
A sample list: `\"test-database;another-database;the-third-database\"`  

- **Type**: `List<string>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.StatusCodes}

A semicolon-separated list of response status codes by which the Traffic Watch logging entities will be filtered.  
If not specified, Traffic Watch entities with any response status code will be included.  
A sample list: `\"200;500;404\"`  

- **Type**: `List<int>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.MinimumResponseSizeInBytes}

Minimum response size by which the Traffic Watch logging entities will be filtered.

- **Type**: `int`
- **Default**: `0`
- **Minimum**: `0`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.MinimumRequestSizeInBytes}

Minimum request size by which the Traffic Watch logging entities will be filtered.

- **Type**: `int`
- **Default**: `0`
- **Minimum**: `0`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.MinimumDurationInMs}

Minimum duration by which the Traffic Watch logging entities will be filtered.

- **Type**: `int`
- **Default**: `0`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.HttpMethods}

A semicolon-separated list of request HTTP methods by which the Traffic Watch logging entities will be filtered.  
If not specified, Traffic Watch entities with any HTTP request method will be included.  
A sample list: `\"GET;POST\"`  

- **Type**: `List<string>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.ChangeTypes}

A semicolon-separated list of Traffic Watch change types by which the Traffic Watch logging entities will be filtered.  
If not specified, Traffic Watch entities with any change type will be included.  
A sample list: `\"Queries;Documents\"`

- **Type**: `List<TrafficWatchChangeType>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: TrafficWatch.CertificateThumbprints}

A semicolon-separated list of specific client certificate thumbprints by which the Traffic Watch logging entities will be filtered.  
If not specified, Traffic Watch entities with any certificate thumbprint will be included,
including those without any thumbprint.
A sample list: `\"0123456789ABCDEF0123456789ABCDEF01234567;FEDCBA9876543210FEDCBA9876543210FEDCBA98\"`

- **Type**: `List<string>`
- **Default**: `null`
- **Scope**: Server-wide only

Possible values:

- `None`
- `Queries`
- `Operations`
- `MultiGet`
- `BulkDocs`
- `Index`
- `Counters`
- `Hilo`
- `Subscriptions`
- `Streams`
- `Documents`
- `TimeSeries`
- `Notifications`
- `ClusterCommands`

{PANEL/}
