# Is SQL Really Faster than a NoSQL Document Database?</a></small>

![Is SQL Really Faster than a NoSQL Document Database?](images/is-sql-faster-than-nosql-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
Traditional thinking holds that rows and columns read faster than documents. That's why a lot of developers continue to choose Oracle and SQL.

NoSQL Databases, specifically Document Databases are taking away market share from the older relational database makers because this way of thinking is no longer valid.

If all the information you needed in a query, like a customer profile or an order, were contained in a single row, relational models would be performance competitive. The fact is that in order to serve a query for data, a relational model must go to several places, taking data from rows nested in different tables, and then put it together before returning the results.
<div class="pull-right margin-left"><div class="quote-textbox-right">A document database has all the information you need in one place.</div></div>

<p style="text-align: justify">A document database has all the information you need in one place. The database just has to retrieve the one location and you have everything all at once. This is a huge reduction in complexity, and <a href="https://ravendb.net/features/high-performance">a huge boost to performance</a>.</p>

Add in an index that makes the jump from query to data and you have a 21<sup>st</sup> century document database that is far more powerful and faster than the ones developed during the days of disco.

<br/>
### Taking the Document Advantage to the Next Level

If Document Databases are faster than relational models, imagine how much faster the ones with superior indexing power are.

RavenDB is the only document database to use [automatic indexes](https://ravendb.net/features/indexes/auto-indexes). When a query is made, RavenDB will automatically use an index by finding one available, creating a new one, or updating one in use. It uses machine learning to improve indexes based on user queries. This eliminates the need for developers to code in their own index.

There are other performance advantages in the document model: Reading the entire result from a single point in the physical disk is much faster than reading small bits from different places on the disk, even on SSD. In a distributed data network, having to fetch bits and pieces of data from the ends of the earth and put it together in one place can be a disaster in performance and complexity.

<a href="https://cloud.ravendb.net/" target="_blank"><img class="img-responsive" src="images/ravendb-cloud.png" style="margin: 30px 0" alt="Try out RavenDB Cloud for Free"></a>

### Data Modeling Made Easy

A document database makes data modeling more practical. When you give someone information, it is usually on a form, or document. On any device, when they ask you to hand over your personal data, that too is in the form of an online document. Modeling your data based on the exact way your application is taking it in makes sense.

It's also easier to make changes.

For example, I have a company address and a shipping address. But I need to change the shipping address to an alternative destination for the new computer that needs to go to the East coast branch. In a document database, you simply change the document. In a relational model, it can be a complex process where you need to decide which tables to change, and what links not to damage.

<br/>
### The Faster Database on a Distributed Network

On a single server/single database/single application architecture, a document database is less complex and structurally faster. But once you scale out to a distributed database cluster of nodes and expand your application to include multiple databases, even putting your database cluster at the back end of multiple applications, you have complexity.

Add microservices to the mix and it can get pretty hairy.

The complexity of tables, joins, and piecing together bits and pieces of data fragments scattered throughout multiple servers across the world can make your application super complex and prone to error.

This is why document databases are best for distributed networks: There are no tables and no joins. For each node all the data you need to fetch is right there. This makes the document database the best candidate for the most common form of distributed network: The Cloud.

Using <a href="https://cloud.ravendb.net/" target="_blank">a document database on any cloud platform</a> can significantly reduce latency, cost, overhead, complexity and headache while increasing the performance of your application.
