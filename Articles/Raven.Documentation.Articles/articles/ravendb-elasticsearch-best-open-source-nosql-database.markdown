# RavenDB vs Elasticsearch: The Best Open Source NoSQL Database<br/><small>by <a href="mailto:oren@hibernatingrhinos.com">Oren Eini</a></small>

![The right NoSQL Document Database can make several server requests register as one, lowering your cloud costs tremendously.](images/ravendb-elasticsearch-best-open-source-nosql-database.jpg)

{SOCIAL-MEDIA-LIKE/}

## When deciding on your next NoSQL Download, which is best for you, RavenDB or ElasticSearch?
A modern top-notch database has to be equipped with a remarkable set of capabilities. Its work rate must be suitable for Big Data management. Its flexibility must allow it to easily upscale when the amount of data ascends exponentially, and its availability must be absolute, keeping it reachable for clients and customers even in the presence of failure.<br/>

How do the best open source NoSQL databases stand up to their challenges?<br/>

Two industry leaders, ElasticSearch and RavenDB, are reviewed by their approach toward and implementation of 10 parameters: data integrity, security, data model, version control, querying, data delivery, sharding, communication, memory management, and scaling out.<br/>

## Data Integrity

#### ElasticSearch
ElasticSearch uses Apache’s Lucene open source search engine. Lucene is not ACID, which exposes your database to various failure modes may end up in data corruption or loss that you may not even be aware of.<br/>

ElasticSearch is transactional on the document level, which gives you a standard of data consistency. The exposure, however, is that it can lead to partial batch transactions in which some documents are committed and others are not.<br/>

#### RavenDB
RavenDB is fully transactional across your database and throughout your cluster. You can use it to transmit documents and document batches with full confidence that whatever the transaction’s scope is, it would be completed as a whole or reverted in its entirety.<br/>

Each RavenDB instance is a standalone server that functions as such whenever the need arises. Each instance resides in cluster state, and can hook into your cluster as one of its nodes in a few clicks. Tasks like expanding your network, distributing your database while maintaining its integrity, sharing data and chores - are all trivial for RavenDB and its users.<br/>

A multi-master distributed strategy allows each node to function on its own if the network ever partitions. Clients can read and write using any node, and the changes made to an isolated server’s database are merged back to the cluster as soon as connectivity heals.

## Data Security

Both ElasticSearch and RavenDB encrypt data in transit and at rest. <br/>

#### ElasticSearch
ElasticSearch encrypts transferred data and authenticates clients using TLS X.509 certification. <br/>

Using a manual installation, your indexes and shards can be backed up fully and incrementally. Backup files are not encrypted.

#### RavenDB
Like ElasticSearch, RavenDB uses TLS X.509 certification during transit to encrypt data and authenticate its clients. <br/>

All data at rest, including documents, indexes and other components, is encrypted using 256-bit encryption and the XChaCha20 algorithm.<br/>

RavenDB provides a comprehensive automated backup system whose output files may be encrypted to keep them secure while stashed for safekeeping.<br/>

You can set backup routines by time interval and type to run a full backup every 24 hours and an incremental one every 30 minutes. <br/>

## Data Models

#### ElasticSearch
ElasticSearch is a distributed document store database. <br/>

#### RavenDB
RavenDB is a multi-model database that includes a Document Store, Key-Value Store, Graph API, Distributed Counters and Binary Attachments. 

<a href="https://ravendb.net/whitepapers/ravendb-healthcare-database-schedule-appointments-detect-fraud-security-phi" target="_blank"><img class="img-responsive" style="margin:auto" alt="Preventing Headaches: RavenDB in the Service of Healthcare" src="images/rdb-healthcare-whitepaper.jpg" /></a>

## Version Control and Auditing

#### ElasticSearch
ElasticSearch offers no version control.

#### RavenDB
RavenDB supports version control at the database and collection level. If you enable it, any change in a document would generate a "revision" - an immutable copy of the document before it's been changed.<br/>

Keeping track of a document's development may be useful in many situations. In the case of regulated industries maintaining a trail of data modifications may even be required by law.<br/>

You can keep all the revisions ever created in our database or limit the revisions history by a chosen number of revisions or for a certain period of time.<br/>

You can revert any document to any of its revisions, traveling back and forth in time. 
An especially powerful feature lets you use revisions on a bigger scale, and revert the whole database to its state at a certain time. This may turn from handy to crucial if a flow of mistaken data has rendered an important database unusable for example, and you want to follow the changes to their origin and salvage your data.<br/>

## NoSQL Database Query Language 

#### ElasticSearch
For anyone accustomed to SQL, getting used to ElasticSearch's DSL (Domain Specific) query language may take some time. <br/>

DSL isn't the easiest thing to implement, especially when somewhat more advanced features are involved like nesting aggregations and filters in a single query. <br/>

Complex searches are performed using a JSON-based format that tends to become overly verbose, yielding large and ridiculously complex queries. <br/>

#### RavenDB
The Raven Query Language (RQL) is an intuitive dialect of SQL. Some 80% of it is SQL, allowing beginners to start easily and experienced users to use their current knowledge as a bridge and move on to NoSQL as pleasantly as possible.<br/>

