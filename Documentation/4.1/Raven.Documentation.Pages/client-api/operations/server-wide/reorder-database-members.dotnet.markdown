# Operations: Server: How to reoder database members?

**ReorderDatabaseMembersOperation** allows you to change the order of nodes in the [Database Group Topology](../../../studio/database/settings/manage-database-group).

## Syntax

{CODE syntax@ClientApi\Operations\Server\ReorderDatabaseMembers.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of a database to operate on |
| **order** | List\<string> | List of node tags of all existing nodes in the database group, listed in the exact order that you wish to have. <br> Throws `ArgumentException` is the reordered list doesn't correspond to the existing nodes of the database group |


## Example I

{CODE example_1@ClientApi\Operations\Server\ReorderDatabaseMembers.cs /}

## Example II

{CODE example_2@ClientApi\Operations\Server\ReorderDatabaseMembers.cs /}
