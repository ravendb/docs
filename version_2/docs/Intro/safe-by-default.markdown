#Safe by default

One of the key design principles that guided the development of RavenDB is that it should be *safe by default*. RavenDB will let you shoot yourself in the foot, but only after you make it absolutely clear that this is what you actually want to do.

This design was influenced by the [Release It!](http://pragprog.com/book/mnee/release-it) book (a highly recommended read!) and years of looking at dozens and hundreds of code bases and production failures. 

## Unbounded result sets

This is probably the most common error that people make with any sort of data access technology. "Just give me the whole thing, I'll make sense of it on the client side." In the RDBMS world, that might be expressed as "SELECT * FROM Orders".

The problem with such queries is they can potentially return an unlimited number of records, and often enough they will return large number of rows. This leads to applications that are slow, consume more bandwidth, use more memory, and eventually become unresponsive.

RavenDB deals with this problem by setting limits on the number of documents that you can get from the server. Those limits are placed at several points:

* On the client side, if you fail to specify the page size (by using the `Take()` method), it will use a page size of _128_ by default.
* On the server side, page sizes that are larger than _1,024_ will be reduced to _1,024_.

While you can override the server-side using the `Raven/MaxPageSize` config, changing this value is not recommended. In most scenarios there are better ways to solve the problem triggering this change, than just changing it.

As a rule of thumb, whenever you are trying to get more than 128 documents in one go, definitely more than 1,024, you are doing something wrong.

## Unbounded number of requests

Another common issue with all data access technologies is being overly chatty with the database. A common scenario might be:

{CODE safe_by_default_1@Intro\SafeByDefault.cs /}

This code will execute a single query to load the recent posts, and then an additional query for each post. The problem with this approach is that it leads to slow applications, increase latency and overall strain on the system. You may already know it by it's name: "SELECT N+1".

RavenDB comes with several options for reducing the number of requests that you have to make to the server:

* All writes to the server are automatically batched and require only one single request.
* You can load multiple documents in a single call using a Load(string[] documentKeys) overload.
* On heavy read situations, you can group several query and load operations using Lazy requests.

Most of the time, however, this problem occurs because people just don't notice this during development (since making requests to a local server has very little cost). In order to expose this problem early on, and to avoid what is essentially a DoS attack against the server, RavenDB limits the number of requests that each session is allowed to make.

By default, that limit is set to 30. On request #31, the session will throw an exception with detailed information about the quota violation.

You can override this behavior by setting a different limit:

* Per session, by setting: IDocumentSession.Advanced.MaxNumberOfRequestsPerSession
* Globally, by setting: DocumentConvention.MaxNumberOfRequestsPerSession
