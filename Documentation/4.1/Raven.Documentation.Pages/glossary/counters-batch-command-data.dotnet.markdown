# Glossary: CountersBatchCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | string | Document id |
| **Counters** | [DocumentCountersOperation](../client-api/operations/counters/counter-batch#documentcountersoperation) | Counter operations to perform |
| **ChangeVector** | string | The change-vector of the last-updated counter |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to a Json. |

## Related articles

- [Counter Batch Operation](../client-api/operations/counters/counter-batch)
