# Revisions and Other Features

When the revisions feature is enabled, a snapshot of a document that holds *Counters* will include in its metadata a `@counters-snapshot` property, 
that holds all the document's counter names **and values** (at the time of the revision creation).  
This can be useful when historic records of the documents and their counter values are needed.

{INFO: How do counter operations affect revision creation }
Creation of a new counter and deletion of a counter modify the parent document (documents hold
their counter names in the metadata) and will trigger a revision creation.  
Incrementing an existing counter does not modify the parent document and will not trigger a revision creation.
{INFO/}

### Example

{CODE:java example@DocumentExtensions\Revisions\ClientAPI\Session\CounterRevisions.java /}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../document-extensions/revisions/overview)  
* [Revert Revisions](../../document-extensions/revisions/revert-revisions)  
* [Counters: Overview](../../document-extensions/counters/overview)
* [Time Series: Overview](../../document-extensions/timeseries/overview)
* [Attachments: What are Attachments](../../document-extensions/attachments/what-are-attachments)

### Client API
* [Revisions: API Overview](../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  

### Studio
* [Settings: Document Revisions](../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../studio/database/document-extensions/revisions)  

### Data Subscriptions
* [What Are Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)  
* [Revisions and Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
