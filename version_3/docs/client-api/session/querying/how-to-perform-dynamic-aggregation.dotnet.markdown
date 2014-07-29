# Querying : How to perform dynamic aggregation?

Dynamic aggregation can be performed using `AggregateBy` method. Internally such aggregation is an extended [faceted search]().

## Syntax

{CODE aggregate_1@ClientApi\Session\Querying\HowToPerformDynamicAggregation.cs /}

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

#### Related articles

TODO