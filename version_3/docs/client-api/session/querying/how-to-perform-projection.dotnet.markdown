# Querying : How to perform projection?

There are a couple types of projections:

- using [Select](../../../client-api/session/querying/how-to-perform-projection#select---simple-projection)
- using transformer [TransformWith](../../../client-api/session/querying/how-to-use-transformers-in-queries)
- using [ProjectFromIndexFieldsInto]()

## Select - simple projection

The most basic projection is done by using LINQ `Select` method.

### Example I

{CODE projection_1@ClientApi\Session\Querying\HowToPerformProjection.cs /}

### Example II

{CODE projection_2@ClientApi\Session\Querying\HowToPerformProjection.cs /}

## TransformWith

Detailed article about using transformers with queries can be found [here](../../../client-api/session/querying/how-to-use-transformers-in-queries).

## ProjectFromIndexFieldsInto

This extension method uses reflection to extract all public fields and properties to fetch and perform projection to the requested type.

### Example

{CODE projection_3@ClientApi\Session\Querying\HowToPerformProjection.cs /}

### Remarks

{NOTE Projections request from server an array of fields to download, if index contains those fields (stores them) they will come directly from index, if not values from document will be used. /}

{NOTE Projected entities (even named types) are not being tracked by session. /}

#### Related articles

TODO