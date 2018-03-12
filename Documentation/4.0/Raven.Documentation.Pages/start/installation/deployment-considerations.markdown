# Installation : Deployment considerations

Deployment of a RavenDB cluster requires some thought about how to setup the network, the cluster and the databases you'll use. 
In terms of hardware, while obviously more is generally better, RavenDB can do quite well on small instances (such as a resource
limited Docker container) and scale upward to machines with hundreds of GB of RAM and large number of cores.

## Performance considerations

For read-mostly scenarios, the more cores the machine have, the faster RavenDB will be, since reads take no locks and require no 
coordination from RavenDB. For write-heavy scenario, faster cores are better than more cores, generally, but the usual limit is 
the speed of the storage. For both reads & writes, given the choice between more cores and more memory, the typical answer would
be to get more memory. 

RavenDB does just fine when the size of the databases exceed the size of the physical memory, it is important to note, but it will
make use of all the memory that it can to ensure speedy queries and fast responses, so the more memory it has available, the better
things are.

In a cluster environment, avoid using a SAN or NAS to store the data from multiple RavenDB nodes in the same physical location, 
it is much better to have each node use its local disks, because this way there is no contention between the different nodes on the
same storage resources. 

Storage latency is also a very important factor in RavenDB's performance. If you are running on cloud infrastructure, you should 
ensure that you are using high IOPS disks. Avoid options such as burstable performance, since under load RavenDB may consume all the
burst capacity available and suffer because it is being throttled. If this happens, you'll usually get a warning about slow I/O in 
the studio. If you are running on physical hardware, use an SSD or NVMe drives. Drives using HDD will work, but may result in high
latencies under load because of the rotational disk seek times.

## Network considerations

RavenDB can be deployed either internally in your organization (secured network, only known good actors) or on the public internet.
Any deployment, aside from maybe on a developer machine, should use the secured mode. See the 
[Setup Wizard](../../start/installation/setup-wizard) for the details on how to do that. 

RavenDB will typically use two ports. One for HTTPS traffic, for clients and browsers and one for TCP, used by the cluster nodes to
communicate with each other. Both the HTTPS and TCP traffic are encrypted by default (unless you explicitly specify the unsecured setup)
using TLS 1.2. Be sure to open both ports in the firewall to allow the cluster node to talk to one another. 

RavenDB should _not_ typically be deployed behind a reversed proxy. The typical advantages of reverse proxies are based on their abitlity
to load balance, cache responses, etc. These features are great when proxying a web application, but actively harmful when you are talking
to a stateful system like a database. Your application should be talking to RavenDB directly, given the URLs of the cluster nodes and let
RavenDB itself handle issues such as high availibility, load balancing and security. 

## Running RavenDB

On Windows, RavenDB is usually run as a service. Make sure that the user running the RavenDB service has permissions to the RavenDB directory
and the specified data directory. You can setup RavenDB as a service using the `setup-as-service.ps1` script. 

On Linux, you'll typically run RavenDB as a daemon. The `install-daemon.sh` can handle the daemon registration for you (Ubunto only).

In either case, you can configure RavenDB using the `settings.json` file. The most important configurations are the data directory and the 
ips and ports RavenDB will listen to. It is recommended that you'll pick the fastest drives for RavenDB's data directory, while the binaries
for RavenDB can reside anywhere in the system.

## Related articles

- [Configuration Section](../../server/configuration/configuration-options)
- [Security in RavenDB](../../server/security/overview)
