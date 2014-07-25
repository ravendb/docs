# Client API : Session : How to perform operations lazily?

Operation execution for operations such as `Load`, `Load with Includes`, `LoadStartingWith`, `MoreLikeThis` and queries can be defered till needed using **lazy session operations**. Those operations ca be accesed using `Lazily` property found in `Advanced` session operations.

## Operations

Available operations are:

- `Load` - described [here]().
- `Load with Includes` - described [here]().
- `LoadStartingWith` - described [here]().
- `MoreLikeThis` - described [here]().

## Querying

Dedicated article about lazy queries can be found [here]().

## Example

{CODE lazy_1@ClientApi\Session\HowTo\Lazy.cs /}

## Executing all pending lazy operations

To execute all pending lazy operations use `ExecuteAllPendingLazyOperations` method from **eager session operations** found under `Eagerly` property in `Advanced` session operations.

{CODE lazy_2@ClientApi\Session\HowTo\Lazy.cs /}

#### Related articles

TODO