# RavenDB vs MongoDB: Performance, Cost, and Complexity<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![RavenDB vs MongoDB: Performance, Cost, and Complexity](images/ravendb-vs-mongodb-performance-cost-and-complexity.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="text-center">
<button id="podcast-play-button" class="play-button" style=""><i class="icon-play" style="margin-right:20px"></i>Play Podcast</button>
<a href="https://s3-us-west-2.amazonaws.com/static.ravendb.net/RavenDB+vs+MongoDB+Voiceover.mp3" download><button id="download-podcast-button" class="download-podcast-button"><i class="icon-download" style="margin-right:20px"></i>Download .mp3</button></a>
</p>

<audio id="podcast-audio" controls="" style="width: 100%">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/RavenDB+vs+MongoDB+Voiceover.mp3" type="audio/ogg">
  <source src="https://s3-us-west-2.amazonaws.com/static.ravendb.net/RavenDB+vs+MongoDB+Voiceover.mp3" type="audio/mpeg">
  Your browser does not support the audio element.
</audio>

<br/>

We get this question every day: How does RavenDB compare to MongoDB?
Everybody wants to know how our database stacks up against MongoDB in specific areas. We took the top areas of concern people have for their application and explain what we offer that other databases don't.
RavenDB is a [document database](https://ravendb.net/features/management-studio-gui) built for fast performance, minimal complexity, short release cycles, little to no need for support, and to meet all your data needs in one place. Our database as a service managed cloud option is designed so you only have to worry about what to do with your data while we take care of your database.

Beginning with our first version over a decade ago, we were the first document database to include ACID data integrity, to offer machine learning dynamic indexes, to model tech support as something we should pay for the more you have to use it, and include MapReduce Aggregations, Full Text Search, a top line GUI, and automatic and aggressive caching as a part of your database.

***Here is what we offer you to maximize the performance of your application while reducing both cost and complexity.***

### Database Performance
**RavenDB** was the first Document Database to offer [ACID Data Integrity](https://ravendb.net/features/acid-transactions). Over the decade since, we have been improving your performance within the framework of providing multi-document ACID Data Integrity, reaching over 1 million reads and 150k writes per second on a single node using commodity hardware.

**ACID Guarantees are** also extended to your database cluster. You have the option to enable Clusterwide transactions on a per transaction basis. RavenDB actively works to minimize the performance challenges you face by relying on an outside network.

**RavenDB is the only database to [automatically create Indexes](https://ravendb.net/features/indexes/auto-indexes) for your queries on the fly, allowing you to save you the trouble of coding them yourself.** We slash your latency and supercharge your performance by using machine learning indexes. With every query you make, RavenDB will learn more about your system and operational behavior and either use an already existing index or create a new one to perform even faster. Indexes that haven't been used for a while will be discarded to keep your memory fresh. This is a part of your database: *you don't need to buy it or install it.* Right away, your indexing will be efficient and smooth.

On the cloud your automatic index query optimizer becomes your 24/7 cost cutting accountant, finding ways to save you money with every query you make, guaranteeing the only investments you make in your cloud platform are ones that give you maximum return.

**Optimistic concurrency**. We don't use locking to manage concurrent updates to your distributed database cluster. We employ snapshot documents which [boosts your performance](https://ravendb.net/features/high-performance). RavenDB's conflict resolution mechanisms prevent duplicate documents when multiple users update the same document at the same time, preventing errors and keeping you running smoothly for multiple users.

This is vital because RavenDB is a multi-master database, enabling your users to read and write independently to any node in your database cluster. Built for distributed applications, RavenDB handles the biggest challenges you can face in such an environment.
<br/>

**RavenDB uses a lot of creative ways to give you high-performance while maintaining the highest level of data integrity. One of these methods is the transactional merger.**

Why wait for a single transaction when you can pack a lot of them on a single commit?

Think of a bus going from one point to the next. Whether it carries one passenger or 100, the amount of gas it uses is relatively the same. A transaction is a similar process.

Before moving your data from the REST endpoint to storage, RavenDB will board as many passengers as possible. It will wait for as many transactions as it can hold before committing so there are far less round-trips back and forth to the disk. This reduces latency and boosts performance. On the cloud, it saves you money with each commit. You make far fewer I/O requests, resulting in lower bills.

The "single ride" is a lot bigger, but in having multiple transactions packaged as one transaction, you don't have a convoy of single transactions all waiting on line to be written. If you can move 10 transactions in the same sequence of movements that you used to move just one, you save 90% of the work in moving the data to disk and sending back notification, while maintaining ACID guarantees.

But what happens if RavenDB is moving 10 transactions and one of them fails? What happens to the transactions that weren't yet persisted to disk?

It's what would happen if the bus carrying 10 passengers successfully dropped off five, broke down, one passenger decided to walk back, and the bus driver called up 4 taxis to take the remaining passengers home. RavenDB will unbundle the remaining transactions and commit them one by one. This is a *rare* scenario, where the cost is merely that we have to run each transaction individually, while still maintaining overall correctness.

Overall, you get massive performance benefits within the ACID framework.

**You will be happy to know that we also include automatic and aggressive caching. It's already part of your database so** you don't have to waste your developers time coding it. Making a query for data that didn't change shouldn't require your application to go to the database server and fetch the same information already in memory. With RavenDB, it doesn't.

### Database Cost

Our primary mission at RavenDB is to build a database that is easy to use, fault tolerant, self-managing and does everything on its own so you *don't need to delay your release cycle making support calls.* We want to save you both time and money at every turn. That is why our business is uniquely modeled for tech support to constitute an expense to us, and not a source of income - making it as annoying to us as it is to you. We make great efforts to design a database so you won't have to call support every time you need something. But if you do, we offer high quality dev tech support.

Our error messages are thorough, guiding you to the solutions at the moment you need them. RavenDB is self-managing, anticipating common problems and adjusting in-flight. It also has a comprehensive warning system, always sending you alerts through your RavenDB GUI to notify you that certain operational, functional, and performance issues require further monitoring.

Our API and documentation are designed to be easy to use and understand, relieving you of the burden of calling support every time you want to figure something new out.

RavenDB's query language, RQL, is the SQL of the Document Database. 80% of it is the traditional query language and the rest is human readable, quick to learn, and [simple to use with great documentation to back it up](https://ravendb.net/whyravendb/ease-of-use).

On the cloud you can set up a [RavenDB Cloud managed database](https://cloud.ravendb.net/) instance in 2 minutes. If you are using RavenDB on premises, the setup wizard will take you 10. You don't need to read long manuals for setup or security. Protocols are built into your system to make sure the most common security vulnerabilities are tackled before you go to production.

RavenDB also works great on older machines and devices with limited memory. We have experience with long term projects where the hardware becomes antiquated in the latter stages of the project, so we made sure RavenDB works well using less computing power. This is great for edge processing, where you need to process data on smaller machines, and it saves you a ton on the cloud, allowing you to provision fewer machines, and cheaper machines while enjoying the same level of output and performance.

Our burstable cloud instances make sure your database is using the most essential operations as you reach your CPU credit limit. RavenDB quickly manages background tasks by reassigning them to nodes in operation if nodes in your cluster get throttled. RavenDB will also suspend certain operations as you reach your CPU credit limit so users won't be impacted by throttling. By managing your resources wisely, even under the threat of being throttled, you can save big on your cloud costs. Clients have reported saving 15-20% using RavenDB's burstable instances.

The average savings on migrating to the cloud is 16%. RavenDB Cloud gets you off to a good start!

### Database Complexity

#### Why should you add additional layers of software to piece together a database that should have it all in one place? RavenDB comes with everything you need.

You don't need Hadoop for MapReduce or ElasticSearch for full text search, RavenDB sports native features for both. You also don't need to spend more money on installing a third-party GUI, RavenDB gives that to you as a part of the database you are paying for. RavenDB minimizes the need for third party addons so you have as simple a database architecture as possible.

**A GUI That Tells You Everything**. Your RavenDB GUI monitors the internals of your database, measuring performance for every step in your queries, aggregations, indexes, MapReduce, and more. Find performance bottlenecks easy and resolve them right away.

Most of what you can do in the RavenDB API you can do on the Management Studio GUI. Execute functions like making queries, create new databases, new nodes, and much more. You don't need a special connection for this, every local node has its own studio instance embedded in the server.

**RavenDB's "Data Includes"** is our answer to the dreaded *select N + 1* issue.

For example, let's query our database to, "get me all open help desk cases *and* their associated customers."

For other databases, if you have 30 customers, your application server will make 1 trip for all the open help desk cases, and another 30 trips for each customer. This is your select N + 1 problem: A typical database will go back and forth to the server to ask if there is one more result that meets its criteria until the database server returns 0.

With RavenDB's "Includes", instead of one object containing copies of the properties from another object, it is only necessary to hold a reference to the second object. Then, the server can be instructed to pre-load the referenced document at the same time that the root object is retrieved.

Data Includes turns these countless trips to and from the server into one big batch read that includes everything you need in a single trip. The impact on performance and [cost on the cloud are significant](https://cloud.ravendb.net/pricing).

#### Changes and Revert Revisions

With RavenDB, you load data and save it to the database. RavenDB checks all the changes and sends them all in one shot to the database as a single transactional operation. You don't have to deal with change tracking. You don't have to deal with change management. You don't have to figure out how to best write to the database.

RavenDB comes with a document history showing you every change made to every document in your database, making auditing your records a snap. We also offer you point in time revisions where you can tell RavenDB to show you how your database looked at any point in time, from 5 seconds prior, to 5 minutes, 5 days, weeks, months, and more.

Using RavenDB, you can enjoy superior performance, reduced cost in both time and money, and minimal complexity that you won't get anywhere else.

**Are you interested in setting up a Proof of Concept?**

**We offer 2 FREE hours of POC support, along with reference materials to help you command the best tools for your next project.**

Contact us at [info@ravendb.net](info@ravendb.net)

Take a free instance of RavenDB Cloud or DBaaS Managed Cloud Service at [cloud.ravendb.net](https://cloud.ravendb.net)

Or take a free RavenDB On Premise download at [ravendb.net/download](https://ravendb.net/download)

### About RavenDB

Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.

Over 1,000 businesses use RavenDB for IoT, Big Data, Microservices Architecture, fast performance, a distributed data network, and everything you need to support a modern application stack for today's user. For more information, please visit [ravendb.net](ravendb.net) or contact.
<br/>
<div class="article-color-banners">
    <a href="https://ravendb.net/learn/webinars"><img src="images/webinars.png" class="img-responsive"/></a>
    <a href="https://ravendb.net/news/use-cases"><img src="images/use-cases.png" class="img-responsive"/></a>
    <a href="https://ravendb.net/learn/docs-guide"><img src="images/documentation.png" class="img-responsive"/></a>
    <a href="https://ravendb.net/learn/bootcamp"><img src="images/bootcamp.png" class="img-responsive"/></a>
</div>