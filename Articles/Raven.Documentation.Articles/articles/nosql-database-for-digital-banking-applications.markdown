# How Fintech Developer TSWG Uses a NoSQL Document Database <br/><small>by <a href="mailto:ayende@ayende.com">Oren Eini</a>, CEO RavenDB</small>

![NoSQL Database for Digital Banking Applications](images/nosql-database-for-digital-banking-applications.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
<div class="pull-right margin-left"><div class="quote-textbox-right">RavenDB is user friendly for developers. It’s very easy to use, the features just work.</div><p style="text-align: right"> ~ <span style="font-weight: bold">Waseem Rauf</span><br/><span style="font-weight: 300">Infrastructure & Security Manager for TSWG</span></p></div>

<p style="text-align: justify">TSWG is an Australian fintech services company providing solutions for digital banking applications, lending platforms, and managed network and hosting services that give clients a competitive advantage. Founded as <em>The System Works</em>, TWSG has been developing software for over three decades.</p>

### Meeting the Data Needs of Digital Banking in Fintech
For their *Digital Play Banking* application, TSWG needed a database to manage its increasing demands for digitalizing more financial services to greater audiences. As the volume of their clients traffic continued to expand, and the depth and breadth of information they wanted increased, they needed a database that could handle all of these requests safely and quickly.

They needed [a database that could handle heavier workloads faster](https://ravendb.net/features/high-performance). Creating software applications for banks demanded a database that was fully transactional. It also needed one that performed quickly.

They needed a database to better enable all users to:

- View all of their online banking accounts
- Check their balances on demand
- See a full transaction history
- Get a detailed view of all their individual transactions
- Schedule payments
- Make transfers
- Schedule transfers
- And more….

They needed a database that was *fast*, and guaranteed ACID promises, making sure that all transactions put the right amounts in the right places, and at all times. 

**Rarer is the need to move fast greater then when someone wants their money.**

<a href="https://ravendb.net/articles/ravendb-42-review-graphs-counters-revisions-and-more"><img class="img-responsive" src="images/podcast.png" alt="Listen to Oren Eini's talk about RavenDB 4.2 in this informative podcast"></a>

### The System Works' Options: Microsoft SQL vs RavenDB NoSQL Document Database
At first, they tried Microsoft’s SQL Database. The relational model was able to do the job, but having to take pieces of information from different places and join tables before serving their customer’s requests chocked performance. Their application was not able to provide the speed their users needed, especially when calling out information over mobile devices.

Using the Document model, they didn’t need the initial step of putting together a data puzzle before serving their user. RavenDB put everything in one document, providing the user all he or she was looking for in one record. RavenDB’s query optimizer will use an index for every query by either creating one, using an already existing one, or updating one – further boosting performance. 

The difference in performance was clear. 

### How RavenDB Serves Their Application Needs
RavenDB was the first database on the market to offer a Document model and [fully transactional ACID data consistency](https://ravendb.net/features/acid-transactions). It has had the most time to improve performance while keeping ACID. After over a decade of improvements, RavenDB operates at *1 million reads* and *150,000 writes* per second.  

RavenDB empowers TSWG to offer up speed while making sure not a single transaction misplaces their customer’s money. TSWG’s *Digital Play Banking* application has processed over a quarter billion transactions over the last 6 years using previous versions of RavenDB, not having a single issue with their data. They can offer their clients the flexibility of a nonrelational database while maintaining the data safety of a traditional relational model. 

To further protect their customers data and minimize latency and load, they use a 4-node database cluster with replication. 

RavenDB is easy to use, operates without a babysitter, and everything just works. TSWG developers are able to dedicate more of their time and resources to managing their data and how it better enables their *Digital Play Banking* application, and a lot less time managing their database.

They are able to keep track of the progress of their business with a polyglot database pipeline. Using RavenDB’s ETL Replication, they take in their data with RavenDB and replicate it to a relational solution which creates analytical metrics for business intelligence. The speed of RavenDB gives them a pulse on the marketplace and clear direction on what needs to take priority at any given moment. 

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

As they constantly improve their features and add more, RavenDB fits seamlessly into their development cycle, enabling them to offer the most modern and up to date digital banking application to their expanding customer base. 