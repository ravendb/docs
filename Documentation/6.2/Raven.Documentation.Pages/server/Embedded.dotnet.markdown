# Server: Running an Embedded Instance
---

{NOTE: }

* This page explains how to run RavenDB as an embedded server.

* In this page:  

  * [Overview](../server/embedded#overview)  
  * [Prerequisites and Recommendations](../server/embedded#prerequisites-and-recommendations)  
  * [Installation](../server/embedded#installation)  
  * [Starting the server](../server/embedded#starting-the-server)  
  * [Server options](../server/embedded#server-options)  
     * [Setting server directory](../server/embedded#setting-server-directory)  
     * [Restarting the server](../server/embedded#restarting-the-server)  
     * [ServerProcessExited Event](../server/embedded#serverprocessexited-event)  
  * [Embedded server licensing](../server/embedded#embedded-server-licensing)  
      * [Licensing configuration options](../server/embedded#licensing-configuration-options)  
      * [License an embedded server using an Environment variable](../server/embedded#license-an-embedded-server-using-an-environment-variable)  
  * [`.NET` FrameworkVersion](../server/embedded#.net-frameworkversion)  
  * [Security](../server/embedded#security)  
  * [Document store](../server/embedded#document-store)  
  * [Get server URL and process ID](../server/embedded#get-server-url-and-process-id)  
  * [Remarks](../server/embedded#remarks)  

{NOTE/}

---

{PANEL: Overview}

RavenDB can be easily embedded in your application.  
Use the Embedded package to integrate RavenDB in just a few easy steps. 

{CODE-TABS}
{CODE-TAB:csharp:Sync embedded_example@Server\Embedded.cs /}
{CODE-TAB:csharp:Async embedded_async_example@Server\Embedded.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Prerequisites and Recommendations}

* **Prerequisites**  
   * Install [`.NET` Core runtime](https://dotnet.microsoft.com/en-us/download), 
     either manually or [along with a RavenDB full version](embedded#setting-server-directory).  
   * Verify that the RavenDB server [FrameworkVersion](../server/embedded#net-frameworkversion) 
     definition matches the `.NET` Core version that you install.  

* **Recommendations**  
   * Projects targeting `.NET Framework 4.6.1+` that use the old `packages.config` 
     for NuGet packages maintenance, should migrate to `PackageReference` package management.  
     Find additional details [below](../server/embedded#migrating-from--to--in-old-csproj-projects).  

---

### `.NET` Core Runtime:

RavenDB Embedded **does not include** the `.NET` Core runtime engine required for its operation.  

By default, `ServerOptions.FrameworkVersion` is set to the `.NET` Core version that we compiled 
the server with and `ServerOptions.DotNetPath` is set to `dotnet` - meaning it is required to have 
it declared in PATH. 

We highly recommend using the `.NET` Core framework version defined in `ServerOptions.FrameworkVersion` 
for proper server function.  
You can download the `.NET` Core runtime engine [here](https://dotnet.microsoft.com/download).  

---

### Migrating from `packages.config` to `PackageReference` in old csproj projects:

Due to NuGet limitations, we recommend installing the Embedded package via newer package management, 
using `PackageReference` rather than the older `packages.config`.  

The transition between the two is made easy by the built-in Visual Studio migrator.  
Find further guidance in this [Microsoft article](https://docs.microsoft.com/en-us/nuget/reference/migrate-packages-config-to-package-reference).  

Please note that _binding redirects_ in `App.config` are still required when 'PackageReference' 
is used in old `csproj` projects. Failing to use _binding redirects_ might result in an assembly 
load exception such as:  

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

* Create a new project (.`NET Standard 2.0+`, `.NET Core 2.0+`, `.NET Framework 4.6.1+`).
* Grab our [NuGet package](https://www.nuget.org/packages/RavenDB.Embedded)
{CODE-BLOCK:powershell}
Install-Package RavenDB.Embedded -Version 4.1.0
{CODE-BLOCK/}

{PANEL/}

{PANEL: Starting the Server}

RavenDB Embedded Server is available under `EmbeddedServer.Instance`.  
Start the server using the `StartServer` method.

{CODE start_server@Server\Embedded.cs /}

For more control over the server startup, pass `StartServer` a `ServerOptions` object.

{PANEL/}

{PANEL: Server Options}

Set `ServerOptions` to change server settings such as `.NET` FrameworkVersion, DataDirectory, 
and additional options.  

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
In case you are not interested in installing the `.NET` run-time environment on your system, you can -  

* [Download](https://ravendb.net/download) a full RavenDB version.  
  This version already includes a `.NET` run-time environment.  
* Extract the downloaded version to a local folder.  
  E.g. `C:\RavenDB`  
* Set the `ServerDirectory` server option to the RavenDB subfolder that contains -
   * `Raven.Server.exe` in Windows  
   * `Raven.Server` in Posix  

{CODE start_server_with_server_directory_option@Server\Embedded.cs /}

---

### Restarting the Server:
To restart the server, use the `<embedded server>.RestartServerAsync()` method.  

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

{PANEL/}

{PANEL: Embedded server licensing}

* The **same license types** available for Standalone RavenDB servers, are available for Embedded servers.  
* A licensed server can be managed using Studio, and is given a superior feature set to that of 
  non-registered servers. See the full list of license types and their features [here](https://ravendb.net/buy).  
* An embedded server can be licensed using **Configuration options** or an **Environment variable**.  

---

### Licensing configuration options

Embedded server licensing configuration options are gathered in the `ServerOptions.Licensing` class.  
After acquiring a license, it can be passed to the server either as a string or as a file.  

* To pass your license to the server as a string, use the `ServerOptions.LicensingOptions.License` 
  configuration option.  
  {CODE LicensingOptions_License@Server\Embedded.cs /}
* To keep your license file in your file system and point the server to its path, 
  use the `ServerOptions.LicensingOptions.LicensePath` configuration option.  
  {CODE LicensingOptions_LicensePath@Server\Embedded.cs /}

---

#### Available LicensingOptions configuration options:

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **License** | `string` | Specifies the full license string directly in the configuration.<br>If both `License` and `LicensePath` are defined, `License` takes precedence. |
| **LicensePath** | `string` | Specifies a path to a license file.<br>If both `License` and `LicensePath` are defined, `License` takes precedence.<br>Default: `license.json` |
| **EulaAccepted** |  `bool` | Set to `false` to present a request to accept our terms & conditions. |
| **DisableAutoUpdate** | `bool` | Disable automatic license updates (from both the `api.ravendb.net` license server **and** the `License` and `LicensePath` configuration options). |
| **DisableAutoUpdateFromApi** | `bool` | Disable automatic license updates from the `api.ravendb.net` license server.<br>Note: when disabled, the license **can** still be updated using the `License` and `LicensePath` configuration options. |
| **DisableLicenseSupportCheck** | `bool` | Control whether to verify the support status of the current license and display it within Studio.<br>`true`: disable verification<br>`false`: enable verification |
| **ThrowOnInvalidOrMissingLicense** | `bool` | Throw an exception if the license is missing or cannot be validated. |

---

### License an embedded server using an Environment variable

You can pass the same configuration options to the embedded server using environment variables.  

* To pass your license to the server as a string, define or edit the environment variable `RAVEN_License`.  
  Provide your license as a value for this variable.  
  **Note**, however, that you must first **reformat** the license JSON that you acquired, and 
  turn it to a single line, eliminating new-line symbols.  

     {CODE-TABS}
{CODE-TAB-BLOCK:json:Original_Format}
{
	"Id": "bad5fe9b-fba4-459c-9220-36b438e06e36",
	"Name": "rdb",
	"Keys": [
	"WBRG3G1zKd536ELfRbWw7x69J",
	"zyFCZ+AcGLI9RgSyRq5r4KS7K",
	"E0hMr5uzmbMBuxAI6WLBXZTSN",
	"t+vGjgrVzqoycTPhHdQxNCK2v",
	"7xOwXKUblAhZmHcDeY3xvF0jn",
	"EZoZLdaeF0D8FFddNB8NrMWeQ",
	"kwzAKfs1BMlXi9ZJsVZO9ABUE",
	"yBSYoSQMqKywtLi8wJzEyMzQV",
	"Fjc4OTo7PD0+nwIfIJ8CICCfA",
	"iEgnwIjIEMkRAlieVc="
	]
}
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:json:Reformatted}
{"Id": "bad5fe9b-fba4-459c-9220-36b438e06e36","Name": "rdb","Keys": ["WBRG3G1zKd536ELfRbWw7x69J","zyFCZ+AcGLI9RgSyRq5r4KS7K","E0hMr5uzmbMBuxAI6WLBXZTSN","t+vGjgrVzqoycTPhHdQxNCK2v","7xOwXKUblAhZmHcDeY3xvF0jn","EZoZLdaeF0D8FFddNB8NrMWeQ","kwzAKfs1BMlXi9ZJsVZO9ABUE","yBSYoSQMqKywtLi8wJzEyMzQV","Fjc4OTo7PD0+nwIfIJ8CICCfA","iEgnwIjIEMkRAlieVc="]}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

     {INFO: }
     You can reformat the license manually, or use a script to do it.  
     E.g., find below a Bash script that specifies the name of the file from which the license 
     will be read, and then uses the `-c` flag to compact the file's contents to a single line.  
     {CODE-BLOCK:plain}
INPUT_FILE="license.json"
jq -c . "$INPUT_FILE"
{CODE-BLOCK/}
     {INFO/}

* Or, you can keep your license file in a folder accessible to the application that embeds your server, 
  and provide the path to the license file in the `RAVEN_LicensePath` environment variable.  

{PANEL/}

{PANEL: `.NET` FrameworkVersion}

The default FrameworkVersion is defined to work with any `.NET` version from the time of the RavenDB server release 
and newer by using the `+` moderator.  
E.g. `ServerOptions.FrameworkVersion = 3.1.17+`  

Thus, by leaving the default FrameworkVersion definition, RavenDB embedded servers will automatically look for the `
.NET` version that is currently running on the machine, starting from the version at the time of the server release.  

{INFO: Making sure that you have the right `.NET` version}

Each RavenDB release is compiled with the `.NET` version that was current at the time of the release.  

* To find which `.NET` version supports RavenDB 5.1, for example, open the 
  [RavenDB 5.1 What's New](https://ravendb.net/docs/article-page/5.1/csharp/start/whats-new) page.  
  The correct `.NET` version for RavenDB 5.1, `.NET` 5.0.6., is listed at the bottom of the **Server** section.  
* By default, your RavenDB server will look for `.NET` 5.0.6, 5.0.7, etc.  
  So as long as you have at least one of these `.NET` versions running on your machine, RavenDB will work well.  

{INFO/}

To stay within a major or minor `.NET` release, but ensure flexibility with patch releases, 
use a floating integer `x`.  
It will always use the newest version found on your machine.  

E.g., `ServerOptions.FrameworkVersion = 3.x` will look for the newest 3.x release,  
`ServerOptions.FrameworkVersion = 3.2.x` will look for the newest 3.2 release.  
Neither will look for 4.x.  

| ServerOption Name | Type | Description |
| ------------- | ------------- | ----- |
| **FrameworkVersion** | string | The `.NET` Core framework version to run the server with |

| Parameter | Description |
| --------- | ------------- |
| `null` | The server will pick the newest `.NET` version installed on your machine. |
| `3.1.17+` | Default setting (Actual version number is set at the time of server release).<br>In this example, the server will work properly with `.NET` patch releases that are greater than or equal to 3.1.17 |
| `3.1.17` | The server will **only** work properly with this exact `.NET` release. |
| `3.1.x` | The server will pick the newest `.NET` patch release on your machine. |
| `3.x` | The server will pick the newest `.NET` minor releases and patch releases on your machine. |

{CODE start_server_with_FrameworkVersion_defined@Server\Embedded.cs /}

{PANEL/}

{PANEL: Security}

RavenDB Embedded supports running a secured server.
Just run `Secured` method in the `ServerOptions` object.

---

### Set up security using the certificate path:

{CODE security@Server\Embedded.cs /}

The first way to enable authentication is to set the 
[certificate with the path to your .pfx](../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally) 
server certificate.  
You can supply the certificate password using certPassword.  

---

### Set up security using a custom script:

To access the certificate via logic that is external to RavenDB, you can use the following approach: 

{CODE security2@Server\Embedded.cs /}

This option is useful when you want to protect your certificate (private key) with other solutions such as 
"Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, 
so you can write your own scripts / mini-programs and apply the logic that you need.

This way a clean separation is kept between RavenDB and the secret store in use.

RavenDB expects to get the raw binary representation (byte array) of the `.pfx` certificate 
through the standard output.

{PANEL/}

{PANEL: Document Store}

After starting the server you can get the `DocumentStore` from the Embedded Server and start working with RavenDB.  
Getting the `DocumentStore` from The Embedded Server is done simply by calling `GetDocumentStore` or `GetDocumentStoreAsync` 
with the name of the database you choose to work with.  

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store@Server\Embedded.cs /}
{CODE-TABS/}

For additional control over the process you can call the methods with a `DatabaseOptions` object.  

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DatabaseRecord** | `DatabaseRecord` | Instance of `DatabaseRecord` containing database configuration |
| **SkipCreatingDatabase** | `bool` | If set to true, will skip try creating the database  |

{INFO /}

{CODE-TABS}
{CODE-TAB:csharp:Sync get_document_store_with_database_options@Server\Embedded.cs /}
{CODE-TAB:csharp:Async get_async_document_store_with_database_options@Server\Embedded.cs /}}
{CODE-TABS/}

{PANEL/}

{PANEL: Get Server URL and Process ID}

#### Server URL:

The `GetServerUriAsync` method can be used to retrieve the Embedded server URL.  
It must be called after the server was started, because it waits for the server's 
initialization to complete.  
The URL can be used, for example, to create a custom document store, omitting the 
`GetDocumentStore` method entirely.

{CODE get_server_url_async@Server/Embedded.cs /}

---

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
