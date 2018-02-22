# Installation : Running as a Service

After completing the Server configuration process either via [Setup Wizard](../../start/installation/setup-wizard) or [Manually](../../start/installation/manual) you can register the Server as a Service using the `rvn` tool that can be found inside the RavenDB Server distribution package.

{PANEL:Windows}

### Registering

To register as a Service on the Windows operating system you need to execute the following command:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service register --service-name RavenDB
{CODE-BLOCK/}

### Unregistering

To remove the Service use the 'unregister' command as follows:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service unregister --service-name RavenDB
{CODE-BLOCK/}

### Starting and Stopping

Service can be also controlled using the `start` and `stop` commands:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service stop --service-name RavenDB
.\rvn.exe windows-service start --service-name RavenDB
{CODE-BLOCK/}

{PANEL/}

{PANEL:Linux}

To run as a Service on Linux, you need to add the following command to your deamon script:

{CODE-BLOCK:bash}
<path/to/ravendb>/Server/Raven.Server --non-interactive
{CODE-BLOCK/}

{PANEL/}
