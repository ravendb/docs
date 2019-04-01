# Session: Querying: How to perform dynamic aggregation?

Dynamic aggregation can be performed using `AggregateBy` method. Internally such aggregation is an extended [faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).

## Syntax

{CODE aggregate_1@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string or Expression<Func&lt;TResult&gt;> | Path (or expression from which path will be extracted) to field on which aggregation will be performed. |
| **displayName** | string | User defined friendly name for aggregation. If `null`, field name will be used. |

| Return Value | |
| ------------- | ----- |
| [DynamicAggregationQuery](../../../glossary/dynamic-aggregation-query)&lt;TResult&gt; | Query containing aggregation methods. |

## Example I - summing

{CODE aggregate_2@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Example II - counting

{CODE aggregate_3@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Example III - average

{CODE aggregate_4@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Example IV - maximum and minimum

{CODE aggregate_5@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Example V - adding ranges

{CODE aggregate_6@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Example VI - multiple aggregations

{CODE aggregate_7@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

## Related articles

- [Indexes : Querying : Dynamic aggregation](../../../indexes/querying/dynamic-aggregation)   
