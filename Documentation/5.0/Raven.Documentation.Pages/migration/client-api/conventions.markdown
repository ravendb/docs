# Migration: Changes in Conventions

In 5.0 we have introduced an abstract layer over the whole JSON serialization process. This will allow us in the future to introduce more JSON serialization options than currently supported Json.NET one.

{PANEL:Raven.Client.Json.Serialization namespace}

New namespace was introduced, the `Raven.Client.Json.Serialization` containing following classes and interfaces needed for the serialization process:

- `ISerializationConventions` - entry point for the JSON serialization process. Set by default to `JsonNetSerializationConventions`.
- `IJsonSerializer`, `IJsonWriter`, `IJsonReader` - control the in and out JSON serialization
- `IBlittableJsonConverter` - allows to convert from entity to blittable and the other way around

{PANEL/}


{PANEL:Raven.Client.Json.Serialization.JsonNet namespace}

This new namespace is the core for the Json.NET serialization. We haven't introduced any changes in how the serialization works. All of the conventions that were directly related to the Json.NET serializer were moved here. Most interesting types in this namespace are:

- `JsonNetSerializationConventions` - contains all of the conventions related to Json.NET that were previously available directly in `DocumentConventions` class
- `DefaultRavenContractResolver` - our default contract resolver, moved from `Raven.Client.Documents.Conventions` namespace

{PANEL/}

{PANEL:DocumentConventions.Serialization}

This property was introduced in order to be able to abstract the JSON serialization and is the sole point where you should configure your serialization-related conventions. By default it is set to `JsonNetSerializationConventions` in the Client API.

{PANEL/}

{PANEL:How to Migrate}

If you have previosuly set-up one of the following conventions:

- `CustomizeJsonSerializer`
- `CustomizeJsonDeserializer`
- `JsonContractResolver`
- `DeserializeEntityFromBlittable`

Then in order to migrate, you need to wrap them in `JsonNetSerializationConventions` and assign that instance to your `DocumentStore.Conventions.Serialization` property.

For example. Let's assume that `JsonContractResolver` was configured by you in following way:

{CODE-BLOCK:csharp}

DocumentStore.Conventions = new DocumentConventions
{
    JsonContractResolver = new CustomJsonContractResolver()
};

{CODE-BLOCK/}

Starting from 5.0 this code will not compile, but fixing is as simple as wrapping that in the `JsonNetSerializationConventions` like follows:

{CODE-BLOCK:csharp}

DocumentStore.Conventions = new DocumentConventions
{
    Serialization = new JsonNetSerializationConventions
    {
        JsonContractResolver = new CustomJsonContractResolver()
    }
};

{CODE-BLOCK/}

That is all. Noting else is required.

{PANEL/}
