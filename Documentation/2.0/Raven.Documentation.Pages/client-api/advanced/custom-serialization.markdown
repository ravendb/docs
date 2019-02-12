# Custom Serialization / Deserialization

When RavenDB is given a POCO to save, all properties (public, private and protected), and all public fields are serialized to JSON. This default behavior covers most of the common use-cases, but sometimes you need to tweak the way your document gets serialized by providing your own mapping.

## Through decoration with attributes 

The easy way to customize serialization is by decorating classes, properties and fields with attributes. RavenDB is using [JSON.NET](https://www.newtonsoft.com/json) for JSON serialization, and all attributes that are made available by the original project are supported by the RavenDB Client API as well. These attributes are in the `Raven.Imports.Newtonsoft.Json` namespace.

Following are some examples for usage in common scenarios.

### Ignoring a property

Ignoring properties which don't contain data, but maybe just provide some data manipulation in their getter or setter, is done by adding a `JsonIgnore` attribute:

{CODE custom_serialization1@ClientApi\Advanced\CustomSerialization.cs /}

Now only Length, but not LengthInInch will be serialized.

### Serializing under a different name

The JsonProperty attribute allows you to specify a custom converter class for a specific property. For example, changing the name of the serialized property is done like this:

{CODE custom_serialization2@ClientApi\Advanced\CustomSerialization.cs /}

### Allowing self references

For reasons concerning scaling and sharding, by default RavenDB does not try to resolve associations between documents. For self references within a document those reasons do not apply. It doesn't work out of the box, but once again, JSON.NET provides a handy attribute for this:

{CODE custom_serialization3@ClientApi\Advanced\CustomSerialization.cs /}

Now the children's `Parent` property will be a reference instead of a full copy of Category (which would result in an endless recursion, if JSON.NET wouldn't detect it and throw an exception).

## JsonContractResolver

You can also provide a custom `IContractResolver` implementation to either provide your hand made serialization contract, or to make a change that is global to the document store. Every Raven DocumentStore object exposes a `JsonContractResolver` property through its Conventions, with which you can set your own.

For example, following is an example for specifying that only public fields and properties will be serialized, ignoring everything non-public:

{CODE custom_serialization4@ClientApi\Advanced\CustomSerialization.cs /}

## Identity field

While serializing, RavenDB makes some assumptions about your ID entity (if exists). The complete discussion can be found [here](https://ravendb.net/docs/theory/document-key-generation?version=2.0).
