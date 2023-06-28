# Installation: Running in a Docker Container
---

{NOTE: }

* RavenDB can be launched using [Docker](https://www.docker.com/).  
* **Stable** and **LTS** Docker images of RavenDB Server are available 
  for **Ubuntu** Linux and **Windows Nano Server**.  
* Information related to setting and running RavenDB on Docker is 
  available in this page and in the RavenDB 
  [dockerhub](https://hub.docker.com/r/ravendb/ravendb/) page.  
* Upgrading RavenDB `5.x` or lower to RavenDB 6.0 or higher requires 
  a simple [migration](../../migration/server/docker) procedure.  

* In this page:  
  * [Requirements](../../start/installation/running-in-docker-container#requirements)  
  * [Setup and Execution](../../start/installation/running-in-docker-container#setup-and-execution)  
  * [Migration](../../start/installation/running-in-docker-container#migration)  
  * [Configuration](../../start/installation/running-in-docker-container#configuration)  
  * [FAQ](../../start/installation/running-in-docker-container#faq)  


{NOTE/}

---

{PANEL: Requirements}  

* **Docker Configuration**  
  Use the default Docker networking configuration.  
* **Security**  
  Do not expose the Docker instance beyond the host machine.  
  {WARNING: }
  If you Do intend to host RavenDB on Docker and expose it 
  externally, please make sure the server is [secure](../../start/installation/setup-wizard#secure-setup-with-a-free-let).  
  {WARNING/}
* **Storage**  
   * Non-NFS file systems **are** Supported.  
     NTFS, Ext4, and other non-NFS volume mounts' file systems are supported.  
   * SMB and CIFS mounts are **not** supported.  
     Linux Docker containers running under Windows Docker hosts via shared volumes 
     are [not supported](../../start/installation/deployment-considerations#storage-considerations) 
     due to CIFS protocol usage.  
* **Platforms**  
  RavenDB images are available for:  
   * **Ubuntu** (22.04, 20.04) or any other Debian-based distribution.  
   * [Windows Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver)  

{PANEL/}

{PANEL: Setup and Execution}

#### RavenDB Versions
To install or run a Docker RavenDB image use an updated **stable** or **nightly** version.  

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
   * _Tag_: `windows-1809-latest`  
     Latest RavenDB version, running on Windows Nano Server version 1809
   * _Tag_: `windows-ltsc2022-latest`  
     Latest RavenDB version, running on Windows Nano Server version LTSC2022
   * An updated tags list is available [here](https://github.com/ravendb/ravendb/blob/v6.0/docker/readme.md#latest-stable).  

* **Latest RavenDB LTS version**  
   * _Tag_: `latest-lts` / `ubuntu-latest-lts`  
     Latest RavenDB LTS version, running on Ubuntu container  
   * _Tag_: `windows-latest-lts`  
     Latest RavenDB LTS version, running on Windows Nano Server  
   * _Tag_: `windows-1809-latest-lts`  
     Latest RavenDB LTS version, running on Windows Nano Server version 1809
   * _Tag_: `windows-ltsc2022-latest-lts`  
     Latest RavenDB LTS version, running on Windows Nano Server version LTSC2022
   * An updated tags list is available [here](https://github.com/ravendb/ravendb/blob/v6.0/docker/readme.md#latest-lts)  

---

#### Running a RavenDB image
To install or run RavenDB start the Docker service, and run a RavenDB image manually or using a script.  

* **Running manually**:  
  Run RavenDB using [docker run](https://docs.docker.com/engine/reference/commandline/run/), e.g. -  
  _Ubuntu image_: `$ docker run -p 8080:8080 ravendb/ravendb:ubuntu-latest`  
  _Windows image_: `$ docker run -p 8080:8080 ravendb/ravendb:windows-latest`  

* **Running using a script:**  
  Run a RavenDB image using a dedicated script for Ubuntu or Windows.  
  [Ubuntu-based image script](https://github.com/ravendb/ravendb/blob/v5.4/docker/run-linux.ps1)  
  [Windows-based image script](https://github.com/ravendb/ravendb/blob/v5.4/docker/run-nanoserver.ps1)  

{NOTE: Setup and Management}
After running the image, access it from a browser using its URL.  
By default:  `http://localhost:8080`  

If the server is not installed yet, connecting it will start the Setup Wizard.  
After installing the server, connecting it will open its [management studio](https://ravendb.net/docs/article-page/latest/csharp/studio/overview).  
{NOTE/}

---

#### Sharing data with Docker Host
To share data with the Docker host using Docker for Windows:  

* The Docker client application must have `sharing` enabled.  
* The directory (e.g. `C:\RavenDb\Data`) must exist.  

---

#### Dockerfiles
The `Dockerfiles` used to build RavenDB Server images and their assets can be found at:  

* [Ubuntu image Dockerfile](https://github.com/ravendb/ravendb/tree/v6.0/docker/ravendb-ubuntu)  
* [Windows Nano Server image Dockerfile](https://github.com/ravendb/ravendb/tree/v6.0/docker/ravendb-nanoserver)  

---

#### Persisting Data

To install using the `latest` tag, and persist the data stored on your 
hard disk if the container is removed, you can use:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v /var/lib/ravendb/data ravendb/ravendb
{CODE-BLOCK/}

* The data will now remain available even if the container is removed.  
* When you start a new instance of the image using a volume mounted to 
  the same directory, the data will still be available.  
* To keep persistence, RavenDB data in a Windows container is always 
  kept in this location: `C:/RavenDB/Server/RavenData`  

---

#### Skipping the Setup Wizard  
To start the RavenDB container on your localhost without running 
through the Setup Wizard each time, you can use:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v /var/lib/ravendb/data --name RavenDb-WithData 
-e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true 
-e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork ravendb/ravendb
{CODE-BLOCK/}

Using this command will skip the Setup Wizard and mount a volume for data persistence.  

{WARNING: Warning} 
Please be aware that running a Docker container with `RAVEN_Setup_Mode=None` and 
`RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork` will run an **Unsecure** server.  
{WARNING/}

{INFO: EULA acceptance}
By setting `RAVEN_License_Eula_Accepted=true` you're accepting our [terms & conditions](https://ravendb.net/terms/commercial).
{INFO/}

{PANEL/}

{PANEL: Migration}

If a Docker image of RavenDB version `5.x` or lower is installed on your 
system and you want to upgrade it to version `6.0` or higher, a short migration 
process will be required.  
Please visit our Docker [migration](../../migration/server/docker) page to 
learn how to perform this migration.  

{PANEL/}

{PANEL: Configuration}
RavenDB can be adjusted using:  

* The [settings.json](../../server/configuration/configuration-options#settings.json) configuration file.  

* [Environment Variables](../../server/configuration/configuration-options#environment-variables), e.g. -
  {CODE-BLOCK:plain}
  RAVEN_Setup_Mode=None
RAVEN_DataDir=RavenData
RAVEN_Certificate_Path=/config/raven-server.certificate.pfx
  {CODE-BLOCK/}

* [CLI arguments](../../server/configuration/configuration-options#command-line-arguments)  
  Variables can be passed to a RavenDB Docker image in a CLI arguments line, e.g. -  
  `./Raven.Server --Setup.Mode=None`  

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
**A:** Mount it as a Docker volume, and use the `--config-path PATH_TO_CONFIG` 
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

 Build 60, Version 6.0, SemVer 6.0.0-custom-60, Commit 10ed5a8
 PID 8232, 64 bits, 8 Cores, Phys Mem 23.866 GBytes, Arch: X64
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
