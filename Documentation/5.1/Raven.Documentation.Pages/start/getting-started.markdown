# Getting Started
---

{NOTE: }

Welcome to RavenDB! 

This article will get you started and guide you through all the aspects of RavenDB needed for basic understanding and a simple setup.  

* In this page:
   * The [Server](../start/getting-started#server) portion will focus on installation, setup & configuration of the RavenDB server  
      * [Prerequisites](../start/getting-started#prerequisites)  
      * [Installation & Setup](../start/getting-started#installation--setup)  
      * [Configuration](../start/getting-started#configuration)  
      * [Studio](../start/getting-started#studio)  
      * [Security Concerns](../start/getting-started#security-concerns)  
   * The [Client](../start/getting-started#client) portion will describe the general logic and principles behind our client libraries 
      * [DocumentStore](../start/getting-started#documentstore)  
      * [Session](../start/getting-started#session)  

{NOTE/}

{PANEL: Server}

Let's start by installing and configuring the server. To do that, first we need to download the server package 
from the [downloads](https://ravendb.net/downloads) page.

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

We highly recommend **updating** your **Linux OS** prior to launching the RavenDB server. Also, check if .NET Core requires any other prerequisites in the [Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: MacOS}

We highly recommend **updating** your **MacOS** and checking the [Prerequisites for .NET Core on macOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites) article written by Microsoft before launching the RavenDB Server.

{NOTE/}

---

### Installation & Setup

{NOTE: Highly Available Clusters}

We recommend setting up your cluster nodes on separate machines so that if one goes down, the others can keep the cluster active. 

{NOTE/}

1. Set up a server folder on each machine that will host the nodes in your cluster. 
 You may want to include the node designation (nodes A, B, C...) in the name of each server folder, to prevent future confusion.  

2. Extract the server package into permanent server folders on each machine.  
 Each folder that contains an extracted server package will become a functional node in your cluster.  
 If you've set up on separate machines, go to step 3 below.  

{WARNING: Important:} If you move this folder after installation, the server will not run.  
You'll receive a 'System.InvalidOperationException: Unable to start the server.' error because it will look for the file path that is set 
when you install. If you must move your folder at a later time, you can [reconfigure the certificate file path](../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally) 
in the `settings.json` file.
{WARNING/}

If you choose to use only one machine (although this will increase the chances of your cluster going down) you'll need to:

1. Set up a parent folder in a permanent location for your installation package and server settings for the next steps.  
2. Set up separate folders in the parent folder for each node and keep it in a safe place for future use.  
  ![Cluster Parent/Nodes Folder](images/Cluster-Parent-Nodes-Folders.png "Cluster Parent/Nodes Folder")  

3. Extract the [downloaded](https://ravendb.net/downloads) `RavenDB...zip` server package into each node folder.  
4. If you want to install the cluster **as a service** (it will improve availability because it will automatically run in the background every time your 
machine restarts), this simple step will be done after initial secure installation via the Setup Wizard or manually. Read [Running as a Service](installation/running-as-service).  
5. Start the [Setup Wizard](../start/installation/setup-wizard) by running `run.ps1` (or `run.sh` in Linux) in PowerShell or [disable the 'Setup Wizard' and configuring the server manually](../start/installation/manual).  
![Running the Setup Wizard](images/run-ps1-with-PowerShell.png "Running the Setup Wizard")

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

{INFO: Help Us Improve Prompt}
When you first launch RavenDB, you will see this prompt asking if you'd be willing to 
anonymously share some Studio usage data with us in order to help us improve RavenDB:  

![NoSQL Database Share Studio Usage](images/help-us-improve.png "Help Us Improve")

Once you respond to this prompt, it should not appear again. However, in some scenarios, 
such as running RavenDB embedded, or working without browser cookies, the prompt may 
appear again.  

If necessary, you can add this flag to the Studio URL to prevent the prompt from 
appearing:  

`<Studio URL>#dashboard?disableAnalytics=true`

{INFO/}

---

### Configuration

The RavenDB server uses a [settings.json](../server/configuration/configuration-options#settings.json) file in each node `Server` folder to store the server-wide configuration options.  
When starting a server, RavenDB will look for the `settings.json` file in the node `Server` folder, so it must be located there.  
The [Setup Wizard](../start/installation/setup-wizard) places it correctly automatically.  

After making changes to this file, a server restart is required for them to be applied.  

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

This means that the server will run:

- On `localhost` with a `random port`
- In `Setup Wizard` mode
- Store the data in the `RavenData` directory.

{INFO/}

{WARNING: Port in Use}

In some cases, the port might be in use. This will prevent the Server from starting with an "address in use" error (`EADDRINUSE`).

The port can be changed by editing the `ServerUrl` value in the `settings.json` file.  
For a list of IPs and ports already in use, run `netstat -a` in the command line.

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
Whenever you run the server folder script `run.ps1` the Studio opens automatically in your browser.  

---

### Security Concerns

**We recommend using the 'Setup Wizard' to easily install RavenDB securely from the very start** to prevent potential future vulnerability.  
[The process](../start/getting-started#installation--setup) in RavenDB only takes a few minutes and is free.  

To let a developer start coding an application quickly, RavenDB will run with the following default security mode:

{WARNING: Default Security Mode}

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration and require the administrator to properly set up the security and 
access control to prevent unauthorized access to your data or to explicitly allow the unsecured configuration.

{WARNING/}

**We recommend using the 'Setup Wizard' to easily install RavenDB securely from the very start** to prevent potential future vulnerability.  The process takes a few minutes and is free.    

Read more about security and how to [enable authentication here](../server/security/overview).

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

The `Session` was designed to help the user write efficient code easily. For example, when a document is being loaded (`.Load`) from the server, there is an option [to retrieve additional documents in the same request](../client-api/session/loading-entities#load-with-includes) (using `.Include`), minimizing the number of expensive calls.

Besides that, the session implements the `Unit of Work` pattern, meaning that all **changes** to loaded entities are **automatically tracked**. The `SaveChanges` call will synchronize (with the server) **only the documents that have changed within the session**. All of those changes are **sent in one request (saving network calls)** and **processed in one transaction** (you can read why RavenDB is an [ACID database here](../client-api/faq/transaction-support)).

{CODE-TABS}
{CODE-TAB:csharp:C# client_3@Start/GettingStarted.cs /}
{CODE-TAB:java:Java client_3@Start\GettingStarted.java /}
{CODE-TAB:nodejs:Node.js client_3@start\gettingStarted.js /}
{CODE-TAB:python:Python client_3@start\getting_started.py /}
{CODE-TABS/}

### Example III - Querying

To satisfy queries, [indexes](../indexes/what-are-indexes) are used. From the querying perspective, an index defines which document fields can be used to find a document. The whole indexing process is done asynchronously, which gives very quick querying response times, even when large amounts of data have been changed. However, an implication of this approach is that the index might be [stale](../indexes/stale-indexes).

When no index is specified in the query (like in the query below), RavenDB will use its [intelligent auto-indexes](../indexes/creating-and-deploying#auto-indexes) feature that will either use an already existing index or create a new one if no match is found.

The other option is to write the index yourself and deploy it to the server. Those indexes are called [Static Indexes](../indexes/creating-and-deploying#static-indexes).

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

**Session** - The following articles can extend your knowledge about the Session:

- [What is a Session and how does it work?](../client-api/session/what-is-a-session-and-how-does-it-work)
- [Opening a Session](../client-api/session/opening-a-session)
- [Storing Entities](../client-api/session/storing-entities)
- [Deleting Entities](../client-api/session/deleting-entities)
- [Loading Entities](../client-api/session/loading-entities)
- [Saving Changes](../client-api/session/saving-changes)

**Querying** - The introductory articles describing Querying can be found here:

- [Basics](../indexes/querying/basics)
- [What is RQL?](../indexes/querying/what-is-rql)

**Indexes** - If you wish to understand Indexes better, we recommend reading the following articles:

- [Indexes: What are indexes?](../indexes/what-are-indexes)
- [Indexes: Creating and deploying indexes](../indexes/creating-and-deploying) (RavenDB's Auto-Indexing and how to set up static indexes)  
- [Indexes: Indexing basics](../indexes/indexing-basics)
- [Indexes: Map indexes](../indexes/map-indexes)

{PANEL/}

## Related Articles

### Installation

- [Setup Wizard](../start/installation/setup-wizard)
- [System Requirements](../start/installation/system-requirements)
- [Running in a Docker Container](../start/installation/running-in-docker-container)

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
