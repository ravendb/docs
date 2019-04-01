# Session: What is a session and how does it work?

After creating a RavenDB document store, we are ready to use the database server instance it is pointing at. For any operation we want to perform on the DB, we start by obtaining a new Session object from the document store. The Session object will contain everything needed to perform any operation necessary:

{CODE:python session_usage_1@ClientApi\Session\WhatIsSession.py /}

The Client API, and using the Session object in particular, is very straightforward. Open the session, do some operations, and finally apply the changes to the RavenDB server. The usage of the second session is similar: open the session, get a document from the server and do something with it.

## Unit of Work

The Client API implements the Unit of Work pattern. That has several implications:

* In the context of a single session, a single document (identified by its key) always resolves to the same instance.

{CODE:python session_usage_3@ClientApi\Session\WhatIsSession.py /}

* The session manages change tracking for all the entities that it has either loaded or stored.

{CODE:python session_usage_2@ClientApi\Session\WhatIsSession.py /}

## Batching

One of the most expensive operations in an application is making remote calls. The RavenDB Client API optimizes this for you by batching all write calls to the RavenDB server into a single call. This is the default behavior whenever using the Session object, so you don't have to do anything to enable it. This also ensures that writes to the database are always executed in a single transaction, no matter how many operations you are actually executing.

## Remarks

A very common problem with all ORMs and ORM-like APIs is the Select N+1 problem. This is relevant to any database API which is designed to work like an ORM, RavenDB Client API included.
How RavenDB API attempts to mitigate this is not immediate, and should never be reached if RavenDB is being utilized correctly. Remote calls are expensive and the number of remote calls per "session" should be as close to "1" as possible. If the limit is reached, it is a sure sign of either a Select N+1 problem or other misuse of the RavenDB session.

{NOTE: Configuring the maximum requests in a session} 
By default, the maximum requests count in the session is 30.
This can be changed by the DocumentConventions::MaxNumberOfRequestsPerSession property.
{NOTE/}

## Related articles

- [Opening a session](./opening-a-session)  
