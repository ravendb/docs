# Dockerfile overview

Let's familiarize ourselves with [RavenDB Dockerfile](https://github.com/ravendb/ravendb/blob/v6.2/docker/ravendb-ubuntu/Dockerfile.x64).


##### Setting the Base Image
{CODE-BLOCK:bash}
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy
{CODE-BLOCK/}

The `FROM` instruction sets the base image for the Dockerfile.
In this case, it uses Microsoft's lightweight .NET runtime dependencies image, specifically tailored for Ubuntu.
This provides a minimal foundation for running RavenDB in Docker, keeping the image size smaller and focusing only on required dependencies.


##### Defining Build Arguments
{CODE-BLOCK:bash}
ARG PATH_TO_DEB RAVEN_USER_ID RAVEN_GROUP_ID
{CODE-BLOCK/}

Here, three build arguments (`ARG`) are defined:

- `PATH_TO_DEB`: Specifies the path to the `.deb` package for installing RavenDB.
- `RAVEN_USER_ID` and `RAVEN_GROUP_ID`: Used to configure the user and group IDs for the `ravendb` user, ensuring proper file permissions and container security.

These arguments are placeholders to be replaced at build time with actual values.


##### Installing Required Dependencies
{CODE-BLOCK:bash}
RUN apt-get update \
    && apt-get install \
    && apt-get install --no-install-recommends openssl jq curl -y
{CODE-BLOCK/}


The `RUN` instruction executes commands to prepare the container environment:

- `apt-get update`: Updates the package list.
- `apt-get update`: Updates the OS package list.
- `apt-get install`: Installs system tools responsible for:
    - `openssl`: managing TLS certificates
    - `jq`: swiss army knife for handling JSON data
    - `curl`: performing HTTP(S) requests

The `--no-install-recommends` flag minimizes the installation of optional dependencies, keeping the image lean.


##### Setting Environment Variables

This section declares default values for environment variables configuring the image runtime. The settings file in RavenDB Docker image is empty and most of the configuration would usually be passed through environment variables.

{CODE-BLOCK:bash}
ENV RAVEN_ARGS='' \
    RAVEN_SETTINGS='' \
    RAVEN_IN_DOCKER='true' \
    RAVEN_Setup_Mode='Initial' \
    ....
{CODE-BLOCK/}


###### Key Environment Variables (Brief Overview)

- **`RAVEN_IN_DOCKER`**: Flag that RavenDB is running in a Dockerized environment for optimal behavior adjustments.
- **`RAVEN_Setup_Mode`**: `'Initial'` enables the setup wizard on the first run.
- **`RAVEN_DataDir`**: Defines where database files are stored (default `/var/lib/ravendb/data`), typically mounted as a volume for persistence.
- **`RAVEN_Security_MasterKey_Path`**: Path to the encryption master key for securing sensitive data.
- **`RAVEN_Security_UnsecuredAccessAllowed`**: `'PrivateNetwork'` permits unsecured access only within private networks, enhancing security if you run unsecured.
- **`RAVEN_ServerUrl_Tcp`**: Specifies the TCP port (`38888`) used for cluster communication between nodes.

These variables enable flexible configuration for development, testing, or production environments.

##### Exposing Ports
{CODE-BLOCK:bash}
EXPOSE 8080 38888 161
{CODE-BLOCK/}

The `EXPOSE` instruction declares the ports that RavenDB uses:

- `8080`: The primary HTTP port for web-based interactions.
- `38888`: TCP port for cluster communication.
- `161`: SNMP (Simple Network Management Protocol) port.

This doesn’t bind the ports but informs users about which ones are available.

##### Adding the RavenDB Package
{CODE-BLOCK:bash}
COPY "${PATH_TO_DEB}" /opt/ravendb.deb

RUN apt install /opt/ravendb.deb -y \
    && apt-get autoremove -y \
    && rm -rf /var/lib/apt/lists/*
{CODE-BLOCK/}

This section installs RavenDB:

1. The `COPY` command transfers the `.deb` package (RavenDB’s installation file) into the container at `/opt/ravendb.deb`.
2. The `RUN` command installs the package using `apt`, cleans up unnecessary files to reduce the image size - removes cached OS package lists (`/var/lib/apt/lists/*`).

---

##### Adding Scripts and Configuration
{CODE-BLOCK:bash}
COPY server-utils.sh cert-utils.sh run-raven.sh healthcheck.sh link-legacy-datadir.sh /usr/lib/ravendb/scripts/
COPY --chown=root:${RAVEN_USER_ID} --chmod=660 settings.json /etc/ravendb
{CODE-BLOCK/}

- Several utility scripts (`server-utils.sh`, `cert-utils.sh`, etc.) are copied into the container. These scripts manage server initialization, health checks, and certificates.
- `settings.json`, a configuration file, is copied into `/etc/ravendb` with specific ownership (`root:${RAVEN_USER_ID}`) and permissions (`660`) for security.

---

##### Setting the User and Health Check
{CODE-BLOCK:bash}
USER ravendb:ravendb

HEALTHCHECK --start-period=60s CMD /usr/lib/ravendb/scripts/healthcheck.sh
{CODE-BLOCK/}

1. `USER`: Switches the container to run as the `ravendb` user instead of `root`, improving security.
2. `HEALTHCHECK`: Defines a command to verify RavenDB's health. The script (`healthcheck.sh`) is run after a 60-second delay to allow the server to initialize.

---

##### Declaring Volumes
{CODE-BLOCK:bash}
VOLUME /var/lib/ravendb/data /etc/ravendb
{CODE-BLOCK/}

`VOLUME` specifies directories to be mounted as volumes. These are:

- `/var/lib/ravendb/data`: For database files.
- `/etc/ravendb`: For configuration and security-related files.

Ensures that data persists outside the container lifecycle, using volumes.

---

##### Setting the Working Directory and Entry Command
{CODE-BLOCK:bash}
WORKDIR /usr/lib/ravendb

CMD [ "/bin/bash", "/usr/lib/ravendb/scripts/run-raven.sh" ]
{CODE-BLOCK/}

- `WORKDIR`: Sets the default working directory inside the container, simplifying subsequent commands.
- `CMD`: Specifies the command to run when the container starts. Here, it launches a Bash shell to execute the `run-raven.sh` script, which handles server initialization and starts RavenDB.

---
