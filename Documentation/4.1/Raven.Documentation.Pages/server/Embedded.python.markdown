# Server : Running an embedded instance

## Overview

RavenDB makes it very easy to be embedded within your application, with PyRavenDB Embedded package you 
don't need to do anything only download the package and start your own PyRavenDB Embedded server.
{CODE:python embedded_example@server\embedded.py /}

## Getting Started
---
* Create a new project.
* Install from [PyPi](https://pypi.python.org/pypi), 
as [pyravendb-embedded](https://pypi.python.org/project/pyravendb-embedded).<br />
`pip install pyravendb-embedded`
* Start a new Embedded Server
* Get the new Embedded Document Store, and start working with the database.

### Start The Server
RavenDB Embedded Server is available under `EmbeddedServer` Instance. In order to start it call `start_server` method.
{CODE:python start_server@server\embedded.py /}

or more control on how to start the server just pass to `start_server` method a `ServerOptions` object and that`s it.

{INFO:ServerOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **framework_version** | str | The framework version to run the server |
| **data_directory** | str | Where to store our database files |
| **server_directory** | str | The path to the server binary files (.dll) |
| **dotnet_path** | str | The path to exec dotnet (If dotnet is in PATH leave it)|
| **accept_eula** |  bool | If set to false, will ask to accept our terms |
| **server_url** | str | What address we want to start our server (default 127.0.0.1:0) |
| **max_server_startup_time_duration** | timedelta | The timeout for the server to start |
| **command_line_args** | list | The command lines arguments to start the server with 

{INFO /}

{CODE:python start_server_with_options@server\embedded.py /}

{NOTE  Without the `server_options`, RavenDB server will start with a default values on 127.0.0.1:{Random Port}  /}

##### Security
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

### Get Document Store
After Starting the server you can get the DocumentStore from the Embedded Server and start working with PyRavenDB.
Getting the DocumentStore from The Embedded Server is pretty easy you only need to call `get_document_store` method with database name 
or with `DatabaseOptions`.

{INFO:DatabaseOptions}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **database_name** | str | The name of the database |
| **skip_creating_database** | bool | If set to True, will skip try creating the database  |

{INFO /}

{CODE:python get_document_store@server\embedded.py /}

{CODE:python get_document_store_with_database_options@server\embedded.py /}

### Open RavenDB studio in the browser
To open RavenDB studio from Pyravendb-Embedded you can use `open_studio_in_browser` method and the studio will open automatically
one your default browser.

{CODE:python open_in_browser@server\embedded.py /}

## Remarks
* **EmbeddedServer** class is a singleton!.<br/> 
Every time we use `EmbeddedServer()` we will get the same instance.
* PyRavenDB Embedded by deafult runs the server with dotnet that can be found in the PATH, if you want to use a different one
or if you don't have dotnet installed you can download it from [here](https://www.microsoft.com/net/download/dotnet-core/2.1),
and change `dotnet_path` from `ServerOptions` to the path of the new **dotnet.exe** and use it to run the server.
{CODE:python run_with_dotnet_path@server\embedded.py /}
