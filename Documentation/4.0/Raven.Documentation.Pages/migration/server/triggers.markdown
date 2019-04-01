# Migration: Triggers

The server side triggers were often the source of problems in RavenDB 3.x, especially related to performance. In RavenDB 4.0 the notion of triggers has been removed completely.

Some of the capabilities can be achieved using [client side events](../../client-api/session/how-to/subscribe-to-events). The simpliest example would be to can use
`OnBeforeStore` / `OnBeforeDelete` to prevent from creating or deleting some documents.
