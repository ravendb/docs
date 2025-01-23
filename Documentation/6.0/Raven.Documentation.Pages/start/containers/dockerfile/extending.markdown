# Extending & Modifying RavenDB Dockerfile

Some custom setups solutions may lead you to a necessity of building over our Dockerfile, or even customizing it.
This article explains in detail, how it works.


For detailed Dockerfile guide, visit Containers > Dockerfile > Overview

## Extending the Existing `ravendb/ravendb` Image


This approach involves using `FROM ravendb/ravendb` to build upon the official image, adding custom commands, scripts, or configurations.


##### Why Should I Extend the RavenDB Dockerfile?

- **Simple customization:** Ideal for small, incremental changes that do not require rebuilding the entire image from scratch.
- **Keeps RavenDB's default behavior:** Build on top of the existing official `ravendb/ravendb` image while preserving core functionality.
- **Customization of startup scripts:** Provides flexibility to replace the `run-raven.sh` startup script with a custom, more focused version tailored to your deployment needs.

##### **Dockerfile Example for Extension**

```docker
# Use the official RavenDB image as the base
FROM ravendb/ravendb:7.0-ubuntu-latest

# Add your custom script or commands
COPY my-script.sh /usr/lib/ravendb/scripts/my-script.sh
RUN chmod +x /usr/lib/ravendb/scripts/my-script.sh

# Add an environment variable
ENV MY_CUSTOM_VAR="MyValue"

# Customize the HEALTHCHECK
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080 || exit 1

# Replace the CMD (optional!)
COPY my-run-raven.sh /usr/lib/ravendb/scripts/run-raven.sh
CMD ["/bin/bash", "/usr/lib/ravendb/scripts/run-raven.sh"]
```

##### Building for Multiple Platforms

Use BuildX if you need to build for multiple architectures (e.g., x64, arm64):

```bash
docker buildx build --platform linux/amd64,linux/arm64 -t my-ravendb .
```

##### Tips
You can:
- Add scripts that configure or extend RavenDB's behavior (e.g., preloading data or setting specific configurations). Ensure scripts are executable (chmod +x).
- Pass custom values or configurations via ENV directives or at runtime using docker run -e.
- Customize HEALTHCHECK to match your deployment’s requirements, ensuring RavenDB is responding as expected.
---

## Customizing the RavenDB Dockerfile

If your use case requires more extensive customization, you can modify the Dockerfile directly to create a tailored image.

##### Possibilities of modifications

###### Change the Base Image
Replace `FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy` with another OS, such as `debian:buster` or `alpine:latest`.

However, make sure to select one that includes .NET runtime dependencies or install them on your own.

###### Add Custom Dependencies
Use `RUN` commands to install additional dependencies specific to your environment.

###### Customize Environment Variables
Adjust `ENV` declarations to preconfigure RavenDB settings, such as data directories or logs.
You can do the same thing in Docker using `-e` argument without tinkering with the Dockerfiles, but may want to have this fixed in the image.

###### Replace the Entry Script
Copy and use a custom `run-raven.sh` script:
```docker
COPY my-run-raven.sh /usr/lib/ravendb/scripts/run-raven.sh
CMD ["/bin/bash", "/usr/lib/ravendb/scripts/run-raven.sh"]
```


##### Entry Script ([run-raven.sh)](https://github.com/ravendb/ravendb/blob/v6.2/docker/ravendb-ubuntu/run-raven.sh)

This script initializes and runs the RavenDB server.
It's designed to work for anyone, so in some scenarios you may want to chisel it down a bit, modify, or completely replace, which we'll cover next.
Key responsibilities include:

###### Legacy Data Migration
```bash
/usr/lib/ravendb/scripts/link-legacy-datadir.sh
```
This script was created to handle legacy data volumes, that were working with RavenDB before 6.0.
The data had different directory structure inside Linux back then, so we needed to migrate them properly after update to 6.0+.

###### Command Construction
Constructs the RavenDB server start command using arguments and environment variables:
```bash
COMMAND="/usr/lib/ravendb/server/Raven.Server"
```

###### Certificate Checks
Ensures proper certificate configuration for secure HTTPS connections.

###### Startup Environment Setup

Configures the `RAVEN_ServerUrl` environment variable if not already set.

###### Graceful Shutdown

Handles termination signals to cleanly stop the server.

###### Database Auto-Creation
Calls a utility script to create the database if the `RAVEN_DATABASE` environment variable is set.

#### Replacing run-raven.sh

Replacing allows full control of the startup process for both extending and modifying approaches. The only requirement is that **the script must ultimately run the RavenDB server**. Everything else is optional and can be customized to fit your specific needs. Some ideas include:

###### Certificates management

Retrieve and configure SSL certificates, such as from a secure vault or web server.

###### Exports Environment Variables

Define and set variables like `RAVEN_ServerUrl` to match your deployment requirements.

###### Handles Shutdown Gracefully

Use signal trapping for smooth termination of the server:
```bash
trap 'kill -TERM "$COMMANDPID"' TERM INT
```

###### Custom Operations
Add database initialization or additional setup scripts as needed.

---

##### **Example Custom Script**

```bash
# Custom startup script for RavenDB

echo "Starting custom RavenDB setup..."

# Get and configure certificates
get_certificates() {
    # Implement logic to retrieve certificates, e.g., from a web server or secured vault
}

check_for_certificates

# Start RavenDB
COMMAND="/usr/lib/ravendb/server/Raven.Server -c /etc/ravendb/settings.json"
$COMMAND &
COMMANDPID=$!

# Handle shutdown gracefully
trap 'kill -TERM "$COMMANDPID"' TERM INT
wait $COMMANDPID
```

---
