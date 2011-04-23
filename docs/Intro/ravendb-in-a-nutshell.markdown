# RavenDB in a nutshell

RavenDB is a **transactional**, **open-source** **[Document Database](/docs/intro/what-is-a-document-database.html)** written in **.NET**, offering a **flexible data model** designed to address requirements coming from real-world systems. RavenDB allows you to build high-performance, low-latency applications quickly and efficiently.

Data in RavenDB is stored **schema-less** as JSON documents, and can be queried efficiently using **Linq** queries from .NET code or using **REST**ful API using other tools. Internally, RavenDB uses **Map/Reduce** indexes which are automatically created based on your usage, or were created explicitly by the consumer.

RavenDB is built for **web-scale**, and is offering **replication** and **sharding** support out-of-the-box.

There's a whole lot more of exciting features - check [RavenDB's features page](http://ravendb.net/features).

## Using RavenDB

RavenDB consists of a server and a client. The server handles data storage and queries, and the client is what a consumer uses to communicate with the server. There is more than one way to deploy RavenDB in your project:

* Server service on a Windows based machine
* Integrated with IIS
* Embedded client - embeds a RavenDB instance in your .NET application, web or desktop.

After you have a RavenDB server instance up and running, its easy to connect to it using the RavenDB client to store and retrieve your data. RavenDB works with your [POCO](http://en.wikipedia.org/wiki/Plain_Old_CLR_Object)s, meaning its super-easy to integrate it with your existing or newly-built application:

{CODE nutshell1@Intro\Nutshell.cs /}

As you may have noticed, RavenDB is using the [Unit of Work pattern](http://martinfowler.com/eaaCatalog/unitOfWork.html), so all changes made before calling session.SaveChanges() will not be persisted in the database. Also, all database calls within a session are fully transactional.

## Development cycle

There are two flavors of RavenDB available - the stable build, which is production ready, and the unstable build. Since we at Hibernating Rhinos make a build out of every push, the unstable build is not recommended for production, although it is being thoroughly tested.

New unstable builds are available daily, sometimes more than one a day. Stable builds are released whenever we feel comfortable enough with recent changes we made - usually when enough time has passed and a handful of people have used the unstable builds.

## Licensing and support

RavenDB is released as open-source under the AGPL license. What that means is it is freely available, but if you want to use this with proprietary software, you must buy a [commercial license](http://ravendb.net/licensing).

RavenDB has a very active [mailing list](http://groups.google.com/group/ravendb), where users and RavenDB developers attend all queries quickly and efficiently.

## Structure

Here's a graph showing the code structure for RavenDB:

![RavenDB code structure](images/ravendb_structure.png)

## Read more

// Links to docs...