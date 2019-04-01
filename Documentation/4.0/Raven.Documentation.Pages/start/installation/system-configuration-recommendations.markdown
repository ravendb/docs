# Installation: System Configuration Recommendations

{PANEL: Linux - Ubuntu 16.04}

RavenDB uses the resources of the machine it is running on, limited to the configuration limitation. In order to benefit from higher resources usage, consider the following setup:

* Note that this might affect different systems with different behaviors
* Some of the settings require super user permissions

---

### Set higher limit to max open file descriptors

In order to handle more connections to more databases and indexes at once, higher limit to open file descriptors (which affects also max number of network connections) is required.

{CODE-BLOCK:bash}
ulimit -n 65535
{CODE-BLOCK/}

---

### Enlarge local available port range

In order to serve more connections at once (heavy traffic usage) from clients, a higher port range is required.

{CODE-BLOCK:bash}
sysctl -w net.ipv4.ip_local_port_range="1024 65535"
{CODE-BLOCK/}

---

### Reuse TIME-WAIT sockets

Time to make reuse of closing tcp sockets. This allows RavenDB to handle more requests faster.

{CODE-BLOCK:bash}
sysctl -w net.ipv4.tcp_tw_reuse=1
{CODE-BLOCK/}

---

### Recycle TIME-WAIT sockets

Similar usage to the above reuse. Not for use when working with NAT, or in Kernel 4.12+

{CODE-BLOCK:bash}
sysctl -w net.ipv4.tcp_tw_recycle=1
{CODE-BLOCK/}

---

### Enlarge memory mapped max usage

RavenDB uses memory mapped files (few per database, indexes, etc). Upper limit is required in order to server larger amounts of databases and indexes

{CODE-BLOCK:bash}
sysctl -w vm.max_map_count=2000000
{CODE-BLOCK/}

---

### Select swap device

Consider different swap physical drive.
For details on current swapping partitions and priorities use:

{CODE-BLOCK:bash}
swapon
{CODE-BLOCK/}

{PANEL/}

## Related articles

### Installation

- [System Requirements](../../start/installation/system-requirements)
- [Deployment Considerations](../../start/installation/deployment-considerations)
