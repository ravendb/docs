# Live Projections

Usually, RavenDB can tell what types you want to return based on the query type and the CLR type encoded in a document, but there are some cases where you want to query on one thing, but the result is completely different. This is usually the case when you are using live projections.

For example, let us take a look at the following index:

{CODE live_projections_1@ClientApi\Querying\StaticIndexes\LiveProjections.cs /}

Note that when we query this index, we can query based on UserId or ProductId, but the result that we get back aren't of the same type that we query on. For that reason, we have the [`OfType<T>` (`As<T>`)](../results-transformation/of-type) extension method. We can use it to change the result type of the query:

{CODE live_projections_2@ClientApi\Querying\StaticIndexes\LiveProjections.cs /}

In the code above, we query the `PurchaseHistoryIndex` using `Shipment` as the entity type to search on, but we get the results as `PurchaseHistoryViewItem`.