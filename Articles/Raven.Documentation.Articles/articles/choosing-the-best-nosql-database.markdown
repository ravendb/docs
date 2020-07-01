# RavenDB or Elastic: The best database for you?
<small>Choosing the best NoSQL database</small>

![RavenDB or Elastic: The best database for you?](images/beach_crossroads.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

As the pace of technology continues to accelerate, your next database may determine whether your business thrives in the new generation of commerce, or fades towards perilous oblivion. Today’s database must be able to perform fast, even with big data. It must be flexible, and able to scale as the amount of information you process increases exponentially. It must have high-availability, so all of your clients and customers will have access to you at any time, even in the presence of failure.  

How do the best databases stand up to these challenges? Here is a comparison of two industry leaders, ElasticSearch and RavenDB:

## Data Integrity

#### ElasticSearch

Elastic uses Apache’s Lucene open source search engine. Lucene is not ACID, meaning that it can loss or corrupt data. Elastic is transactional on the document level, but not the batch. If you try to commit 100 documents, and just 55.5 were committed when there was a power disruption or some other kind of error, the .5 document will not be saved, but the previous 55 will be committed. This leads to inconsistencies because only part of the transaction has been committed and not the whole batch. Certain failure modes can cause data corruption or data loss.

#### RavenDB

RavenDB is an ACID and fully transactional database. Unlike Elastic, which is primarily an index store, RavenDB is used for primary data storage and is capable of handling multi documents transactions. RavenDB is one of the few NoSQL databases to offer this capability, giving the user one less thing to worry about.

As the amount of data required to run your organization increases, the consequences of a disruption to the flow of that data to your network can be severe. When a crash, power outage, or network partition failure occurs, the state of the data must reflect the last completed transaction. A transactional database will not commit an update unless the entire transaction has been written down (Atomicity, the A in ACID). If only a portion has been processed when an error happens, the transaction will be rolled back. This maintains consistency of data, and protects the integrity of your database. RavenDB also ensures that the data is safely persist to disk, in case of a power failure (Durability, the D in ACID).

RavenDB also make it trivial to run your database as  a cluster of nodes, all holding copies of your data. If there is an outage and one of the servers, is down, the other nodes have the data and will transparently pick up the slack and answer queries or writes. The user maintains full access to the database even during unplanned contingencies.

RavenDB implements a multi master distributed strategy, allowing each individual node to function on its own in the case of network partitions, any changes made to the isolated node will be merged back to the cluster when the partition heals.

## Data Security

#### ElasticSearch

Elastic offers no support for database encryption.

#### RavenDB

RavenDB offers full support to encrypt all data at rest in memory. This is done with 256-bit encryption using the ChaCha20 algorithm. RavenDB will only hold unencrypted data in memory for the active transactions, after which it will be immediately wiped.

## Version Control and Auditing

{SOCIAL-MEDIA-FOLLOW/}

#### ElasticSearch

Elastic offers no version control.

#### RavenDB

RavenDB supports version control at the database and collection level. Any change in a document will generate an immutable revision. You have access to the history of a document, containing all of its revisions. This is ideal for regulated industries where the law requires an organization’s data modification to track who did what and when. You can see the history of changes that have been made to any document in your database.

You can keep these revisions for a period of time or for a number of revisions, or even keep full history of all changes ever made, giving you the complete view of everything that have happened in your database.

## Data Delivery Performance

### Querying and Aggregating the Data 

#### ElasticSearch

Elastic break it’s databases into shards, each being hosted on its own node. This speeds up performance in enabling each user to access the data he or she needs from the closest node, reducing latency. Simple queries can typically be answered by searching the index, but more complex queries, especially aggregation queries, may require iterating over the result set and tallying the final results each time you execute the query. 

#### RavenDB
RavenDB supports dynamic queries as well as predefined indexes. When you make queries, the query optimizer will create indexes on the fly. As you make more queries, you are feeding the query optimizer to make better decisions to retrieve your data.
Automatically updating indexes provides you answers to common queries before you even make them. For aggregation queries, RavenDB will perform the computations when new data is written to the database. This happens far less frequently then performing computations each time a user request the aggregated data. 
Query results will come faster because RavenDB is not required to do a lot of work. It is simply using precomputed numbers RavenDB kept current for you. The query optimizer works just make sure your queries are fast.

### Caching

#### ElasticSearch

Elasticsearch aggressively caches data for future queries. Bitsets are reused when data is requested in a similar manner. These small caches are "smart" in that they are updated as the database is updated with new information. 

#### RavenDB

RavenDB supports caching at multiple levels. Repeated requests to RavenDB are detected and often served directly from the cache, only talking to the server to verify that they are still current. This can dramatically reduce the number of bytes transferred over the network and improve overall system performance.

RavenDB also supports aggressive caching, taking it one step further. With aggressive caching, you don’t even need to go to the server to verify that the cached data is current. Whenever data changes on a node, the server sends notifications to the client machines. Those caches with data that have been made outdated by the update will receive notifications that their cache has been invalidated. 

## Sharding the Data 

#### ElasticSearch

To handle large data sets, Elastic will cut up, or shard its database into pieces. The shards will be hosted on nodes physically closest to the clients most likely to use that particular data. Each client will work from the shortest distance to their nearest server. The more the shards, the lighter the load is for each server request. 

#### RavenDB

RavenDB is capable of handling very large datasets. Replications allow you to spread the load among multiple nodes. 

RavenDB doesn’t support automatic sharding, but you can shard your data according to your needs, as long as you configure it manually. 

## Communication with Machines Outside Your Cluster

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

#### ElasticSearch

You can perform Extract, Transform, and Load operations on ElasticSearch, but to do so, you need to integrate with a third-party tool.

#### RavenDB

Raven supports automatic ETL processes to relational databases, such as SQL Server, Oracle and Postgres SQL. You don’t need an outside application, it is supported as a core part of the RavenDB. You can take the documents in your database, and have RavenDB replicate them to a relational database. This enables you to perform a wide variety of analysis and reporting on your data in a familiar setting, using your existing reporting toolset. 

## Memory Management

### Garbage Collection

#### ElasticSearch

The standard garbage collection (GC) for the JVM, which Elastic uses, can stop any programming flow at an arbitrary point. This is called Stop the World garbage collection and is a frequent performance hazard. when 75% of a computer’s memory is clogged with unused objects, the GC executed arbitrarily and automatically. Such GCs can cause higher CPU usage, increased latencies, and even cause shards to relocate frequently as Elastic tries to keep the cluster balanced and enough replicas available.  

Elastic’s solution is to manually raise the % of memory being used that triggers GC to minimize the times garbage is collected. This requires higher resource allocation to the system then is actually needed.

#### RavenDB

RavenDB runs on a managed runtime as well (the CoreCLR) and has to deal with similar issues with regards to the GC. The RavenDB approach has been to take ownership of a lot of the memory hungry operations and move them to native memory, outside the scope of the GC.

This means that RavenDB is able to further optimize its memory utilization behavior, reduce memory usage and greatly diminish the cost of garbage collection.
 
This is part of the reason RavenDB is so fast. Handling tens of thousands of requests per second per node with consistent latency and throughput.

## Scaling Up

### Schema vs Schemaless

#### ElasticSearch

In Elastic, you must define the data according to its data type. This requires a schema. Once you set the data type for any field, you cannot change it. If you need to scale up queries, you may need an expensive migration of data. 

#### RavenDB

RavenDB is seamless and schemaless. You do not need to set data types. You can modify documents as you like. Queries are not based on your schema or data structure, but based on the information you are looking for. 

You can scale up without having to make fundamental changes to the way your database has been set to receive information. There is no need for migration.

Enjoy an additional layer of security as a full copy of your database is always safely hosted in at least one location. At any time, you can increase the number of nodes hosting replicated pieces of your database, reducing latency and lightening load. 

## Closing Argument

#### ElasticSearch

ElasticSearch is a very specific tool for a very specific purpose, searching for data. It’s schema is tailored to its focus. With data types that must be declared, and initial database layout that is very rigid, Elastic is made to be a database search engine, finding the data wherever it may be. 

#### RavenDB

As a document database, RavenDB is made to scale. As a transactional database, RavenDB was designed to hold “source of truth” information. It is meant to be the database where primary information is stored. 

Say you move from New York to California, and you inform the population registry of your move. The main database of the Federal government will see to it that you get your tax refund in California, and that the total population of both states have changed. But you are still getting your mail sent to New York. In this case, the primary database is the Federal government, and the secondary one is the post office’s version.

Only a database that is able to report and update the most accurate data is suited to be a “source of truth” database. RavenDB is transactional and structure less. Its flexibility enables you to scale up seamlessly, while its ACIDity ensures that the current state of data is accurate. Elastic is structured, which makes it strong for its primary purpose, to search for information. But it’s strength is also its weakness, that it is better suited as a ‘secondary’ destination for data.

> *Dreaming dreams no mortal ever dared to dream before.*
>
> The Raven (Edgar Allen Poe)

<p style="font-size: larger">RavenDB is among the few NoSQL databases that is both ACID and transactional. Built for speed, it pushes your server to the limits. Made for scale, RavenDB will accommodate the expansion of your data, and the success of your business. <a href="https://ravendb.net/downloads#server/dev">Take a free copy</a> and see for yourself!</p>