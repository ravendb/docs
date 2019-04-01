# Listeners: What are query listeners and how to work with them?

The reason we have document query listeners is to apply query customizations globally. In order to do this the user need to create their own implementation of the `IDocumentQueryListener`.

{CODE:java document_query_listener@ClientApi\Listeners\Query.java /}

##Example

If we want to disable caching of all query results, you can implement `DisableCachingQueryListener` which will add `noCaching` customization to each performed query.

{CODE:java document_query_example@ClientApi\Listeners\Query.java /}

## Related articles

- [Indexes : Querying : Basics](../../indexes/querying/basics)
