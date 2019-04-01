# Operations: Server: How to Get Server Build Number

To get the server build number use **GetBuildNumberOperation** from `Maintenance.Server`

## Syntax

{CODE:csharp buildNumber_1@ClientApi\Operations\Server\GetBuildNumber.cs /}

### Return Value

The result of executing GetBuildNumberOperation is a **BuildNumber** object: 

{CODE:csharp buildNumber_2@ClientApi\Operations\Server\GetBuildNumber.cs /}

| ------------- |----- |
| **ProductVersion** | current product version e.g. "4.0" |
| **BuildVersion** | current build version e.g. 40 |
| **CommitHash** | git commit SHA e.g. ""a377982"" |
| **FullVersion** | semantic versioning e.g. "4.0.0" |

##Example

{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}
