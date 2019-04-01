# Commands: How to get server build number?

To check with what version of server commands you are working use `GetBuildNumber` command from `GlobalAdmin`.

## Syntax

{CODE build_number_1@ClientApi\Commands\HowTo\BuildNumber.cs /}

<hr />

{CODE buildnumber@Common.cs /}

| Return Value | BuildNumber | Information about current product (server) version and build number. |
| ------------- | ------------- | ----- |
| **ProductVersion** | string | String representing current product version e.g. `"3.0.0 / 6dce79a"` |
| **BuildVersion** | string | String indicating current build version e.g. `"3260"` |

## Example

{CODE build_number_2@ClientApi\Commands\HowTo\BuildNumber.cs /}

## Remarks

`BuildVersion` for custom builds is `"13"`
