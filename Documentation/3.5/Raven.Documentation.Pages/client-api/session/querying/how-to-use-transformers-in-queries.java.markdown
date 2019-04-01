# Session: Querying: How to use transformers in queries?

Transformers can be used with session queries using `transformWith` method.

## Syntax

{CODE:java transformers_1@ClientApi\Session\Querying\HowToUseTransformers.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **transformerName** | String | Name of a transformer to use. |

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods and extensions. |

## Example I

{CODE:java transformers_2@ClientApi\Session\Querying\HowToUseTransformers.java /}

## Example II

Loading document inside transformer and projecting results to different type.

{CODE:java transformers_5@ClientApi\Session\Querying\HowToUseTransformers.java /}

{CODE:java transformers_4@ClientApi\Session\Querying\HowToUseTransformers.java /}

{CODE:java transformers_3@ClientApi\Session\Querying\HowToUseTransformers.java /}

## Related articles

- [What are transformers?](../../../transformers/what-are-transformers)   
