# Installation : Running in a Docker Container

A RavenDB Server can run on [Docker](https://www.docker.com/) using our official images available in these channels:

- [ravendb/ravendb](https://hub.docker.com/r/ravendb/ravendb/) containing stable, patch, rc and beta images
- [ravendb/ravendb-nightly](https://hub.docker.com/r/ravendb/ravendb-nightly/) containing our nightly releases

## Platforms

Server images are published on Docker for the following platforms:

- Ubuntu 16.04
- Windows Nano Server

## Tags

Beside tags matching the exact builds e.g. `4.0.7-ubuntu.16.04-x64` or `4.0.7-windows-nanoserver` each of the repositories contain the following tags for your convenience:

- `4.0-latest` this is an alias to `4.0-ubuntu-latest`
- `4.0-ubuntu-latest` is an alias to the latest RavenDB Ubuntu image
- `4.0-windows-nanoserver-latest` is an alias to the latest RavenDB Windows Nano Server image

## Examples

To install the `latest` tag, you can issue a command as follows:

{CODE-BLOCK:bash}
docker run -d -p 8080:8080 -p 38888:38888 ravendb/ravendb
{CODE-BLOCK/}

To install the `latest` tag but also persist the data to your hard-disk if the container is removed, you can issue a command as follows:

{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData ravendb/ravendb
{CODE-BLOCK/}

This requires that your docker client application has `sharing` enabled and that the folder (in this case, `C:\RavenDb\Data`) exists. So now, if the container is removed, the data remains. Later on, you can start up a new instance of the image with the volume mounted, the data comes back!

Finally, you might not want to run through the setup-wizard each time you wish to start RavenDb container on your localhost. To skip that setup-wizard you can issue the following command:
{CODE-BLOCK:bash}
docker run --rm -d -p 8080:8080 -p 38888:38888 -v c:/RavenDb/Data:/opt/RavenDB/Server/RavenData --name RavenDb-WithData -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork ravendb/ravendb
{CODE-BLOCK/}

This will skip the wizard and mount a volume for data persistence.

You can access the RavenDB Management Studio by going to `http://localhost:8080` in your browser. This assumes that you are using the default networking configuration with Docker, and that the Docker instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it externally, make sure to go through the security configuration first.

## Remarks

For more detailed information on how to use and setup RavenDB on Docker, please visit our **Docker Hub** page available [here](https://hub.docker.com/r/ravendb/ravendb/).

## Related Articles

### Installation

- [Running as a Service](../../start/installation/running-as-service)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)
