# Session: Querying: How to use transformers in queries?

Transformers can be used with session queries using `TransformWith` method.

## Syntax

{CODE transformers_1@ClientApi\Session\Querying\HowToUseTransformers.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **transformerName** | string | Name of a transformer to use. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods and extensions. |

## Example I

{CODE transformers_2@ClientApi\Session\Querying\HowToUseTransformers.cs /}

## Example II

Loading document inside transformer and projecting results to different type.

{CODE transformers_5@ClientApi\Session\Querying\HowToUseTransformers.cs /}

{CODE transformers_4@ClientApi\Session\Querying\HowToUseTransformers.cs /}

{CODE transformers_3@ClientApi\Session\Querying\HowToUseTransformers.cs /}

## Related articles

- [What are transformers?](../../../transformers/what-are-transformers)   
