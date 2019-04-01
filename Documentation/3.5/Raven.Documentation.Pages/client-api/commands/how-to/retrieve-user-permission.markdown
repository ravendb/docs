# Commands: How to retrieve user permission for a specified database?

`GetUserPermission` used to retrieve user permissions for a specified database

## Syntax

{CODE get_user_permission_1@ClientApi\Commands\HowTo\GetUserPermission.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | name of the database we want to retrive the permissions |
| **readOnly** | bool | the type of the operations allowed, read only , or read-write |


| Return Value | |
| ------------- | ----- |
| [UserPermission](../../../glossary/user-permission) | Retrieves user permissions for a specified database. |

## Example I

{CODE get_user_permission_2@ClientApi\Commands\HowTo\GetUserPermission.cs /}

## Related articles

- [How to Retrieves user info](../../../client-api/commands/how-to/retrieve-user-info)
