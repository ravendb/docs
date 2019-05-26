# Comparison: Cloud Database vs RavenDB NoSQL Document Database<br/><small>by <a href="mailto:oren@hibernatingrhinos.com">Oren Eini</a></small>

![The right NoSQL Document Database can make several server requests register as one, lowering your cloud costs tremendously.](images/comparison-cloud-database-vs-ravendb-nosql-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

## Comparison between a Cloud Database and RavenDB NoSQL Document Database

*RavenDB, the industry’s best NoSQL Document Database, shines in a comparison with a cloud database. We offer better Replication, Performance, and portability.*<br/>

At RavenDB, we do our best to anticipate developers’ needs. We prefer it when you get everything you need without asking for it, as if you entered a restaurant and found your entrée all set and your favorite wine ready to be poured. <br/>

It was an illuminating experience for us then, to receive an email from a new user sharing his recent experience with a Cloud NoSQL Database offered by one of the main cloud providers.
We were happy for the opportunity to hear him out, be thankful for our choices, and offer him the best NoSQL solution for use on-prem, in the cloud, and cross-platform. 
Let’s go through the main issues he raised, one by one.<br/>

## Whether Cloud Database or Portable, The Best NoSQL Database Has Certain Standards

### *Issue #1: This cloud database only works on its provider’s cloud.*

This is a common problem with databases offered by cloud providers. They are as movable as a chair bolted to the floor, providing too little portability for your needs. Services given by other cloud providers tend not to work smoothly when they approach your cloud, increasing complexity when you try to piece various components together - especially with cross-platform digital topologies. <br/>

Different departments in your company may use different cloud providers, or your company may acquire another company using the services of a different cloud platform. Creating applications that need to navigate both systems well, requires a database well-compatible with both clouds. <br/>

Put RavenDB on any cloud, and it will work well. <br/>

Put RavenDB on a local development environment, and it will work perfectly there too. This is extremely helpful in development environments, where approaching local instances rather than remote cloud instances simplifies common tasks, testing and continuous integration, and makes development significantly easier.<br/>

### *Issue #2: Performance was as unpredictable as cost. They had a transfer rate of only 2 MBps between two instances that serve 100,000 Resource Units, which is over 12 hours for a 100 GB database.*

RavenDB does extremely well in unifying and minimizing trips to the server, boosting its performance to a staggering rate. In comparison with the sad 12-hours-long transfer we’ve been consulted about, RavenDB can replicate a 52 GB database in 5 minutes, less than the time it takes to read the front-page news.<br/>

We can execute 1 million reads and 150,000 writes per second on simple commodity hardware, making RavenDB one of the fastest NoSQL document databases available. 

### *Issue #3: Cost is unpredictable. 200 MB of data can cost $375 to store and manage.*

Minimizing trips to the server for each request not only boosts performance, but also lowers cloud costs and makes them more predictable, helping to keep them within an acceptable range. <br/>

Our data profiler tells you where your queries and indexes cost performance, enabling you to open up bottlenecks and helping you improve your code to further reduce cloud costs.<br/>

Whether you run RavenDB locally or on the cloud, you aren’t nickeled and dimed at each turn but get this vital state-of-the-art property, in an outstanding speed and at a predictable price.

<a href="https://ravendb.net/whitepapers/ravendb-healthcare-database-schedule-appointments-detect-fraud-security-phi" target="_blank"><img class="img-responsive" style="margin:auto" alt="Preventing Headaches: RavenDB in the Service of Healthcare" src="images/rdb-healthcare-whitepaper.jpg" /></a>

### *Issue #4: There is no procedure for disaster recovery backup. If you have accidentally deleted or corrupted your data, you have 8 hours to fix it, or else.*

If you ever make a mistake, you’d probably want to inspect a copy of your database that precedes this mistake. And if your database is lost for whatever reason, you need to have a copy of it ready at a moment’s notice.<br/>
RavenDB gives you high availability with fast replications and reliable backups.<br/>
You can create tasks that regularly save your databases to files or replicate them to other nodes or servers, save backups as files or snapshots, and encrypt them if you like.<br/>

If you find out that your data hasn’t been properly processed for the passing 2 months, you may wish to return to prior revisions of your database and get a clear view of data development over time.<br/>
With RavenDB’s Revisions feature you can keep copies of your documents whenever they are modified, and revert them if you like. The powerful Revert Revisions feature allows you to swiftly return ALL documents that have revisions, to their state in a particular point in time.

## Tech Support and Developer Communities for a Cloud Database and a Portable NoSQL Document Database

### *Issue #5: The cloud database has a virtually zero developer community support, and poor documentation. Most “online support” comes from outside affiliates interested in selling something.*

Problem solving shouldn’t be complex. It should reduce complexity. Easy to use documentation, a vibrant developer community and readily-available learning resources are vital for a top NoSQL Database that keeps your application up and running at all times and is issue-free. <br/>

If you need help, you can turn immediately to the following resources:<br/>
    - [RavenDB’s easy to use (and understand) documentation](https://ravendb.net/learn/docs-guide) <br/>
    - [The RavenDB Boot Camp](https://ravendb.net/learn/bootcamp)<br/>
    - [The RavenDB Developer Community Forum](https://groups.google.com/forum/#!forum/ravendb)<br/>
    - [Inside RavenDB](https://github.com/ravendb/book/releases) - Oren Eini’s book about RavenDB, broken down into easy-to-read sections with code snippets and explanations. <br/>
    - [Pluralsight Getting-Started course](https://www.pluralsight.com/courses/ravendb-4-getting-started)<br/>

These resources are always available for you and are free of charge.

### *Issue #6: Paging is so hard! You can’t just open the third page of your results, you have to actually go through the first two pages to get there.*

Where the cloud database uses a <em>continuation-token</em> and forces you to meticulously go through all result pages up to the one you’re actually interested in, RavenDB simply lets you get there. 
You can paginate query results (or not) as you wish, leaf and skip through them, and effortlessly get where you want.<br/>

This is not a secluded incident, but yet another reflection of a basic difference in approaches. RavenDB is developed by people who need working features, and honestly produce them for themselves and others. We strive and succeed to include easily applicable features that evidently suit most applications, so you don’t have to struggle for them. 

### *Issue #7: Shouldn’t you just get the cloud professional support?*

Most large companies, especially ones whose company stock is traded on public markets, view your need for support as an opportunity to charge you more. You end up paying twice: once by allowing a complicated and clunky service consume your resources, and then again for the paid support you have to rely upon to make it work – which may cost you way more than the database itself. <br/>

We on the other hand view support the way you do, as a cost incurred by us. We are a company founded by developers, run by developers and guided by developers, whose mission is to create a product every developer would love to use. RavenDb strives to minimize your need to stop what you are doing, make a call, and wait until someone with a heartbeat picks up the phone. <br/>

We created a <em>low overhead</em> database, so your DBA wouldn’t have to play the role of babysitter. 
One of our clients has used our database for 6 years straight without having to touch it. 
We have provided RavenDB with self-optimization features to warn you of any potential issues and keep technical debt low, and put other fail-safe measures in motion to resolve many issues without wasting your time. <br/>

If you do need to speak to someone, we offer Dev Tech Support.
The developers helping you are the ones who built RavenDB, which ensures that you get a top-notch service and that our dev team isn’t isolated from the real issues you may run into in the real world.

<hr style="border-color: grey">
<div class="bottom-line">
    <strong>About RavenDB</strong><br/>
Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of pleased customers from Startups to Fortune 100 Large Enterprises.<br/>
    <strong><a href="https://ravendb.net/buy">RavenDB Features</a></strong> include:
    <ul>
<li><strong>Easy to Use</strong><br/> Easy to install, quick to learn</li>
<li><strong>ACID Transactions</strong><br/> RavenDB is fully-transactional</li>
<li><strong>High Performance</strong><br/> 1 million reads & 150,000 writes per second over commodity hardware</li>
<li><strong>High Availability</strong><br/> Multi-master replication over a distributed data cluster</li>
<li><strong>Smooth integration with Relational databases</strong><br/> Easily migrate relational data into RavenDB and transfer data over ETL from RavenDB to a relational database</li>
<li><strong>Multi-Clients</strong><br/> .NET/CLR (C#, VB.Net, F#), C++, JVM (Java, Kotlin, Clojure), Node.js, Python, Go</li>
<li><strong>Multi-Platform</strong><br/> Windows, Linux, Mac OS, Docker, Raspberry Pi</li>
<li><strong>Multi-Model Architecture</strong><br/>Documents, Key-Value, Graph Queries, Counters, Attachments, Revisions</li>
<li><strong>Authentication and Data Encryption</strong><br/>Data is secured at rest and in transit</li>
<li><strong>Advanced built-in features</strong><br/>Full-Text Search, Map-Reduce, and Storage Engine</li>
<li><strong>Management Studio</strong><br/>An enjoyable user experience</li>
<li><strong>Schema Free</strong></li><br/>
</ul>
    <strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong><br/>
    <strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB 4.2 for free</a></strong> and get:
<ul>
<li>3 cores</li>
<li>Our state-of-the-art GUI Studio</li>
<li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
<li>6 gigabyte RAM database with up to 3-server cluster</li>
<li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
<li>See the full features list in: <a>https://ravendb.net/features</a></li>
</ul>
</div>
    
