# Installation: Running as a Service

* Running servers as OS services reduces downtime whenever a machine restarts because the servers automatically start every time the machine boots.  

* After completing the Server configuration process either via the [Setup Wizard](../../start/installation/setup-wizard) 
  or [Manually](../../start/installation/manual), you can register the Server as a Service using the `rvn` tool that can be found inside the RavenDB Server 
  distribution package.

* After registering RavenDB as a service, be sure to check your OS "Services" manager to see that the "RavenDB" service is there 
  and that the Startup Type is "Automatic".  

In this page: 

* [Windows](../../start/installation/running-as-service#windows)  
  * [Registering](../../start/installation/running-as-service#registering)  
  * [Unregistering](../../start/installation/running-as-service#unregistering)  
  * [Starting and Stopping](../../start/installation/running-as-service#starting-and-stopping)  
* [Linux - Ubuntu 16.04](../../start/installation/running-as-service#linux---ubuntu-16.04)  

{INFO:Prerequisites}

The prerequisites for running RavenDB as a Service are defined [here](../../start/getting-started#prerequisites).  

{INFO/}

{PANEL:Windows}

### Registering

To register RavenDB as a Service on Windows OS, run PowerShell with administrator privileges.  
Navigate to the RavenDB package root and execute:  

{CODE-BLOCK:powershell}
`.\setup-as-service.ps1`
{CODE-BLOCK/}

If you receive the following error:  
"setup-as-service.ps1 cannot be loaded. The file [YourFileLocation] is not digitally signed" 

1. Run the following command `Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass`.
2. Affirm the "Execution Policy Change" with `Y`. 
3. Run `.\setup-as-service.ps1` again.  

Alternatively, navigate to the node **Server** folder and execute the following command:  

{CODE-BLOCK:powershell}
`.\rvn.exe windows-service register --service-name RavenDB`
{CODE-BLOCK/}

If you want to run the service under a non-default user (**Local Service** is default) then execute the following command:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service register --service-name RavenDB --service-user-name MyUser --service-user-password MyPassword
{CODE-BLOCK/}

### Unregistering

You can end the service using:

{CODE-BLOCK:powershell}
.\uninstall-service.ps1
{CODE-BLOCK/}

Alternatively, use the 'unregister' command as follows:

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

{PANEL:Linux - Ubuntu 16.04}

You can run RavenDB as a daemon by running the script `install-daemon.sh` from the package root.

Alternatively, open a bash terminal, and create the following file `/etc/systemd/system/ravendb.service`, using super-user permissions:

{CODE-BLOCK:bash}
[Unit]
Description=RavenDB v4.0
After=network.target

[Service]
LimitCORE=infinity
LimitNOFILE=65536
LimitRSS=infinity
LimitAS=infinity
User=<desired-user>
Restart=on-failure
Type=simple
ExecStart=<path-to-RavenDB>/run.sh

[Install]
WantedBy=multi-user.target
{CODE-BLOCK/}

Note: Replace `<desired-user>` with your **username** and `<path-to-RavenDB>` with your **path**.

Then register the service and enable it on startup:
{CODE-BLOCK:bash}
systemctl daemon-reload
systemctl enable ravendb.service
{CODE-BLOCK/}

Start the service:
{CODE-BLOCK:bash}
systemctl start ravendb.service
{CODE-BLOCK/}

View its status using:
{CODE-BLOCK:bash}
systemctl status ravendb.service
{CODE-BLOCK/}
or
{CODE-BLOCK:bash}
journalctl -f -u ravendb.service
{CODE-BLOCK/}

{PANEL/}

## Related articles

### Installation

- [Setup Wizard](../../start/installation/setup-wizard)
- [Manual Setup](../../start/installation/manual)
- [Running in a Docker Container](../../start/installation/running-in-docker-container)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)
- [System Requirements](../../start/installation/system-requirements)
- [System Configuration Recommendations](../../start/installation/system-configuration-recommendations)
