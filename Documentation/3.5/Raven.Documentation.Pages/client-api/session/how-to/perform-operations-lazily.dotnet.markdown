# Session: How to perform operations lazily?

Operation execution for operations such as `Load`, `Load with Includes`, `LoadStartingWith`, `MoreLikeThis` and queries can be deferred till needed using **lazy session operations**. Those operations ca be accessed using `Lazily` property found in `Advanced` session operations.

## Operations

Available operations are:

- `Load` - described [here](../../../client-api/session/loading-entities#load).
- `Load with Includes` - described [here](../../../client-api/session/loading-entities#load-with-includes).
- `LoadStartingWith` - described [here](../../../client-api/session/loading-entities#loadstartingwith).
- `MoreLikeThis` - described [here](../../../client-api/session/how-to/use-morelikethis).

## Querying

Dedicated article about lazy queries can be found [here](../../../client-api/session/querying/how-to-perform-queries-lazily).

## Example

{CODE lazy_1@ClientApi\Session\HowTo\Lazy.cs /}

## Executing all pending lazy operations

To execute all pending lazy operations use `ExecuteAllPendingLazyOperations` method from **eager session operations** found under `Eagerly` property in `Advanced` session operations.

{CODE lazy_2@ClientApi\Session\HowTo\Lazy.cs /}

## Related articles

- [How to perform queries lazily?](../querying/how-to-perform-queries-lazily)
