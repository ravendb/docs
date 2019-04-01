# Installation: Running as a service

RavenDB supports running as a system service, creating its own HTTP server, and processing all requests internally.

## Installing the RavenDB service

1. Extract the zip with the build files (if you do not have it yet, download one from [here](https://ravendb.net/download)).
2. Go to the Server directory
3. Execute the following command on the command line: <code>Raven.Server.exe /install</code>  
    _Note:_ Raven may ask you for administrator privileges while installing the service.

RavenDB is now installed and running as a service.

## Uninstalling the RavenDB service

1. Go to the Server directory
2. Execute the following command on the command line: <code>Raven.Server.exe /uninstall</code>

The server storage and indexes will not be deleted when the server is uninstalled.

## Server Configuration

Many configuration options are available for tuning RavenDB and fitting it to your needs. See the [Configuration options](https://ravendb.net/docs/server/administration/configuration) page for the complete info.

## Related articles

 - [Using installer](./using-installer)
