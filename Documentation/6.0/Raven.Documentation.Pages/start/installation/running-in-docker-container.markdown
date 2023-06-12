# Installation: Running in a Docker Container

RavenDB Server can run via [Docker](https://www.docker.com/), with 
its updated stable and nightly versions always available here:  

- [Stable and LTS images](https://hub.docker.com/r/ravendb/ravendb/)  
- [Nightly releases](https://hub.docker.com/r/ravendb/ravendb-nightly/)  

## Platforms

Server images are published on Docker for the following platforms:

- **Ubuntu** (20.04, 18.04, 16.04, or any other Debian-based distribution)  
- [Windows Nano Server](https://hub.docker.com/_/microsoft-windows-nanoserver)  

## Storage Requirements

* **Non-NFS file systems are Supported**  
  NTFS, Ext4, and other non-NFS volume mounts' file systems are supported.  
* **SMB and CIFS mounts are Not supported**  
  Linux Docker containers running under Windows Docker hosts via shared volumes 
  are [not supported](../../start/installation/deployment-considerations#storage-considerations) 
  due to CIFS protocol usage.  

## Tags

RavenDB tags indicate whether it is the latest stable version, or the latest LTS version.  

* **Latest Stable**  
   * `latest` / `ubuntu-latest`  
     The latest version of RavenDB, running on Ubuntu container.  
   * `windows-1809-latest`  
     The latest version of RavenDB, running on Windows nanoserver (in this case - Windows version 1809).  
   * `windows-ltsc2022-latest`  
     The latest version of RavenDB, running on Windows nanoserver (in this case - Windows version 2022).  

* **Latest LTS**  
  * `latest-lts` / `ubuntu-latest-lts`  
    The latest LTS version of RavenDB, running on Ubuntu container.  
  * `windows-1809-latest-lts`  
    The latest LTS version of RavenDB, running on Windows nanoserver (in this case - Windows version 1809).  
  * `windows-ltsc2022-latest-lts`  
    The latest LTS version of RavenDB, running on Windows nanoserver (in this case - Windows version 2022).  

## Examples

Run a RavenDB image using [docker run](https://docs.docker.com/engine/reference/commandline/run/). e.g. -  

* Linux image:  
  {CODE-BLOCK:bash}
  $ docker run -p 8080:8080 ravendb/ravendb:ubuntu-latest  
  {CODE-BLOCK/}

* Ubuntu ARM image:  
  {CODE-BLOCK:bash}
  $ docker run -p 8080:8080 ravendb/ravendb:ubuntu-arm32v7-latest 
  {CODE-BLOCK/}

* Windows image:  
  {CODE-BLOCK:bash}
  $ docker run -p 8080:8080 ravendb/ravendb:windows-latest  
  {CODE-BLOCK/}

## Running RavenDB Studio

To run the RavenDB [management studio](https://ravendb.net/docs/article-page/latest/csharp/studio/overview) 
access the server URL using a browser.  
E.g., `http://localhost:8080`  

## Requirements

* Use the default networking configuration with Docker.  
* Do not expose the Docker instance beyond the host machine.  
  If you intend to host RavenDB on Docker and expose it 
  externally, please go through the security configuration first.  

## Persisting Data
To install using the `latest` tag and persist the data stored on your 
hard disk if the container is removed, you can use:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData ravendb/ravendb
{CODE-BLOCK/}
The data will now remain available even if the container is removed.  
If you start a new instance of the image later on, using a volume 
mounted to the same directory, the data will be accessible again.  

### Sharing data with Docker host
To share data with the docker host using docker for Windows the 
docker client application must have `sharing` enabled and the folder 
(e.g. `C:\RavenDb\Data`) must exist.  

### Skipping the Setup Wizard  
To start the RavenDB container on your localhost without running 
through the Setup Wizard each time, you can use the following command:  
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData --name RavenDb-WithData -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork ravendb/ravendb
{CODE-BLOCK/}

This will skip the Setup Wizard and mount a volume for data persistence.  

{WARNING: } 
Please note that running a docker container with `RAVEN_Setup_Mode=None` 
and `RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork` will start an 
**Unsecure** server.  
{WARNING/}

{INFO: EULA acceptance}
By setting `RAVEN_License_Eula_Accepted=true` you're accepting our [terms & conditions](https://ravendb.net/terms/commercial).
{INFO/}

## Configuration

Configuration can be adjusted using environment variables.  
The server will use all the environment variables that are preceded by 
a `RAVEN_` prefix and apply their values to specified configuration keys.  
All period `.` characters in configuration keys should be replaced with 
an underscore character (`_`) when used in environment variables.  

### Example

{CODE-BLOCK:plain}
RAVEN_Setup_Mode=None
RAVEN_DataDir=RavenData
RAVEN_Certificate_Path=/config/raven-server.certificate.pfx
{CODE-BLOCK/}

In addition, our docker image allows providing `RAVEN_ARGS` 
environment variable, which will be passed as a server CLI 
arguments line.  

## FAQ

### Q: I'm using compose / doing automated installation. How do I disable the setup wizard?
Set the `Setup.Mode` configuration option to `None`m like so:  
{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
{CODE-BLOCK/}

### Q: I want to try RavenDB on my local / development machine. How do I run an unsecure server?
Set the env variables like so:  
{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
RAVEN_Security_UnsecuredAccessAllowed='PrivateNetwork'
{CODE-BLOCK/}

### Q: How can I pass command line arguments?
By modifying `RAVEN_ARGS` environment variables, 
that will be passed on as a CLI arguments line.  

### Q: Can I see RavenDB logs in container logs?
To get logs available when running the `docker logs` command, you need to enable 
this option in RavenDB server. E.g., set the environment variables as shown below 
to enable logging to the console.  
{CODE-BLOCK:plain}
RAVEN_ARGS='--log-to-console'
{CODE-BLOCK/}

{NOTE: }
Please note that this behavior may have performance implications.  
To modify the logging level use the `RAVEN_Logs_Mode` variable.  
{NOTE/}

For more information about logging configuration please check 
[the following article](../../server/configuration/logs-configuration).  

### Q: How do I use a custom config file?

Mount it as a docker volume, and use the `--config-path PATH_TO_CONFIG` 
command line argument to use settings file from outside of server directory.  
Alternatively, you can pass your custom `settings.json` content via the 
`RAVENDB_SETTINGS` environment variable.

### Q: How can I manage server running in a container?

Except for the RavenDB Studio, which can be accessed from the browser, 
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

For more information on using RavenDB Console please refer to the 
[Administration - RavenDB CLI](../../server/administration/cli) article.  

## Dockerfiles

These images were built using the following Dockerfiles:

* [Windows Nanoserver image Dockerfile](https://github.com/ravendb/ravendb/blob/v4.1/docker/ravendb-nanoserver/Dockerfile)  
* [Ubuntu image Dockerfile](https://github.com/ravendb/ravendb/blob/v4.1/docker/ravendb-ubuntu/Dockerfile)  

## Remarks

For additional informatiopn regardng using and setting RavenDB on Docker, 
please visit our **Docker Hub** page available [here](https://hub.docker.com/r/ravendb/ravendb/).  

## Related Articles

### Installation

- [Running as a Service](../../start/installation/running-as-service)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)

### Setup Examples

- [Docker on AWS Linux VM](../../start/installation/setup-examples/aws-docker-linux-vm)

