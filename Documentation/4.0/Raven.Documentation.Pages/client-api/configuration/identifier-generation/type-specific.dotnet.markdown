#Type-Specific Identifier Generation

[In the previous article](../../../client-api/configuration/identifier-generation/global), Global Identifier generation conventions were introduced. Any customization made by using those conventions changes the behavior for all stored entities.
Now we will show how to override the default ID generation in a more granular way, for particular types of entities.

To override default document identifier generation algorithms, you can register custom conventions per an entity type. You can include your own identifier generation logic.

##RegisterAsyncIdConvention

{CODE register_id_convention_method_async@ClientApi\Configuration\IdentifierGeneration\TypeSpecific.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **func** | Func<string, TEntity, Task&lt;string&gt;> | Identifier generation function that supplies a result in async way for given database name (`string`) and entity object (`TEntity`). |

| Return Value | |
| ------------- | ----- |
| DocumentConventions | Current `DocumentConventions` instance. |

{NOTE This method applied to both synchronous and asynchronous operations /}

{INFO:Database name parameter}
The database name parameter is passed to the register convention methods to allow users to make Id generation decision per database.
{INFO/}

###Example

Let's say that you want to use semantic identifiers for `Employee` objects. Instead of `employee/[identity]` you want to have identifiers like `employees/[lastName]/[firstName]`
(for the sake of simplicity, let us not consider the uniqueness of such identifiers). What you need to do is to create the convention that will combine the `employee` prefix, `LastName` and `FirstName` properties of an employee.

{CODE employees_custom_async_convention@ClientApi\Configuration\IdentifierGeneration\TypeSpecific.cs /}

Now, when you store a new entity:

{CODE employees_custom_convention_example@ClientApi\Configuration\IdentifierGeneration\TypeSpecific.cs /}

the client will associate the `employees/Bond/James` identifier with it.

##Inheritance

Registered conventions are inheritance-aware so all types that can be assigned from registered type will fall into that convention according to inheritance-hierarchy tree.

###Example

If we create a new class `EmployeeManager` that will derive from our `Employee` class and keep the convention registered in the last example, both types will use the following:

{CODE employees_custom_convention_inheritance@ClientApi\Configuration\IdentifierGeneration\TypeSpecific.cs /}

If we register two conventions, one for `Employee` and the second for `EmployeeManager` then they will be picked for their specific types.

{CODE custom_convention_inheritance_2@ClientApi\Configuration\IdentifierGeneration\TypeSpecific.cs /}

##Related Articles

- [Document identifier generation](../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [Global identifier generation conventions](../../../client-api/configuration/identifier-generation/global)
