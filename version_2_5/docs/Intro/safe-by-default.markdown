
## Safe by default

One of the key design principles guiding the development of RavenDB is that it should be *safe by default*. RavenDB will let you shoot yourself in the foot, but only after you have made it absolutely clear that this is what you actually want to do.

This design was influenced by the [Release It!](http://pragprog.com/book/mnee/release-it) book (a highly recommended read!) and by years of looking at hundreds of code bases and production failures.

We will cover some behaviors in other systems that RavenDB addresses using defaults

### Unbounded result sets

This is probably the most common error that people make with any sort of data access technology. "Just give me the whole thing, I'll make sense of it on the client side." In the RDBMS world, this would be expressed as "SELECT * FROM Orders".

The problem with such queries is they can potentially return an unlimited number of records, and often enough they will return large number of rows. This leads to applications that are slow, consume more bandwidth, use more memory, and eventually become unresponsive.

RavenDB deals with this problem by setting limits on the number of documents that you can get from the server. Those limits are placed at several points:

* On the client side, if you fail to specify the page size (by using the `Take()` method), it will use a page size of _128_ by default.
* On the server side, page sizes that are larger than _1,024_ will be reduced to _1,024_.

While you can override the server-side using the `Raven/MaxPageSize` configuration setting, changing this value is not recommended. In most scenarios there are better ways to solve the problem triggering this change, rather than just changing this global default value.

As a rule of thumb, whenever you are trying to get more than 128 documents in one go, definitely more than 1,024, you are doing something wrong.

### Unbounded number of requests

Another common issue with all data access technologies is being overly chatty with the database. A common scenario might be:

	var posts = postsRepository.GetRecentPosts();
	foreach (var post in posts)
	{
		var comments = commentsRepository.GetRecentCommentsForPost(post);
		recentComments.AddRange(comments);
	}

This code will begin by executing a single query to load the recent posts (line 1), and then an additional query for *each* post  (line 3). The problem with this approach is that it leads to slow applications, increased latency and an overall strain on the system. You may already know it by its name: "SELECT N+1".

RavenDB comes with several options for reducing the number of requests that you have to make to the server:

* All writes to the server are automatically batched and require only one single request.
* You can load multiple documents in a single call using a Load(string[] documentKeys) overload.
* On heavy read situations, you can group several query and load operations using Lazy requests.

Most of the time, however, this problem occurs because people just don't notice this during development, since making requests to a local server has very little cost.

In order to expose this problem early on, and to avoid what is essentially a Denial of Service attack against the server, RavenDB limits the number of requests that each client session is allowed to make.

By default, that limit is set to 30. On request #31, the session will throw an exception with detailed information about the quota violation.

You can override this behavior by setting a different limit:

* Per client session by setting: IDocumentSession.Advanced.MaxNumberOfRequestsPerSession
* Globally, by setting: DocumentConvention.MaxNumberOfRequestsPerSession