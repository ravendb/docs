# Integrating with Akka.NET Persistence
---

{NOTE: }

* This article provides guidance on integrating RavenDB with Akka.Persistence.

* In this page:
   * [Overview](../../integrations/akka.net-persistence/integrating-with-akka-persistence#overview)
   * [Akka.Persistence.RavenDB](../../integrations/akka.net-persistence/integrating-with-akka-persistence#akka.persistence.ravendb)
   * [Installing the RavenDB persistence plugin](../../integrations/akka.net-persistence/integrating-with-akka-persistence#installing-the-ravendb-persistence-plugin)
   * [Configuring the RavenDB persistence plugin with HOCON](../../integrations/akka.net-persistence/integrating-with-akka-persistence#configuring-the-ravendb-persistence-plugin-with-hocon)
       * [Configuration keys](../../integrations/akka.net-persistence/integrating-with-akka-persistence#configuration-keys)
   * [Configuring the RavenDB persistence plugin with Hosting](../../integrations/akka.net-persistence/integrating-with-akka-persistence#configuring-the-ravendb-persistence-plugin-with-hosting)
       * [Syntax](../../integrations/akka.net-persistence/integrating-with-akka-persistence#syntax)

{NOTE/}

---

{PANEL: Overview}

**What is Akka.Net**  
[Akka.NET](https://getakka.net/) is a robust set of open-source libraries for building highly concurrent, distributed, and scalable applications on the .NET platform.
It employs the message-driven actor model to simplify concurrency management, resilience and fault isolation, making it easier to develop reliable systems.

**What is Akka.Persistence**  
Akka.Persistence is a library that extends the core functionality of Akka.NET by enabling durable state management for actors.
It allows the creation of actors whose internal state can be persisted and restored after an actor has restarted.

This durability is achieved through event sourcing, where state changes are stored as a sequence of events.  
Additionally, optional snapshots can capture the state at specific points in time for quicker recovery.  
Upon actor restart, the stored events and snapshots are replayed to restore the actor's internal state.

However, simply including Akka.Persistence only allows for persisting and recovering an actor's state from Akka's default **in-memory** store. 
This approach is insufficient if the entire application crashes or restarts, as the in-memory store would be lost.

**Using a persistence database plugin**  
To ensure durability across application restarts, use a dedicated plugin that allows the state to be persisted and replayed from an external database. 
Akka.NET supports various persistence stores through a plugin model, which specifies how an actor's state is persisted and recovered.

Ensuring that your actor’s data and any critical messages are persisted and recovered is paramount to building a reliable system. 
Persistence database plugins play a crucial role by providing the necessary mechanisms to achieve this reliability.

{PANEL/}

{PANEL: Akka.Persistence.RavenDB}

[Akka.Persistence.RavenDB](https://github.com/ravendb/Akka.Persistence.RavenDB) is a **persistence plugin** for Akka.NET that integrates RavenDB as the durable storage backend.

RavenDB is a NoSQL database designed for high performance, scalability, and ease of use.  
Among the available plugin options, RavenDB stands out as a highly efficient and flexible choice.  

By integrating the RavenDB plugin with Akka.Persistence, you can leverage RavenDB's powerful features  
to ensure that your actor state and critical messages are securely persisted and quickly recovered.

With the RavenDB plugin your application can:  

  * Persist and recover Events to/from a **Journal store**. Learn more in [Events](../../integrations/akka.net-persistence/events-and-snapshots#storing-events).
  * Persist and recover Snapshots to/from a **Snapshot store**. Learn more in [Snapshots](../../integrations/akka.net-persistence/events-and-snapshots#storing-snapshots). 
  * Query the stored events. Learn more in [Queries](../../integrations/akka.net-persistence/queries).

{PANEL/}

{PANEL: Installing the RavenDB persistence plugin}

Integrate RavenDB with Akka.Persistence using one of the two available NuGet packages:  

  * [Akka.Persistence.RavenDB](https://www.nuget.org/packages/Akka.Persistence.RavenDB)  
    This package allows you to configure the plugin solely through HOCON (Human-Optimized Config Object Notation), 
    which is typically embedded within your _app.config_ or _web.config_ file, or a dedicated HOCON file.
    
    {CODE-BLOCK: powershell}
# Installing via .NET CLI:
dotnet add package Akka.Persistence.RavenDB
    {CODE-BLOCK/}

  * [Akka.Persistence.RavenDB.Hosting](https://www.nuget.org/packages/Akka.Persistence.RavenDB.Hosting)  
    This package includes the base _Akka.Persistence.RavenDB_, offering greater flexibility 
    by allowing you to configure the plugin through **Hosting** or via a **HOCON** configuration file. 
    Using Hosting provides a fast and easy way to set up your app and its persistence without the need to configure HOCON.  
    
    {CODE-BLOCK: powershell}
# Installing via .NET CLI:
dotnet add package Akka.Persistence.RavenDB.Hosting
    {CODE-BLOCK/}

---

Installing either package will also install the _Akka.Persistence_ package.

{INFO: }
When configuring the plugin using both Hosting and HOCON, if the same parameters are specified in both,  
the configuration provided via Hosting takes precedence and will override the corresponding HOCON settings.
{INFO/}

{PANEL/}

{PANEL: Configuring the RavenDB persistence plugin with HOCON} 

* While both the journal and the snapshot-store share the same configuration keys, they reside in separate scopes.  
  So when configuring using HOCON, the settings for the journal and snapshot-store must be defined separately,  
  as shown in the example below.

* For example, properties `urls` and `name` can have the same values for both stores,  
  but they must still be defined distinctly within their respective sections.  
  Provide different values for each store as needed.  

* The following is a sample HOCON configuration under the `<akka>` section.  
  See the full description of each configuration key [below](../../integrations/akka.net-persistence/integrating-with-akka-persistence#configuration-keys).

{CODE:csharp hocon_config@Integrations\AkkaPersistence\hocon-configuration.cs /}

----

### Configuration keys 

{NOTE: }

#### Journal and snapshot config keys

---

Predefined plugins and class names to use:

* **journal.plugin**  
  The fully qualified name of the RavenDB plugin to be used for the journal store.  
  Value to set: `"akka.persistence.journal.ravendb"`   
* **journal.ravendb.class**  
  The fully qualified class name for the RavenDB persistence journal actor.  
  Value to set: `"Akka.Persistence.RavenDb.Journal.RavenDbJournal, Akka.Persistence.RavenDb"`  
* **snapshot-store.plugin**  
  The fully qualified name of the RavenDB plugin to be used for the snapshot store.  
  Value to set: `"akka.persistence.snapshot-store.ravendb"`  
* **snapshot-store.ravendb.class**  
  The fully qualified class name for the RavenDB persistence snapshot actor.  
  Value to set: `"Akka.Persistence.RavenDb.Snapshot.RavenDbSnapshotStore, Akka.Persistence.RavenDb"`  

---

Common config keys for journal and snapshot-store:

* **plugin-dispatcher**  
The dispatcher responsible for managing the thread pool and scheduling tasks for the actor.  
Default:  `"akka.actor.default-dispatcher"`  
* **urls**  
An array of server URLs where the RavenDb database is stored.  
Default: No default, param must be provided.  
e.g.: `["http://localhost:8080"]`  
* **name**  
The name of the database where the persistence data should be stored.  
It is recommended to create a separate database for Akka storage, distinct from your other work databases.  
Default: No default, param must be provided.  
e.g.: `"MyAkkaStorageDB"`  
* **auto-initialize**  
Create the database if it doesn't exist.   
No exception is thrown if the database already exists.  
Default: `false`
* **certificate-path**  
Location of a client certificate to access a secure RavenDB database.  
If a password is required, it should be stored in the `RAVEN_CERTIFICATE_PASSWORD` env variable.  
Default: `null`  
e.g.: `"\\path\\to\\cert.pfx"`  
* **save-changes-timeout**  
Timeout for 'save' requests sent to RavenDB, such as writing or deleting  
as opposed to stream operations which may take longer and have a different timeout (12h).  
Client will fail requests that take longer than this.  
Default: `30s`  
* **http-version**  
Http version for the RavenDB client to use in communication with the server.  
Default: `"2.0"`  
* **disable-tcp-compression**  
Determines whether to compress the data sent in the client-server TCP communication.   
Default: `false`  

{NOTE/}

{NOTE: }

#### Query config keys

---

* **query.ravendb.class**  
  The fully qualified class name for the RavenDB journal provider.  
  Value to set: `"Akka.Persistence.RavenDb.Query.RavenDbReadJournalProvider, Akka.Persistence.RavenDb"`  
* **refresh-interval**  
  The interval at which to check for new ids/events.  
  Default: `3s`  
* **max-buffer-size**  
  The number of events to keep buffered while querying until they are delivered downstream.  
  Default: `65536`

{NOTE/}
{PANEL/}

{PANEL: Configuring the RavenDB persistence plugin with Hosting}

* Using Hosting, you can easily set up the RavenDB plugin during your application's startup.

* Use method `WithRavenDbPersistence` to configure all relevant parameters.  
  See the available parameters and method overloads in the syntax section [below](../../integrations/akka.net-persistence/integrating-with-akka-persistence#syntax).

* The following example shows a basic configuration using Hosting:

{CODE:csharp basic_hosting_config@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}

---

#### Syntax

{CODE:csharp syntax_1@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}
{CODE:csharp syntax_2@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}

---

{CODE:csharp syntax_3@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}
{CODE:csharp syntax_4@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}

---

{CODE:csharp syntax_5@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}
{CODE:csharp syntax_6@Integrations\AkkaPersistence\integrating-with-akka-persistence.cs /}

{PANEL/}

## Related Articles

### Integrations

[Events and Snapshots](../../integrations/akka.net-persistence/events-and-snapshots)  
[Queries](../../integrations/akka.net-persistence/queries)  

### RavenDB Articles

[Using RavenDB Persistence in an Akka.NET application](https://ravendb.net/articles/using-ravendb-persistence-in-an-akka-net-application)
[Notes from integrating RavenDB with Akka.NET Persistence](https://ravendb.net/articles/notes-from-integrating-ravendb-with-akka-net-persistence)
