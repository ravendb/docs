# Installation: DEB Package

---

{NOTE: }

* RavenDB can be installed on **Debian** and **Ubuntu** systems using a **RavenDB DEB package**.  

* Installing an application using a DEB package automatizes a series of setup operations, 
  including the scattering of files between system directories, the creation of users and 
  groups needed by the application, the creation of a `systemd daemon`, and others.  

* After installing the RavenDB DEB package, you can connect the RavenDB service from 
  a browser and complete the setup process using our setup wizard.  

* In this page:  
   * [Downloading and Installing RavenDB](../../../start/installation/gnu-linux/deb#downloading-and-installing-ravendb)  
      * [1. Download DEB Package](../../../start/installation/gnu-linux/deb#download-deb-package)  
      * [2. Install RavenDB](../../../start/installation/gnu-linux/deb#install-ravendb)  
      * [3. Complete the setup](../../../start/installation/gnu-linux/deb#complete-the-setup)  
      * [4. Entering RavenDB Studio When Setup is Complete](../../../start/installation/gnu-linux/deb#entering-ravendb-studio-when-setup-is-complete)  
   * [Upgrading RavenDB](../../../start/installation/gnu-linux/deb#upgrading-ravendb)  
   * [Removing RavenDB](../../../start/installation/gnu-linux/deb#removing-ravendb)  
      * [Remove RavenDB and Leave its Data and Settings Intact](../../../start/installation/gnu-linux/deb#remove-ravendb-and-leave-its-data-and-settings-intact)  
      * [Remove RavenDB Completely](../../../start/installation/gnu-linux/deb#remove-ravendb-completely)  
   * [File System Locations](../../../start/installation/gnu-linux/deb#file-system-locations)  
   * [Default Settings](../../../start/installation/gnu-linux/deb#default-settings)  
   * [RavenDB Service Definitions](../../../start/installation/gnu-linux/deb#ravendb-service-definitions)  

{NOTE/}

---

{PANEL: Downloading and Installing RavenDB}

### 1. Download DEB Package

First, download the RavenDB DEB package for the version you want to install.  
Find it here: [https://ravendb.net/download](https://ravendb.net/download)  

!["Download DEB Package"](images/download-deb-package.png "Download DEB Package")


---

### 2. Install RavenDB

* Open a terminal, navigate to the folder you downloaded the RavenDB DEB package to, 
  and install it using: `sudo apt install <package name>`  
  {CODE-BLOCK:plain}
  sudo apt install ./ravendb_5.3.102-0_amd64.deb
  {CODE-BLOCK/}

  {NOTE: }
  The same command, `sudo apt install <package name>`, is also used to 
  [Upgrade RavenDB](../../../start/installation/gnu-linux/deb#upgrading-ravendb).  
  {NOTE/}

* The RavenDB service will be created and activated automatically.  

    !["Install RavenDB"](images/extract-DEB-package.png "Install RavenDB")

* To complete the setup process, connect the address suggested by the setup process.  
  By default, the address is: `http://127.0.0.1:53700`  

    {INFO: If you only have SSH access:}

    * Open these ports on the target machine:  
       * Port 53700 (RavenDB setup)  
       * Port 443 (RavenDB HTTPS server)  
       * Port 38888 (RavenDB TCP server)  
    * Set port tunneling through SSH:  
       * Tunnel port 53700 from `localhost` of the target machine to `localhost:8080` of the SSH client machine.  
         {CODE-BLOCK:plain}
         ssh -N -L localhost:8080:localhost:53700 ubuntu@target.machine.com
         {CODE-BLOCK/}
       * If you connect using a key, provide it using the `-i` option.  
         This will allow you to open the RavenDB Setup Wizard on http://localhost:8080 
         and proceed with the setup process.  
    {INFO/}

---

### 3. Complete the setup

Connecting the RavenDB service will invoke the setup wizard.  
  
!["Setup Wizard"](images/setup-wizard.png "Setup Wizard")

* The first choice you need to make, is whether to set RavenDB as a **Secure Server** 
  or allow any client to connect and configure it.  
  {WARNING: }
  We strongly recommend that you set RavenDB as a secure server.  
  {WARNING/}
        
* [Find Here](../../../start/installation/setup-wizard) a **step by step guide** 
  to the rest of the setup process.  

---

### 4. Entering RavenDB Studio When Setup is Complete

RavenDB can be managed using the **RavenDB Studio** management GUI.  
To open Studio, connect the RavenDB service from a browser.  

{INFO: To connect the service from a browser and open Studio:}
The address used to connect the RavenDB service during the setup process 
is temporary, and will change when the process is done.  

* If RavenDB is installed insecurely, its default address will be: `127.0.0.1:8080`  
* If RavenDB is installed securely, its address will be determined by the 
  domain name you registered during setup, e.g.: `https://a.raven.development.run:8080`  
{INFO/}

!["RavenDB Studio"](images/studio.png "RavenDB Studio")

{INFO: }
Learn more about Studio [here](../../../studio/overview).  
{INFO/}

{PANEL/}


{PANEL: Upgrading RavenDB}

To upgrade RavenDB:  

* Enter [https://ravendb.net/download](https://ravendb.net/download) 
  and download the DEB package for the version you want to upgrade to.  

* Open a terminal, navigate to the folder you downloaded the DEB package to, 
  and install it using: `sudo apt install <package name>`  
  {CODE-BLOCK:plain}
  sudo apt install ./ravendb_5.3.102-0_amd64.deb
  {CODE-BLOCK/}
  This is the same command used to install RavenDB in the first place, only this 
  time it will upgrade the installed database without harming its settings and 
  the data stored in it.  

{PANEL/}

{PANEL: Removing RavenDB}

To remove the installed RavenDB package, you need its name.  
It is `ravendb`, you can use `dpkg --list` to confirm it.  

!["RavenDB Service"](images/dpkg--list.png "RavenDB Service")

RavenDB can be removed **with** or **without** purging the data stored in it and its settings.  

---

### Remove RavenDB and Leave its Data and Settings Intact

To remove RavenDB and **leave its settings and data intact**, use: `sudo apt-get remove <package name>`  
{CODE-BLOCK:plain}
sudo apt-get remove ravendb
{CODE-BLOCK/}

!["Leave Data and Settings Intact"](images/sudo_apt-get_remove_ravendb.png "Leave Data and Settings Intact")

[Reinstalling](../../../start/installation/gnu-linux/deb#upgrading-ravendb) 
RavenDB after removing it this way will **restore** its data and settings.  

---

### Remove RavenDB Completely

To **remove RavenDB completely**, purging its data and settings, use: `sudo dpkg -P <package name>`  
{CODE-BLOCK:plain}
sudo dpkg -P ravendb
{CODE-BLOCK/}

!["Purge Data and Settings"](images/sudo_dpkg_-P_ravendb.png "Purge Data and Settings")

{WARNING: }
Running this command requires no confirmation, and **will remove your data and settings irrevocably**.  
{WARNING/}

{PANEL/}

{PANEL: File System Locations}

Once installed, these are the locations used by RavenDB.  

* **Settings**  
  `/etc/ravendb/settings.json`  
* **Security settings (e.g. certificate)**  
  `/etc/ravendb/security`  
* **Data**  
  `/var/lib/ravendb/data`  
* **Logs**  
  `/var/log/ravendb/logs`  
* **Audit logs**  
  `/var/log/ravendb/audit`  
* **Binaries**  
  `/usr/lib/ravendb`  
* **rvn link**  
  `/usr/bin/rvn` -> `/usr/lib/ravendb/rvn`  
* **`systemd` unit file**  
  `/lib/systemd/ravendb.service`  

{PANEL/}

{PANEL: Default Settings}

The database settings are located in: `/etc/ravendb/settings.json`  
Their default values are:  
{CODE-BLOCK:json}
{
    // http://127.0.0.1:53700 during setup
    // http://127.0.0.1:8080 after setup for an insecure server
    // the registered domain URL after setup for a secure server
    "ServerUrl": "http://127.0.0.1:53700", 
    
    // "Initial" during setup 
    // "None" when Setup is done
    "Setup.Mode": "Initial", 

    // Audit logs definitions
    "Logs.RetentionTimeInHrs": 336, // Audit log retention time (14 days)
    "Security.AuditLog.Compress": true,
    "Security.AuditLog.RetentionTimeInHrs": "52560", // Security log retention time (6 years)

    // Use a Eula license
    "License.Eula.Accepted": true
}
{CODE-BLOCK/}

Learn more about configuration variables [here](../../../server/configuration/configuration-options) 
and in pages that describe specific configuration options (e.g. 
[Security Configuration](../../../server/configuration/security-configuration)).  

{PANEL/}

{PANEL: RavenDB Service Definitions}

The RavenDB service is defined here: `/lib/systemd/ravendb.service`  
Its default values are:  
{CODE-BLOCK:json}
[Unit]
Description=RavenDB NoSQL Database

# Run after the network interfaces are up
After=network.target

[Service]

# Process limits  
LimitCORE=infinity 
LimitNOFILE=65535
LimitRSS=infinity
LimitAS=infinity
LimitMEMLOCK=infinity
TasksMax=infinity

# Run as user ravendb
User=ravendb

StartLimitBurst=0
Restart=on-failure

# Single-process application
Type=simple

TimeoutStopSec=300

# RavenDB directories
Environment="RAVEN_DataDir=/var/lib/ravendb/data"
Environment="RAVEN_Indexing_NugetPackagesPath=/var/lib/ravendb/nuget"
Environment="RAVEN_Logs_Path=/var/log/ravendb/logs"
Environment="RAVEN_Security_AuditLog_FolderPath=/var/log/ravendb/audit"
Environment="RAVEN_Security_MasterKey_Path=/etc/ravendb/security/master.key"
Environment="RAVEN_Setup_Certificate_Path=/etc/ravendb/security/server.pfx"
Environment="HOME=/var/lib/ravendb"

# Startup process
ExecStart=/usr/lib/ravendb/server/Raven.Server -c "/etc/ravendb/settings.json"

[Install]
WantedBy=multi-user.target
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Server
- [Security in RavenDB - Overview](../../../server/security/overview)
- [Common Setup Wizard Errors and FAQ](../../../server/security/common-errors-and-faq#setup-wizard-issues) 

### Getting Started
- [Manual Setup](../../../start/installation/manual)
- [Running as a Service](../../../start/installation/running-as-service)
- [Setup Example - AWS Windows VM](../../../start/installation/setup-examples/aws-windows-vm)
- [Setup Example - AWS Linux VM](../../../start/installation/setup-examples/aws-linux-vm)
