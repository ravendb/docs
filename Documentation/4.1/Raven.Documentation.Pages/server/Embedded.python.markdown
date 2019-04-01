# Server: Running an Embedded Instance

{PANEL:Overview}

RavenDB makes it very easy to be embedded within your application, 
with PyRavenDB Embedded package you can integrate your RavenDB server with few easy steps.
{CODE:python embedded_example@server\embedded.py /}

{PANEL/}

{PANEL:Prerequisites}
 PyRavenDB Embedded **does not include .NET Core runtime required for it to run**. 
 By default the `ServerOptions.framework_version` is set to the .NET Core version that we compiled the server with and `ServerOptions.dotnet_path` is set to `dotnet` meaning that it will require to have it declared in PATH. 
 We highly recommend using the .NET Core framework version defined in `ServerOptions.framework_version` for proper functioning of the Server.
 If you don't have dotnet installed The .NET Core runtime can be downloaded from [here](https://dotnet.microsoft.com/download),
to use it just add it to the PATH or change `ServerOptions.dotnet_path` to **dotnet.exe** path you just downloaded.

{CODE:python run_with_dotnet_path@server\embedded.py /}

{PANEL/}

{PANEL:Getting Started}

### Installation
---
* Create a new project.
* Install from [PyPi](https://pypi.org/), 
as [pyravendb-embedded](https://pypi.org/project/pyravendb-embedded/).<br />
`pip install pyravendb-embedded`
* Start a new Embedded Server
* Get the new Embedded Document Store, and start working with the database.

### Starting the Server
RavenDB Embedded Server is available under `EmbeddedServer` Instance. In order to start it call `start_server` method.
{CODE:python start_server@server\embedded.py /}

or more control on how to start the server just pass to `start_server` method a `ServerOptions` object and that`s it.

{INFO:ServerOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **framework_version** | str | The .NET Core framework version to run the server with |
| **data_directory** | str | Indicates where your data should be stored |
| **server_directory** | str | The path to the server binary files (.dll) |
| **dotnet_path** | str | The path to exec `dotnet` (if it is in PATH, leave it)|
| **accept_eula** |  bool | If set to `false`, will ask to accept our terms & conditions |
| **server_url** | str | What address we want to start our server (default `127.0.0.1:0`) |
| **max_server_startup_time_duration** | timedelta | The timeout for the server to start |
| **command_line_args** | list | The [command lines arguments](../server/configuration/configuration-options#command-line-arguments) to start the server with |

{INFO /}

{CODE:python start_server_with_options@server\embedded.py /}

{NOTE  Without the `server_options`, RavenDB server will start with a default values on `127.0.0.1:{Random Port}`  /}

### Security
PyRavenDB Embedded support running a secured server.
There are two options to make ravendb secured in Pyravendb-Embedded:<br />

1) `secured(certificate_path, certificate_password=None)` - For this option you will put a path to a .pfx file for the server and a password to the file
if you have one.
    {CODE:python security@server\embedded.py /}

2) `secured_with_exec(self, cert_exec, cert_exec_args, server_cert_fingerprint, client_cert_path,
                          client_cert_password=None)` - For this option you will have to put executable program (ex. powershell, python, etc) and the arguments for him,
                          the fingerprint of the cert file you can use pyravendb Utils for that (see get_cert_file_fingerprint method), 
                          the client cert path and the password if you have one.
    {CODE:python security2@server\embedded.py /}

{NOTE:Note} This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", 
"HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. 
It creates a clean separation between RavenDB and the secret store in use.
RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.
In this options you can control on your client certificate and to use in a different certificate for your client.
{NOTE /}

### Document Store
After Starting the server you can get the DocumentStore from the Embedded Server and start working with PyRavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `get_document_store` method with database name 
or with `DatabaseOptions`.

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **database_name** | str | The name of the database |
| **skip_creating_database** | bool | If set to True, will skip try creating the database  |

{INFO /}

{CODE-TABS}
{CODE-TAB:python:database_name: get_document_store@server\embedded.py /}
{CODE-TAB:python:DatabaseOptions: get_document_store_with_database_options@server\embedded.py /}
{CODE-TABS/}

{PANEL/}

{PANEL:Remarks}

* You can have only one instance of EmbeddedServer
* Method EmbeddedServer().open_studio_in_browser() can be used to open an browser instance with Studio

{PANEL/}
