# Session: What is a Session and How Does it Work

After creating a RavenDB document store, we are ready to use the database server instance it is pointing at. For any operation we want to perform on RavenDB, we start by obtaining a new Session object from the document store. The Session object will contain everything we need to perform any operation necessary.

{CODE:java session_usage_1@ClientApi\Session\WhatIsSession.java /}

The Client API, and using the Session object in particular, is very straightforward. Open the session, do some operations, and apply the changes to the RavenDB server. The usage of the second session is similar: open the session, get a document from the server, and do something with it.

{NOTE: Storing Entities} 
You can read more about storing data with the session [here](./storing-entities).
{NOTE/}


## Unit of Work

The Client API implements the Unit of Work pattern. That has several implications:

* In the context of a single session, a single document (identified by its ID) always resolves to the same instance.

{CODE:java session_usage_3@ClientApi\Session\WhatIsSession.java /}

* The session manages change tracking for all the entities that it has either loaded or stored.

{CODE:java session_usage_2@ClientApi\Session\WhatIsSession.java /}

{INFO:How to Disable Entities Tacking}

Entities tracking can be disabled using the `SessionOptions.NoTracking` property when session is being [opened](../../client-api/session/opening-a-session#example-ii---disabling-entities-tracking).

{INFO/}

## Batching

One of the most expensive operations in an application is making remote calls. The RavenDB Client API optimizes this for you by batching all write calls to the RavenDB server into a single call. This is the default behavior whenever using the Session object, you don't have to do anything to enable it. This also ensures that writes to the database are always executed in a single transaction, no matter how many operations you are actually executing.

## Remarks

A very common problem with all ORMs and ORM-like APIs is the Select N+1 problem. This is relevant to any database API which is designed to work like an ORM, RavenDB Client API included.
How RavenDB API attempts to mitigate this is not immediate, and should never be reached if RavenDB is being utilized correctly. Remote calls are expensive and the number of remote calls per "session" should be as close to "1" as possible. If the limit is reached, it is a sure sign of either a Select N+1 problem or other misuse of the RavenDB session.

{NOTE: Configuring the maximum requests in a session} 
By default, the maximum requests count in the session is 30.
This can be changed by the DocumentConventions::maxNumberOfRequestsPerSession property.
{NOTE/}

## Related Articles

### Session

- [Opening a Session](../../client-api/session/opening-a-session)
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
