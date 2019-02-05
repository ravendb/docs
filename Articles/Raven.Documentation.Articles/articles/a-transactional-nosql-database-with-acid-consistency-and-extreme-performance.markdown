# A Transactional NoSQL Database That’s Blazing Fast<br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

![A Transactional NoSQL Database That’s Blazing Fast](images/a-transactional-nosql-database-with-acid-consistency-and-extreme-performance.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

## RavenDB is a Transactional NoSQL Database That’s Blazing Fast

A database without transactions is… not much of a database. ACID requirements are the gold standard for transactions and for over a decade RavenDB has offered ACID guarantees without having to sacrifice performance. An ACID Document Database is the ideal way to crunch today’s masses of complex data.
 
RavenDB is one the first nonrelational databases to offer ACID not just on a single value, but on multiple values <em>throughout your database</em>. RavenDB is a distributed database so it also offers ACID guarantees <em>throughout your database cluster</em>. 
 
Your developers don’t need to carefully code around limitations in the system. They can keep using the transactional model and focus on building robust applications, delivering value to the business and not dealing with the intricacies of data storage.

RavenDB is commonly used for point of sale applications, trading systems, order fulfillment, inventory tracking, ERP applications and more.

## Transactional NoSQL Database that’s Blazing Fast
[![Trustworthy Database That Just Works!](images/trustworthy-database.jpg)](https://www.gartner.com/reviews/review/view/550581)


Just having transactions isn’t enough if it’s at the cost of performance. In today’s environment of Big Data, consistent results that are too late are of little use. Trading algorithms need to update aggregate figures in real time, processing petabytes of trading data each moment. 

Health care applications need to rapidly track test results over time and compare them to millions of similar cases to enable the doctor to best decide the next course of action. Point of sale applications need to update inventory totals, regional sales, and purchasing needs on the hour. 

This requires a database that can handle <em>massive amounts of data</em> in real time while guaranteeing its integrity for every transaction. It must accomplish this while using up as little resources as possible to enable teams with aging hardware to be able to run this enterprise software. 

In some cases, data is coming from edge points consisting of small sensors in machinery, clothing, even inside our bodies. The data is relayed to smaller servers for immediate processing. On a Raspberry PI, a $25 machine running on low powered ARM chips and a mere 1 GB of RAM, RavenDB can handle over 13,000 reads per second and over 1,000 writes per second. This is more than enough for most small to medium applications.

For larger options, whether on-premises, in the cloud, or a hybrid of both, a single machine with a price tag of less than $1,000 running RavenDB can handle over 150,000 writes per second and over million reads per second. 

## How RavenDB Gives You ACID Consistency with Superior Performance

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

Improving performance within the limits of keeping ACID consistency is just enough a challenge to get us really creative. 

One of the latest improvements we made was to develop Voron, our custom-made storage engine designed specifically to <em>maximize RavenDB performance</em>. An in-house engine eliminates all integration issues. If something doesn’t fit right, we can remake it to fit perfectly. If the database has too much of a sharp edge, we can even tweak the database itself to make sure everything fits just right. 

We took it to the next step with the transactional merger. In bundling multiple transactions to persist all at once, latency is reduced and performance zooms up by <em>orders of magnitude</em>. 

But how can it be done while keeping Atomic promises?

We will take multiple transactions and bundle them up into a “cab.” That cab will carry your transactions to the disk  as a single operation. If one part were to fail, then all the other transactions will be sent to disk individually, insuring that the failed transaction doesn’t get written to disk while all the others do. This insures atomicity while reducing the time it takes to write all the transactions to disk. 

Voron and transactional mergers are a small sample of the things we have done over the years to make RavenDB both fast and ACID, enabling you to keep the best of the relational databases while moving forward to conquer today’s world of nonrelational data. 


<div class="bottom-line">
<strong>About RavenDB</strong><br/>
Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of customers from Startups to Fortune 100 Large Enterprises.<br/><br/>

<strong><a href="https://ravendb.net/features">RavenDB Features</a></strong> include:

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
    
    
<strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong>
<br/><br/>
    
<strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB for free</a></strong> and get:
<ul>
    <li>3 cores</li>
    <li>our state-of-the-art GUI interface</li>
    <li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
    <li>6 gigabyte RAM database with up to a 3-server cluster</li>
    <li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
</ul>
    
</div>
