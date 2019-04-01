# Commands: How to get server build number?

To check with what version of server commands you are working use `getBuildNumber` command from `GlobalAdmin`.

## Syntax

{CODE:java build_number_1@ClientApi\Commands\HowTo\BuildNumber.java /}

<hr />

{CODE:java buildnumber@Common.java /}

| Return Value | BuildNumber | Information about current product (server) version and build number. |
| ------------- | ------------- | ----- |
| **ProductVersion** | String | String representing current product version e.g. `"3.0.0 / 6dce79a"` |
| **BuildVersion** | String | String indicating current build version e.g. `"3260"` |

## Example

{CODE:java build_number_2@ClientApi\Commands\HowTo\BuildNumber.java /}

## Remarks

`BuildVersion` for custom builds is `"13"`
