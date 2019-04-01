# Glossary: BulkOperationOptions

Holds different setting options for base operations.

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **AllowStale** | bool | Indicates whether operations are allowed on stale indexes |
| **StaleTimeout** | TimeSpan? | How long should the operation wait for an index to become non stale |
| **MaxOpsPerSec** | int? | Limits the amount of base operation per second allowed |
| **RetrieveDetails** | bool | Determines whether operation details about each document should be returned by server |
