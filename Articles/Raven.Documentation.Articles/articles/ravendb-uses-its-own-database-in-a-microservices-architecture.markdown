# We Use Our Own Database<br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![How RavenDB Uses its Own Database in a Microservices Architecture for all its Data Needs](images/ravendb-uses-its-own-database-in-a-microservices-architecture.jpg)

{SOCIAL-MEDIA-LIKE/}


### How RavenDB Uses its Own Database in a Microservices Architecture for all its Data Needs
<p class="lead">There was a story about a guy that worked for Coke company for 20 years. One day he was seen at lunch drinking a Pepsi. The company suspended him without pay. About a month later, he gets a check from Pepsi reimbursing him for his contribution to their marketing efforts.</p>

There is no greater red flag for a product than knowing that the makers of that product are not confident enough to use it. If you see Tim Cook installing an Android 10 on a Samsung Galaxy phone, you might want to reconsider <a href="https://techcrunch.com/2019/10/28/apple-releases-ios-13-2-with-deep-fusion/" target="_blank">queuing up for that iPhone 11 Pro</a>. However, if he is playing Dick Tracy on his Apple Watch Series 5, then getting your loved one the latest iPad may be the way to go.

Everybody tells you their database is the fastest, easiest to use, with the highest ROI. They all tell you their version of the perfect fairy tale. It's only once your payment clears do you see that they suddenly stopped sweating.

*You can be assured that a database is everything they say it is by this one simple test:*

Are they using it themselves?

A company with real confidence in what it tells you it's product can do will use it on their own systems. That is why RavenDB runs RavenDB database as the centerpiece of its own microservices environment.

<div class="text-center" style="margin: 30px">
    <a href="https://cloud.ravendb.net" target="_blank"><img src="images/ravendb-cloud.png" class="img-responsive" style="margin: 0 auto;"/></a>
</div>

### Expanding from Monolith to Microservices
The past 5 years has seen RavenDB soar. Right after landing a major Fortune 500 client to put RavenDB in each of its 37,000 locations worldwide, word spread that our [Document Database](https://ravendb.net) was the real deal. Not long afterwards, RavenDB 4.0 was released, an Enterprise Grade Version literally built from scratch. 

All of the [major improvements to RavenDB](https://ravendb.net/articles/21-improvements-to-our-nosql-document-database) brought lots of new interest and customers started to burst through the doors. In the years that followed, our number of employees tripled, as did sales.

This put a lot of pressure on the poor monolith server screaming to any developer who would walk by yelling "Help me, I need air!".

This is the amazing development that happens when a company scales out. A handful of systems can no longer serve the needs of the entire company without burdening it with too much complexity, overlap, and lots of unfortunate bottlenecks.

We exercised the best option - establish a microservices architecture and reorganize the company around our software topology. As we hired more people, we localized job roles. Employees no longer held 4 to 5 roles each, roles were doled out to new employees. Our systems were broken up so each employee had their own microsystem, based on their own role.

The only question was, which database do we use for each system? Which database would we trust our own money, information, and future to?

<div class="text-center" style="margin: 30px">
    <a href="https://ravendb.net/learn/webinars/ravenDB-101-querying-indexing-aggregates-document-database-cloud-on-prem-hybrid-environment"><img src="images/ravendb-101.jpg" alt="RavenDB 101 Webinar Available On-Demand" class="img-responsive" style="margin: 0 auto;"/></a>
</div>

### We Aren't Only RavenDB Developers, We Are Also Clients

<div class="text-center" style="margin: 30px"><iframe width="560" height="315" src="https://www.youtube.com/embed/IuRLGdGnqSU?t=50" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe></div>

To expand our systems, we needed a database that could support microservices, perform fast, and give us [high availability](https://ravendb.net/why-ravendb/high-availability) over a distributed network.

We believe in our database so much that it's at the core of ***every RavenDB*** system. Every developer, marketer, and salesperson uses a RavenDB database to do his job.

That every RavenDB internal application uses RavenDB gives our development team the opportunity to better know the ins and outs of the database. It also gives new developers the chance to ramp up fast.

At RavenDB, RavenDB is responsible for the following tasks:
<ul>
<li>The entire infrastructure for RavenDB Cloud</li>
<li>API for ravendb.net</li>
<li>The API for the authentication to access RavenDB instances</li>
<li>All pricing possibilities for RavenDB Cloud instances</li>
<li>All our documentation</li>
<li>Website content</li>
<li>Web analytics</li>
<li>Internal commands for RavenDB</li>
<li>Security certificates</li>
<li>Domains for User instances</li>
<li>Leads management system</li>
<li>Email lists</li>
<li>Finances</li>
<li>Orders</li>
<li>Tech support requests and questions</li>
<li>Error messages</li>
<li>Legacy operations</li>
<li>All email activity</li>
</ul>

*We will never ask you to trust RavenDB to do anything it isn't already doing for us.*

<div class="text-center" style="margin: 30px">
    <a href="https://ravendb.net/whitepapers/mongodb-ravendb-best-nosql-open-source-document-database"><img src="images/ravendb-vs-mongodb.png" alt="RavenDB vs MongoDB Whitepaper" class="img-responsive" style="margin: 0 auto;"/></a>
</div>
