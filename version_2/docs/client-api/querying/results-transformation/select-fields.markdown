#SelectFields

The `SelectFields<T>` method is very similar to [`ProjectFromIndexFieldsInto<T>`](project-from-index-fields-into) but it works with Lucene queries.
After applying it the query results will become objects of the specified type `T`. The transformation is done server side and the projected fields are retrievied directly from stored index fields, what influences positively time of the query execution.
It means that an index definition should indicate what filed have to be stored inside Lucene index, for example:

{CODE index_def@ClientApi\Querying\ResultsTransformation\SelectFields.cs /}

Now you can take advantage of those field when querying the index:

{CODE select_fields_1@ClientApi\Querying\ResultsTransformation\SelectFields.cs /}

The classes `Product` and `ProductViewModel` look as following:

{CODE product_class@ClientApi\Querying\ResultsTransformation\Product.cs /}
{CODE product_view_model_class@ClientApi\Querying\ResultsTransformation\ProductViewModel.cs /}

The default behavior of `SelectFields<T>` method is the same like of `ProjectFromIndexFieldsInto<T>`, so it means that the projection is performed from index stored values if they are available (note the usage of `Stores.Add` in the index definition),
otherwise the entire document is pulled and appropriate properties are used for the transformation. It works differently if you specify exactly what fields you want to fetch directly from index, e.g.:

{CODE select_fields_2@ClientApi\Querying\ResultsTransformation\SelectFields.cs /}

In case above only *Name* property will be retrieved and the resulting objects will consist of projected *Name* value while *Description* will be empty.



