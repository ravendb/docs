# Migration: Migrating from Docker Image 5.x or lower to 6.0 or higher
---

{NOTE: }

* Starting from version `6.0` RavenDB for Docker introduces an 
  improved security model, using a dedicated user rather than `root`.  
* RavenDB `6.0` and up also use a Debian archive file 
  ([.deb package](../../start/installation/gnu-linux/deb)), 
  applying a uniform internal structure for Ubuntu OS platforms.  
* To conform with these changes, installing RavenDB `6.0` or higher 
  in a system that already hosts RavenDB `5.x` or lower requires 
  the migration procedure explained below.  
* Read [here](../../start/containers/image-usage) more about running a RavenDB Docker image.  

* In this page:  
  * [Changes Made In RavenDB `6.0` And Up](../../migration/server/docker#changes-made-in-ravendb-6.0-and-up)  
  * [Migrating To `6.0` And Up](../../migration/server/docker#migrating-to-6.0-and-up)  

{NOTE/}

---

{PANEL: Changes Made In RavenDB 6.0 And Up}  

The **directory structure** used by RavenDB of version `6.0` 
and above, and the **user** we use to run RavenDB, are different 
from the directory structure and user used by older versions.  

* RavenDB Docker images up to `5.x`:  
   * Create a unique directory structure under Windows.  
   * Are installed and accessed using the `root` user on Ubuntu.  

* RavenDB Docker images from `6.0` up:  
   * Use a Debian archive file ([.deb package](../../start/installation/gnu-linux/deb)) 
     and create a similar directory structure under Windows and Ubuntu.  
   * Are installed and accessed using a dedicated `ravendb` user 
     instead of `root`, to improve security.  

Learn below how to address these differences when migrating 
from version `5.x` or lower to version `6.0` or higher.  

{PANEL/}

{PANEL: Migrating To `6.0` And Up}  

## Permit `ravendb` user to access mounted data directory.  

The default **UID** (User ID) and **GID** (Group ID) 
used by `ravendb` are **999**.  
Change owner of the RavenDB data directory to `ravendb` (`999` UID by default) on the container host.
E.g. `chown -R 999:999 $TARGET_DATA_DIR`

## Customizing the RavenDB data directory owner user UID/GID 

To customize the **UID** and **GID** of the `ravendb` user (e.g. to match your host user or for volume permissions), you can build your own image using the official `ravendb` base image. 

{CODE-BLOCK:csharp}
FROM ravendb/ravendb:7.0-ubuntu-latest

ARG USER_ID=1000
ARG GROUP_ID=1000

USER root

RUN groupmod -g "${GROUP_ID}" "ravendb" && \
usermod  -u "${USER_ID}" -g "${GROUP_ID}" ravendb && \
chown root:${USER_ID} /etc/ravendb/settings.json && \
find / -xdev -uid 0 -gid 999 -exec chown "root:${GROUP_ID}" {} +


USER ravendb
{CODE-BLOCK/}

### Build Instructions 

1. Save the above as `dockerfile` 
2. Open a terminal in that directory 
3. Build the custom image with your desired UID and GID: `docker build --build-arg USER_ID=YourUserID --build-arg GROUP_ID=YourGroupID -t image-name -f dockerfile`

## Migrate files and data  

The setup process will create the directory structure detailed 
[here](../../start/installation/gnu-linux/deb#file-system-locations).  
  
The script within the image will attempt to link the old version's 
data directory to the new version's data directory automatically upon start.  
If this attempt fails, an error will be produced.  

When mounting host directory make sure that **RavenDB data** is mounted under its new 
location on the container: `/var/lib/ravendb/data`  

Old data directory (default): `/opt/RavenDB/Server/RavenData`
New data directory (default): `/var/lib/ravendb/data`

{PANEL/}
