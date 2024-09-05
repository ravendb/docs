# Server: Running an Embedded Instance

{PANEL:Overview}

RavenDB makes it very easy to be embedded within your application, with RavenDB Embedded package you can integrate your RavenDB server with few easy steps.

{CODE:java embedded_example@Server\Embedded.java /}

{PANEL/}

{PANEL:Prerequisites}

There is one prerequsite and one recommendation for the Embedded package:

Prerequsite:

- **.NET Core runtime** must be installed manually

{NOTE:.NET Core Runtime}

RavenDB Embedded **does not include .NET Core runtime required for it to run**. 

By default the `ServerOptions.FrameworkVersion` is set to the .NET Core version that we compiled the server with and `ServerOptions.DotNetPath` is set to `dotnet` meaning that it will require to have it declared in PATH. 

We highly recommend using the .NET Core framework version defined in `ServerOptions.FrameworkVersion` for proper functioning of the Server. The .NET Core runtime can be downloaded from [here](https://dotnet.microsoft.com/download).

{NOTE/}

{PANEL/}

{PANEL:Getting Started}

### Installation

* Create a new project 
* Add package [net.ravendb:ravendb-embedded](https://search.maven.org/search?q=a:ravendb-embedded) as dependency

### Starting the Server

RavenDB Embedded Server is available under `EmbeddedServer.INSTANCE`. In order to start it call `startServer` method.

{CODE:java start_server@Server\Embedded.java /}

For more control on how to start the server just pass to `startServer` method a `ServerOptions` object and that`s it.

{INFO:ServerOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **frameworkVersion** | String | The .NET Core framework version to run the server with |
| **dataDirectory** | String | Indicates where your data should be stored |
| **dotNetPath** | String | The path to exec `dotnet` (if it is in PATH, leave it)|
| **targetServerLocation** | String | The path to extract server binaries |
| **acceptEula** |  boolean | If set to `false`, will ask to accept our terms & conditions |
| **erverUrl** | String | What address we want to start our server (default `127.0.0.1:0`) |
| **maxServerStartupTimeDuration** | `Duration` | The timeout for the server to start |
| **commandLineArgs** | `List<String>` | The [command lines arguments](../server/configuration/configuration-options#command-line-arguments) to start the server with |

{INFO /}

{CODE:java start_server_with_options@Server\Embedded.java /}

{NOTE  Without the `ServerOptions`, RavenDB server will start with a default values on `127.0.0.1:{Random Port}`  /}

### Security

RavenDB Embedded support running a secured server.
Just run `secured` method in `ServerOptions` object.

We have two overloads to `secured`:
{CODE:java security@Server\Embedded.java /}

The first way to enable authentication is to set certificate with the path to your .pfx 
server certificate. You may supply the certificate password using certPassword.

{CODE:java security2@Server\Embedded.java /}

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. 
RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use.
RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.
In this options you can control on your client certificate and to use in a different certificate for your client.

### Document Store

After starting the server you can get the DocumentStore from the Embedded Server and start working with RavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `getDocumentStore` with the name of the database you like to work with. 

{CODE:java get_document_store@Server\Embedded.java /}

For more control on the process you can call the methods with `DatabaseOptions` object.

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DatabaseRecord** | DatabaseRecord | Instance of `DatabaseRecord` containing database configuration |
| **SkipCreatingDatabase** | boolean | If set to true, will skip try creating the database  |

{INFO /}

{CODE:java get_document_store_with_database_options@Server\Embedded.java /}

### Get Server URL

The `getServerUri` method can be used to retrieve the Embedded server URL. It must be called after server was started, because it waits for the server initialization to complete.
The URL can be used for example for creating a custom document store, omitting the `getDocumentStore` method entirely.

{CODE:java get_server_url@Server/Embedded.java /}

{PANEL/}

{PANEL:Remarks}

* You can have only one instance of `EmbeddedServer`
* Method `EmbeddedServer.INTANCE.openStudioInBrowser()` can be used to open an browser instance with Studio

{PANEL/}
