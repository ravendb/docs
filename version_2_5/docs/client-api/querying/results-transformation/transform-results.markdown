##### TransformResults

If you want to do a client side results transformation in most cases it will be enough to you use standard Linq `Select` method. 
However if your intention is to create a bit more complex projection then `TransformResults` customization method might be useful.
An advantage of this approach is that you have an access to the executed index query and the collection of all returned results.

In order to use this method you have to `Customize` your query as is in the example presented below.

###### Example

Let's assume that we have the following classes:

{CODE product_item_class@ClientApi\Querying\ResultsTransformation\TransformResults.cs /}
{CODE product_item_view_model@ClientApi\Querying\ResultsTransformation\TransformResults.cs /}
{CODE warehouse_class@ClientApi\Querying\ResultsTransformation\TransformResults.cs /}

and an index defined as follow:

{CODE index_def@ClientApi\Querying\ResultsTransformation\TransformResults.cs /}

To transform the query results that are `Products` into `Warehouses` you can use the code:

{CODE transform_to_warehouses@ClientApi\Querying\ResultsTransformation\TransformResults.cs /}