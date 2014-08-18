# Using transformers

Assuming that you already know how to [create](../transformers/creating) transformers, you will what can be done with them and what projection functions can be created.

{INFO All examples in this article use `AbstractTransformerCreationTask` approach for better understandability. /}

## Creating projections

Transformers core is its projection function. It is a LINQ-based function with the ability to load or include additional documents. Parameters can also be passed to customize the behavior.

### Basics

For start, let's create a projection that will return only `FirstName` and `LastName` from each returned `Employee` from `Northwind` database.

- first, let's start creating a transformer `Employees/FirstAndLastName`

{CODE transformers_1@Transformers/Using.cs /}

You probably noticed that we're passing `Employee` as a generic parameter to `AbstractTransformerCreationTask`, by doing this our transformation function will have a strongly-typed syntax.

- next step is to create a transformation itself, to do it we need to set the `TransformResults` property with our function in **parameterless constructor**.

{CODE transformers_2@Transformers/Using.cs /}

or if you preffer method syntax in LINQ:

{CODE transformers_3@Transformers/Using.cs /}

- final step is to [deploy it to the server](../transformers/creating) (omitted) and transform our query results using [TransformWith](../client-api/session/querying/how-to-use-transformers-in-queries) query extension method:

{CODE transformers_4@Transformers/Using.cs /}

Probably `dynamic` is not the best return type, so of course projections to concrete types are supported:

{CODE transformers_5@Transformers/Using.cs /}

{CODE transformers_6@Transformers/Using.cs /}

So our final transformer looks like:

{CODE transformers_7@Transformers/Using.cs /}

{WARNING:Important}
Before moving further, please note that property values of objects passed to projection function (in our example we are passing `employees`) are taken from stored index fields if present, otherwise they are loaded from a database.   

**Example** 

If we would [store]() `FirstName` and `LastName` in index that was queried, then above transformer would use values from index directly, without loading them from database.  
{WARNING/}

### Projecting simple property

Property can be projected with ease:

{CODE transformers_8@Transformers/Using.cs /}

{CODE transformers_9@Transformers/Using.cs /}

### Projecting complex property

Complex types can be projected as well:

{CODE transformers_1_0@Transformers/Using.cs /}

{CODE transformers_1_1@Transformers/Using.cs /}

### Loading external documents

To load external documents use `LoadDocument` method. It can be used for loading single document by its Id or multiple documents from an array containing Ids:

{CODE transformers_1_2@Transformers/Using.cs /}

### Passing parameters

Parameters can be passed to alter transformations by:

- using `AddTransformerParameter` in session [Query](../client-api/session/querying/how-to-use-transformers-in-queries)
- using `SetTransformerParameters` in session [DocumentQuery](../client-api/session/querying/lucene/how-to-use-lucene-in-queries)
- filling `TransformerParameters` in `IndexQuery` that can be used in commands [Query](../client-api/commands/transformers/how-to/transform-query-results)
- passing parameters directly in various [Get](../client-api/commands/documents/get) methods from commands

To access passed parameters from within a transformer use on of the two available methods: `Parameter` or `ParameterOrDefault`.

{CODE transformers_1_3@Transformers/Using.cs /}

### Nesting transformers

Ttransformers can even execute one from another by using `TransformWith` method in projection function.

{CODE transformers_1_4@Transformers/Using.cs /}

### Including additional documents in results