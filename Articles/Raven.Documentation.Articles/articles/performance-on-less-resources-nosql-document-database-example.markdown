# Performance on Less Resources: A NoSQL Document Database Usage Example<br/><small>by <a href="mailto:oren@hibernatingrhinos.com">Oren Eini</a></small>

![Performance on Less Resources: A NoSQL Document Database Usage Example](images/performance-on-less-resources-nosql-document-database-example.jpg)

{SOCIAL-MEDIA-LIKE/}<br/>

ZAP is one of only two Yellow Pages franchises in the world to go completely digital, discarding its famous phone books a few years back. Today, it is an exclusively online business serving 18 million visits per month.<br/>
They needed to fix a series of legacy websites with rapidly deteriorating performance, and increase the amount of information they could provide their users - while retaining top SEO ranking. Performance, especially on mobile devices, is a critical factor in SEO - so time was of the essence.<br/>
Follow through this article to see what happened when RavenDB – a NoSQL document database - was put into action.

<a href="https://youtu.be/THeXFDMxr6s" target="_blank">
<img class="img-responsive" alt="Follow this article to see how a Yellow Pages Company has doubled performance and tripled the data presented to users while using fewer servers, just by using RavenDB - the NoSQL Document Database." src="images/what-businesses-are-saying-about-ravendb.jpg" />
</a>


## The Challenges
ZAP manages a portfolio of websites that make over half a million visits a day, with each visit averaging multiple queries to the database. Their job is to show the right data, in the right order, based on some filtered information from the user. 
They were using both MS SQL Server and ApproxiMATCH, a purpose-built search engine solution, which proved inadequate to handle their increasing workflow. <br>
They needed to hurdle the following obstacles:
<ul>
<li><strong>Slow queries:</strong> Very slow queries from the custom-built search solution they were using. Their data platform was unable to sustain minimal required performance levels under the ever-growing load. Some complex queries took over a second to process. <strong>Because searching was so slow, they decided to reduce the amount of information the user gets when making a query.</strong> The number of returned results has been reduced, along with the amount of data for each result. Their data systems were so slow, they had to cut back their user experience to maintain performance.</li>
<li><strong>Expensive database management:</strong> Every change to the database costs money, i.e. adding a field to a document or managing the sorting mechanism.</li> 
<li><strong>Too many servers:</strong> For example, 4 servers were required to support the search engine at one of their sites.</li> 
<li><strong>Too many points of failure:</strong> In addition to the search engine component, the databases, and the multitude of servers, workarounds had to be developed to overcome the search engine limitations, adding-up to many points of failure where things can go wrong, thus withholding ZAP from improving their sites.</li>
</ul> 

## Leveraging RavenDB - a NoSQL Document Database - Fixed All Issues and Increased Performance

<a href="https://ravendb.net" target="_blank">
    <img class="pull-right margin-left img-responsive" alt="I really, really recommend RavenDB ~ Hagai Albo, CTO, Zap Group" src="images/zap-group-quote.png"/>
</a>

Switching to RavenDB has eliminated all above technical complexities. ZAP was able to build a very fast performing search engine while enjoying the following:<br/>

**Replication:** After trying replication using MSSQL and finding it difficult and expensive, Zap easily managed the same task with RavenDB, using just a few lines of code. Scaling-up was easy – a server can be added with just a few clicks to a data-cluster that is self-managed. Nodes failover, documents conflict resolution, load-balancing, are all self-handled by RavenDB’s distributed system. The data itself is highly-available as RavenDB’s multi-master replication enables users to read and write on any node in the data cluster.

<div class="pull-left margin-right quote-textbox-left" style="text-align: left">All ZAP's sites are using RavenDB's search capabilities now.</div>

**Performance:** ZAP’s business model is based on fast data reads. A website speed, which is at the top of SEO considerations, directly impacts sales, especially for online businesses. Amazon found out that for every additional 100ms of latency their users suffer, revenue drops 1%. By using RavenDB, their average page load-time has dropped dramatically. Added to that, the amount of data provided per page has increased, thus providing a better service and user experience. This translated to better site engagement, higher ROI, and more money for the company. 

**Less Resources:** As performance has more than doubled, ZAP was able to reduce the number of deployed servers. Only 2 RavenDB servers were needed to support all their websites.

<div class="pull-right margin-left quote-textbox-right">RavenDB is easy and fast to develop on.</div>

