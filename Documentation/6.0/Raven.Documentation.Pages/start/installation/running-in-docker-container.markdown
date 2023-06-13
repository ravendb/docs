# Installation: Running in a Docker Container
---

{NOTE: }

* RavenDB can be launched using [Docker](https://www.docker.com/).  
* **Stable** and **LTS** RavenDB Server Docker Images are available 
  for **Ubuntu** Linux and **Windows Nano Server**.  
* Additional information regarding using and setting RavenDB 
  on Docker is available in the [Docker Hub page](https://hub.docker.com/r/ravendb/ravendb/).  

* In this page:  
  * [Installation](../../start/installation/running-in-docker-container#installation)  
  * [Running RavenDB Server in a Docker Container](../../start/installation/running-in-docker-container#running-ravendb-server-in-a-docker-container)  
  * [Configuration](../../start/installation/running-in-docker-container#configuration)  
  * [FAQ](../../start/installation/running-in-docker-container#faq)  


{NOTE/}

---

{PANEL: Installation}  

#### Requirements

* Use the default Docker networking configuration.  
* Do **not** expose the Docker instance beyond the host machine.  
  If you intend to host RavenDB on Docker and expose it 
  externally, please go through the security configuration first.  

---

#### Platforms
Server images are published on Docker for the following platforms:

* **Ubuntu** (20.04, 18.04, 16.04) or any other Debian-based distribution.  
* [Windows Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver)  

---

#### Storage Requirements

* **Non-NFS file systems are Supported**  
  NTFS, Ext4, and other non-NFS volume mounts' file systems are supported.  
* **SMB and CIFS mounts are Not supported**  
  Linux Docker containers running under Windows Docker hosts via shared volumes 
  are [not supported](../../start/installation/deployment-considerations#storage-considerations) 
  due to CIFS protocol usage.  

{PANEL/}

{PANEL: Running RavenDB Server in a Docker Container}
To run RavenDB via Docker use an updated **stable** or **nightly** version.  

* [Stable and LTS images](https://hub.docker.com/r/ravendb/ravendb/)  
* [Nightly releases](https://hub.docker.com/r/ravendb/ravendb-nightly/)  

---

#### Available Image Tags
Use the following tags to install the latest **Stable** or **LTS** RavenDB Server version.  

* **Latest RavenDB version**  
   * _Tag_: `latest` / `ubuntu-latest`  
     Latest RavenDB version, running on Ubuntu container  
   * _Tag_: `windows-latest`  
     Latest RavenDB version, running on Windows Nano Server  

* **Latest RavenDB LTS version**  
   * _Tag_: `latest-lts` / `ubuntu-latest-lts`  
     Latest RavenDB LTS version, running on Ubuntu container  
   * _Tag_: `windows-latest-lts`  
     Latest RavenDB LTS version, running on Windows Nano Server  

---

#### Examples
Run a RavenDB image using [docker run](https://docs.docker.com/engine/reference/commandline/run/), e.g. -  

* **Linux image**  
  {CODE-BLOCK:bash}
  $ docker run -p 8080:8080 ravendb/ravendb:ubuntu-latest  
  {CODE-BLOCK/}

* **Windows image**  
  {CODE-BLOCK:bash}
  $ docker run -p 8080:8080 ravendb/ravendb:windows-latest  
  {CODE-BLOCK/}

---

#### Sharing data with Docker Host
To share data with the docker host using docker for Windows:  

* The docker client application must have `sharing` enabled.  
* The folder (e.g. `C:\RavenDb\Data`) must exist.  

---

#### Dockerfiles
The `Dockerfiles` used to build RavenDB Server images and their assets can be found at:  

* [Ubuntu image Dockerfile](https://github.com/ravendb/ravendb/tree/v5.4/docker/ravendb-ubuntu)  
* [Windows Nano Server image Dockerfile](https://github.com/ravendb/ravendb/tree/v5.4/docker/ravendb-nanoserver)  

{NOTE: Running RavenDB Management Studio}
After running RavenDB, access its [management studio](https://ravendb.net/docs/article-page/latest/csharp/studio/overview) 
from a browser using the server's URL.  
E.g., `http://localhost:8080`  
{NOTE/}

---

#### Persisting Data
To install using the `latest` tag and persist the data stored on your 
hard disk if the container is removed, you can use:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData ravendb/ravendb
{CODE-BLOCK/}
The data will now remain available even if the container is removed.  
When you start a new instance of the image using a volume mounted to 
the same directory, the data will still be available.  

---

#### Skipping the Setup Wizard  
To start the RavenDB container on your localhost without running 
through the Setup Wizard each time, you can use:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData --name RavenDb-WithData -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork ravendb/ravendb
{CODE-BLOCK/}

Using this command will skip the Setup Wizard and mount a volume for data persistence.  

{WARNING: Warning} 
Please be aware that running a docker container with `RAVEN_Setup_Mode=None` and 
`RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork` will run an **Unsecure** server.  
{WARNING/}

{INFO: EULA acceptance}
By setting `RAVEN_License_Eula_Accepted=true` you're accepting our [terms & conditions](https://ravendb.net/terms/commercial).
{INFO/}

{PANEL/}

{PANEL: Configuration}
Configuration can be adjusted using environment variables.  

* The server will use all the environment variables that are preceded by 
  a `RAVEN_` prefix and apply their values to specified configuration keys.  
* All period `.` characters in configuration keys should be replaced with 
  an underscore character (`_`) when used in environment variables.  

{NOTE: Example}
{CODE-BLOCK:plain}
RAVEN_Setup_Mode=None
RAVEN_DataDir=RavenData
RAVEN_Certificate_Path=/config/raven-server.certificate.pfx
{CODE-BLOCK/}
{NOTE/}

In addition, `RAVEN_ARGS` environment variable can be passed 
to a RavenDB docker image as a server CLI arguments line.  

{PANEL/}

{PANEL: FAQ}

#### Q: I use `compose` or run an automated installation. How do I disable the setup wizard?
**A:** Set the `Setup.Mode` configuration option to `None`m like so:  
{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
{CODE-BLOCK/}

---

#### Q: I want to try RavenDB on my local / development machine. How do I run an unsecure server?
**A:** Set the env variables like so:  
{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
RAVEN_Security_UnsecuredAccessAllowed='PrivateNetwork'
{CODE-BLOCK/}

---

#### Q: How can I pass command line arguments?
**A:** By modifying `RAVEN_ARGS` environment variables, 
that will be passed on as a CLI arguments line.  

---

#### Q: Can I see RavenDB logs in container logs?
**A:** To get logs available when running the `docker logs` command, you need to enable 
this option in RavenDB server.  
E.g., set the environment variables as shown below to enable logging to the console.  
{CODE-BLOCK:plain}
RAVEN_ARGS='--log-to-console'
{CODE-BLOCK/}

{NOTE: }
Please note that this behavior may have performance implications.  
To modify the logging level use the `RAVEN_Logs_Mode` variable.  

Additional information regarding logging configuration is available 
[here](../../server/configuration/logs-configuration).  
{NOTE/}

---

#### Q: How do I use a custom config file?
**A:** Mount it as a docker volume, and use the `--config-path PATH_TO_CONFIG` 
command line argument to use a settings file from outside of the server directory.  
Alternatively, you can pass your custom `settings.json` content via the 
`RAVENDB_SETTINGS` environment variable.

---

#### Q: How can I manage server running in a container?
**A:** Except for the RavenDB Studio, which can be accessed from the browser, 
you can connect the RavenDB administration console using the `rvn` utility 
as follows:  
{CODE-BLOCK:plain}
> docker exec -it CONTAINER_ID /opt/RavenDB/Server/rvn admin-channel
Will try to connect to discovered Raven.Server process : 8...

       _____                       _____  ____
      |  __ \                     |  __ \|  _ \
      | |__) |__ ___   _____ _ __ | |  | | |_) |
      |  _  // _` \ \ / / _ \ '_ \| |  | |  _ <
      | | \ \ (_| |\ V /  __/ | | | |__| | |_) |
      |_|  \_\__,_| \_/ \___|_| |_|_____/|____/


      Safe by default, optimized for efficiency

 Build 40040, Version 4.1, SemVer 4.1.4, Commit dc2e9e3
 PID 8, 64 bits, 2 Cores, Phys Mem 1.934 GBytes, Arch: X64
 Source Code (git repo): https://github.com/ravendb/ravendb
 Built with love by Hibernating Rhinos and awesome contributors!
+---------------------------------------------------------------+
Connected to RavenDB Console through named pipe connection...

ravendb> help
...
{CODE-BLOCK/}

{NOTE: }
Additional information about running RavenDB as a console is available 
[here](../../server/administration/cli).  
{NOTE/}

{PANEL/}

## Related Articles

### Installation
- [Running as a Service](../../start/installation/running-as-service)  
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)  

### Setup Examples
- [Docker on AWS Linux VM](../../start/installation/setup-examples/aws-docker-linux-vm)  
