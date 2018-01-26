# Getting Started

Welcome to this introductory article that will guide you through all the parts of RavenDB needed for basic understanding and simple setup.

{PANEL: Server}

Let's start by installing and configuring the Server. In order to do that first we need to download the server package from the [downloads](https://ravendb.net/downloads) page.

RavenDB is cross-platform with a support for following operating systems:

- Windows x64 / x86
- Linux x64
- Docker 
- MacOS
- Raspberry Pi

---

### Prerequisites

RavenDB is written in .NET Core, because of that it requires same set of prerequisites as the .NET Core.

{NOTE: Windows}

Please install [Visual C++ 2015 Redistributable Package](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) (or newer) before launching the Server. This package should be the sole requirement for 'Windows' platforms, but in occursion of any troubles please check [.NET Core prerequisites for Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: Linux}

We highly recommend **updating** your **Linux OS** prior launching an instance of RavenDB. Please also check if .NET Core does not require any other prerequisites. This can be checked in [.NET Core prerequisited for Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites) article written by Microsoft.

{NOTE/}

{NOTE: MacOS}

We highly recommend **updating** your **MacOS** and checking [.NET Core prerequsites for MacOS](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites) article written by Microsoft prior running the RavenDB Server.

{NOTE/}

---

### Installation & Setup

After extraction of the Server package you can start the [Setup Wizard](../start/installation/setup-wizard) by running the `run.ps1` (or `run.sh`) script or [disable the 'Setup Wizard' and configure the server manually](../start/setup-wizard#manual-setup).

{NOTE: Running in a Docker container}

If you are interested in hosting the Server in a Docker container. Please read our [dedicated article](../start/installation/running-in-docker-container).

{NOTE/}

---

### Configuration

RavenDB Server is using `settings.json` file to store the server-wide configuration options. This file is located in the `Server` directory, but please note that after making changes to this file Server restart is required in order for them to be applied.

You can read more about available configuration options in our dedicated article that can be found [here](../server/configuration/configuration-options).

{INFO:Default configuration}

After downloading the Server package the configuration file will look like follows:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

Which means that the Server will run:

- In `Setup Wizard` mode
- On `localhost` with `random port`
- Store the data in `RavenData` directory.

{INFO/}

{WARNING:Port in Use}

In some cases the port might be in use, this will prevent the Server from starting with "address in use" error (`EADDRINUSE`).

The port can be changed by editing the `ServerUrl` value.

{WARNING/}

---

### Studio

{SAFE: Free}

Our GUI, the RavenDB Management Studio, comes **free** with **all the licenses**:

- Community,
- Professional,
- Enterprise

{SAFE/}

After installation and setup, the Studio can be accessed via the browser using the `ServerUrl` or `ServerPublicUrl` value e.g. `http://localhost:8080`.

---

### Security Concerns

To let a developer start coding an application quickly, RavenDB will run with the following default security mode:

{WARNING:Default Security Mode}

As long as the database is used inside the local machine and no outside connections are allowed, you can ignore security concerns 
and you require no authentication. Once you set RavenDB to listen to connections outside your local machine, 
your database will immediately block this now vulnerable configuration, and require the administrator to properly setup the security and 
access control to prevent unauthorized access to your data.

{WARNING/}

**RavenDB will not let you listen to requests outside your local machine until you have adequately provided security for it. We recommend using the Setup Wizard to easily install RavenDB securely from the very start.**  

Read more about security and how to enable authentication [here](../server/security/overview).

{PANEL/}


