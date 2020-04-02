#Type-Specific Identifier Generation

[In the previous article](../../../client-api/configuration/identifier-generation/global), Global Identifier generation conventions were introduced. Any customization made by using those conventions changes the behavior for all stored entities.
Now we will show how to override the default ID generation in a more granular way, for particular types of entities.

To override default document identifier generation algorithms, you can register custom conventions per an entity type. You can include your own identifier generation logic.

##RegisterIdConvention

{CODE:nodejs register_id_convention_method_async@client-api\configuration\identifierGeneration\typeSpecific.js /}

| Parameters | | |
| ------------- | ------------- | ----- | 
| clazz | class or object | Entity type | 
| idConvention | function `(databaseName, entity) => Promise<string>` | Identifier generation function that supplies a result for given database name and entity object. Must return a `Promise` resolving to a string. |

| Return Value | |
| ------------- | ----- |
| DocumentConventions | Current `DocumentConventions` instance. |

{INFO:Database name parameter}
The database name parameter is passed to the register convention methods to allow users to make Id generation decision per database.
{INFO/}

###Example

Let's say that you want to use semantic identifiers for `Employee` objects. Instead of `employee/[identity]` you want to have identifiers like `employees/[lastName]/[firstName]`
(for the sake of simplicity, let us not consider the uniqueness of such identifiers). What you need to do is to create the convention that will combine the `employee` prefix, `LastName` and `FirstName` properties of an employee.

{CODE:nodejs employees_custom_async_convention@client-api\configuration\identifierGeneration\typeSpecific.js /}

Now, when you store a new entity:

{CODE:nodejs employees_custom_convention_example@client-api\configuration\identifierGeneration\typeSpecific.js /}

the client will associate the `employees/Bond/James` identifier with it.

{INFO ID convention function must return a Promise since it *can* be asynchronous. /}

###Example: Object literal based entities

{CODE:nodejs employees_custom_async_convention_typedescriptor@client-api\configuration\identifierGeneration\typeSpecific.js /}

##Related Articles

- [Document identifier generation](../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [Global identifier generation conventions](../../../client-api/configuration/identifier-generation/global)
