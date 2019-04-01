# Settings: Custom Functions

This view allows you to write custom JavaScript functions that can be used by all functionalities on server side that take leverage of JavaScript e.g. [patching API](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents) or [SQL Replication](../../../server/bundles/sql-replication/basics). To write custom functions, you need to create your own [commonjs module](http://wiki.commonjs.org/wiki/Modules/1.1).

## Example

- for start, let's define a function that will produce from `firstName` and `lastName` a fullname:

![Figure 1. Settings. Custom Functions.](images/custom-functions-1.png)

- now, in [Patch View](../../../studio/overview/documents/patch-view) we can use it as follows:

![Figure 2. Settings. Custom Functions. Patch View.](images/custom-functions-2.png)
