# Counter Revisions

When the revisions feature is enabled, a snapshot of a document that holds *Counters* will include in its metadata a `@counters-snapshot` property, 
that holds all the document's counter names **and values** (at the time of the revision creation).  
This can be useful when historic records of the documents and their counter values are needed.

{INFO: How do counter operations affect revision creation }
Creation of a new counter and deletion of a counter modify the parent document (documents hold
their counter names in the metadata) and will trigger a revision creation.  
Incrementing an existing counter does not modify the parent document and will not trigger a revision creation.
{INFO/}

### Example

{CODE:java example@ClientApi\Session\Revisions\CounterRevisions.java /}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/loading)
- [Loading](../../../client-api/session/revisions/loading)

### Counters

- [Counter Batch Operation](../../../client-api/operations/counters/counter-batch)
- [Get Counters Operation](../../../client-api/operations/counters/get-counters)