Carrying your experience along to your next level doesn't stop here, as advanced features like Graph querying are supported by RQL as well.

## Data Delivery Performance

### Querying and Aggregating the Data

#### ElasticSearch
ElasticSearch breaks its indexes into shards that can each be stored on a different node. An index that has grown beyond a single machine's capacity to handle it can thus be handled by several. This considerably improves server performance and lowers client latency. <br/>

While suitable for simple queries, more complex ones are likely to cause overheads like iterating through result sets and tallying the final result each time the query is executed. It is therefore common to find an add-on like Apache Hadoop aiding ElasticSearch in queries optimization.

#### RavenDB
To boost performance, RavenDB supports dynamic queries as well as predefined indexes. When you create and execute queries, a query optimizer automatically finds and improves their indexes. As your queries evolve, index optimization continues as well. The more queries you run, the better indexes RavenDB sets for you and the faster your queries are answered.<br/>

RavenDB implements a native MapReduce feature, eliminating the need for third-party add-ons. For aggregation queries, RavenDB tallies totals “the old-fashion way”, just the first time an aggregation query is made. From here on, the total is updated each time data is written to the database. This reduces the time it takes to produce aggregates by up to 99%. 

### Caching

#### ElasticSearch
ElasticSearch aggressively caches data for future queries.<br/>

Frequently used aggregates, like ones loaded from popular pages, can be cached for reuse.  Queries are kept in editable form, but are also translated to Bitsets to provide clients with quicker response. Bitsets are cached to make them immediately available when reused data is requested, and are wisely updated as queries are added.

#### RavenDB
RavenDB supports caching at multiple levels. Repeated client requests are detected and often served directly from the client-side cache, only talking to the server to verify they are still current. This can dramatically reduce the amount of data transferred over the network and improve overall system performance.<br/>

RavenDB also supports aggressive caching, taking it one step further. Clients do not even need to approach the server to verify that the cached data is current. Instead, the server notifies them whenever data changes on a node, and outdated cached data is invalidated.

## Sharding the Data

#### ElasticSearch
To handle large data sets, ElasticSearch divides its database into indices. Each index can be further divided into shards, which can be then replicated if necessary.<br/>

Shards would be normally placed on node machines near their potential users. Short distances to clients and the right number of shards reduce the load per each server request.

#### RavenDB
RavenDB doesn’t support automatic sharding in its current version, this feature is planned to be included in version 5.0. 

## Communication with Machine outside Your Cluster

#### ElasticSearch
In order to perform Extract, Transform and Load operations on ElasticSearch, you need to add on a third-party tool.

#### RavenDB
RavenDB supports automated data transfers using ETL between itself and relational databases, non-relational databases, and the cloud.  No outside applications or third-party add-ons are needed. <br/>

You can replicate documents from your database to a relational database, enabling a wide variety of analyses and reports using your familiar setting and existing reporting toolset. It also makes working with polyglot architectures, especially microservices, a snap. 

## Memory Management

### Garbage Collection

#### ElasticSearch
ElasticSearch uses JVM, whose standard garbage collection (GC) may stop any programming flow at an arbitrary point. This is called Stop the World garbage collection.<br/>

When 75% of a computer’s memory is clogged with unused objects, garbage collection is executed automatically. Such GC can cause higher CPU usage, increased latency, and frequent shard relocation as ElasticSearch tries to keep the cluster available and balanced.<br/>

ElasticSearch’s solution is to raise memory usage percentage manually, triggering the JVM to minimize garbage collection periods. This requires higher resource allocation to the system.

#### RavenDB
RavenDB runs on a managed runtime as well (the CoreCLR) and has to deal with similar garbage collection issues. Its solution is very different though: it has taken direct ownership over many memory operations, and now manages them itself outside the scope of the GC.<br/>

This means that RavenDB is able to further optimize its memory utilization, reduce memory usage and greatly diminish the cost of garbage collection.<br/>

This is part of the reason RavenDB is so fast, with each node capable of handling tens of thousands of requests per second with consistent latency and throughput.

## Scaling Up

### Schema vs Schemaless

#### ElasticSearch
ElasticSearch requires you to define data by its type, which requires a schema. Once you set the data type for any field, you cannot change it. If you need to scale up queries, you may need a data migration.

#### RavenDB
RavenDB is schemaless. You do not need to set data types, and can modify documents as you please. Queries are not based on your schema or data structure, but on the information you are looking for.<br/>

You can scale up without having to make fundamental changes to your database setup, and need no migration. This may save much valuable time in your release cycle. <br/>

Easy scalability reduces latency, lightens the load off each node, and provides you with an extra layer of security as full database copies and chosen database pieces are easily replicated.<br/>

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
    <li><strong>Smooth integration with Relational databases</strong><br/> Easily migrate relational data into RavenDB, ETL data from RavenDB to a relational database.</li>
    <li><strong>Multi-Clients</strong><br/> C#, Node.js, Java, Python, Ruby, Go</li>
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
    
