# Session: How to Perform Operations Lazily

Operation execution for operations such as `load`, `load with includes`, `loadStartingWith` and queries can be deferred till needed using **lazy session operations**. Those operations can be accessed using `lazily()` property found in `advanced` session operations.

## Operations

Available operations are:

- `load` - described [here](../../../client-api/session/loading-entities#load).
- `load with includes` - described [here](../../../client-api/session/loading-entities#load-with-includes).
- `loadStartingWith` - described [here](../../../client-api/session/loading-entities#loadstartingwith).

## Querying

Dedicated article about lazy queries can be found [here](../../../client-api/session/querying/how-to-perform-queries-lazily).

## Example

{CODE:nodejs lazy_1@client-api\session\howTo\lazy.js /}

## Executing All Pending Lazy Operations

To execute all pending lazy operations use `executeAllPendingLazyOperations()` method from **eager session operations** found under `eagerly` property in `advanced` session operations.

{CODE:nodejs lazy_2@client-api\session\howTo\lazy.js /}

## Related Articles

### Session

- [How to Perform Queries Lazily](../../../client-api/session/querying/how-to-perform-queries-lazily)
