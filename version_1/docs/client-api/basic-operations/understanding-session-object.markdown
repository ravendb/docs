﻿# Understanding the Session object

After creating a RavenDB document store, we are ready to use the database server instance it is pointing at. For any operation we want to perform on the DB, we start by obtaining a new Session object from the document store. The Session object will contain everything needed to perform any operation necessary:

{CODE session_usage_1@Intro\BasicOperations.cs /}

The Client API, and using the Session object in particular, is very straightforward. Open the session, do some operations, and finally apply the changes to the RavenDB server. The usage of the second session is similar: open the session, get a document from the server and do something with it.

## Unit of Work

The Client API implements the Unit of Work pattern. That has several implications:

* In the context of a single session, a single document (identified by its key) always resolves to the same instance. `Assert.Same(session.Load<Company>(companyId),  session.Load<Company>(companyId));`

* The session manages change tracking for all the entities that it has either loaded or stored.

{CODE session_usage_2@Intro\BasicOperations.cs /}

## Batching

One of the most expensive operations in an application is making remote calls. The RavenDB Client API optimizes this for you by batching all write calls to the RavenDB server into a single call. This is the default behavior whenever using the Session object, so you don't have to do anything to enable it. This also ensures that writes to the database are always executed in a single transaction, no matter how many operations you are actually executing.

## Safe by default

By default, RavenDB will not allow operations that might compromise the stability of either the server or the client. There are mainly two examples that present themselves most often - sending too many requests, or receiving too large a response. Therefore, a RavenDB session automatically enforces the following limitations:

* If a page size value is not specified, the length of the results will be limited to 128 results. At the server side as well, there is a hard limit to the page size of 1,024 results (configurable).
* The number of remote calls to the server per session is limited to 30 (configurable).

The first one is obvious - unbounded result sets are dangerous, and have been the cause of many failures in RDBMS based systems - unless a result-size has been specified, RavenDB will automatically limit the size of the returned result set.

The second example is less immediate, and should never be reached if RavenDB is being utilized correctly - remote calls are expensive, and the number of remote calls per "session" should be as close to "1" as possible. If the limit is reached, it is a sure sign of either a Select N+1 problem or other mis-use of the RavenDB session.