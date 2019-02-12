# Polymorphism in RavenDB

RavenDB stores document in JSON format, which make it very flexible, but also make some code patterns harder to work with. In particular, the RavenDB Client API will not, by default, record type information into embedded parts of the JSON document. That makes for a much easier to read JSON, but it means that using polymorphism for objects that are embedded inside another document requires some modification.

{NOTE There is no problem with polymorphism for entities that are stored as documents, only with embedded documents. /}

That modification happens entirely at the [JSON.Net](https://www.newtonsoft.com/json) layer, which is responsible for serializing and deserializing documents. The problem is when you have a model such as this:

{CODE polymorphism_1@Faq/Polymorphism.cs /}

And you want to store the following data:

{CODE polymorphism_2@Faq/Polymorphism.cs /}

With the default JSON.Net behavior, you can serialize this object graph, but you can't deserialize it, because there isn't enough information in the JSON to do so.

RavenDB gives you the following extension point to handle that:

{CODE polymorphism_3@Faq/Polymorphism.cs /}
