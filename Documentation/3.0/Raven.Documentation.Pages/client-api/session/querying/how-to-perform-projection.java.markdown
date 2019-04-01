# Session: Querying: How to perform projection?

There are a couple types of projections:

- using [Select](../../../client-api/session/querying/how-to-perform-projection#select)
- using transformer [TransformWith](../../../client-api/session/querying/how-to-use-transformers-in-queries)

{PANEL:Select}

The most common projection is done by using `select` method.

### Example I

{CODE:java projection_1@ClientApi\Session\Querying\HowToPerformProjection.java /}

### Example II

{CODE:java projection_2@ClientApi\Session\Querying\HowToPerformProjection.java /}

{PANEL/}

{PANEL:TransformWith}

Detailed article about using transformers with queries can be found [here](../../../client-api/session/querying/how-to-use-transformers-in-queries).

{PANEL/}

## Remarks

{NOTE:Note}
Projections request from server an array of fields to download, if index contains those fields (stores them) they will come directly from index, if not values from document will be used. You can read more about storing fields [here](../../../indexes/storing-data-in-index).

`Raven/ImplicitFetchFieldsFromDocumentMode` setting can be altered to change the behavior of field fetching. By default it allows fetching fields from document if index is missing them (they are not stored), but this can be changed to skipping those fields or even throwing an exception. Read more about this configuration option [here](../../../server/configuration/configuration-options#index-settings).
{NOTE/}


{NOTE Projected entities (even named types) are not being tracked by session. /}

## Related articles

- [Indexes : Querying : Projections](../../../indexes/querying/projections)
