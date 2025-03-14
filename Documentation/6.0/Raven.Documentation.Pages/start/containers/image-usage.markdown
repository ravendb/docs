﻿# RavenDB Images

## Usage
RavenDB offers official images based on Ubuntu and Windows NanoServer.

---

First, let's describe basic development usage of our images.
We'll **focus on basics**, not going *too deep* into security, networking and storage.
For detailed instructions on how to spin up your production, you can read our guides for specific container/orchestration platforms - search through our [articles](https://ravendb.net/articles), or go *deep* with our [RavenDB Containers Docs](.) that also addresses containerized RavenDB [production requirements](./requirements).

To quickly try out RavenDB, you can run the following command:

{CODE-BLOCK:bash}
docker run -it --rm -p 8080:8080 ravendb/ravendb
{CODE-BLOCK/}

This will:

1. Download the appropriate image from DockerHub (if not already cached).
2. Run RavenDB, exposing the web interface on port `8080`.
3. Enter the container shell.
4. Kill and remove the container at exit.


#### **Available Tags**

RavenDB images are available in the following flavors:

- `latest`/`latest-lts`: The latest stable or latest long-term support (LTS) version of RavenDB
- `ubuntu-latest / ubuntu-latest-lts`: Ubuntu floating tags
- `windows-latest`, `windows-latest-lts`: Windows Nanoserver floating tags
- **Fixed** tags like `6.2.2-ubuntu.22.04-arm32v7`, `6.0.108-ubuntu.22.04-x64`, `6.2.2-windows-1809`, and more. Check the full [tags list](https://hub.docker.com/r/ravendb/ravendb/tags) for details.

#### **Runtime customization**

While running your container with RavenDB inside, you may need to use some options, that either modify the container behavior, or edit Raven configuration.
Here are some examples:

| **Option**                                                        | **Description**                                                         |
|-------------------------------------------------------------------|-------------------------------------------------------------------------|
| `-p 8080:8080`                                                    | Maps the RavenDB web interface to port `8080` on the host machine.      |
| `-v /my/config:/etc/ravendb`                                      | Mounts a custom configuration directory.                                |
| `-v /my/data:/var/lib/ravendb/data`                               | Mounts a custom data directory for persistence.                         |
| `-e RAVEN_Setup_Mode=Initial`                                     | Configures the setup mode (e.g., `None`, `Initial`, `LetsEncrypt`).     |
| `-e RAVEN_ServerUrl=http://0.0.0.0:46290`                         | Run RavenDB on custom HTTP port.                                        |
| `-e RAVEN_Logs_Mode=Operations` (in 7.0+: `RAVEN_Logs_Min_Level`) | Sets the logging level.                                                 |
| `-e RAVEN_Security_UnsecuredAccessAllowed=PublicNetwork`          | Allows unsecured access for development purposes.                       |
| `--restart unless-stopped`                                        | Ensures the container restarts automatically unless explicitly stopped. |

#### **Using Environment Variables to Configure RavenDB**

RavenDB's behavior can be configured through environment variables. These variables allow you to:

- Disable the setup wizard: `RAVEN_Setup_Mode=None`
- Set RavenDB License: `RAVEN_License`
- Set Public Server Url: `RAVEN_PublicServerUrl`
- Configure logging: `RAVEN_Logs_Mode=Operations` (in 7.0+: `RAVEN_Logs_Min_Level=3`)

Example:

{CODE-BLOCK:bash}
docker run -p 8080:8080 \
  -e RAVEN_Setup_Mode=None \
  -e RAVEN_Security_UnsecuredAccessAllowed=PrivateNetwork \
  -e RAVEN_Logs_Min_Level=3 \
  ravendb/ravendb:ubuntu-latest
{CODE-BLOCK/}

For more options, visit this page: [Server Configuration](../../server/configuration)

#### **Storing data**

For development purposes, you may want to persist your data.
RavenDB uses the following volumes for persistence and configuration:

- **Configuration Volume**: `/etc/ravendb` (e.g., settings.json)
- **Data Volume**: `/var/lib/ravendb/data`

To mount these volumes, use:

{CODE-BLOCK:bash}
docker run -v /path/to/config:/etc/ravendb \
  -v /path/to/data:/var/lib/ravendb/data \
  ravendb/ravendb:ubuntu-latest
{CODE-BLOCK/}

To learn about statefullness and storing RavenDB data in a containers, or if you run into trouble, visit [Containers > Requirements > Storage](./requirements/storage).

#### **Advanced Networking**
To read more on RavenDB networking containerized environment, go to [Containers > Requirements > Networking](./requirements/networking).

{PANEL: FAQ}

**Q: I use Docker Compose or automated installation. How do I disable the setup wizard?**
A: Set the `Setup.Mode` configuration option to `None` like so:

{CODE-BLOCK:bash}
RAVEN_ARGS='--Setup.Mode=None'
{CODE-BLOCK/}

**Q: How can I try RavenDB on my local development machine in unsecured mode?**
A: Set the environment variables:

{CODE-BLOCK:bash}
RAVEN_ARGS='--Setup.Mode=None'
RAVEN_Security_UnsecuredAccessAllowed='PrivateNetwork'
{CODE-BLOCK/}

**Q: How can I pass command-line arguments through environment variables?**
A: By modifying the `RAVEN_ARGS`, which will pass the arguments to the RavenDB server:

{CODE-BLOCK:bash}
RAVEN_ARGS='--log-to-console'
{CODE-BLOCK/}

**Q: Can I view RavenDB logs using the `docker logs` command?**
A: Yes, but you need to enable console logging by setting the following environment variable:

{CODE-BLOCK:bash}
RAVEN_ARGS='--log-to-console'
{CODE-BLOCK/}

Additionally, use `RAVEN_Logs_Min_Level` (7.0+) to set more specific desired logging levels.

Note that enabling logging to the console may impact performance.

**Q: How do I use a custom configuration file?**
A: Mount the configuration file as a Docker volume and use the `--config-path` argument:

{CODE-BLOCK:bash}
docker run -v /path/to/settings.json:/etc/ravendb/settings.json \
-e RAVEN_ARGS='--config-path /etc/ravendb/settings.json' \
ravendb/ravendb
{CODE-BLOCK/}

Alternatively, pass the custom `settings.json` content via the `RAVENDB_SETTINGS` environment variable.

**Q: How can I manage a server running in a container using CLI?**
A: Besides using the RavenDB Studio, you can connect to the RavenDB administration console using the `rvn` utility:

{CODE-BLOCK:bash}
docker exec -it CONTAINER_ID /var/lib/ravendb/Server/rvn admin-channel
{CODE-BLOCK/}

This will connect you to the RavenDB admin console, where you can manage the server interactively.

{PANEL/}

---
