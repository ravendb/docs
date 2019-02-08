# Getting started

Welcome to this introductory article that will guide you through all the parts of RavenDB needed for basic understanding and simple setup.

## Starting server

For starters, you need to have a RavenDB server running. There are couple of ways you can acquire it:

- Distribution package or installer which can be downloaded from [ravendb.net](https://ravendb.net/download)
- [NuGet package](http://www.nuget.org/packages/RavenDB.Server/)

{NOTE In RavenDB 3.5 we had the distribution package split into two ZIP packages to make things more comfortable and have a separate Tools package. /}

Now, we will focus on the distribution package (if you want to read about the installer go [here](../server/installation/using-installer)).   
There are 2 zip files available, the main zip contains the following:

- `Bundles` - contains all non built-in [plugins](../server/plugins/what-are-plugins) e.g. [Authorization](../server/bundles/authorization) or [Encryption](../server/bundles/encryption),
- `Client` - contains all .NET client libraries needed for development,
- `Server` - contains all server files (including configuration files such as [Raven.Server.exe.config](../server/configuration/configuration-options))
- `Web` - contains all files needed for [IIS deployment](../server/installation/iis)

The package contains `Start.cmd` which will start a server as a console application (aka [debug mode](../server/troubleshooting/running-in-debug-mode)), 
which is great for development purposes or simply to try out various functionalities, but not for production release. When server has started, [the Studio](../studio/accessing-studio) 
will be available at `http://localhost:<port>/`. RavenDB is configured by default to use port 8080, or the next available, if this is one is occupied.   

The second zip file called `Tools` is for advanced users and contains the following:

- `Raven.Backup.exe` - A utility for doing backups, [read more](../server/administration/backup-and-restore). 
- `Raven.Smuggler.exe` - A utility for exporting and importing data between servers, [read more](../server/administration/exporting-and-importing-data).
- `Raven.Monitor.exe` - A utility for monitoring RavenDB I/O disk rates, [read more](../server/administration/monitoring/disk-io-perf-monitor). 
- `Raven.StorageExporter.exe` - A utility for exporting a database directly from the Esent / Voron data files, [read more](../server/administration/storage-exporter).
- `Raven.Traffic.exe` - A utility to record and replay requests that are being received by RavenDB, [read more](../server/administration/monitoring/request-tracking). 
- `Raven.ApiToken.exe` - A utility to generate an ApiToken, not through the studio.

{INFO If you want to install RavenDB as a service, please visit [this](../server/installation/as-a-service) article. There is also a possibility to install it on [IIS](../server/installation/iis) or run it as an [embedded](../server/installation/embedded) instance. /}

<hr />

## Client

In the `Client` directory (in the distribution package), all .NET client libraries can be found. After referencing them in your project, all classes are found under the `Raven.*` namespace, with the `DocumentStore` as the most interesting one. This is the main entry point for your application, which will establish and manage connection channel between your application and a server. We recommend reading the following articles:

- [What is a document store?](../client-api/what-is-a-document-store)
- [Creating a document store](../client-api/creating-document-store)
- [Setting up a default database](../client-api/setting-up-default-database)

{INFO Worth knowing is that the `DocumentStore` is a heavyweight object and there should only be one instance of it per application (singleton). /}

There are two ways to manipulate data using the store, first one (and the **recommended** one) is a [Session](../client-api/session/what-is-a-session-and-how-does-it-work), second one are [Commands](../client-api/commands/what-are-commands), which are a low-level way to manipulate data and should only be used when needed. Both the `Session` and the `Commands` contain asynchronous and synchronous methods.

Please read the following articles if you want to get more details:

- [What is a session and how does it work?](../client-api/session/what-is-a-session-and-how-does-it-work) and [Opening a session](../client-api/session/opening-a-session)
- [What are commands?](../client-api/commands/what-are-commands)

<hr />

## Examples

Before we will proceed, we would like to point out that in most of the articles the `Northwind` database is being used. More details about it and information on how to deploy it can be found [here](../start/about-examples).

<hr />

## Theory & few examples

RavenDB is a document database, which means that all stored objects are called `documents`. Each document contains a **key** that identifies it, **data**, and adjacent **metadata**, both stored in JSON format. The metadata contains various information describing the document, e.g. the modification date or the [collection](../client-api/faq/what-is-a-collection) assignment.

### Creating Document Store, Opening a Session, Storing and Loading entities

The example below will demonstrate how to create the `DocumentStore`, open a `Session`, store, and load few entities.

{CODE start_1@Start/GettingStarted.cs /}

### Querying

To satisfy queries, indexes must be used. In short, index is a server-side function that defines using which fields (and what values) document can be searched on. The whole indexation process is done asynchronously, which gives a very quick response times, even when large amounts of data have been changed, however implication of this approach is that the index might be [stale](../indexes/stale-indexes). Before you continue, we advise reading the following articles:

- [What are indexes?](../indexes/what-are-indexes)
- [Creating and deploying indexes](../indexes/creating-and-deploying)
- [Indexing basics](../indexes/indexing-basics)
- [Map indexes](../indexes/map-indexes)

This example assumes that your database contains the `Northwind` sample data. If you are not sure how to deploy it, please visit [this](../studio/overview/tasks/create-sample-data) article.

{CODE start_2@Start/GettingStarted.cs /}

<hr />

## Few words about the documentation

The documentation has been divided into few sections:

- in `Indexes` you can find all the theory about indexes and querying,
- `Transformers` contain information about server-side transformation functions that can shape your query results,
- in `Client API` you can find API reference with basic examples for most of the methods available in client. There is also a lot of theory on how things work on client side,
- `Server` contains information about administration, maintenance, configuration, installation, and troubleshooting of the server,
- `Studio` - go there if you want to check what can be done using the Studio

<hr />

## Samples

The following sample applications are available:

- [MVC Starter Kit](../samples/mvc-starter-kit)

## Playground Server

Please visit [this](../start/playground-server) dedicated article if you are interested in this topic.