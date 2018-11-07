# Installation : Running in a Docker Container

A RavenDB Server can run on [Docker](https://www.docker.com/) using our official images available in these channels:

- [ravendb/ravendb](https://hub.docker.com/r/ravendb/ravendb/) containing stable, patch, rc and beta images
- [ravendb/ravendb-nightly](https://hub.docker.com/r/ravendb/ravendb-nightly/) containing our nightly releases

## Platforms

Server images are published on Docker for the following platforms:

- Ubuntu 16.04
- Windows Nano Server

## Storage Requirements

NTFS, ext4 file systems and other non NFS volumes mounts are supported.

{NOTE: SMB / CIFS mounts}

Linux Docker container running under Windows Docker host with sharing volumes [isn't supported due to CIFS protocol usage](../../start/installation/deployment-considerations#storage-considerations) 

{NOTE /}

## Tags

Beside tags matching the exact builds e.g. `4.0.7-ubuntu.16.04-x64` or `4.0.7-windows-nanoserver` each of the repositories contain the following tags for your convenience:

- `4.0-latest` this is an alias to `4.0-ubuntu-latest`
- `4.0-ubuntu-latest` is an alias to the latest RavenDB Ubuntu image
- `4.0-windows-nanoserver-latest` is an alias to the latest RavenDB Windows Nano Server image

## Example

To install the `latest` tag, you can issue a command as follows:

{CODE-BLOCK:bash}
docker run -d -p 8080:8080 -p 38888:38888 ravendb/ravendb
{CODE-BLOCK/}

You can access the RavenDB Management Studio by going to `http://localhost:8080` in your browser. This assumes that you are using the default networking configuration with Docker, and that the Docker instance is not exposed beyond the host machine. If you intend to host RavenDB on Docker and expose it externally, make sure to go through the security configuration first.

## Remarks

For more detailed information on how to use and setup RavenDB on Docker, please visit our **Docker Hub** page available [here](https://hub.docker.com/r/ravendb/ravendb/).

## Related Articles

### Installation

- [Running as a Service](../../start/installation/running-as-service)
- [Upgrading to New Version](../../start/installation/upgrading-to-new-version)
