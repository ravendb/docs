# Keeping Our New Year's Resolution: A Lean Database for 2020
<small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![For 2020 and beyond, RavenDB promises to keep its NoSQL Database lean and mean.](images/a-lean-nosql-database-for-2020.jpg)

{SOCIAL-MEDIA-LIKE/}
<br/>

In a recent survey, 57% of New Year's Eve partygoers said that losing weight is their resolution for 2020. You have to admire their persistence; they made the promise in 2016.

At RavenDB, that was also our resolution for 2016. We renewed our pledge to stay lean for 2017, 2018, 2019, and 2020.

Can a database lose weight? Can your infrastructure keep the bloat off?

When a vendor releases a new version of their software, there is a hidden cost behind your next update: Bloat.

In the age of the cloud, any system not designed to minimize its consumption of disk space and memory will cost you dearly. You will be the one who has to pay for the extra storage and computing power every second you carry someone else's application in your cloud architecture.

## A Lean Database Makes for a Light Cloud Bill

With each new rendition of your software, the code can get plump with extra features and additional resource consumption.

If a vendor is trying to break into a new market, say a file-sharing application trying to see if you like their graphic arts platform, new updates may contain features you don't need, will never use, didn't ask them to provide for you, but will have to pay for.

If you have an on-premise system and extra memory to spare, it won't be costly. But unless your vendor makes sure to refactor new versions to keep resource usage steady, you are picking up the tab for their product testing.

## Lean and Mean in the Skies and on the Cloud

Every year, RavenDB resolves to stay lean so your Database ROI should consistently see an [increase in *return* and not *investment*](https://ravendb.net/articles/cost-benefits-ravendb-nosql-acid-database).

We designed RavenDB to be lightweight, consuming as little memory as possible. In terms of memory usage, disk space, and performance, RavenDB 5.0 is more efficient than RavenDB 3.5, consuming fewer resources while giving you superior performance.<br/>
<br/>
<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>
<br/>
We are not bloatware, making sure that the memory consumption you have expected from RavenDB on your systems will not change. As we add new features to your database, we make sure RavenDB focuses on doing what a database should do and only adding features that enable you to process tomorrow's data faster.

When we roll out new features, we pay careful attention to design. We monitor memory usage to keep your CPU levels low. We won't hijack parts of your PC you need for vital operations.

## Unique Capabilities to Keep Your Systems Fit

RavenDB minimizes the need for third party components like full-text search and MapReduce by including these features in the database itself. Unique capabilities like bundled queries and [automatic indexes](https://ravendb.net/features/indexes/auto-indexes) keep performance robust even as we expanded from a Document Model Database to one that includes Graph, Key-Value, Distributed Counters, and Time Series.

Why add bloat with third party addons and other databases to accomplish what a single layer will provide?

You get the added benefit of being able to [run RavenDB efficiently on older and smaller machines](https://ravendb.net/articles/ravendb-point-of-sale-raspberry-pi-acid). Long-term projects budgeted for just one set of hardware can see that equipment become antiquated at the end of the project. A lightweight database keeps the database inside those ancient machines running at a cutting-edge pace.

You can also use RavenDB effectively on edge devices, which can carry minimal amounts of memory. As smaller devices proliferate for Big Data and IoT projects, RavenDB will empower you to [operate seamlessly across them](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/7-scaling-distributed-work-in-ravendb) without the unpleasant surprise of a new version requiring too much memory for these smaller devices to handle.

For 2020 and beyond, we at RavenDB are happy to keep the same yearly resolution: To stay lean.<br/>
<br/>
<div>
    <a href="https://ravendb.net/live-demo"><img src="images/see-for-yourself.png" class="img-responsive m-0-auto" alt="Schedule a one on one free demo of RavenDB"></a>
</div>
<br/>