# Session: Querying: How to use intersect?

To return only documents that match **all** provided sub-queries we have introduced `intersect` extension that enables us to do server-side intersection queries.

## Syntax

{CODE:java intersect_1@ClientApi\Session\Querying\HowToUseIntersect.java /}

| Return Value | |
| ------------- | ----- |
| IRavenQueryable | Instance implementing IRavenQueryable interface containing additional query methods. |

## Example

{CODE:java intersect_2@ClientApi\Session\Querying\HowToUseIntersect.java /}

## Related articles

- [Indexes : Querying : Intersection](../../../indexes/querying/intersection)
