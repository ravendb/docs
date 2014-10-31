# Getting started

Welcome to this introductory article that will guide you through all the parts of RavenDB needed for basic understanding and simple setup.

## Starting server

For start, you need to have a RavenDB server running and there are couple of ways to acquire it:

- distribution package or installer can be downloaded from [ravendb.net](http://ravendb.net/download),
- [NuGet package](http://www.nuget.org/packages/RavenDB.Server/)

Now, we will focus on distribution package (if you want to read about installer go [here](../server/installation/using-installer)). The package contains following directories:

- `Backup` - contains [Raven.Backup](../server/administration/backup-and-restore) utility for doing backups,
- `Bundles` - contains all non built-in [plugins](../server/plugins/what-are-plugins) e.g. [Authorization](../server/bundles/authorization) or [Encryption](../server/bundles/encryption),
- `Client` - contains all .NET client libraries needed for development,
- `Server` - contains all server files (including configuration files such as [Raven.Server.exe.config](../server/configuration/configuration-options))
- `Smuggler` - contains utility for [importing and exporting](../server/administration/exporting-and-importing-data) data between servers,
- `Web` - contains all files needed for [IIS deployment](../server/installation/iis)

In package there is a file `Start.cmd` which will start server as a console application (aka [debug mode](../server/troubleshooting/running-in-debug-mode)) which is great for development or trying out various functionalities, but not suitable for production release. When server has started, [the Studio](../studio/accessing-studio) will be available at `http://localhost:<port>//`, by default RavenDB is configured to use 8080 port or next available if it is taken.

{INFO If you want to install RavenDB as a service please visit [this](../server/installation/as-a-service) article. There is also a possibility to install it on [IIS](../server/installation/iis) or run it as [embedded](../server/installation/embedded) instance. /}

<hr />

## Client

In `Client` directory (in distribution package), all .NET client libraries can be found. After referencing them in your project, all classes are found under `Raven.*` namespace with `DocumentStore` as the most interesting one. This is the main entry point for your application, that will establish and manage connection channel between your application and server. We urge you to read following articles:

- [What is a document store?](../client-api/what-is-a-document-store)
- [Creating document store](../client-api/creating-document-store)
- [Setting up default database](../client-api/setting-up-default-database)

{INFO Worth knowing is that `DocumentStore` is a heavyweight object and there should only be one instance of it per application (singleton). /}

There are two ways to manipulate data using the store, first one (and **recommended** one) is a [Session](../client-api/session/what-is-a-session-and-how-does-it-work), second one are [Commands](../client-api/commands/what-are-commands) which are a low-level way to manipulate data and should only be used when needed. Both `Session` and `Commands` contains asynchronous and synchronous methods.

Please read following articles if you want to get more details:

- [What is a session and how does it work?](../client-api/session/what-is-a-session-and-how-does-it-work) and [Opening a session](../client-api/session/opening-a-session)
- [What are commands?](../client-api/commands/what-are-commands)

<hr />

## Examples

Before we will proceed, we would like to point that in most of the articles `Northwind` database is being used. More details about it and information how to deploy it can be found [here](../start/about-examples).

<hr />

## Theory & few examples

RavenDB is a document database, which means that all stored objects are called `documents`. Each document contains **key** that identifies it, **data** and adjacent **metadata**, both stored in JSON format. The metadata contains various information that describes the document, e.g. modification date or [collection](../client-api/faq/what-is-a-collection) assignment.

### Creating Document Store, Opening a Session, Storing and Loading entities

Below example will demonstrate how to create `DocumentStore`, open a `Session`, store, then load few entities.

{CODE start_1@Start/GettingStarted.cs /}

### Querying

To satisfy queries, indexes must be used. In short words, index is a server-side function that defines using which fields (and what values) document can be searched on. The whole indexation process is done asynchronously, which gives a very quick response times, even when large amounts of data have changed, however implication of this approach is that index might be [stale](../indexes/stale-indexes). Before continuing we advise you to read following articles:

- [What are indexes?](../indexes/what-are-indexes)
- [Creating and deploying indexes](../indexes/creating-and-deploying)
- [Indexing basics](../indexes/indexing-basics)
- [Map indexes](../indexes/map-indexes)

This example assumes that your database contains `Northwind` sample data. If you are not sure how to deploy it, please visit [this](../studio/overview/tasks/create-sample-data) article.

{CODE start_2@Start/GettingStarted.cs /}

<hr />

## Few words about the documentation

The documentation has been divided into few sections:

- in `Indexes` you can find all the theory about indexes and querying,
- `Transformers` contains information about server-side transformation functions that can shape your query results,
- in `Client API` you can find API reference with basic examples for most of the methods available in client, also there is a lot of theory for how things work on client side
- `Server` contains information about administration, maintenance, configuration, installation and troubleshooting of the server,
- `Studio` - go here if you want to check what can be done using Studio