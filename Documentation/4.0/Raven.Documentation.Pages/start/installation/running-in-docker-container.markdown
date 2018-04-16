# Installation : Running in a Docker container

A RavenDB Server can be run on [Docker](https://www.docker.com/) using our official images available in two channels:

- [ravendb/ravendb](https://hub.docker.com/r/ravendb/ravendb/) containing stable, patch, rc and beta images
- [ravendb/ravendb-nightly](https://hub.docker.com/r/ravendb/ravendb-nightly/) containing our nightly releases

## Platforms

Server images are published on Docker for the following platforms:

- Ubuntu 16.04
- Windows Nano Server

## Tags

Beside tags matching the exact builds e.g. `4.0.0-rc-40025-ubuntu.16.04-x64` or `4.0.0-rc-40025-windows-nanoserver` each of the repositories contain the following tags for your convenience:

- `latest` this is an alias to `ubuntu-latest`
- `ubuntu-latest` is an alias to the latest RavenDB Ubuntu image
- `windows-nanoserver-latest` is an alias to the latest RavenDB Windows Nano Server image

## Example

To install `latest` tag, you can issue a command as follows:

{CODE-BLOCK:bash}
docker run -d -e PUBLIC_SERVER_URL=http://10.0.75.2:8080 
        -e PUBLIC_TCP_SERVER_URL=http://10.0.75.2:38888 
        -p 8080:8080 
        -p 38888:38888 
        ravendb/ravendb
{CODE-BLOCK/}

You can access the RavenDB Management Studio by going to `http://10.0.75.2:8080` in your browser. This is assuming that you are using the default networking configuration with Docker, and that the Docker instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it externally, make sure to go through the security configuration first.

## Remarks

For more detailed information on how to use and setup RavenDB on Docker please visit our **Docker Hub** page available [here](https://hub.docker.com/r/ravendb/ravendb/).

## Related articles

- [Running as a Service](../../start/installation/running-as-service)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)
