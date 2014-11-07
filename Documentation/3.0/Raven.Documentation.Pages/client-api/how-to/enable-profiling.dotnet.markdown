#How to enable profiling?

The document store is able to automatically gather profiling information by tracking the requests sent within a session. By default the profiling is disabled. In order to
activate this feature you need to call:

{CODE initialize_profiling@ClientApi\HowTo\EnableProfiling.cs /}

If the profiling has been initialized then you can control whether info should be grabbed by changing the value of `DisableProfiling` [convention](../configuration/conventions/misc#disableprofiling).

Another profiling method used to retrieve the profiling info associated with a particular session is:

{CODE get_profiling_info@ClientApi\HowTo\EnableProfiling.cs /}

where the `id` is the id of the session you want to profile. In result you will get `ProfilingInformation` object which has the following fields:

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | Guid | The relavant session id |
| **Requests** | List&lt;RequestResultArgs&gt; | The requests made by this session (url, method, status, duration etc.) |
| **At** | DateTime | The time when the session was created |
| **DurationMilliseconds** | double | The duration of a session |
| **Context** | IDictionary<string, string> | Additional information added by extension |

##Example

{CODE example@ClientApi\HowTo\EnableProfiling.cs /}