# What to Consider when Building a Database for Today's Data<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![What to Consider when Building a Database for Today's Data](images/what-to-consider-when-building-a-database-for-todays-data.jpg)

{SOCIAL-MEDIA-LIKE/}

<br>


## Accomplish large scale applications on a slim budget

Everybody wants to build applications that scale out like Google. The trick is how to accomplish that without having to fund a budget that only a company like Google can underwrite.<div class="pull-left margin-right"><div class="quote-textbox-left">I want a database to pamper me.</div><p style="text-align: right"> ~ <span style="font-weight: bold">Oren Eini</span><br><span style="font-weight: 300">CEO of RavenDB</span></p></div>

Whether you are building your database in-house or provisioning one on the cloud, the core challenge for your database is the same: ***How do I get the most out of my database while investing the least amount of time, money, and developer resources into it?***

The cloud can save, on average, 15% for any application. The challenge is that this is not a given. Unless you have a database that knows how and where to take advantage of what the cloud offers, you might wind up paying a cloud provider more.

The right managed cloud solution, like [RavenDB Cloud](https://cloud.ravendb.net/), will take on all the operational overhead involved with running your database. You won't have to worry about having to monitor, manage and operate your database throughout your application development and production life cycles. You just have to concern yourself with what data you want to put in, and what should be taken out.

Great. Now how do I get my savings?

## Save Database Processing Time and Cost

Things like setup, caching, and indexing no longer need to be coded by your developers. RavenDB does that as well. Using automatic indexing means you don't have to create indexes, or even update them. RavenDB will always use an index, or improve one to make sure you are getting your data as quickly as possible. This saves you processing time and cost on the cloud.

For both on-prem solutions and the cloud, set up is a 5-10 minute process. You get setup and secured without having to pour over hundreds of pages of documentation.

## Processing Big Data, Small Data, Anywhere and Everywhere

Today's data is coming in from anything that has a chip implanted on it. That means phones, clothing, cars, silverware, baby toys, even inside the human body. Big data is all about edge computing, taking data from hundreds, thousands, even millions of end points and processing it up the data hierarchy.

This means you need to process data from machines and devices that have very little computing power. RavenDB has been doing this and improving on it for over a decade.

From the beginning, RavenDB has been the best at operating on older and smaller machines. We understand that an application can have a life of 7-10 years and in many cases, the companies funding those projects will buy the hardware just once. At the latter stages of a project, you need to run your app on what has become antiquated machines, but at [the same level of performance and efficiency as if they were state of the art](https://ravendb.net/features/high-performance).

In the era of the cloud, being able to run on less costly machines with equal ability saves every user money each minute his application is running.

For edge computing, it means RavenDB will be able to work whatever device you need to pull data from as efficiently as possible. Our pull replication enables you to move data up the hierarchy without security issues or performance challenges.

<br/>
[![Managed RavenDB Cloud Hosting](images/ravendb-cloud.png)](https://cloud.ravendb.net/)
<br/><br/>

## Fault Tolerant

The big advantage of today's data is that it gives you more accurate results. For any question you have, your answers come with more data and analytics, giving you a lot better guidance for your next decision - or that of your users.

But only if the data is reliable. If your data is off, so are your answers.

RavenDB includes features designed to make sure your data is exactly what your database is telling you it is.

ACID guarantees. For over a decade, we have offered [NoSQL Document modeled data that is also fully transactional](https://ravendb.net/articles/ravendb-best-nosql-database-example-for-startups), giving you the best in data integrity.

## Work offline

RavenDB is also able to continue to work offline. If a node is no longer connected to the database cluster, it will continue to collect data at its local point. Once your systems are fully online, the node will update the rest of the database cluster, and get updated so you will have all your data available.

This works for subscriptions as well. If you are transmitting or receiving a certain subset of data and the wire gets cut off, RavenDB will remember all the relevant events that occurred while you were offline and send them to you once things are running again.

As a REST protocol database, you can *read and write* to each individual node. This lets every user access your database with minimal latency and high-availability. Database tasks that are assigned to a node that goes down will automatically be reassigned to nodes still operational.

As the pace and size of data continues to accelerate, you need to take it all in without having to waste developer time on something that should just work.

<br/>
[![The Best Document Database Got Better: RavenDB 4’s 21 Improvements Podcast](images/podcast-banner.png)](https://ravendb.net/articles/21-improvements-to-our-nosql-document-database)