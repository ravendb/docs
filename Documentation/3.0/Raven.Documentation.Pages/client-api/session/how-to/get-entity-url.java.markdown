# Session: How to get entity url?

`getDocumentUrl` is a method in `advanced()` session operations that returns full url for a given entity.

## Syntax

{CODE:java get_entity_url_1@ClientApi\Session\HowTo\GetEntityUrl.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | Object | Instance of an entity for which url will be returned. |

| Return Value | |
| ------------- | ----- |
| String | Full url for a given `entity`. |

## Example

{CODE:java get_entity_url_2@ClientApi\Session\HowTo\GetEntityUrl.java /}

## Remarks

If the entity is transient (not attached) this method will throw `IllegalStateException`.
