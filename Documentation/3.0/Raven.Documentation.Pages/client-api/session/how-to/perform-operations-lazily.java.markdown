# Session: How to perform operations lazily?

Operation execution for operations such as `load`, `load with includes`, `loadStartingWith`, `moreLikeThis` and queries can be deferred till needed using **lazy session operations**. Those operations ca be accessed using `lazily` in `advanced()` session operations.

## Operations

Available operations are:

- `load` - described [here](../../../client-api/session/loading-entities#load).
- `load with Includes` - described [here](../../../client-api/session/loading-entities#load-with-includes).
- `loadStartingWith` - described [here](../../../client-api/session/loading-entities#loadstartingwith).
- `moreLikeThis` - described [here](../../../client-api/session/how-to/use-morelikethis).

## Querying

Dedicated article about lazy queries can be found [here](../../../client-api/session/querying/how-to-perform-queries-lazily).

## Example

{CODE:java lazy_1@ClientApi\Session\HowTo\Lazy.java /}

## Executing all pending lazy operations

To execute all pending lazy operations use `executeAllPendingLazyOperations` method from **eager session operations** found under `eagerly` in `advanced()` session operations.

{CODE:java lazy_2@ClientApi\Session\HowTo\Lazy.java /}

## Related articles

- [How to perform queries lazily?](../querying/how-to-perform-queries-lazily)
