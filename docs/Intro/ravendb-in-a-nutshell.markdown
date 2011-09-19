# RavenDB in a nutshell

RavenDB is a **transactional**, **open-source** [Document Database](what-is-a-document-database.markdown) written in **.NET**, offering a **flexible data model** designed to address requirements coming from real-world systems. RavenDB allows you to build high-performance, low-latency applications quickly and efficiently.

Data in RavenDB is stored **schema-less** as JSON documents, and can be queried efficiently using **Linq** queries from .NET code or using **RESTful** API using other tools. Internally, RavenDB make use of indexes which are automatically created based on your usage, or were created explicitly by the consumer.

RavenDB is built for **web-scale**, and is offering **replication** and **sharding** support out-of-the-box.

There's a whole lot more of exciting features - check [RavenDB's features page](http://ravendb.net/features).

## Using RavenDB

RavenDB consists of a server and a client. The server handles data storage and queries, and the client is what a consumer uses to communicate with the server. There is more than one way to deploy RavenDB in your project:

* Server service on a Windows based machine
* Integrated with IIS
* Embedded client - embeds a RavenDB instance in your .NET application, web or desktop.

After you have a RavenDB server instance up and running, its easy to connect to it using the RavenDB client to store and retrieve your data. RavenDB works with your [POCO](http://en.wikipedia.org/wiki/Plain_Old_CLR_Object)s, meaning its super-easy to integrate it with your existing or newly-built application:

{CODE nutshell1@Intro\Nutshell.cs /}

As you may have noticed, RavenDB is using the [Unit of Work pattern](http://martinfowler.com/eaaCatalog/unitOfWork.html), so all changes made before calling session.SaveChanges() will be persisted in the database in a single transaction.

## Building from source

RavenDB requires .NET 4.0 SDK installed to build. You should be able to just open Raven in Visual Studio 2010 and start working with it immediately.

Raven uses PowerShell to execute its build process. From the PowerShell prompt, execute: .\psake.ps1 default.ps1

You may need to allow script execution in your power shell configuration: 

  Set-ExecutionPolicy unrestricted

The build process will, by default, execute all the tests, which may take a while. You may skip the tests by executing: 
  
  .\psake.ps1 default -task ReleaseNoTests

## Development cycle

There are two flavors of RavenDB available - the stable build, which is production ready, and the unstable build. Since we at Hibernating Rhinos make a build out of every push, the unstable build is not recommended for production, although it is being thoroughly tested.

New unstable builds are available daily, sometimes more than once a day. Stable builds are released whenever we feel comfortable enough with recent changes we made - usually when enough time has passed and a handful of people have used the unstable builds. This is usually done on a biweekly basis.

In addition to the stable and unstable options, there is also the commercial version, which is the supported version for running in production.

## Reporting bugs

Bugs should be reported in the mailing list: [http://groups.google.com/group/ravendb](http://groups.google.com/group/ravendb).

When reporting a bug, please include:

* The version and build of Raven that you used (can be the build number, or the git commit hash).
* What did you try to do?
* What happened?
* What did you expect to happen?
* Details about your environment.
* Details about how to reproduce this error.

*Bugs that comes with a way for us to reproduce the program locally (preferably a unit test) tends to be fixed much more quickly.*

A list of outstanding bugs can be found here: http://github.com/ravendb/ravendb/issues

## Licensing and support

RavenDB is released as open-source under the AGPL license. What that means is it is freely available, but if you want to use this with proprietary software, you **must** buy a [commercial license](http://ravendb.net/licensing).

RavenDB has a very active [mailing list](http://groups.google.com/group/ravendb), where users and RavenDB developers attend all queries quickly and efficiently.

## Structure

Here's a graph showing the code structure for RavenDB:

![RavenDB code structure](images/ravendb_structure.png)

## Read more

// Links to docs...