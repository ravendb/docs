# Session: How to Perform Operations Lazily

Operation execution for operations such as `load`, `load with includes`, `loadStartingWith` and queries can be deferred till needed using **lazy session operations**. Those operations ca be accessed using `lazily` property found in `advanced` session operations.

## Operations

Available operations are:

- `load` - described [here](../../../client-api/session/loading-entities#load).
- `load with includes` - described [here](../../../client-api/session/loading-entities#load-with-includes).
- `loadStartingWith` - described [here](../../../client-api/session/loading-entities#loadstartingwith).

## Querying

Dedicated article about lazy queries can be found [here](../../../client-api/session/querying/how-to-perform-queries-lazily).

## Example

{CODE:java lazy_1@ClientApi\Session\HowTo\Lazy.java /}

## Executing All Pending Lazy Operations

To execute all pending lazy operations use `executeAllPendingLazyOperations` method from **eager session operations** found under `eagerly` property in `advanced` session operations.

{CODE:java lazy_2@ClientApi\Session\HowTo\Lazy.java /}

## Related Articles

### Session

- [How to Perform Queries Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
