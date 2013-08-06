##### OfType (As)

{NOTE Both methods `OfType<T>` and `As<T>` are obsolete in favor of [Result Transformers](result-transformers) /}

RavenDB provides a feature called [Live Projections](../static-indexes/live-projections) that offers the ability to transform results of a query on a server side. 
In order to use it in the Client API you have to use `OfType<T>` (`As<T>`) extension method accessible from `Raven.Client` namespace.

{INFO RavenDB supports both methods `OfType<T>` and `As<T>` but the recommended usage is `OfType<T>`. /}

This method lets RavenDB know that you want to get the transformed results of the index according to its transform definition.
This way you are able to construct queries where the query model is the same type as the Map/Reduce result but the final query result will be changed to the specified type (its properties must match the TransformResult ones).

The `OfType<T>` method is to transform the type of the `IQueryable` on the client side. For example when we have the simple index as follow:

{CODE linq_extensions_of_type_product_by_quantity_index@ClientApi\Querying\ResultsTransformation\OfType.cs /}

which indexes `Products` by quantity and transform the results into much simpler object that only consists of `Name` and `Description`.

{CODE product_class@ClientApi\Querying\ResultsTransformation\Product.cs /}
{CODE product_view_model_class@ClientApi\Querying\ResultsTransformation\ProductViewModel.cs /}

The results of the index can be converted to `ProjectViewModel` type. Now you are able to execute the query:

{CODE linq_extensions_of_type_of_type_query@ClientApi\Querying\ResultsTransformation\OfType.cs /}

There are two important points to mention. Firstly note that the type of the query model is `Product`, the `Where` statement contains the predicate against indexed `QuantityInWarehouse` field. However the usage of `OfType<ProductViewModel>()` makes that the query result becomes the collection of `ProductViewModel` items.

By default indexes return entire documents. Here we use the live projections hence the data that we will get will contain only indicated fields. 
Those fields internally are grabbed from the document. It means that in the database server the entire document is pulled and it transformed according to transformation definition. In result only those fields are sent to the client what results in lower data transfer.