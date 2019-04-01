# Session: Querying: How to perform dynamic aggregation?

Dynamic aggregation can be performed using `aggregateBy` method. Internally such aggregation is an extended [faceted search](../../../client-api/session/querying/how-to-perform-a-faceted-search).

## Syntax

{CODE:java aggregate_1@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | String or Path | Path (or expression from which path will be extracted) to field on which aggregation will be performed. |
| **displayName** | String | User defined friendly name for aggregation. If `null`, field name will be used. |

| Return Value | |
| ------------- | ----- |
| [DynamicAggregationQuery](../../../glossary/dynamic-aggregation-query)&lt;TResult&gt; | Query containing aggregation methods. |

## Example I - summing

{CODE:java aggregate_2@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Example II - counting

{CODE:java aggregate_3@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Example III - average

{CODE:java aggregate_4@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Example IV - maximum and minimum

{CODE:java aggregate_5@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Example V - adding ranges

{CODE:java aggregate_6@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Example VI - multiple aggregations

{CODE:java aggregate_7@ClientApi\Session\Querying\HowToPerformDynamicAggregation.java /}

## Related articles

- [Indexes : Querying : Dynamic aggregation](../../../indexes/querying/dynamic-aggregation)   
