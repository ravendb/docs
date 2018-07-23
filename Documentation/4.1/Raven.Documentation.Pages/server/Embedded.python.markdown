# PyRavenDB Embedded

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
* Ask the embedded client for a document store, and start working with documents.

### Start The Server
To start RavenDB server you only need to run `start_server()` method from `EmbeddedServer` instance.
{CODE:python start_server@server\embedded.py /}

To be more in control about your server `start_server` method can take one parameter [`server_options`](../../python/glossary/server-options).
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
or with [`DatabaseOptions`](../../python/glossary/database-options).

{CODE:python get_document_store@server\embedded.py /}

{CODE:python get_document_store_with_database_options@server\embedded.py /}

### Open RavenDB studio in the browser
To open RavenDB studio from Pyravendb-Embedded you can use `open_studio_in_browser` method and the studio will open automatically
one your default browser.

{CODE:python open_in_browser@server\embedded.py /}

## Acknowledgments
* **EmbeddedServer** class is a singleton!.<br/> 
Every time we use `EmbeddedServer()` we will get the same instance.
* If you don't have dotnet install you can download it from [here](https://www.microsoft.com/net/download/dotnet-core/runtime-2.1.1) 
and used it to run the server with.
{CODE:python run_with_dotnet_path@server\embedded.py /}
