#Getting file systems stats

{PANEL: Names of existing file systems}

The **GET** method returns the names of all existing file systems in the server.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON Array** | The task result is the array containing file names |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs
< HTTP/1.1 200 OK

[
    "NewNorthwindFS",
    "NorthwindFS"
]
{CODE-BLOCK/}

{PANEL/}

{PANEL: Stats of all file systems}

The **GET** method returns statistics of currently loaded file systems.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/stats \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON Array** | The array containing stats of loaded file systems |


### Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8080/fs
< HTTP/1.1 200 OK

[
    {
        "Name":"NorthwindFS",
        "FileCount":3,
        "Metrics":{
            "FilesWritesPerSecond":0.0,
            "RequestsPerSecond":0.004,
            "Requests":{
                "Type":"Meter",
                "Count":1,
                "MeanRate":0.002,
                "OneMinuteRate":0.016,
                "FiveMinuteRate":0.003,
                "FifteenMinuteRate":0.001
            },
            "RequestsDuration":{
                "Type":"Historgram",
                "Counter":0,
                "Max":0.0,
                "Min":0.0,
                "Mean":0.0,
                "Stdev":0.0,
                "Percentiles":{
                    "50%":0.0,
                    "75%":0.0,
                    "95%":0.0,
                    "99%":0.0,
                    "99.9%":0.0,
                    "99.99%":0.0
                }
            }
        },
        "ActiveSyncs":[],
        "PendingSyncs":[]
    }
]
{CODE-BLOCK/}
{PANEL/}

{PANEL: Stats of particular file system}

The **GET** method returns statistics of a selected file system.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/stats \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON Array** | The file system stats |

{PANEL/}