# Migration: Changes in DocumentStore

This article describes the changes in public API of `DocumentStore`.

{PANEL:Namespace}

`IDocumentStore` can be referenced using `Raven.Client.Documents` (previously `Raven.Client`).

{PANEL/}

{PANEL:DefaultDatabase}

The `DefaultDatabase` property has been renamed to `Database`.

{PANEL/}

{PANEL:Credentials and ApiKey}

The support for WindowsAuth and OAuth has been dropped. RavenDB 4.0 uses X.509 certificate-based authentication.

{PANEL/}

{PANEL:Url and FailoverServers}

When initializing `DocumentStore`, you can provide multiple URLs to RavenDB servers holding your database in the cluster. It will grab the cluster topology from the first accessible server. 

{CODE urls_1@Migration\ClientApi\DocumentStoreChanges.cs /}  

{PANEL/}

{PANEL:ConnectionStringName}

As the configuration system has been changed in .NET Core, we removed the `ConnectionStringName` property. Instead you can use the .NET core configuration mechanism, retrieve the connection string entry from `appsettings.json`, convert it, and manually set `Urls` and `Database` properties.

{PANEL/}

{PANEL:Listeners}

All listeners have been removed in favor of events.

- Usage of the events:   

{CODE events_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

- Document conflicts are solved only on the server side using conflict resolution scripts defined in a database.

{PANEL/}

{PANEL:JsonRequestFactory}

Instead of `JsonRequestFactory`, the `IDocumentStore` instance has `RequestExecutor`. Using it you can:

  - send any command:

{CODE request_executor_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

  - check the number of cached items or clear the cache (cache size capacity can be configured via `MaxHttpCacheSize` convention):

{CODE request_executor_2@Migration\ClientApi\DocumentStoreChanges.cs /}   

  - check the number of sent requests:

{CODE request_executor_3@Migration\ClientApi\DocumentStoreChanges.cs /}   

- change default request timeout:

{CODE request_executor_4@Migration\ClientApi\DocumentStoreChanges.cs /}  

The timeout change can be also scoped with the usage of:

{CODE request_executor_5@Migration\ClientApi\DocumentStoreChanges.cs /} 

{PANEL/}

{PANEL:Conventions}

All conventions needs to be set before `DocumentStore.Initialize` is called. Otherwise, an `InvalidOperationException` will be thrown.

Read more about changes in document conventions in [dedicated article](../../migration/client-api/conventions).

{PANEL/}
