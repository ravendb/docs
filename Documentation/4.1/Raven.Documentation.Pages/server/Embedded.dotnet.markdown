# Server : Running an embedded instance

## Overview

RavenDB makes it very easy to be embedded within your application, with RavenDB Embedded package you 
don't need to do anything only download the package and start your own RavenDB Embedded server.
{CODE-TABS}
{CODE-TAB:csharp:Sync embedded_example@Server\Embedded.cs /}
{CODE-TAB:csharp:Async embedded_async_example@Server\Embedded.cs /}
{CODE-TABS/}

## Getting Started
---
* Create a new project (.NET Standard 2.0+, .NET Core 2.0+, .NET Framework 4.6.1+).
* Grab the [pre-release bits from MyGet](https://myget.org/feed/ravendb/package/nuget/RavenDB.embedded).
`Install-Package RavenDB.Embedded -Version 4.1.0 -Source https://www.myget.org/F/ravendb/api/v3/index.json` 
* Start a new Embedded Server
* Get the new Embedded Document Store, and start working with the database.

### Start The Server
RavenDB Embedded Server is available under `EmbeddedServer.Instance`. In order to start it call `StartServer` method.
{CODE start_server@Server\Embedded.cs /}

For more control on how to start the server just pass to `StartServer` method a `ServerOptions` object and that`s it.

{INFO:ServerOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **FrameworkVersion** | string | The framework version to run the server |
| **DataDirectory** | string | Where to store our database files |
| **DotNetPath** | string | The path to exec dotnet (If dotnet is in PATH leave it)|
| **AcceptEula** |  bool | If set to false, will ask to accept our terms |
| **ServerUrl** | string | What address we want to start our server (default 127.0.0.1:0) |
| **MaxServerStartupTimeDuration** | TimeSpan | The timeout for the server to start |
| **CommandLineArgs** | List&lt;string&gt; | The command lines arguments to start the server with |

{INFO /}


{CODE start_server_with_options@Server\Embedded.cs /}

{NOTE  Without the `ServerOptions`, RavenDB server will start with a default values on 127.0.0.1:{Random Port}  /}

##### Security
RavenDB Embedded support running a secured server.
Just run `Secured` method in `ServerOptions` object.

We have two overloads to `Secured`:
{CODE security@Server\Embedded.cs /}

The first way to enable authentication is to set certificate with the path to your .pfx 
server certificate. You may supply the certificate password using certPassword.

{CODE security2@Server\Embedded.cs /}

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. 
RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use.
RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.
In this options you can control on your client certificate and to use in a different certificate for your client.

### Get Document Store
After Starting the server you can get the DocumentStore from the Embedded Server and start working with RavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `GetDocumentStore` or `GetDocumentStoreAsync` with the name of the database you like to work with. 

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store@Server\Embedded.cs /}
{CODE-TABS/}

For more control on the process you can call the methods with `DatabaseOptions` object.

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **databaseRecord** | DatabaseRecord | Instance of `DatabaseRecord` containing database configuration |
| **SkipCreatingDatabase** | bool | If set to true, will skip try creating the database  |

{INFO /}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store_with_database_options@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store_with_database_options@Server\Embedded.cs /}}
{CODE-TABS/}

### Get Server Url
For getting the server url you only need to call `GetServerUriAsync` method, this async method will return `Uri` object.
If `StartServer` is not yet complete this method will wait for the server to start.

{CODE get_server_url_async@Server/Embedded.cs /}

## Remarks
* You can have only one instance of `EmbeddedServer`.
* {NOTE: Open RavenDB studio in the browser}
You can open the studio in the browser with `OpenStudioInBrowser` method
  {CODE open_in_browser@Server/Embedded.cs /}
{NOTE /}
*RavenDB Embedded by deafult runs the server with dotnet that can be found in PATH, if you want to use a different one
or if you don't have dotnet installed you can download it from [here](https://www.microsoft.com/net/download/dotnet-core/2.1),
and change `DotNetPath` from `ServerOptions` to the path of the new **dotnet.exe** and use it to run the server.
{CODE run_with_dotnet_path@Server\Embedded.cs /}

