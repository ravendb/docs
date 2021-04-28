<h1>NoSQL Database Scalability</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/how-a-database-delivers-performance-under-heavy-load-ravendb.jpg" alt="How a Database Delivers Performance Under Heavy Load" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size: 24px;">As applications evolve to meet the new load demands, the need for database scalability is at new highs.</p>

The pace of digital transformation went into overdrive in 2020 as we were all forced into our homes *and onto our computers, devices, and smartphones*. As a result, traffic for all applications went up exponentially.

Almost all applications covering every major industry saw a significant increase in data load, information queries, and documents to store.

Most of the improvisations we made in 2020 will stick with us in one form or another. Millions of new startups and small businesses will emerge in the coming years, many of them expanding into full-fledged new enterprises.

Applications enter this brave new world poised to get a lot busier.

As applications evolve to meet the new load demands, databases are falling behind. In 2020, over 20% of databases slowed down their applications *more than six times* due to unexpected load. Demand for replacing databases with versions that can expand on-demand is at new highs.

### Scalability is the New Performance

To keep performance robust, a database must be able to [scale on demand](https://ravendb.net/articles/cost-benefits-ravendb-nosql-acid-database). It would be best if you had the trigger to expand your database layer immediately, with no friction, whenever you need it.

The best options are [distributed databases](https://ravendb.net/docs/article-page/4.2/Csharp/server/clustering/distribution/distributed-database). Solutions like RavenDB were built from scratch to be distributed alongside the ultimate platforms for distributed applications: The Cloud.

There are two levels to keeping availability and performance steady even as your load is constantly rising:

**1. Master-Master Replication.** A database can have multiple nodes, all replicating to one another, giving you numerous databases at several locations. This allows users to get their information from the node with the shortest latency and keeps you in business even if a node, or two, go offline.

But if one node runs the show and the others rely on that node to operate, like in a Master-Subordinate situation, there are severe limits to how distributed your database is. If the central node goes offline, you won't be able to write new data to your system. Updates by your users will have to wait until the primary node is online, causing bottlenecks, freezes, and delays.

A Master-Master replication is where any node can read and write data at all times. It can work independently of the other nodes. It can even work offline. Suppose a node goes offline, or the network that node is running on is disconnected, like a local hospital running on generators. In that case, your database will **still work**, taking in data and presenting it to your user base. Once everything is restored, the nodes will update one another.

**2. Fast Setup and Recovery.** When traffic expands, you need a new node right away. You also need to replicate all of your data to that new node before it can go live and put an additional service point in your application.

If a node is disabled, you need the same: A new node up and running *and* updated ASAP.

RavenDB was able to take a dataset of 1.35 billion documents with a size of 985 GB and load it onto a node in hours, getting you [set up the same day you need more](https://ravendb.net/docs/article-page/5.0/Csharp/start/installation/setup-wizard) capacity. Other solutions took almost a week to get started.

Unprecedented levels of traffic mean conducting data queries under a massive load on your application while maintaining top performance. The database of tomorrow meets these challenges today.