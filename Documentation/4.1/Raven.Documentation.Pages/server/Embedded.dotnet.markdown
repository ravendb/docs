# Server : Running an Embedded Instance

{PANEL:Overview}

RavenDB makes it very easy to be embedded within your application, with RavenDB Embedded package you can integrate your RavenDB server with few easy steps.

{CODE-TABS}
{CODE-TAB:csharp:Sync embedded_example@Server\Embedded.cs /}
{CODE-TAB:csharp:Async embedded_async_example@Server\Embedded.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Prerequisites}

There are two prerequsites for the Embedded package:

- **.NET Core runtime** must be installed manually
- **Projects targeting .NET Framework 4.6.1+** that use old `packages.config` for maintaining NuGet packages must be **migrated to `PackageReference` package management**

{NOTE:.NET Core Runtime}

RavenDB Embedded **does not include .NET Core runtime required for it to run**. 

By default the `ServerOptions.FrameworkVersion` is set to the .NET Core version that we compiled the server with and `ServerOptions.DotNetPath` is set to `dotnet` meaning that it will require to have it declared in PATH. 

We highly recommend using the .NET Core framework version defined in `ServerOptions.FrameworkVersion` for proper functioning of the Server. The .NET Core runtime can be downloaded from [here](https://www.microsoft.com/net/download).

{NOTE/}

{NOTE:Migrating from `packages.config` to `PackageReference` in old csproj projects}

Due to the NuGet limitations, the Embedded package requires newer package management via `PackageReference` instead of old `packages.config`. 

The transition between those two is easy due to built-in into Visual Studio 2017 migrator written by Microsoft. Please read following [article](https://docs.microsoft.com/en-us/nuget/reference/migrate-packages-config-to-package-reference) written by Microsoft that will guide you through the process.

{NOTE/}

{PANEL/}

{PANEL:Getting Started}

### Installation

* Create a new project (.NET Standard 2.0+, .NET Core 2.0+, .NET Framework 4.6.1+).
* Grab the package from our [NuGet](https://www.nuget.org/packages/RavenDB.Embedded)
{CODE-BLOCK:powershell}
Install-Package RavenDB.Embedded -Version 4.1.0
{CODE-BLOCK/}

### Starting the Server

RavenDB Embedded Server is available under `EmbeddedServer.Instance`. In order to start it call `StartServer` method.
{CODE start_server@Server\Embedded.cs /}

For more control on how to start the server just pass to `StartServer` method a `ServerOptions` object and that`s it.

{INFO:ServerOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **FrameworkVersion** | string | The .NET Core framework version to run the server with |
| **DataDirectory** | string | Indicates where your data should be stored |
| **DotNetPath** | string | The path to exec `dotnet` (if it is in PATH, leave it)|
| **AcceptEula** |  bool | If set to `false`, will ask to accept our terms & conditions |
| **ServerUrl** | string | What address we want to start our server (default `127.0.0.1:0`) |
| **MaxServerStartupTimeDuration** | `TimeSpan` | The timeout for the server to start |
| **CommandLineArgs** | `List<string>` | The [command lines arguments](../server/configuration/configuration-options#command-line-arguments) to start the server with |

{INFO /}

{CODE start_server_with_options@Server\Embedded.cs /}

{NOTE  Without the `ServerOptions`, RavenDB server will start with a default values on `127.0.0.1:{Random Port}`  /}

### Security

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

### Document Store

After starting the server you can get the DocumentStore from the Embedded Server and start working with RavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `GetDocumentStore` or `GetDocumentStoreAsync` with the name of the database you like to work with. 

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store@Server\Embedded.cs /}
{CODE-TABS/}

For more control on the process you can call the methods with `DatabaseOptions` object.

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DatabaseRecord** | DatabaseRecord | Instance of `DatabaseRecord` containing database configuration |
| **SkipCreatingDatabase** | bool | If set to true, will skip try creating the database  |

{INFO /}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store_with_database_options@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store_with_database_options@Server\Embedded.cs /}}
{CODE-TABS/}

### Get Server URL

The `GetServerUriAsync` method can be used to retrieve the Embedded server URL. It must be called after server was started, because it waits for the server initialization to complete.
The URL can be used for example for creating a custom document store, omitting the `GetDocumentStore` method entirely.

{CODE get_server_url_async@Server/Embedded.cs /}

{PANEL/}

{PANEL:Remarks}

* You can have only one instance of `EmbeddedServer`
* Method `EmbeddedServer.Instance.OpenStudioInBrowser()` can be used to open an browser instance with Studio

{PANEL/}
