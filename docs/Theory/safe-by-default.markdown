#Safe by default
One of the key design principles that guided Raven's development is that it should be safe by default. Raven will let you shoot yourself in the foot, but only after you make it absolutely clear that this is what you actually wants. This design was influenced by the [Release It!](http://pragprog.com/book/mnee/release-it) book (which we highly recommend) and years of looking over dozens and hundreds of code bases and production failures. 

##Unbounded result sets
This is probably the most common error that people make with any sort of data access technology. "Just give me the whole thing, I'll make sense of it on the client side." In the RDBMS world, that might be expressed as "select * from orders".

The problem with such queries that they can potentially return unlimited number of records, and often enough they will return large number of rows. This leads to applications that are slow, consume more bandwidth, use more memory and eventually, unresponsive.

Raven deals with the problem by setting limits on the number of documents that you can get from the server. Those limits are placed at several points:

* On the client side, if you fail to specify the page size (by using the Take() method), it will use a page size of 128 by default.
* On the server side, page sizes that are larger than 1,024 will be reduced to 1,024.
* * You can override that setting using Raven/MaxPageSize app settings, but changing this value is not recommended.

##Unbounded number of requests
Another common issue mistake in all data access technologies is being overly chatty with the database. A common scenario might be:

    var posts = postsRepository.GetRecentPosts();
    foreach (var post in posts)
    {
       var comments = commentsRepository.GetRecentCommentsForPost(post);
       recentComments.AddRange(comments);
    }

This code will execute a single query to load the recent posts, and then an additional query per each post. The problem with this approach is that it leads to slow applications, increase latency and overall strain on the system.

Raven comes with several options for reducing the number of requests that you have to make to the server:

* All writes to the server are batched and requires only a single request.
* You can load multiple documents in a single call using a Load(string[] documentKeys) overload.

Most of the time, however, this problem occurs because people just don't notice this during development (since making requests to a local server has very little cost). In order to expose this problem early on, and to avoid what is essentially a DoS attack against the server, Raven limits the number of requests that each session is allowed to make.

By default, that limit is set to 30. On request #31, the session will throw an exception with detailed information about the quota violation.

You can override this behavior by setting a different limit:

* Globally, by setting: DocumentConvention.Conventions.MaxNumberOfRequestsPerSession
* Per session, by setting: IDocumentSession.Advanced.MaxNumberOfRequestsPerSession
