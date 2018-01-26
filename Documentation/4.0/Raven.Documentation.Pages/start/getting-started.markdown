# Getting Started

Welcome to RavenDB! This introduction will guide you through your first steps.

## Downloading the Server  

RavenDB is cross-platform. You can use it on the following:

- Windows x64 / x86  
- Linux x64  
- Docker 
- MacOS  
- Raspberry Pi   

Go to [https://ravendb.net/downloads](https://ravendb.net/downloads). Select the appropriate version and platform, and download the zip package.   

## Prerequisites  

#### Windows
Microsoft Visual C++ 2015 Redistributable Package should be installed prior to RavenDB launch.  
See: [Visual C++ Downloads](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) and [Windows Prerequisites](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites)

#### Linux/MacOS
It is recommended that you update your OS before launching an instance of RavenDB.
For example, Ubuntu-16.x as an updated OS doesn't require any additional packages.
Libsodium (1.0.13 or up) must be installed prior to RavenDB launch.

In Ubuntu 16.x: 

{CODE-BLOCK:bash}
apt-get install libsodium-18
{CODE-BLOCK/}

In MacOS 10.12: 

{CODE-BLOCK:bash}
brew install libsodium
{CODE-BLOCK/}

You might need to also install additional packages, for example:

{CODE-BLOCK:bash}
apt-get install libunwind8 liblttng-ust0 libcurl3 libssl1.0.0 libuuid1 libkrb5 zlib1g libicu55
{CODE-BLOCK/}

See also: [Linux Prerequisites](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) or [MacOS Prerequisites](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites)

## Installing RavenDB Using the Setup Wizard

1. Extract the zip/tar file to a directory of your choice.  

2. In **Windows**, use the `run.ps1` script. In **Linux**, use the `run.sh` script. It will run RavenDB in an initial setup mode and redirect you to the Setup Wizard in your browser.

3. Follow this [detailed walk through](setup-wizard) to complete the wizard successfully. 

## Installing RavenDB Manually on Windows/Linux/Mac

1. Extract the zip/tar file to a directory of your choice.  

2. Find the `settings.json` file which is located in the server folder. To disable the setup wizard, change the `Setup.Mode` value to `None`.

3. In **Windows**, use the `run.ps1` script or run `Server\Raven.Server.exe` directly. In **Linux**, use the `run.sh` script or run `Server\Raven.Server` directly.  

This will run RavenDB in interactive mode inside a console application:

![Figure 1: RavenDB console.](images\console.png) 

You can read all about 'settings.json' in the [Configuration Section](../server/configuration/configuration-options). These are the default initial settings:  

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

This is likely what you will change it to if you decide to setup manually:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:8080",
    "Setup.Mode": "None",
    "DataDir": "RavenData"
} 
{CODE-BLOCK/}
 
{WARNING:Port in Use}
If port **8080** is already being used, RavenDB will fail to start and give you an "address in use" error (`EADDRINUSE`).

Change the default port by editing the `ServerUrl` value in `settings.json`.
{WARNING/}

{INFO:Registering as a Service}

Registering as a service in **Windows**:

{CODE-BLOCK:bash}
    .\rvn.exe windows-service register --service-name RavenDB4
{CODE-BLOCK/}

Running as a service in **Linux**, add the following to your daemon script:

{CODE-BLOCK:bash}
    <path/to/ravendb>/Server/Raven.Server --daemon
{CODE-BLOCK/}

{INFO/}

Once things are set up and the server is running, you can access our GUI, the [RavenDB Management Studio](..\studio\overview), by going to `http://localhost:8080` in your browser.

**RavenDB Management Studio comes free with all licenses: Community, Professional, and Enterprise. **

![Figure 2: Accessing the Studio for the first time.](images\dashboard.png)

You are ready to continue to the next section, [Getting to Know RavenDB](getting-to-know).

## Installing RavenDB with Docker

If you already have Docker installed, just run the following command:

{CODE-BLOCK:bash}
    docker run -d -e PUBLIC_SERVER_URL=http://10.0.75.2:8080 
        -e PUBLIC_TCP_SERVER_URL=http://10.0.75.2:38888 
        -p 8080:8080 
        -p 38888:38888 
        ravendb/ravendb
{CODE-BLOCK/}

Docker will pull the latest stable RavenDB version and spin up a new container to host it. It is also possible to get the latest nightly build by using the 'ravendb/ravendb-nightly' repository.

{WARNING:Port in Use}
If port **8080** is already being used, RavenDB will fail to start and give you an "address in use" error (`EADDRINUSE`).

In that case, simply change the port in the docker run command.
{WARNING/}

**Our GUI, the RavenDB Management Studio, comes free with all licenses: Community, Professional, and Enterprise.**

You can access the [RavenDB Management Studio](..\studio\overview) by going to `http://10.0.75.2:8080` in your browser. This is assuming that you are using the default networking
configuration with Docker, and that the Docker instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it 
externally, make sure to go through the security configuration first. 

For more options and information on running RavenDB under Docker, please visit the [Docker Section]() (coming soon...).

## Security Concerns

To let a developer start coding an application quickly, RavenDB will run with the following default security mode:

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration, and require the administrator to properly setup the security and 
access control to prevent unauthorized access to your data.

**RavenDB will not let you listen to requests outside your local machine until you have adequately provided security for it. We recommend using the Setup Wizard to easily install RavenDB securely from the very start.  **  

Read more about security and how to enable authentication [here](../server/security/overview)
