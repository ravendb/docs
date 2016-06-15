# Basic transformations

Assuming that you already know how to [create](../transformers/creating-and-deploying) transformers, you will want to know what can be done with them and what projection functions can be created.

{INFO:Projection function}
Transformers core is its projection function. It is a LINQ-based function with the ability to [load](../transformers/loading-documents) or [include](../transformers/including-documents) additional documents. [Parameters](../transformers/passing-parameters) can be also passed to customize the behavior.
{INFO/}

## Basics

To start, let's create a projection that will return only the `FirstName` and the `LastName` from each returned `Employee` from the `Northwind` database.

- first, let's start creating a transformer `Employees/FirstAndLastName`

{CODE transformers_1@Transformers/Basics.cs /}

You have probably noticed that we're passing `Employee` as a generic parameter to `AbstractTransformerCreationTask`. By doing so, our transformation function will have a strongly-typed syntax.

- the next step is to create a transformation itself. To do so, we need to set the `TransformResults` property with our function in **parameterless constructor**.

{CODE-TABS}
{CODE-TAB:csharp:Query-syntax transformers_2@Transformers/Basics.cs /}
{CODE-TAB:csharp:Method-syntax transformers_3@Transformers/Basics.cs /}
{CODE-TABS/}

- the final step is to [deploy it to the server](../transformers/creating-and-deploying) (omitted) and transform our query results using the [TransformWith](../client-api/session/querying/how-to-use-transformers-in-queries) query extension method:

{CODE transformers_4@Transformers/Basics.cs /}

Probably `dynamic` is not the best return type so, obviously, projections to concrete types are supported:

{CODE transformers_5@Transformers/Basics.cs /}

{CODE transformers_6@Transformers/Basics.cs /}

Our final transformer looks like this:

{CODE transformers_7@Transformers/Basics.cs /}

{WARNING:Important}
Before moving further, please note that property values of the objects passed to the projection function (in our example we are passing `employees`) are taken from the stored index fields, if present, otherwise they are loaded from a database.   

**Example** 

If we would [store](../indexes/storing-data-in-index) the `FirstName` and the `LastName` in the index that was queried, then the above transformer would use the values from index directly, without loading them from a database.  
{WARNING/}

## Projecting a single property

You do not have to create a new objects each time: when only single property is required, all you need to do is select that property:

{CODE transformers_8@Transformers/Basics.cs /}

{CODE transformers_9@Transformers/Basics.cs /}

## Projecting a complex property

When your documents contain nested objects and you want to return only those, then the projection can look as follows:

{CODE transformers_1_0@Transformers/Basics.cs /}

{CODE transformers_1_1@Transformers/Basics.cs /}

## Related articles

- [What are transformers?](../transformers/what-are-transformers)
- [Creating and deploying transformers](../transformers/creating-and-deploying)
- [Loading documents](../transformers/loading-documents)
