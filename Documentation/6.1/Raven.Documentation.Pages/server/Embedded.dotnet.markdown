# Server: Running an Embedded Instance
---

{NOTE: }

* This page explains how to run RavenDB as an embedded server.

* In this page:  

  * [Overview](../server/embedded#overview)  
  * [Prerequisites](../server/embedded#prerequisite)  
  * [Installation](../server/embedded#installation)  
  * [Starting the Server](../server/embedded#starting-the-server)  
  * [Server Options](../server/embedded#server-options)  
     * [Setting Server Directory](../server/embedded#setting-server-directory)  
     * [Restarting the Server](../server/embedded#restarting-the-server)  
     * [ServerProcessExited Event](../server/embedded#serverprocessexited-event)  
     * [Licensing options](../server/embedded#licensing-options)  
  * [.NET FrameworkVersion](../server/embedded#.net-frameworkversion)  
  * [Security](../server/embedded#security)  
  * [Document Store](../server/embedded#document-store)  
  * [Get Server URL and Process ID](../server/embedded#get-server-url-and-process-id)  
  * [Remarks](../server/embedded#remarks)  

{NOTE/}

---

{PANEL: Overview}

RavenDB makes it very easy to be embedded within your application, with RavenDB Embedded package you can integrate your RavenDB server with a few easy steps. 

{CODE-TABS}
{CODE-TAB:csharp:Sync embedded_example@Server\Embedded.cs /}
{CODE-TAB:csharp:Async embedded_async_example@Server\Embedded.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Prerequisite}

There is one prerequisite and one recommendation for the Embedded package:

Prerequsite:

- Install [.NET Core runtime](https://dotnet.microsoft.com/en-us/download) either manually or [along with a RavenDB full version](embedded#setting-server-directory)
  - Be sure that the RavenDB server [FrameworkVersion](../server/embedded#net-frameworkversion) definition matches the .NET Core 
    version that you install.  

Recommendation:

- **Projects targeting .NET Framework 4.6.1+** that use old `packages.config` for maintaining NuGet packages should be **migrated to 
  `PackageReference` package management** (please refer to section below on how to achieve this)

---

### .NET Core Runtime:

RavenDB Embedded **does not include .NET Core runtime required for it to run**. 

By default, the `ServerOptions.FrameworkVersion` is set to the .NET Core version that we compiled the server with and 
`ServerOptions.DotNetPath` is set to `dotnet` meaning that it will require to have it declared in PATH. 

We highly recommend using the .NET Core framework version defined in `ServerOptions.FrameworkVersion` for proper functioning 
of the Server. The .NET Core runtime can be downloaded from [here](https://dotnet.microsoft.com/download).

---

### Migrating from `packages.config` to `PackageReference` in old csproj projects:

Due to the NuGet limitations, we recommend that the Embedded package should be installed via newer package management using 
`PackageReference` instead of old `packages.config`. 

The transition between those two is easy due to built-in into Visual Studio 2017 migrator written by Microsoft. Please read 
following [article](https://docs.microsoft.com/en-us/nuget/reference/migrate-packages-config-to-package-reference) written by Microsoft that will guide you through the process.

Please note that **binding redirects** in `App.config` are still required when 'PackageReference' is used in old csproj projects. 
Not doing so might result in an assembly load exception e.g.

```
Could not load file or assembly 'System.Runtime.CompilerServices.Unsafe, Version=4.0.4.0, Culture=neutral, 
PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. The located assembly's manifest definition does not 
match the assembly reference. (Exception from HRESULT: 0x80131040)
```

{CODE-TABS}
{CODE-TAB-BLOCK:xml:Sample App.config}
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.4.1" newVersion="4.0.4.1" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.3.0" newVersion="4.0.3.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Installation}

* Create a new project (.NET Standard 2.0+, .NET Core 2.0+, .NET Framework 4.6.1+).
* Grab the package from our [NuGet](https://www.nuget.org/packages/RavenDB.Embedded)
{CODE-BLOCK:powershell}
Install-Package RavenDB.Embedded -Version 4.1.0
{CODE-BLOCK/}

{PANEL/}

{PANEL: Starting the Server}

RavenDB Embedded Server is available under `EmbeddedServer.Instance`.  
In order to start it, call `StartServer` method.

{CODE start_server@Server\Embedded.cs /}

For more control on how to start the server, pass to `StartServer` method a `ServerOptions` object.

{PANEL/}

{PANEL: Server Options}

Set `ServerOptions` to change server settings such as .NET FrameworkVersion, DataDirectory, 
and additional options:  

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DataDirectory** | `string` | Indicates where your data should be stored |
| **DotNetPath** | `string` | The path to exec `dotnet` (if it is in PATH, leave it)|
| **AcceptEula** |  `bool` | If set to `false`, will ask to accept our terms & conditions |
| **ServerUrl** | `string` | What address we want to start our server (default `127.0.0.1:0`) |
| **MaxServerStartupTimeDuration** | `TimeSpan` | The timeout for the server to start |
| **CommandLineArgs** | `List<string>` | The [command lines arguments](../server/configuration/configuration-options#command-line-arguments) to start the server with |
| **ServerDirectory** | `string` | The path to the server binary files<sup>[*](../server/embedded#setting-server-directory) |

{CODE start_server_with_options@Server\Embedded.cs /}

If `ServerOptions` is not provided, RavenDB server will start with a default value of `127.0.0.1:{Random Port}`.

---

### Setting Server Directory:
In case you're not interested in installing the .NET run-time environment on your system, you can -  

* [Download](https://ravendb.net/download) a full RavenDB version.  
  This version already includes a .NET run-time environment.  
* Extract the downloaded version to a local folder.  
  E.g. `C:\RavenDB`.  
* Set the `ServerDirectory` server option to the RavenDB subfolder that contains -
   * `Raven.Server.exe` in Windows  
   * `Raven.Server` in Posix  

{CODE start_server_with_server_directory_option@Server\Embedded.cs /}

---

### Restarting the Server:
To restart the server, use the method `<embedded server>.RestartServerAsync()`.

{CODE-BLOCK:csharp}
public async Task RestartServerAsync();
{CODE-BLOCK/}

In code:

{CODE restart_server@Server\Embedded.cs /}

---

### ServerProcessExited Event:
Use `<embedded server>.ServerProcessExited` to observe when the server has crashed or exited.  

{CODE-BLOCK:csharp}
event EventHandler<ServerProcessExitedEventArgs>? ServerProcessExited;
{CODE-BLOCK/}

Event data is of type `ServerProcessExitedEventArgs`.

---

### Licensing options:
A `ServerOptions.Licensing` class gathers configuration options related to the licensing of the embedded server:  

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **License** | `string` | Specifies the full license string directly in the configuration.<br>If both `License` and `LicensePath` are defined, `License` takes precedence. |
| **LicensePath** | `string` | Specifies a path to a license file.<br>If both `License` and `LicensePath` are defined, `License` takes precedence.<br>Default: `license.json` |
| **EulaAccepted** |  `bool` | Set to `false` to present a request to accept our terms & conditions |
| **DisableAutoUpdate** | `bool` | Disable automatic license updates (from both the `api.ravendb.net` license server **and** the `License` and `LicensePath` configuration options) |
| **DisableAutoUpdateFromApi** | `bool` | Disable automatic license updates from the `api.ravendb.net` license server.<br>Note: when disabled, the license **can** still be updated using the `License` and `LicensePath` configuration options |
| **DisableLicenseSupportCheck** | `bool` | Control whether to verify the support status of the current license and display it within Studio.<br>`true`: disable verification<br>`false`: enable verification |
| **ThrowOnInvalidOrMissingLicense** | `bool` | Throw an exception if the license is missing or cannot be validated |

{PANEL/}

{PANEL: .NET FrameworkVersion}

The default FrameworkVersion is defined to work with any .NET version from the time of the RavenDB server release 
and newer by using the `+` moderator.  For example, `ServerOptions.FrameworkVersion = 3.1.17+`.  

Thus, by leaving the default FrameworkVersion definition, RavenDB embedded servers will automatically look for the .NET 
version that is currently running on the machine, starting from the version at the time of the server release.  

{INFO: Making Sure That You Have the Right .NET Version}

Remember that each RavenDB release is compiled with the .NET version that was current at the time of release.  

* To find what .NET version supports RavenDB 5.1, for example, open the [RavenDB 5.1 What's New](https://ravendb.net/docs/article-page/5.1/csharp/start/whats-new) page.
  The correct .NET version for RavenDB 5.1, .NET 5.0.6., is listed at the bottom of the "Server" section."
* By default, your RavenDB server will look for .NET 5.0.6, 5.0.7, etc. So, as long as you have at least one of these .NET versions running on your machine,
  RavenDB will work well.  

{INFO/}

To stay within a major or minor .NET release, but ensure flexibility with patch releases, 
use a floating integer `x`.  
It will always use the newest version found on your machine.  

For example, `ServerOptions.FrameworkVersion = 3.x` will look for the newest 3.x release.  
`...= 3.2.x` will look for the newest 3.2 release.  
Neither will look for 4.x.  

| ServerOption Name | Type | Description |
| ------------- | ------------- | ----- |
| **FrameworkVersion** | string | The .NET Core framework version to run the server with |

| Parameter | Description |
| --------- | ------------- |
| null | The server will pick the newest .NET version installed on your machine. |
| 3.1.17+ | Default setting (Actual version number is set at the time of server release.) In this example, the server will work properly with .NET patch releases that are greater than or equal to 3.1.17 |
| 3.1.17 | The server will **only** work properly with this exact .NET release |
| 3.1.x | The server will pick the newest .NET patch release on your machine |
| 3.x | The server will pick the newest .NET minor releases and patch releases on your machine |

{CODE start_server_with_FrameworkVersion_defined@Server\Embedded.cs /}

{PANEL/}

{PANEL: Security}

RavenDB Embedded supports running a secured server.
Just run `Secured` method in the `ServerOptions` object.

---

### Set up security using the certificate path:

{CODE security@Server\Embedded.cs /}

The first way to enable authentication is to set the [certificate with the path to your .pfx](../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally) 
server certificate. You may supply the certificate password using certPassword.  

---

### Set up security using a custom script:

To access the certificate via logic that is external to RavenDB, you can use the following approach: 

{CODE security2@Server\Embedded.cs /}

This option is useful when you want to protect your certificate (private key) with other solutions such as 
"Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, 
so you can write your own scripts / mini-programs and apply the logic that you need.

It creates a clean separation between RavenDB and the secret store in use.

RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.

{PANEL/}

{PANEL: Document Store}

After starting the server you can get the DocumentStore from the Embedded Server and start working with RavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `GetDocumentStore` or `GetDocumentStoreAsync` 
with the name of the database you like to work with. 

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

{PANEL/}

{PANEL: Get Server URL and Process ID}

#### Server URL:

The `GetServerUriAsync` method can be used to retrieve the Embedded server URL. It must be called after server was started, 
because it waits for the server initialization to complete.
The URL can be used for example for creating a custom document store, omitting the `GetDocumentStore` method entirely.

{CODE get_server_url_async@Server/Embedded.cs /}

#### Process ID:

The `GetServerProcessIdAsync` method can be used to retrieve the system-generated process ID for the 
embedded server.  

{CODE-BLOCK:csharp}
public async Task<int> GetServerProcessIdAsync(CancellationToken token = default);
{CODE-BLOCK/}

{CODE get_server_process_id@Server/Embedded.cs /}

{PANEL/}

{PANEL: Remarks}

* You can have only one instance of `EmbeddedServer`.  
* The `EmbeddedServer.Instance.OpenStudioInBrowser()` method can be used to open a browser instance with Studio.  

{PANEL/}

## Related Articles

### TestDriver 
- [TestDriver](../start/test-driver)  
- [TestDriver Licensing](../start/test-driver#licensing)  

### Troubleshooting
- [Collect information for support](../server/troubleshooting/collect-info)