<p style="text-align: justify;"><strong>Reduced TCO:</strong> Everything they needed was already an integral part of RavenDB, making it easy to set up and use, saving them time and developers’ resources - as well as freeing up the DBA to put out other fires. This dramatically reduced TCO, compounding annual OPEX savings.</p>

**Productivity:** RavenDB is easy and fast to develop on. In addition to RavenDB’s built-in Facets and Spatial query features that proved extremely useful, ZAP developers could write any query needed very quickly using RavenDB’s C# Client-API which they integrated with. Moreover, query results can be custom-sorted to match specific needs using simple code. While ZAP previously needed to implement their own caching mechanism, they now enjoy the client-server caching that comes with RavenDB out of the box.

**GUI for Monitoring and Debugging:** RavenDB’s Studio is available on every RavenDB license, even the free community edition. Using the Studio, ZAP easily monitored the databases and the cluster’s state, detecting bottlenecks and problems at real time. Attending the database’s early system warnings enabled fixing mounting irregularities that could have evolved into issues, before becoming hard to handle.

## The Bottom Line

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

ZAP’s migration to RavenDB serves as a prime NoSQL document database example of what is possible with the right software. 
Migrating to RavenDB enabled ZAP to do a lot of things better!
<ul>
    <li>Their page load time decreased from .7s to .3s after migrating to RavenDB 4.0.</li>
    <li>Average query time dropped to 10ms.</li>
    <li>They needed just half the servers they originally did, and less cloud resources should they choose this route.</li>
    <li>The amount of data they were able to provide their users with per page has doubled, along with additional data that can now be integrated from other sites.</li>
    <li>They can serve at least 300 queries per second per server. All previous performance constraints were made obsolete by RavenDB.</li>
    <li>Improved performance was registered with the SEO algorithms. Their traffic increased by 25%.</li>
    <li>As bounce-rate dropped and site-engagement improved dramatically, pages per visit surged.</li>
    <li>Zap developers’ productivity has improved as new features can be easily added now, reducing Time-to-Market.<br>
    </ul>
<a href="https://youtu.be/7MuqEPgq_Yk"  target="_blank" ><img class="img-responsive" alt="Zapping Ever Faster" src="images/zapping-ever-faster.jpg"/></a>
<hr style="border-color: grey">
<div class="bottom-line">
    <strong>About RavenDB</strong><br/>
Mentioned in both Gartner and Forrester research, RavenDB is a pioneer in NoSQL database technology with over 2 million downloads and thousands of pleased customers from Startups to Fortune 100 Large Enterprises.
    <strong><a href="https://ravendb.net/buy">RavenDB Features</a></strong> include:
    <ul>
<li><strong>Easy to Use</strong><br/> Easy to install, quick to learn</li>
<li><strong>ACID Transactions</strong><br/> RavenDB is fully-transactional</li>
<li><strong>High Performance</strong><br/> 1 million reads & 150,000 writes per second over commodity hardware</li>
<li><strong>High Availability</strong><br/> Multi-master replication over a distributed data cluster</li>
<li><strong>Smooth integration with Relational databases</strong><br/> Easily migrate relational data into RavenDB.
ETL data from RavenDB to a relational database</li>
<li><strong>Multi-Clients</strong><br/> C#, Node.js, Java, Python, Ruby, Go</li>
<li><strong>Multi-Platform</strong><br/> Windows, Linux, Mac OS, Docker, Raspberry Pi</li>
<li><strong>Multi-Model Architecture</strong><br/> Documents, Key-Value, Graph Queries, Counters, Attachments, Revisions</li>
<li><strong>Authentication and Data Encryption</strong><br/> Data is secured at rest and in transit</li>
<li><strong>Advanced built-in features</strong><br/> Full-Text Search, Map-Reduce, and Storage Engine</li>
<li><strong>Management Studio</strong><br/> An enjoyable user experience</li>
<li><strong>Schema Free</strong></li><br>
</ul>
    <strong><a href="https://ravendb.net/#play-video">Watch our intro video here!</a></strong><br/>
    <strong><a href="https://ravendb.net/downloads#server/dev">Grab RavenDB for free</a></strong> and get:
    <ul>
<ul>
<li>3 cores</li>
<li>Our state-of-the-art GUI Studio</li>
<li>Connectivity secured with TLS 1.2 and X.509 certificates</li>
<li>6 gigabyte RAM database with up to a 3-server cluster</li>
<li>Easy compatibility with cloud solutions like AWS, Azure, and more</li>
</ul>
    
</div>
</div>