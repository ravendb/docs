# Simple Store Uses NoSQL for E-commerce
<small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![NoSQL database enables a private database for each user. Automatic Indexing and Aggregated MapReduce make complex computations fast using NoSQL for Ecommerce."](images/how-simple-store-uses-ravendb-nosql-document-database-for-digital-ecommerce.jpg)

{SOCIAL-MEDIA-LIKE/}

How our NoSQL database enables Simple Store to provide businesses a reliable digital eCommerce platform.

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <a href="https://simplestore.io/" target="_blank" rel="nofollow"><img src="images/simple-store.png" class="img-responsive m-0-auto" alt="Simple Store"/></a>
</div>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">RavenDB indexing is by far, faster than querying a relational database structure. The ability to run a complex query that executes in sub-milliseconds gives us the confidence to build features on top of that without finding ourselves entangled in a relational JOIN hell.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Sarmaad Amin, Simple Store</span>
</div>

*Once the dust clears from the COVID-19 pandemic, one thing is for sure: Every company will have to serve an audience of people that simply won't be able to leave the house and will have to conduct all their personal and business affairs digitally.*

Simple Store provides eCommerce solutions to enable even the local kiosk to commence quick and easy eCommerce operations to be able to reach right into anyone's home and offer them what they need.

### What is Simple Store?

**Simple Store** is a headless eCommerce platform that's multitenant. Each customer has its own database. RavenDB is the only database they use.

It was born from its parent company, IDP Solutions. The developers saw the same problems and patterns emerging across all their clients. They realized a need to develop a one stop system for all their eCommerce needs.

For example, say you want to automate different sales channels. Simple Store can look after your product master data, so you create your product master data and then, using APIs, syndicate those product catalogs across channels such as your website, your mobile app, or your kiosk.

**RavenDB and Simple Store leverage one distributed database system to serve both digital and physical points of sale.**

The same idea applies for your sales data. Simple Store is your single source, so you can see all your orders coming in from different channels.

### What is the Simple Store Story?

What was a luxury is now the standard.

People want to sell more of what they offer online where the process is completely digitalized.

Many companies, especially ones that classically sell in-house, have no infrastructure online.

They saw no need for eCommerce.

But now, with possible multiple waves of lockdowns or at least limited quarantines, there will always be market segments for people who cannot leave the house and have to conduct all their business digitally.

These businesses need systems in place **very quickly** to meet the needs of people living in quarantine from the COVID-19 virus. Countries emerging from the crisis like Singapore, New Zealand, and Israel are now in "isolated quarantine" mode where instead of a general lockdown of the population, isolated outbreaks in schools, offices, and apartments determine a "constant rolling" population of people within countries, provinces, cities, even communities under temporary lockdown.

This creates a new market demographic that businesses, especially ones that were 100% brick and mortar, are racing to fill.

**Simple Store** solves the problem of companies that never needed to sell digitally by enabling them to set up their eCommerce platform quickly and swiftly.

### Latency in Real Life

Here is a simple use case: You are selling perfumes or cosmetics. Your company has three warehouses in Europe, Asia, and North America. For every order, you have to decide which warehouse you will dispatch the product from. If you are getting a handful of orders per week, it's manageable.

It becomes an absolute nightmare if you need to process 100 orders per day.

Simple Store can manage your warehouses and route each product to the correct dispatch center. Because it is all integrated, when that particular dispatch center ships your product, it updates your order automatically so your customer can see that, "It got shipped!"

Sales, inventory, and distances to each order destination are updated in real-time to make available at every moment what products are in stock that is closest to the customer. This saves money on shipping costs. It also saves time in delivery to your buyers.

### Database Needs for Simple Store that RavenDB Provides

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">Databases are scary. They hold your data. If something goes wrong, you are in trouble. Knowing how RavenDB works internally gives me a sense of peace that if I get into trouble, I know exactly what to do.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Sarmaad Amin, Simple Store</span>
</div>

The developers for Simple Store have been using RavenDB since the 1.0 version. They discovered the NoSQL Document Database from the "<a href="https://ayende.com/blog/" target="_blank" rel="nofollow">Ayende Rahien</a>" blog written by RavenDB CEO Oren Eini.

They needed the following:

* The ability to work effectively in embedded mode
* A fully transactional distributed database that is fast. Being eCommerce, ACID must be baked into the cake
* .NET Solutions all built-in .NET C#
* Multitenant

### Security for All Customers and Clients

**If a blue-chip client needs Simple Store, one of their security requirements is that data will not just be logically separated, but physically separated as well.**

RavenDB allows for a database for each user inside each client on the fly. Clients can manage all their data while keeping it safe. When a customer creates an account, a new database inside their RavenDB cluster is created.

RavenDB has an excellent way to create a new database, provision indexes automatically, and guide every interaction you have within the platform. Clients can work with their database rather than sharing data and "logically" separating them because RavenDB physically separates it.

Now you can encrypt data per tenant. A specific client can generate their own keys for their database, which is a great security feature they offer their end users.

### A Tool that Fights for You

When looking for their database, they avoided MongoDB because it failed horribly on the transactional end. They used Couchbase, SQL Server, and MYSQL.

Relational databases proved harder to write code for, and they increase latency. The feeling was that when you are building something, you want to make progress, so you don't want to fight with the tool.

For development, RavenDB proved to be easy. No Impudence Mismatch. You code an object and you get an object. The development story and the solution process become a lot more simplified.

In these blogs, Oren has consistently "opened the hood" of RavenDB. Followers have the comfort in knowing what exactly this database is. There are no surprises.

<div class="margin-top margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="RavenDB Cloud"/></a>
</div>

### Their Main Benefit of RavenDB as an eCommerce Solution

Performance is critical to any eCommerce application. *Almost 10% of potential buyers abandon their shopping carts due to performance. Your database impacts your **bottom line.***

Simple Store requires read retrieve and write operations to take less than 40 milliseconds. Keeping CRUD ops under 40ms means their customers get it under 150ms.

Using a database that can give you 40ms is one thing, a database that will keep that time, and continuously improve on it, that's another.

To demonstrate, a Simple Store user clicked a "Quick Buy" link over 10,000 kilometers away from the server, and the latency kept to a quarter second even after the application made a dozen API calls to the database.

RavenDB allows them to build complex products on top of the database and produce answers rapidly to keep the customer happy and moving along the buyer journey.

The way indexing in RavenDB works is far faster than querying a relational database structure. A complex query can execute in sub-milliseconds to give Simple Store the confidence to build features on top of that without finding themselves entangled in a relational 'JOIN hell.'

A great example is: You want to offer a "summer break" special for the last week in June.

Everyone gets 10% off. This discount is selectively applied to a bag of products. With each price query for an item, your database has to:

* Figure out if this product gets a discount
* Determine if there are any other deals, discounts, offers related to the product
* Decide which one applies
* Return a price for rendering

These API calls must be done without decreasing performance for the user right at the most critical point in the buyer journey.

RavenDB creates [MapReduce indexes](https://ravendb.net/docs/article-page/4.2/csharp/indexes/map-reduce-indexes) that automatically reduce the specials to the applicable products. From there, Simple Store will just load the data on request because it's ready before the query is made. They return the API call in sub-millisecond time.

The developers for Simple Store experimented with this feature for another project using a relational database. It took them three weeks to build. The database needed 4 seconds, or 4,000ms to calculate the correct price for a product.

For every purchase, shopping cart tally, even landing on a product page, you need to make this "special" calculation and render a price for the customer. Being able to tell the customer how much money he is saving right away is a significant advantage for any eCommerce company.

### Scalability

As an eCommerce store expands, so do the number of products it offers its customers. As the products increase, so does the complexity.

This creates more work for the database.

For a relational database, if a product belongs to multiple tables, all of that has to be computed.

With RavenDB, the index is a MapReduce index so it's like live code. Any changes in the product will cause the index to recalculate. The specials are always updated and *waiting for the query by the user.*

If a product is no longer available, the index will automatically update upon the removal of the product, not the next time a customer asks for it.

This bundle of features makes RavenDB right for an all-purpose digital eCommerce solution.

See for yourself!

Take out a free [RavenDB Cloud Instance](https://cloud.ravendb.net/) and try it out. Want to see more? Take a one on one [demo of RavenDB](https://ravendb.net/live-demo) with a database engineer.