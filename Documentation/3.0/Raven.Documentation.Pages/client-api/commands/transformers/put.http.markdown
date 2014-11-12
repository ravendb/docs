# Commands : Transformers : Put

**PutTransformer** is used to add a transformer to a database.

## Syntax


{CODE-BLOCK:json}
  curl -X PUT http://{serverName}/databases/{databaseName}/transformers/{transformerName}
	-d @transformerDefiniton
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
|  [TransformerDefinition](../../../glossary/transformer-definition) |

| Query parameters | Required |  |
| ------------- | -- | ---- |
| **transformerName** | Yes | name of a transformer |

### Response

| Status code | |
| ----------- | - |
| `201` | Created |

| Return Value | |
| ------------- | ------------- |
| Transformer | Transformer **name**. |

## Example

{CODE-BLOCK:json}
curl -X PUT "http://localhost:8080/databases/NorthWind/transformers/Order/Statistics" 
-d "{\"TransformResults\":\"from order in results select new {     order.OrderedAt,     order.Status,     order.CustomerId,     CustomerName = LoadDocument(order.CustomerId).Name,     LinesCount = order.Lines.Count }\",\"IndexId\":0,\"Name\":\"Order/Statistics\"}"
< HTTP/1.1 201 Created
{"Transformer":"Order/Statistics"}
{CODE-BLOCK/}

## Related articles

- [GetTransformer](../../../client-api/commands/transformers/get)  
- [DeleteTransformer](../../../client-api/commands/transformers/delete)  