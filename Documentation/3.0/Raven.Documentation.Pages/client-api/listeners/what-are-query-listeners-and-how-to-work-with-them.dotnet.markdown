# Listeners: What are query listeners and how to work with them?

The reason we have document query listeners is to apply query customizations globally. In order to do this the user need to create their own implementation of the `IDocumentQueryListener`.

{CODE document_query_listener@ClientApi\Listeners\Query.cs /}

##Example

If we want to disable caching of all query results, you can implement `DisableCachingQueryListener` which will add `NoCaching` customization to each performed query.

{CODE document_query_example@ClientApi\Listeners\Query.cs /}

## Related articles

- [Indexes : Querying : Basics](../../indexes/querying/basics)
