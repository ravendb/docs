# Storage Requirements for Running RavenDB in Containers

## **Overview**

RavenDB is a *database*, and requires reliable and durable storage for its data.
This article focuses on describing the storage needs for **containerized environments**,
addressing the unique challenges and requirements of running RavenDB in containers.
Containers, being stateless, require robust storage configurations to persist RavenDB data across container restarts, upgrades, or failures.

If you are looking for a broader understanding of RavenDB's storage mechanisms, please refer to the following articles:
- [Storage Engine](https://ravendb.net/docs/article-page/6.2/php/server/storage/storage-engine)
- [Directory Structure](https://ravendb.net/docs/article-page/6.2/php/server/storage/directory-structure)

---

## **Why Storage Matters for RavenDB Containers**

Containers encapsulate applications for consistency and portability but lack built-in mechanisms for persisting data.
As a database, RavenDB must store its data in a **persistent volume** or an equivalent storage solution to survive the ephemeral nature of containers.
Without proper configuration:
- Data will be lost if the container is restarted or replaced.
- Performance may degrade due to suboptimal storage setups.
- Inconsistent behavior can arise during scaling, updates or failover operations.

---

## **Requirements**

1. **Volume Configuration**
    - RavenDB requires a volume (or equivalent storage backend) to store its data files, journals, and indexes.
    - The volume must be explicitly mounted into the container and made accessible to the `ravendb` process.

2. **Permissions**
    - The container runs with the `ravendb` user (`UID:GID 999:999`).
    - Ensure the mounted storage has the correct read/write permissions for the `ravendb` user.
    - If you're using volume, that is a slice of already-existing file system, the original file system needs to already have correct permissions set for `ravendb (999:999)`.

3. **Storage Backend Options for Containers**
    - Host-mounted volumes
    - Managed storage services like AWS EBS, Azure Disk, or Google Persistent Disks

---

## **Guides**
For more specific setup guides, please refer to step-by-step guides for containerized and orchestrated setups - https://ravendb.net/articles

