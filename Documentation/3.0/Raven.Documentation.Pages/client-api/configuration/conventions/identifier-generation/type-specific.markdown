#Type-specific identifier generation

[In previous article](./global) key generation conventions has been shown that apply globally. Any customization made by using those conventions changes the behavior for all stored entities.
Now we will present how to override the default ID generation in a more granular way, that is only for particular types of entities.

To override default document key generation algorithms, you can register custom conventions where you are able to include own identifier generation logic.
There are two methods to satisfy that:

{CODE register_id_convention_methods@ClientApi\Configuration\Conventions.cs /}

Let's say that you want to use semantic identifiers for `Employee` objects. Instead of `employee/[identity]` you want to have keys like `employees/[lastName]/[firstName]`
(for simplicity let us not consider the uniqueness of such IDs). What you need to do is to create the convention that will combine the `employee` prefix, `LastName` and `FirstName` 
properties of an employee.

{CODE eployees_custom_convention@ClientApi\Configuration\Conventions.cs /}