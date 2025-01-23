# Getting Started

Welcome to RavenDB! 

This article will get you started and guide you through all the parts of RavenDB needed for basic understanding and simple setup. It consists of two parts:

- The [Server](../start/getting-started#server) part will focus on installation, setup & configuration of the RavenDB server
- The [Client](../start/getting-started#client) part will describe the general principles behind our client libraries 

{PANEL: Server}

Let's start by installing and configuring the server. In order to do that first we need to download the server package from the [downloads](https://ravendb.net/downloads) page.

RavenDB is cross-platform with support for the following operating systems:

- Windows x64 / x86
- Linux x64
- Docker 
- MacOS
- Raspberry Pi

---

### Prerequisites

RavenDB is written in .NET Core so it requires the same set of prerequisites as .NET Core.

{NOTE: Windows}

Please install [Visual C++ 2015 Redistributable Package](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) (or newer) before launching the RavenDB server. This package should be the sole requirement for 'Windows' platforms. If you're experiencing difficulties, please check the [Prerequisites for .NET Core on Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: Linux}

We highly recommend **updating** your **Linux OS** prior to launching the RavenDB server. Also check if .NET Core requires any other prerequisites in the [Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: MacOS}

We highly recommend **updating** your **MacOS** and checking the [Prerequisites for .NET Core on macOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites) article written by Microsoft before launching the RavenDB Server.

{NOTE/}

---

### Installation & Setup

After extraction of the server package, you can start the [Setup Wizard](../start/installation/setup-wizard) by running the `run.ps1` (or `run.sh`) script or by [disabling the 'Setup Wizard' and configuring the server manually](../start/installation/manual).

{NOTE: Running in a Docker container}

If you are interested in hosting the server in a Docker container, please read our [dedicated article](../start/installation/running-in-docker-container).

{NOTE/}

{NOTE: Running in a VM}

If you are interested in hosting the server on a VM, please refer to

- [AWS Windows VM](../start/installation/setup-examples/aws-windows-vm)
- [AWS Linux VM](../start/installation/setup-examples/aws-linux-vm)

{NOTE/}

{NOTE: Running in RavenDB Cloud}

If you want to test RavenDB without manual setup try [RavenDB Cloud](https://cloud.ravendb.net).
We offer one free instance per customer. For more information, please read our [dedicated article](../cloud/cloud-overview).

{NOTE/}

---

### Configuration

The RavenDB server is using a [settings.json](../server/configuration/configuration-options#settings.json) file to store the server-wide configuration options. This file is located in the `Server` directory, but please note that after making changes to this file, a server restart is required in order for them to be applied.

You can read more about the available configuration options in our [dedicated article](../server/configuration/configuration-options).

{INFO:Default configuration}

The configuration file included in each RavenDB server distribution package is as follows:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the server will run:

- On `localhost` with a `random port`
- In `Setup Wizard` mode
- Store the data in the `RavenData` directory.

{INFO/}

{WARNING: Port in Use}

In some cases the port might be in use. This will prevent the Server from starting with an "address in use" error (`EADDRINUSE`).

The port can be changed by editing the `ServerUrl` value.

{WARNING/}

{NOTE: Write Permissions}

RavenDB requires write permissions to the following locations:

- The folder where RavenDB server is running (to update [settings.json](../server/configuration/configuration-options#settings.json) by the [Setup Wizard](../start/installation/setup-wizard))
- The data folder ([`DataDir`](../server/configuration/core-configuration#datadir) setting)
- The logs folder ([`Logs.Path`](../server/configuration/logs-configuration#logs.path) setting)

If you intend to run as a service, the write permissions should be granted to the user running the service (e.g. "Local Service").

{NOTE/}

---

### Studio

{SAFE: Free}

Our GUI, the RavenDB Management Studio, comes **free** with **every license type**:

- Community
- Professional
- Enterprise

{SAFE/}

After installation and setup, the Studio can be accessed via the browser using the `ServerUrl` or the `ServerPublicUrl` value e.g. `http://localhost:8080`.

---

### Security Concerns

To let a developer start coding an application quickly, RavenDB will run with the following default security mode:

{WARNING: Default Security Mode}

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration and require the administrator to properly setup the security and 
access control to prevent unauthorized access to your data or to explicitly allow the unsecured configuration.

{WARNING/}

**We recommend using the 'Setup Wizard' to easily install RavenDB securely from the very start.**  

Read more about security and how to enable authentication [here](../server/security/overview).

{PANEL/}

{PANEL: Client}

After your server is up and running, to write an application you need to acquire one of the `Client` access libraries:

- .NET from [NuGet](https://www.nuget.org/packages/RavenDB.Client/)
- Java from [Maven](https://search.maven.org/#search%7Cga%7C1%7Cg%3A%22net.ravendb%22%20AND%20a%3A%22ravendb%22)
- Node.js from [NPM](https://www.npmjs.com/package/ravendb)
- Python from [PyPi](https://pypi.org/project/pyravendb/)
- [Ruby](https://github.com/ravendb/ravendb-ruby-client)
- [Go](https://github.com/ravendb/ravendb-go-client)

<hr />

### DocumentStore

In order to start, you need to create an instance of the `DocumentStore` - the main entry point for your application which is responsible for establishing and managing connections between a RavenDB server (or cluster) and your application.

{INFO: Examples}

Before proceeding to the examples, we would like to point out that most of the articles are using the `Northwind` database. You can read more about it and how to deploy it [here](../studio/database/tasks/create-sample-data).

{INFO/}

{CODE-TABS}
{CODE-TAB:csharp:C# client_1@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_1@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_1@start\gettingStarted.js /}
{CODE-TAB:python:Python client_1@start\getting_started.py /}
{CODE-TABS/}

{INFO: Singleton}

The `DocumentStore` is capable of working with multiple databases and for proper operation we **recommend** having only one instance of it per application.

{INFO/}

The following articles can extend your knowledge about the `DocumentStore` and its configuration:

- [What is a Document Store?](../client-api/what-is-a-document-store)
- [How to Create a Document Store?](../client-api/creating-document-store)
- [How to Setup a Default Database?](../client-api/setting-up-default-database)
- [How to configure the Document Store using Conventions?](../client-api/configuration/conventions)

<hr />

### Session

The `Session` is used to manipulate the data. It implements the `Unit of Work` pattern and is capable of batching the requests to save expensive remote calls. In contrast to a `DocumentStore` it is a lightweight object and can be created more frequently. For example, in web applications, a common (and recommended) pattern is to create a session per request.

### Example I - Storing

RavenDB is a Document Database. All stored objects are called `documents`. Each document contains a **unique ID** that identifies it, **data** and adjacent **metadata**, both stored in JSON format. The metadata contains information describing the document, e.g. the last modification date (`@last-modified` property) or the [collection](../client-api/faq/what-is-a-collection) (`@collection` property) assignment.

{CODE-TABS}
{CODE-TAB:csharp:C# client_2@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_2@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_2@start\gettingStarted.js /}
{CODE-TAB:python:Python client_2@start\getting_started.py /}
{CODE-TABS/}

### Example II - Loading

The `Session` was designed to help the user write efficient code easily. For example, when a document is being loaded (`.Load`) from the server, there is an option to retrieve additional documents in the same request (using `.Include`), keeping the number of expensive calls to minimum.

Besides that, the session implements the `Unit of Work` pattern, meaning that all **changes** to loaded entities are **automatically tracked**. The `SaveChanges` call will synchronize (with the server) **only the documents that have changed within the session**. **All of those changes are sent in one request (saving network calls)** and **processed in one transaction** (you can read why RavenDB is an **ACID database** [here](../client-api/faq/transaction-support)).

{CODE-TABS}
{CODE-TAB:csharp:C# client_3@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_3@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_3@start\gettingStarted.js /}
{CODE-TAB:python:Python client_3@start\getting_started.py /}
{CODE-TABS/}

### Example III - Querying

To satisfy queries, indexes are used. From the querying perspective, an index defines which document fields can be used to find a document. The whole indexing process is done asynchronously, which gives very quick querying response times, even when large amounts of data have been changed. However, an implication of this approach is that the index might be [stale](../indexes/stale-indexes).

When no index is specified in the query (like in the query below), RavenDB will use its **intelligent auto-indexes** feature that will either use an already existing index or create a new one if no match is found. The other option is to write the index yourself and deploy it to the server. Those indexes are called [Static Indexes](../indexes/creating-and-deploying#static-indexes).

Behind the scenes, queries are translated to the Raven Query Language (RQL) syntax. Read more about RQL [here](../indexes/querying/what-is-rql).

{CODE-TABS}
{CODE-TAB:csharp:C# client_4@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_4@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_4@start\gettingStarted.js /}
{CODE-TAB:python:Python client_4@start\getting_started.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Products
where UnitsInStock > 5
select Name
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The following articles can extend your knowledge about the `Session`:

- [What is a Session and how does it work?](../client-api/session/what-is-a-session-and-how-does-it-work)
- [Opening a Session](../client-api/session/opening-a-session)
- [Storing Entities](../client-api/session/storing-entities)
- [Deleting Entities](../client-api/session/deleting-entities)
- [Loading Entities](../client-api/session/loading-entities)
- [Saving Changes](../client-api/session/saving-changes)

The introductory articles describing `Querying` can be found here:

- [Basics](../indexes/querying/basics)
- [What is RQL?](../indexes/querying/what-is-rql)

If you wish to understand `Indexes` better, we recommend reading the following articles:

- [Indexes: What are indexes?](../indexes/what-are-indexes)
- [Indexes: Creating and deploying indexes?](../indexes/creating-and-deploying)
- [Indexes: Indexing basics](../indexes/indexing-basics)
- [Indexes: Map indexes](../indexes/map-indexes)

{PANEL/}

## Related Articles

### Installation

- [Setup Wizard](../start/installation/setup-wizard)
- [System Requirements](../start/installation/system-requirements)

### Client API

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)
- [Opening a Session](../client-api/session/opening-a-session)
- [Storing Entities](../client-api/session/storing-entities)
- [Deleting Entities](../client-api/session/deleting-entities)
- [Loading Entities](../client-api/session/loading-entities)
- [Saving Changes](../client-api/session/saving-changes)

### Querying

- [Basics](../indexes/querying/basics)
- [What is RQL](../indexes/querying/what-is-rql)

### Indexes

- [What are Indexes](../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
- [Indexing Basics](../indexes/indexing-basics)
- [Map Indexes](../indexes/map-indexes)
