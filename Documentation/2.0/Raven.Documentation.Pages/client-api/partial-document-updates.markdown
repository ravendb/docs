# Partial document updates using the Patching API

The process of document patching allows for modifying a document on the server without having to load it in full and saving it back. This is usually useful for updating denormalized data in entities.

In a normal use case, the client would issue a Load command to the server, deserialize the response into an entity, make changes to that entity, and then send it back for the server serialized. Using the Patching API the client can issue a single Patch command and the server will perform the requested operation on the JSON representation of the document. This can save bandwidth and be faster, but it is not a transactional operation, and as such only the last patching command is actually going to be persisted.

{NOTE Since this feature involves low-level document manipulation, it is considered to be an expert feature and generally should not be used as a general purpose solution. If you have reached a scenario where you are considering using this, you might want to recheck your data model and see if it can be optimized to prevent usage of the Patching API. The only exception is updating denormalized data, where this approach is valid but not always recommended. /}

The patching API is exposed through RavenDB's `DatabaseCommands`, available from the document store object and `session.Advanced`. A patch command is issued by calling a single function `Patch`, accepting three parameters: the document key, an array of PatchRequests and an optional Etag:

{CODE patching1@ClientApi/PartialDocumentUpdates.cs /}

The document key is the unique key for the document in the current database, on which this patch command will be performed. Specifying an Etag would ensure changes are only made if no writes were performed since the client acquired the specified Etag.

Following are a description of the `PatchRequest` object, and the different options available through the Patching API. We will be using a simplistic example of a blog engine, using these classes:

{CODE blogpost_classes@Common.cs /}

## The PatchRequest object

When creating a PatchRequest object to be used in a patch command, at least 2 properties have to be specified: `Name` and `Type`.

Given the object graph stored under a given key, `Name` is the path from the root to a property (or properties) within that object graph. The syntax is similar to how XPath operates on XML, only more simplistic.

`Type` is what defines the patch command. It can be one of the following:

* **Set** - Set a property
* **Unset** - Unset (remove) a property
* **Inc** - Increment a property by a specified value
* **Rename** - Rename a property
* **Copy** - Copy a property value to another property
* **Modify** - Modify a property value by providing a nested set of patch operation
* **Add** - Add an item to an array
* **Insert** - Insert an item to an array at a specified position
* **Remove** - Remove an item from an array at a specified position

## Performing simple updates

A property in a stored document is a field from an entity. To change it's value using the Patching API, provide its path in `Name`, and initialize `Type` with `PatchCommandType.Set`. Then, serialize the object you want to save into that property, and pass it as `Value`.

The new value you set can be anything: a native type, an object, or a collection of entities. You can use `RavenJObject.FromObject(object)` to easily serialize it:

{CODE patching2@ClientApi/PartialDocumentUpdates.cs /}

Removing a property is done by simply passing `PatchCommandType.Unset` as `Type`.

To rename a property, or copy its value to another property, specify the new path as the `Value`:

{CODE patching3@ClientApi/PartialDocumentUpdates.cs /}

Numeric values used as counters can be incremented or decremented, without worrying about their actual value. Use positive values to increment, and negative values to have it decremented:

{CODE patching4@ClientApi/PartialDocumentUpdates.cs /}

## Conditional updates

If `PrevVal` is set, it's value will be compared against the current value of the property to verify a change isn't overwriting new values. If the value is `null`, the operation is always successful.

## Working with arrays

Any collection in your entity will be serialized into an array in the resulting JSON document. You can perform collection-specific operations on it easily, by using the `Position` property:

{CODE patching_arrays1@ClientApi/PartialDocumentUpdates.cs /}

Being a JSON object, you can treat the entire array as value like shown above. Sometimes, however, you want to access certain items in the array

## Working with nested operations
The nested operations are only valid of the 'Type' is `PatchCommandType.Modify`.  
If we want to change all items in a collection we could do that by setting the AllPositions porparty to 'true'

**Here are a few examples of nested operations:**

Set value in a nested element:

{CODE nested1@ClientApi/PartialDocumentUpdates.cs /}

Remove value in a nested element:

{CODE nested2@ClientApi/PartialDocumentUpdates.cs /}

## Performing complex updates
If you need to deal with more complex patching algorithm, using the methods shown above, might become cumbersome.

That's where the `ScriptedPatchRequest` comes in handy. It allows you to send a JavaScript snippet to the database which is executed against the JSON of the document which should be updated.

Adding a new `BlogComment` is as simple as this:

{CODE scriptedpatching1@ClientApi/PartialDocumentUpdates.cs /}

`ScriptedPatchRequest` also provides an easy way to remove items from an array:

{CODE scriptedpatching2@ClientApi/PartialDocumentUpdates.cs /}

Often, you don't simply want to remove items from arrays but remove them conditionally instead. Even this can be done easily using `ScriptedPatchRequest`:

{CODE scriptedpatching3@ClientApi/PartialDocumentUpdates.cs /}

Of course, `ScriptedPatchRequest` allows you to use any arbitrary JavaScript functionality like for-loops as well.

### lodash.js

You can also take advantage of any function from [Lo-Dash](https://lodash.com/) library which is included into Raven script engine by default. Example with <em>forEach</em> usage:

{CODE scriptedpatching_lodash@ClientApi/PartialDocumentUpdates.cs /}

## Concurrency

If we wanted to we could run several batch operations in parallel, but we will not be able to set which one will end first.