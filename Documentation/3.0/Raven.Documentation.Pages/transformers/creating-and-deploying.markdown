# Creating and deploying transformers

{PANEL}
Transformers can be created and send to server in couple of ways, starting from using low-level [commands](../transformers/creating-and-deploying#using-commands) to creating [custom classes](../transformers/creating#using-abstracttransformercreationtask) and sending them individually or even scanning an assembly.
{PANEL/}

{PANEL:**using AbstractTransformerCreationTask**}

Special abstract class has been created for strongly-typed transformer creation called `AbstractTransformerCreationTask`. 

There are certain benefits of using it:

- **strongly-typed syntax**
- ability to deploy it easily
- ability to deploy it using assembly scanner (more about it later)
- ability to pass transformer as generic type is various methods without having to hardcode string-based names

Disadvanage of this is approach is that transformer names are auto-generated from type name and cannot be changed, but there are certain naming conventions that can be followed that will help shape up the name (more about it later).

{NOTE We recommend creating and using transformers in this form due to its simplicity, many benefits and minor amount of disadvantages. /}

### Naming conventions

Actually there is only one naming conventions: each `_` in class name will be translated to `/` in transformer name.

E.g.

In `Northwind` samples there is a transformer called `Orders/Company`. To get such a transformer name, we need to create class called `Orders_Company`.

{CODE transformers_1@Transformers/Creating.cs /}

### Sending to server

Since transformers are server-side projections, they must be stored on server. To do so, we need to create instance of our class that implements `AbstractTransformerCreationTask` and use one of the deployment methods: `Execute` or `ExecuteAsync` for asynchronous call.

{CODE transformers_2@Transformers/Creating.cs /}

{CODE transformers_3@Transformers/Creating.cs /}

{SAFE If transformer exists on server and definition (name, transform function) is the same as the one that was send, then it will **not** be overwritten. /}

### Using assembly scanner

All classes that inherit from `AbstractTransformerCreationTask` can be deployed at once using one of `IndexCreation.CreateIndexes` methods.

{CODE transformers_4@Transformers/Creating.cs /}

Underneath, the `IndexCreation` will call `Execute` methods for each of found transformers (and indexes).

{WARNING `IndexCreation.CreateIndexes` will also deploy all classes that inherit from `AbstractIndexCreationTask`. /}

### Example

{CODE transformers_5@Transformers/Creating.cs /}

{PANEL/}

{PANEL:**using Commands**}

Another way to create transformer is to use low-level `PutTransformer` command from `DatabaseCommands`. API reference for this command can be found [here](../client-api/commands/transformers/put).

The advanatge of this approach is that you can define transformer name as you feel fit, but you loose all other possibilities when `AbstractTransformerCreationTask` is used.

{CODE transformers_6@Transformers/Creating.cs /}

Probably one of the worst things with this approach is the lack of strongly-typed definition. It can be partially addressed by creating `TransformerDefinition` from class that implements `AbstractTransformerCreationTask` by invoking `CreateTransformerDefinition` method.

{CODE transformers_7@Transformers/Creating.cs /}

{SAFE If transformer exists on server and definition (name, transform function) is the same as the one that was send, then it will **not** be overwritten. /}

{INFO Commands approach is not recommended and should be used only if needed. /}

{PANEL/}

## Related articles

- [What are transformers?](../transformers/what-are-transformers)
- [Basic transformations](../transformers/basic-transformations)
- [[Client API] PutTransformer](../client-api/commands/transformers/put)
