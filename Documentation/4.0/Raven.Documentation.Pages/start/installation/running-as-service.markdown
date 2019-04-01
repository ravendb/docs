# Installation: Running as a Service

{INFO:Prerequisites}

The prerequisites for running RavenDB as a Service are defined [here](../../start/getting-started#prerequisites).  

{INFO/}

After completing the Server configuration process either via the [Setup Wizard](../../start/installation/setup-wizard) or [Manually](../../start/installation/manual), you can register the Server as a Service using the `rvn` tool that can be found inside the RavenDB Server distribution package.

{PANEL:Windows}

### Registering

To register as a Service on the Windows operating system you need to execute the following command:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service register --service-name RavenDB
{CODE-BLOCK/}

If you want to run the service under a non-default user (`Local Service` is default) then execute following command:

{CODE-BLOCK:powershell}
.\rvn.exe windows-service register --service-name RavenDB --service-user-name MyUser --service-user-password MyPassword
{CODE-BLOCK/}

### Unregistering

To remove the Service, use the 'unregister' command as follows:

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

Open a bash terminal, and create the following file `/etc/systemd/system/ravendb.service`, using super user permissions:

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

- [System Requirements](../../start/installation/system-requirements)
- [System Configuration Recommendations](../../start/installation/system-configuration-recommendations)
- [Running in a Docker Container](../../start/installation/running-in-docker-container)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)
