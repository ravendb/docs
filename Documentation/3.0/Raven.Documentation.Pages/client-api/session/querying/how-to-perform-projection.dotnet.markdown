# Session: Querying: How to perform projection?

There are a couple types of projections:

- using [Select](../../../client-api/session/querying/how-to-perform-projection#select)
- using [ProjectFromIndexFieldsInto](../../../client-api/session/querying/how-to-perform-projection#projectfromindexfieldsinto)
- using [OfType (As)](../../../client-api/session/querying/how-to-perform-projection#oftype-(as)---simple-projection)
- using transformer [TransformWith](../../../client-api/session/querying/how-to-use-transformers-in-queries)

{PANEL:Select}

The most common projection is done by using LINQ `Select` method.

### Example I

{CODE projection_1@ClientApi\Session\Querying\HowToPerformProjection.cs /}

### Example II

{CODE projection_2@ClientApi\Session\Querying\HowToPerformProjection.cs /}

{PANEL/}

{PANEL:TransformWith}

Detailed article about using transformers with queries can be found [here](../../../client-api/session/querying/how-to-use-transformers-in-queries).

{PANEL/}

{PANEL:ProjectFromIndexFieldsInto}

This extension method uses reflection to extract all public fields and properties to fetch and perform projection to the requested type.

### Example

{CODE projection_3@ClientApi\Session\Querying\HowToPerformProjection.cs /}

{PANEL/}

{PANEL:OfType (As) - simple projection}

`OfType` or `As` is a client-side projection. The easiest explanation of how it works is: take results that server returned and map them to given type. This may become useful when querying index that contains fields that are not available in mapped type.

### Example

{CODE projection_4@ClientApi\Session\Querying\HowToPerformProjection.cs /}

{CODE projection_5@ClientApi\Session\Querying\HowToPerformProjection.cs /}

{PANEL/}

## Remarks

{NOTE:Note}
Projections request from server an array of fields to download, if index contains those fields (stores them) they will come directly from index, if not values from document will be used. You can read more about storing fields [here](../../../indexes/storing-data-in-index).

`Raven/ImplicitFetchFieldsFromDocumentMode` setting can be altered to change the behavior of field fetching. By default it allows fetching fields from document if index is missing them (they are not stored), but this can be changed to skipping those fields or even throwing an exception. Read more about this configuration option [here](../../../server/configuration/configuration-options#index-settings).
{NOTE/}

{NOTE Projected entities (even named types) are not being tracked by session. /}

## Related articles

- [Indexes : Querying : Projections](../../../indexes/querying/projections)
