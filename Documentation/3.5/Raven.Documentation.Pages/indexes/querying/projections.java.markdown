# Projections

There are couple a couple of ways to perform projections in RavenDB:

- simple projections using [Select](../../indexes/querying/projections#select---basic-projections)
- using transformer with [TransformWith](../../indexes/querying/projections#transformwith)

## Select - basic projections

This method uses reflection to extract all public fields and properties to fetch and perform projection to the requested type.

{CODE-TABS}
{CODE-TAB:csharp:Query projections_1_0@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_1_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Commands projections_1_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index projections_1_3@Indexes\Querying\Projections.cs /}
{CODE-TABS/}

This will issue a query to a database, requesting only `FirstName` and `LastName` from all documents that index entries match query predicate from `Employees/ByFirstAndLastName` index. What does it mean? It means that, if index entry matches our query predicate, then we will try to extract all requested fields from that particular entry and if all requested fields are available in there, then we do not download it from storage. Index `Employees/ByFirstAndLastName` used in above query is not storing any fields so documents will be fetched from storage.

{INFO:Projections and Stored fields}
If projection function only requires fields that are stored, then document will not be loaded from storage and all data will come from index directly. This can increase query performance (by the cost of disk space used) in many situations when whole document is not needed. You can read more about field storing [here](../../indexes/storing-data-in-index).

`Raven/ImplicitFetchFieldsFromDocumentMode` setting can be altered to change the behavior of field fetching. By default it allows fetching fields from document if index is missing them (they are not stored), but this can be changed to skipping those fields or even throwing an exception. Read more about this configuration option [here](../../server/configuration/configuration-options#index-settings).
{INFO/}

So following above rule, if we create index that stores `FirstName` and `LastName` and request only those fields in query, then **data will come from index directly**.

{CODE-TABS}
{CODE-TAB:csharp:Query projections_2_0@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:DocumentQuery projections_2_1@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Commands projections_2_2@Indexes\Querying\Projections.cs /}
{CODE-TAB:csharp:Index projections_2_3@Indexes\Querying\Projections.cs /}
{CODE-TABS/}


## TransformWith

Detailed article about transformer basics can be found [here](../../transformers/what-are-transformers).

## Related articles

- [Client API : Session : How to perform projection?](../../client-api/session/querying/how-to-perform-projection)
- [Transformers : What are transformers?](../../transformers/what-are-transformers)
