# Session: How to get entity url?

`GetDocumentUrl` is a method in `Advanced` session operations that returns full url for a given entity.

## Syntax

{CODE get_entity_url_1@ClientApi\Session\HowTo\GetEntityUrl.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of an entity for which url will be returned. |

| Return Value | |
| ------------- | ----- |
| string | Full url for a given `entity`. |

## Example

{CODE get_entity_url_2@ClientApi\Session\HowTo\GetEntityUrl.cs /}

## Remarks

If the entity is transient (not attached) this method will throw `InvalidOperationException`.
