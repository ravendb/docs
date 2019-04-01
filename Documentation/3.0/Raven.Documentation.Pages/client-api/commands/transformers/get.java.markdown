# Commands: Transformers: Get

There are few methods that allow you to retrieve transformers from a database:   
- [GetTransformer](../../../client-api/commands/transformers/get#gettransformer)   
- [GetTransformers](../../../client-api/commands/transformers/get#gettransformers)   

{PANEL:GetTransformer}

**GetTransformer** is used to retrieve a single transformer

### Syntax

{CODE:java get_1_0@ClientApi\Commands\Transformers\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | transformer name | 

| Return Value | |
| ------------- | ----- |
| [TransformerDefinition](../../../glossary/transformer-definition) | Instance of TransformerDefinition representing transformer. |

### Example

{CODE:java get_1_1@ClientApi\Commands\Transformers\Get.java /}

{PANEL/}

{PANEL:GetTransformers}

**GetTransformers** is used to retrieve a multiple transformers

### Syntax

{CODE:java get_2_0@ClientApi\Commands\Transformers\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | number of documents that should be skipped |
| **pageSize** | int | maximum number of transformers that will be retrieved |

| Return Value | |
| ------------- | ----- |
| [TransformerDefinition](../../../glossary/transformer-definition) | Instance of TransformerDefinition representing a transformer. |

### Example

{CODE:java get_2_1@ClientApi\Commands\Transformers\Get.java /}  

{PANEL/}

## Related articles

- [PutTransformer](../../../client-api/commands/transformers/put)  
- [DeleteTransformer](../../../client-api/commands/transformers/delete)  
