# Session: How to mark entity as readonly?

Entities can be marked as read-only using `MarkReadOnly`. This operation is equal to adding to entity metadata a key `Raven-Read-Only` with value set to `True`.   

Implications of setting entity as read-only are as follows:   

- change tracking won't apply to such entity
- forcing updates or deletes (e.g. using `Commands`) will throw `OperationVetoedException`

## Syntax

{CODE mark_as_readonly_1@ClientApi\Session\HowTo\MarkAsReadonly.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of an entity that will be marked as read-only. |

## Example

{CODE mark_as_readonly_2@ClientApi\Session\HowTo\MarkAsReadonly.cs /}
