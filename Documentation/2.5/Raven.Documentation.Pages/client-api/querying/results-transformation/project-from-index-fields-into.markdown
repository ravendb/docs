#ProjectFromIndexFieldsInto (AsProjection)

The `ProjectFromIndexFieldsInto<T>` (`AsProjection<T>`) method (extension accessible from `Raven.Client` namespace) provides the way to change the type of a query result according to *stored fields in an index*.

{INFO RavenDB supports both methods `ProjectFromIndexFieldsInto<T>` and `AsProjection<T>` but the recommended usage is `ProjectFromIndexFieldsInto<T>`. /}

This method the same way like [`OfType<T>`](of-type) allows you to move between the query model and the query result. The difference between them is the way how they pull the data.
In contrast to `OfType<T>`, the usage of `ProjectFromIndexFieldsInto<T>` causes that fields are grabbed directly from the stored values in the indexes (Lucene documents).

Let's consider the example. We have a simple map-only index:

{CODE index_def@ClientApi\Querying\ResultsTransformation\ProjectFromIndexFieldsInto.cs /}

Apart from <em>__document_id</em> (which is stored by default) it stores *Name* and *Description*. We also have two classes `Product` and `ProductViewModel`:

{CODE product_class@ClientApi\Querying\ResultsTransformation\Product.cs /}
{CODE product_view_model_class@ClientApi\Querying\ResultsTransformation\ProductViewModel.cs /}

Now we are able to do projection of the index results into `ProductViewModel` by using fields stored in the index:

{CODE query@ClientApi\Querying\ResultsTransformation\ProjectFromIndexFieldsInto.cs /}

The query as above will sent additional query string parameters: *?fetch=Name&fetch=Description*, what is a hint for RavenDB that those values should be pulled from index. 
By default all properties of the type specified in `ProjectFromIndexFieldsInto<T>` method will be marked to retrieve from index fields. If the index will not store the field then it will be retrieved by loading the entire document and projecting needed property.

Such query is treated as the projection (but very different than Live Projection - note that the index does not contain `TransformResults` definition). In result RavenDB will return only fields corresponding with properties that the type `T` contains. They will be transformed on the client side and returned as collection of specified type objects.