
### Intersection

To allow users to `Intersect` queries on the server-side and return only documents that match **all** the provided sub-queries we have introduced the query intersection feature.

Lets consider a case, where we have a T-Shirt class:

{CODE intersection_1@ClientApi\Querying\Intersection.cs /}

Index:

{CODE intersection_2@ClientApi\Querying\Intersection.cs /}

Few records:

{CODE intersection_3@ClientApi\Querying\Intersection.cs /}

Now we want to return all the T-shirts that are manufactured by `Raven` and contain both `Small` `Blue` and `Large` `Gray` types. To do this, we just need to use `Intersect` query extension from `Raven.Client` namespace

{CODE intersection_4@ClientApi\Querying\Intersection.cs /}

or its equivalent in Lucene syntax

{CODE intersection_5@ClientApi\Querying\Intersection.cs /}

Above query will return `tshirts/1` and `tshirts/4` as a result. Document `tshirts/2` will not be included, because it is not manufactured by `Raven` and `tshirts/3` is not available in `Small` `Blue` so it does not match **all** the sub-queries.
