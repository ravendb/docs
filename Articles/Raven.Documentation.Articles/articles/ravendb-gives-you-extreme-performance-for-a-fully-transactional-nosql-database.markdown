# Extreme Performance for a Fully Transactional NoSQL Database<br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![Extreme Performance for a Fully Transactional NoSQL Database](images/ravendb-gives-you-extreme-performance-for-a-fully-transactional-nosql-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
Today’s data is coming in faster than ever. There are 5 billion people and 20 billion “things” all producing <em>enormous amounts of information</em> which needs to be managed. The amount of data worldwide doubles every year. Today’s database has to process <em>petabytes of data</em> in real time and keep expanding its abilities to keep up with the needs of today’s business. 

RavenDB, your fully transactional NoSQL database does exactly that. Using its in house storage engine, Voron, RavenDB can perform over 150,000 writes per second and 1 million reads on simple commodity hardware. We developed unique MapReduce, Queries and Dynamic Indexing methods to maximize RavenDB’s performance while keeping ACID guarantees.

Utilizing server resources to the fullest, RavenDB performs fast on older and cheaper hardware, enabling you to get the most out of your on-premises infrastructure and to get maximum ROI on the cloud. 

It also makes edge processing a breeze so you can take in data fast using Raspberry Pis and ARM chips. 

<a href="https://www.gartner.com/reviews/review/view/609931">
    <img class="img-responsive" alt="Gartner Peer Review" src="images/performance-is-awesome.jpg" />
</a>

## Performance in a NoSQL ACID Database

Since 2009, we have been mastering the art of maximizing performance while maintaining ACID consistency. The pioneer NoSQL ACID Database, we have developed new methods for everything to <em>give you the performance you need</em>, and the stability required of today’s applications. 

## Dynamic Indexing

Having an index ready and waiting for you saves time in your queries. For every query you make, RavenDB will create a new index or improve an already existing index. Every time you make an additional query there will be more indexes and better indexes available to <em>process your data quickly</em>. 

This works great with new builds. Instead of manually creating new indices, RavenDB will do it for you. Your development cycle itself will <em>accelerate</em>. 

## NoSQL MapReduce: Running Tallies for Rapid Retrieval

Wouldn’t it be great if the answers to all your questions were waiting for you the moment you asked them? That’s what our NoSQL MapReduce aggregates are all about. <em>Saving you time</em>. 

RavenDB will aggregate a MapReduce query the "old-fashioned" way combing through everything to organize it according to your request, <em>just once</em>. With every new data write, if it is something that should be counted as part of the aggregate, RavenDB will include it in the updated total. 

So, if you perform a MapReduce asking “how much in sales did we do from Europe? How many from America? How many from Asia?” RavenDB will go over all your sales orders and make the tallies just once. Then, when you make a $150 sale in New York, as it gets written, it will also be updated to the aggregate total for US sales. The next time you ask, the answer will be waiting for you. There will be no need to recomb the database.

This process performs <em>10 times faster</em>. 

RavenDB NoSQL MapReduce is part of the database. You don’t need to addon <em>Hadoop</em>. This is great for complex architectures like microservices which seek to minimize complexity and for development teams that want to maximize their productivity by learning just one technology for their database.

<a href="https://ravendb.net/download">
    <img class="img-responsive" alt="Extreme Performance with ACID Consistency for a Fully Transactional NoSQL Database" src="images/rdb-case-study.jpg" />
</a>

## A Storage Engine Build Exclusively for RavenDB Performance

We developed Voron, our own custom-made storage engine tailored to soup up RavenDB’s performance. We even remolded our NoSQL database itself to make Voron an even better fit for pushing RavenDB's top velocity <em>to the max</em>.

<a href="https://ravendb.net/">
    <img class="img-responsive" alt="Extreme Performance with ACID Consistency for a Fully Transactional NoSQL Database" src="images/orens-quote.jpg" />
</a>

Our storage engine is built by the same developers who built RavenDB so it all fits perfectly and is geared for superior performance. This is a boon for performance, ease of use, and especially tech support.

## Closing the Gaps for ACID Database Performance

ACID consistency normally creates performance gaps. A database will typically process a transaction in two steps: Step 1 is to make the operation transactional by adding to it the ACID guarantees, and step 2 is to persist the now ACID transaction.

During step 1, the system waits. That’s like sleeping 12 hours a day, how productive is that? RavenDB will process operations by persisting a transaction to disk while preparing the following operation to hold ACID guarantees. Voron will perform steps 1 and 2 simultaneously. This closes the gaps, maxes out your resource utilization letting you work on older and smaller machines more efficiently, and increases performance <em>by orders of magnitude</em>.  

## Minimizing Server Calls to Maximize NoSQL Database Performance with ACID Consistency

The easiest way to kill your application performance is to make a lot of remote calls. The most likely culprit is your database. That’s why RavenDB bundles multiple transactions together to minimize calls to the server. 

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

The transactional merger sends blocks of transactions to disk instead of one at a time. RavenDB will accumulate a large number of transactions and persist them together all at once, minimizing calls to the server. If one of them were to fail, the bunch would be broken up and processed one by one to maintain ACID guarantees. 99.9% of the time this isn’t necessary. As a result, <em>you go faster</em>. 

This is where the advantage of packaging an operation for ACID while at the same time persisting another one compounds your performance increases. Instead of dressing and pushing two transactions at the same time, RavenDB does it exponentially, performing the "gapless processing" for 10 transactions at a time, or more.

In our internal tests, we routinely bumped into hardware limits (the network card cannot process packets any faster, the disk I/O is saturated, etc.), not software ones.

Now that’s fast!

<div class="bottom-line">
    <p>
        <strong>About RavenDB</strong><br/>
        Mentioned in both <em>Gartner</em> and <em>Forrester</em> research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.
    </p>
    <p>
        <strong><a href="https://ravendb.net/buy">RavenDB Features</a></strong> include:
        <ul>
            <li><strong>Easy to Use:</strong> RQL Query language is based on SQL</li>
            <li>Works well with existing relational database. ETL feature and migration to Document model available.</li>
            <li><strong>Multiplatform:</strong> C#, Node.js, Java, Python, Ruby, Go</li>
            <li><strong>Multisystem:</strong> Windows, Linux, Mac OS, Docker, Raspberry Pi</li>
            <li><strong>Multi-model:</strong> Document, Key-Value, Graph, Counters, Attachments</li>
            <li><strong>Works efficiently</strong> on older machines and smaller devices</li>
            <li>Built in Full-Text Search, MapReduce, and Storage Engine</li>
            <li>Schema Free</li>
        </ul>
    </p>
    <p>
        <strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong>
    </p>
    <p>
        <strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB for free</a></strong> and get:
        <ul>
            <li>3 cores</li>
            <li>our state-of-the-art GUI interface</li>
            <li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
            <li>6 gigabyte RAM database with up to a 3-server cluster</li>
            <li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
        </ul>
    </p>
</div>
