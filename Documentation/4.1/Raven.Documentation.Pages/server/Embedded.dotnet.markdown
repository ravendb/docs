# RavenDB Embedded

## Overview

RavenDB makes it very easy to be embedded within your application, with RavenDB Embedded package you 
don't need to do anything only download the package and start your own RavenDB Embedded server.
{CODE-TABS}
{CODE-TAB:csharp:Sync embedded_example@Server\Embedded.cs /}
{CODE-TAB:csharp:Async embedded_async_example@Server\Embedded.cs /}
{CODE-TABS/}

## Getting Started
---
* Create a new project (.NET Core, .Net Framework, whatever).
* Grab the [pre-release bits from MyGet](https://myget.org/feed/ravendb/package/nuget/RavenDB.embedded).
Install-Package RavenDB.Embedded -Version 4.1.0 -Source https://www.myget.org/F/ravendb/api/v3/index.json
* Start a new Embedded Server
* Ask the embedded client for a document store, and start working with documents.

### Start The Server
To start a RavenDB Embedded server you only need to get the Instance of the Embedded Server and then call `StartServer` method.
{CODE start_server@Server\Embedded.cs /}

For more control on how to start the server just pass to `StartServer` method a [`ServerOptions`](../../csharp/glossary/server-options) object and that`s it.
{CODE start_server_with_options@Server\Embedded.cs /}

{NOTE  Without the `ServerOptions`, RavenDB server will start with a default values on 127.0.0.1:{Random Port}  /}

##### Security
RavenDB Embedded support running a secured server.
Just run `Secured` method in [`ServerOption`](../../csharp/glossary/server-options) object.
We have two overloads to `Secured`:
{CODE security@Server\Embedded.cs /}

The first way to enable authentication is to set certificate with the path to your .pfx 
server certificate. You may supply the certificate password using certPassword.

{CODE security2@Server\Embedded.cs /}

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use.
RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.
In this options you can control on your client certificate and to use in a different certificate for your client.

### Get Document Store
After Starting the server you can get the DocumentStore from the Embedded Server and start working with RavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `GetDocumentStore` or `GetDocumentStoreAsync` with the name of the database you like to work with. 

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store@Server\Embedded.cs /}
{CODE-TABS/}

For more control on the process you can call the methods with [`DatabaseOptions`](../../csharp/glossary/database-options) object.

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store_with_database_options@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store_with_database_options@Server\Embedded.cs /}}
{CODE-TABS/}

### Get Server Url
For getting the server url you only need to call `GetServerUriAsync` method, this async method will return `Uri` object.
If `StartServer` is not yet complete this method will wait for the server to start.

{CODE get_server_url_async@Server/Embedded.cs /}

## Acknowledgments
* You can have only one instance of `EmbeddedServer`.
* {NOTE: Open RavenDB studio in the browser}
You can open the studio in the browser with `OpenStudioInBrowser` method
  {CODE open_in_browser@Server/Embedded.cs /}
{NOTE /}
* If you don't have dotnet install you can download it from [here](https://www.microsoft.com/net/download/dotnet-core/runtime-2.1.1) 
and used it to run the server with.
{CODE run_with_dotnet_path@Server\Embedded.cs /}

