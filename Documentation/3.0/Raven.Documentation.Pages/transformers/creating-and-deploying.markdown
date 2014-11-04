# Creating and deploying transformers

{PANEL}
Transformers can be created and send to a server in a couple of ways, starting from the low-level [commands](../transformers/creating-and-deploying#using-commands), to creating [custom classes](../transformers/creating#using-abstracttransformercreationtask) and sending them individually, or even scanning an assembly.
{PANEL/}

{PANEL:**using AbstractTransformerCreationTask**}

Special abstract class has been created for strongly-typed transformer creation called `AbstractTransformerCreationTask`. 

There are certain benefits of using it:

- **strongly-typed syntax**
- ability to deploy it easily
- ability to deploy it using assembly scanner (more about that later)
- ability to pass transformer as a generic type in various methods without having to hardcode string-based names

By default the transformer's names are auto-generated from the type name. You can change that if you really want to (override the `TransformerName` property), but this option is used rarely. More details later in this article.

This approach limits you to following the strongly typed rules, while server transformers aren't limited by them. However, it is rarely an issue, unless you write a transformer to cover multiple types without a common ancestor.

{NOTE We recommend creating and using transformers in this form because of its simplicity: there are many benefits and only few disadvantages. /}

### Naming conventions

Actually there is only one naming convention: each `_` in class name will be translated to `/` in transformer name. If you want to customize the transformer's name, you can do it by overriding the `TransformerName` property.

e.g.

In the `Northwind` samples, there is a transformer called `Orders/Company`. To get such a name for a transformer, we need to create a class called `Orders_Company`.

{CODE transformers_1@Transformers/Creating.cs /}

### Sending to server

Since transformers are server-side projections, they must be stored on a server. To do so, we need to create instance of our class that inherits from `AbstractTransformerCreationTask`, and use one of the deployment methods: `Execute` or `ExecuteAsync` for asynchronous call.

{CODE transformers_2@Transformers/Creating.cs /}

{CODE transformers_3@Transformers/Creating.cs /}

{SAFE If a transformer exists on a server and a stored definition (name, transform function) is the same as the one that was send, then it will **not** be overwritten. /}

### Using assembly scanner

All classes that inherit from `AbstractTransformerCreationTask` can be deployed at once with one of `IndexCreation.CreateIndexes` method overloads.

{CODE transformers_4@Transformers/Creating.cs /}

Underneath, the `IndexCreation` will call `Execute` methods for each of the transformers (and indexes) found.

{WARNING `IndexCreation.CreateIndexes` will also deploy all the classes that inherit from the `AbstractIndexCreationTask` (more about it [here](../indexes/creating-and-deploying)). /}

### Example

{CODE transformers_5@Transformers/Creating.cs /}

{PANEL/}

{PANEL:**using Commands**}

Another way to create a transformer is to use low-level `PutTransformer` command from `DatabaseCommands`. API reference for this command can be found [here](../client-api/commands/transformers/put).

The advantage of this approach is defining transformer's name as you feel fit, yet at the same time you loose all other possibilities when the `AbstractTransformerCreationTask` is used.

{CODE transformers_6@Transformers/Creating.cs /}

This approach lacks any strongly-typed definition guarantees, but the advantage is that we aren't limited by the system type. In this case, we can
execute this transformer on any type with a Name property. Note that in practice, you _can_ use any transformer on any type. They execute on the server
and have no concept of your user defined types. However, it is usually easier to look at untyped transformers and see that they can operate on all types, 
than to look at a typed transformer and understand that it can operate on types other than what it is defined for.

TransformerDefinition can also be partially addressed by creating `TransformerDefinition` from class that implements `AbstractTransformerCreationTask` by invoking `CreateTransformerDefinition` method.

{CODE transformers_7@Transformers/Creating.cs /}

{SAFE If transformer exists on server and definition (name, transform function) is the same as the one that was send, then it will **not** be overwritten. /}

{INFO Commands approach is not recommended and should be used only if needed. /}

{PANEL/}

## Related articles

- [What are transformers?](../transformers/what-are-transformers)
- [Basic transformations](../transformers/basic-transformations)
- [[Client API] PutTransformer](../client-api/commands/transformers/put)
