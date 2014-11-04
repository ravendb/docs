# Intersection

To allow users to `Intersect` queries on the server-side and return only documents that match **all** the provided sub-queries we have introduced the query intersection feature.

Lets consider a case, where we have a T-Shirt class:

{CODE intersection_1@Indexes\Querying\Intersection.cs /}

And we fill our database with few records:

{CODE intersection_3@Indexes\Querying\Intersection.cs /}

Now we want to return all the T-shirts that are manufactured by `Raven` and contain both `Small Blue` and `Large Gray` types.

To do this, we need to do the following:

- add `Raven.Client` namespace to usings
- use `Intersect` query extension:

{CODE-TABS}
{CODE-TAB:csharp:Query intersection_4@Indexes\Querying\Intersection.cs /}
{CODE-TAB:csharp:DocumentQuery intersection_5@Indexes\Querying\Intersection.cs /}
{CODE-TAB:csharp:Commands intersection_6@Indexes\Querying\Intersection.cs /}
{CODE-TAB:csharp:Index intersection_2@Indexes\Querying\Intersection.cs /}
{CODE-TABS/}

Above query will return `tshirts/1` and `tshirts/4` as a result. Document `tshirts/2` will not be included, because it is not manufactured by `Raven` and `tshirts/3` is not available in `Small Blue` so it does not match **all** the sub-queries.

## Related articles

- [Client API : Session : How to use intersect?](../../client-api/session/querying/how-to-use-intersect)