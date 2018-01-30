# Getting Started

Welcome to this introductory article that will guide you through all the parts of RavenDB needed for basic understanding and simple setup.

This article consists of two parts:

- [Server](../start/getting-started#server) part will focus on installation, setup & configuration of RavenDB Server
- [Client](../start/getting-started#client) part will describe general principles behind our client libraries 

{PANEL: Server}

Let's start by installing and configuring the Server. In order to do that first we need to download the server package from the [downloads](https://ravendb.net/downloads) page.

RavenDB is cross-platform with a support for the following operating systems:

- Windows x64 / x86
- Linux x64
- Docker 
- MacOS
- Raspberry Pi

---

### Prerequisites

RavenDB is written in .NET Core, because of that it requires the same set of prerequisites as the .NET Core.

{NOTE: Windows}

Please install [Visual C++ 2015 Redistributable Package](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) (or newer) before launching the Server. This package should be the sole requirement for 'Windows' platforms, but in occurence of any troubles please check [.NET Core prerequisites for Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: Linux}

We highly recommend **updating** your **Linux OS** prior to launching an instance of RavenDB. Please also check if .NET Core does not require any other prerequisites. This can be checked in the [.NET Core prerequisites for Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: MacOS}

We highly recommend **updating** your **MacOS** and checking the [.NET Core prerequisites for MacOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites) article written by Microsoft prior to running the RavenDB Server.

{NOTE/}

---

### Installation & Setup

After extraction of the Server package you can start the [Setup Wizard](../start/installation/setup-wizard) by running the `run.ps1` (or `run.sh`) script or [disabling the 'Setup Wizard' and configuring the Server manually](../start/installation/manual).

{NOTE: Running in a Docker container}

If you are interested in hosting the Server in a Docker container. Please read our [dedicated article](../start/installation/running-in-docker-container).

{NOTE/}

---

### Configuration

RavenDB Server is using a `settings.json` file to store the server-wide configuration options. This file is located in the `Server` directory, but please note that after making changes to this file, a Server restart is required in order for them to be applied.

You can read more about the available configuration options in our dedicated article which can be found [here](../server/configuration/configuration-options).

{INFO:Default configuration}

The configuration file included in each RavenDB Server distribution package is as follows:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the Server will run:

- In `Setup Wizard` mode
- On `localhost` with a `random port`
- Store the data in `RavenData` directory.

{INFO/}

{WARNING:Port in Use}

In some cases the port might be in use. This will prevent the Server from starting with "address in use" error (`EADDRINUSE`).

The port can be changed by editing the `ServerUrl` value.

{WARNING/}

---

### Studio

{SAFE: Free}

Our GUI, the RavenDB Management Studio, comes **free** with **all the license types**:

- Community,
- Professional,
- Enterprise

{SAFE/}

After installation and setup, the Studio can be accessed via the browser using the `ServerUrl` or the `ServerPublicUrl` value e.g. `http://localhost:8080`.

---

### Security Concerns

To let a developer start coding an application quickly, RavenDB will run with the following default security mode:

{WARNING:Default Security Mode}

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration, and require the administrator to properly setup the security and 
access control to prevent unauthorized access to your data.

{WARNING/}

**RavenDB will not let you listen to requests outside your local machine until you have adequately provided security for it. We recommend using the Setup Wizard to easily install RavenDB securely from the very start.**  

Read more about security and how to enable authentication [here](../server/security/overview).

{PANEL/}

{PANEL:Client}

After your Server is up and running, to write an application you need to acquire one of the `Client` access libraries:

- .NET from [NuGet](https://www.nuget.org/packages/RavenDB.Client/)
- Java from [Maven](https://search.maven.org/#search%7Cga%7C1%7Cg%3A%22net.ravendb%22%20AND%20a%3A%22ravendb%22)
- Node.js from [NPM](https://www.npmjs.com/package/ravendb)
- Python from [PyPi](https://pypi.python.org/pypi/pyravendb)
- [Ruby](https://github.com/ravendb/ravendb-ruby-client)
- [Go](https://github.com/ravendb/ravendb-go-client)

<hr />

### DocumentStore

In order to start you need to create an instance of `DocumentStore` - the main entry point for your application which is responsible for establishing and managing connections between a RavenDB Server (or Cluster) and your application.

{INFO:Examples}

Before proceeding to the examples we would like to point out that most of the articles are using `Northwind` database. You can read more about it and how to deploy it [here](../studio/database/tasks/create-sample-data).

{INFO/}

{CODE-TABS}
{CODE-TAB:csharp:C# client_1@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_1@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_1@start\gettingStarted.js /}
{CODE-TABS/}

{INFO:Singleton}

The `DocumentStore` is capable of working on multiple databases and for proper operation we **recommend** having only one instance of it per application.

{INFO/}

Following articles can extend your knowledge about `DocumentStore` and its configuration:

- [What is a Document Store?](../client-api/what-is-a-document-store)
- [How to Create a Document Store?](../client-api/creating-document-store)
- [How to Setup a Default Database?](../client-api/setting-up-default-database)
- [How to configure Document Store using Conventions?](../client-api/configuration/conventions)

<hr />

### Session

The `Session` is used to manipulate the data. It implements the `Unit of Work` pattern and is capable of batching the requests to save expensive remote calls. In contrast to `DocumentStore` it is a lightweight object and can be created more frequently e.g. in Web applications a common (and recommended) pattern is to create a Session per each request.

### Example I - Storing

RavenDB is a Document Database, which means that all stored objects are called `documents`. Each document contains a **unique ID** that identifies it, **data** and adjacent **metadata**, both stored in JSON format. The metadata contains various information describing the document, e.g. the last modification date (`@last-modified` property) or the [collection](../client-api/faq/what-is-a-collection) (`@collection` property) assignment.

{CODE-TABS}
{CODE-TAB:csharp:C# client_2@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_2@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_2@start\gettingStarted.js /}
{CODE-TAB-BLOCK:python:Python}
...
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:ruby:Ruby}
...
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:go:Go}
...
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Loading

The `Session` was designed to help the user write a efficient code in as easy way as possible. For example, when document is being loaded (`.Load`) from the Server, there is an option to retrieve additional document in a single request (using `.Include`), reducing number of expensive calls to minimum.

Beside that Session implements `Unit of Work` pattern, meaning that all **changes** to loaded entities are **automatically tracked** so `SaveChanges` call will **synchronize with the Server only the ones that have changed within that Session**. Worth noting at this point is that **all of those changes are send in one request (saving network calls)** and **processed in one transaction** (you can read why RavenDB is an **ACID database** [here](../client-api/faq/transaction-support)).

{CODE-TABS}
{CODE-TAB:csharp:C# client_3@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_3@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_3@start\gettingStarted.js /}
{CODE-TABS/}

### Example III - Querying

To satisfy queries, indexes are used. From the querying perspective index defines which document fields can be used to find a document. The whole indexing process is done asynchronously, which gives a very quick querying response times, even when large amounts of data have been changed, however implication of this approach is that the index might be [stale](../indexes/stale-indexes).

When index is not specified in the query (e.g. like in the query bellow) then RavenDB is using its **intelligent auto-indexes** feature that will use already existing index or create new one if no match is found. The other options is to write the index yourself and deploy it to the Server, those indexes are called [Static](../indexes/creating-and-deploying#static-indexes).

Underneath all of the clients are translating the query to the Raven Query Language (RQL) syntax. If you are interested then you can read more about RQL [here](../indexes/querying/what-is-rql).

{CODE-TABS}
{CODE-TAB:csharp:C# client_4@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_4@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_4@start\gettingStarted.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Products
where UnitsInStock > 5
select Name
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Following articles can extend your knowledge about `Session`:

- [What is a Session and how does it work?](../client-api/session/what-is-a-session-and-how-does-it-work)
- [Opening a Session](../client-api/session/opening-a-session)
- [Storing Entities](../client-api/session/storing-entities)
- [Deleting Entities](../client-api/session/deleting-entities)
- [Loading Entities](../client-api/session/loading-entities)
- [Saving Changes](../client-api/session/saving-changes)

Best introductory articles describing `Querying` can be found here:

- [Basics](../indexes/querying/basics)
- [What is RQL?](../indexes/querying/what-is-rql)

If you are interested in `Indexes` subject then we recommend reading following articles:

- [Indexes: What are Indexes?](../indexes/what-are-indexes)
- [Indexes: Creating and deploying indexes?](../indexes/creating-and-deploying)
- [Indexes: Indexing basics](../indexes/indexing-basics)
- [Indexes: Map indexes](../indexes/map-indexes)

{PANEL/}
