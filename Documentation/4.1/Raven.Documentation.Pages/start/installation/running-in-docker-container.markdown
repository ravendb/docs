# Installation: Running in a Docker Container

A RavenDB Server can run on [Docker](https://www.docker.com/) using our official images available in these channels:

- [ravendb/ravendb](https://hub.docker.com/r/ravendb/ravendb/) containing stable, patch, rc and beta images
- [ravendb/ravendb-nightly](https://hub.docker.com/r/ravendb/ravendb-nightly/) containing our nightly releases

## Platforms

Server images are published on Docker for the following platforms:

- Ubuntu 18.04
- Windows Nano Server

## Storage Requirements

NTFS, ext4 file systems and other non NFS volumes mounts are supported.

{NOTE: SMB / CIFS mounts}

Linux Docker container running under Windows Docker host via shared volumes [isn't supported due to CIFS protocol usage](../../start/installation/deployment-considerations#storage-considerations) 

{NOTE /}

## Tags

For version `4.1` following tags are available:

- `latest` and `windows-nanoserver-latest` 

- `4.1-ubuntu-latest` - contains the latest version of RavenDB 4.1 running on Ubuntu 18.04 container

- `4.1-windows-nanoserver-latest` - contains the latest version of RavenDB 4.1 running running on Windows nanoserver

- every 4.1 release to has its own image set for both Ubuntu and Windows containers e.g. `4.1.4-ubuntu.18.04-x64` or `4.1.7-windows-nanoserver`

- `latest` points to `4.1-ubuntu-latest`

- `ubuntu-latest` and `windows-nanoserver-server` point to `4.1-*-latest`


## Examples

To run latest image of RavenDB `4.1`, you can issue a command as follows:

For Ubuntu:
{CODE-BLOCK:bash}
docker run -d -p 8080:8080 -p 38888:38888 ravendb/ravendb
{CODE-BLOCK/}

For Windows:
{CODE-BLOCK:bash}
docker run -d -p 8080:8080 -p 38888:38888 ravendb/ravendb:4.1-windows-nanoserver-latest
{CODE-BLOCK/}

You can access the RavenDB Management Studio by going to `http://localhost:8080` in your browser. This assumes that you are using the default networking configuration with Docker, and that the Docker instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it externally, make sure to go through the security configuration first.

To install the `latest` tag but also persist the data to your hard disk if the container is removed, you can issue a command as follows:

{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData ravendb/ravendb
{CODE-BLOCK/}

 So now, if the container is removed, the data remains. Later on, you can start up a new instance of the image with the volume mounted to that same directory, the data comes back!

{INFO Sharing data with Docker host with Docker for Windows}
This requires that your docker client application has `sharing` enabled and that the folder (in this case, `C:\RavenDb\Data`) exists. 
{INFO/}

Finally, you might not want to run through the Setup Wizard each time you wish to start RavenDB container on your localhost. To skip that Setup Wizard you can issue the following command:

{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData --name RavenDb-WithData -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork ravendb/ravendb
{CODE-BLOCK/}

This will skip the Setup Wizard and mount a volume for data persistence.

{WARNING} 
Running a docker container with `RAVEN_Setup_Mode=None` and `RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork` is going to start an *unsecured* server.
{WARNING/}

{INFO EULA acceptance}
By setting `RAVEN_License_Eula_Accepted=true` you're accepting our (terms & conditions)[https://ravendb.net/terms/commercial].
{INFO/}

## Configuration

Configuration can be adjusted using environment variables. Server is going to pick up all environment variables preceded by `RAVEN_` prefix and apply their values to specified configuration keys. All period `.` characters in configuration keys should be replaced with an underscore character (`_`) when used in environment variables.

### Example

{CODE-BLOCK:plain}
RAVEN_Setup_Mode=None
RAVEN_DataDir=RavenData
RAVEN_Certificate_Path=/config/raven-server.certificate.pfx
{CODE-BLOCK/}

In addition our docker image allows to provide `RAVEN_ARGS` environment variable, which is going to be passed as server CLI arguments line.

## FAQ

### I'm using compose / doing automated installation. How do I disable setup wizard?
    
Set `Setup.Mode` configuration option to `None` like so:

{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
{CODE-BLOCK/}

### I want to try it out on my local / development machine. How do I run unsecured server?

Set env variables like so:
{CODE-BLOCK:plain}
RAVEN_ARGS='--Setup.Mode=None'
RAVEN_Security_UnsecuredAccessAllowed='PrivateNetwork'
{CODE-BLOCK/}

### How can I pass command line arguments?

By modifying `RAVEN_ARGS` environment variable. It's passed as an CLI arguments line.

### Can I see RavenDB logs in container logs?

In order to get logs available when running `docker logs` command, you need to enable it in RavenDB server. Setting below environment variables like so is going to enable logging to console. Please note such behavior may have performance implications. Log level may be modified using `RAVEN_Logs_Mode` variable. 

{CODE-BLOCK:plain}
RAVEN_ARGS='--log-to-console'
{CODE-BLOCK/}

For more information about logging configuration make sure to check [the following article](../../server/configuration/logs-configuration);

### How to use a custom config file?

Mount it as a docker volume and use `--config-path PATH_TO_CONFIG` command line argument in order to use settings file from outside of server directory. Alternatively you can pass your custom `settings.json` content via `RAVENDB_SETTINGS` environment variable.

### How can I manage server running in a container?

Except the RavenDB Studio that you can access from the browser, you can connect to RavenDB administration console with the `rvn` utility like so:

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

For more information on using RavenDB Console please refer to [Administration - RavenDB CLI](../../server/administration/cli) article.

## Dockerfiles

These images were built using the following Dockerfiles:

- [Windows Nanoserver image Dockerfile](https://github.com/ravendb/ravendb/blob/v4.1/docker/ravendb-nanoserver/Dockerfile)

- [Ubuntu image Dockerfile](https://github.com/ravendb/ravendb/blob/v4.1/docker/ravendb-ubuntu/Dockerfile)

## Remarks

For more detailed information on how to use and setup RavenDB on Docker, please visit our **Docker Hub** page available [here](https://hub.docker.com/r/ravendb/ravendb/).

## Related Articles

### Installation

- [Running as a Service](../../start/installation/running-as-service)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)

### Setup Examples


