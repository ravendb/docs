# Client API : Transformers : Get

There are few methods that allow you to retrieve transformers from a database:   
- [GetTransformer](../../../client-api/commands/transformers/get#gettransformer)   
- [GetTransformers](../../../client-api/commands/transformers/get#gettransformers)   

## GetTransformer

**GetTransformer** is used to retrieve a single transformer

### Syntax

{CODE get_1_0@ClientApi\Commands\Transformers\Get.cs /}

**Parameters**   

- name - transformer name   

**Return Value**

{CODE transformerdefinition@Common.cs /}

- TransformResults - function that will convert (transform) results   
- TransformerId - internal transformer Id
- Name - transformer name   

### Example

{CODE get_1_1@ClientApi\Commands\Transformers\Get.cs /}

## GetTransformers

**GetTransformers** is used to retrieve a multiple transformers

### Syntax

{CODE get_2_0@ClientApi\Commands\Transformers\Get.cs /}

**Parameters**   

- start - number of documents that should be skipped   
- pageSize - maximum number of documents that will be retrieved   

**Return Value**

{CODE transformerdefinition@Common.cs /}

- TransformResults - function that will convert (transform) results   
- TransformerId - internal transformer Id
- Name - transformer name 

### Example

{CODE get_2_1@ClientApi\Commands\Transformers\Get.cs /}  

#### Related articles

- [PutTransformer](../../../client-api/commands/transformers/put)  
- [DeleteTransformer](../../../client-api/commands/transformers/delete)  