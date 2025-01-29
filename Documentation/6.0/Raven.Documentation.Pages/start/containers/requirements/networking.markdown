# Networking Requirements for Running RavenDB in Containers

## **Overview**

Networking is a critical aspect of running RavenDB in containers.
As a database that relies on both HTTP and TCP protocols for communication,
proper network configuration is essential to ensure healthy cluster node-to-node communication and client connectivity.
This article outlines the specific networking requirements and configurations for RavenDB in containerized environments.


## **Key Networking Concepts for RavenDB**

#### Ports Used by RavenDB

- **HTTP Port:** Used for client communication, management, and the RavenDB Studio. Default is `8080`. In production, RavenDB should use secured, certificate-backed connection (HTTPS).
- **TCP Port:** Used for cluster communication between nodes. Default is `38888`.

Both ports must be properly exposed and accessible for RavenDB to function correctly, especially in a cluster setup.
Additionally, RavenDB must be able to **reach itself** e.g. for cluster health checks.


#### Cluster Communication

- RavenDB nodes within a cluster use both HTTP and TCP ports to perform clustering and replication. See more here: [Clustering Overview](../../../server/clustering/overview)
- It is crucial that traffic between these ports can flow freely across all nodes in the cluster.

#### PublicServerUrl

- In containerized environments, `RAVEN_ServerUrl` should point at the network interface bound to the container itself. While this is sufficient for internal communication, external clients and nodes cannot access this address.
- The `RAVEN_PublicServerUrl` must be set to a **DNS name**, **public-facing IP**, or **routing mechanism** (e.g., load balancer or proxy) that directs traffic to the RavenDB container. This ensures external traffic can properly reach the RavenDB instance.

**Common Issue**: Setting `RAVEN_PublicServerUrl` to `127.0.0.1` can cause confusion and connectivity problems.  
In containerized environments, **`127.0.0.1` refers to the loopback address inside the container, not the host machine**.  
This mismatch often leads to connectivity issues for external clients or nodes.


## **Networking Configuration for Containers**

#### Expose the server ports

Ensure both server ports are exposed and accessible internally.
Check firewall rules, security groups, port-forwarding configuration, network policies - anything that can interrupt the network buzz, depending on your setup.
Depending on your container runtime (e.g., Docker, Kubernetes), configure the ports to be published or mapped to the host machine or external network.

#### Configure PublicServerUrl

For external traffic, set `RAVEN_PublicServerUrl` to match the network visibility you require:

- For **LAN** access: Use the machine’s LAN IP (e.g., 192.168.x.x) and ensure it is reachable by other devices on the local network.
- For **external internet** access: Use a public-facing DNS name or IP address, often routed via a load balancer or reverse proxy.
- For localhost **testing** only: Use the loopback address (127.0.0.1), but note that this will restrict access to clients on the same container.

**Important Note**: The address set in RAVEN_PublicServerUrl determines how other systems (e.g., clients, other nodes, or users) perceive and reach the server. Ensure it aligns with your intended deployment network.

#### Ensure node-to-node communication

Nodes in a RavenDB cluster must communicate over both HTTP and TCP ports.
Networking configurations should allow unrestricted traffic flow between nodes to maintain cluster health.

#### Ensure client communication

Make sure your client traffic (including RavenDB Studio) reaches the server correctly.

On local networks, ensure the DNS or IP address provided in RAVEN_PublicServerUrl resolves correctly within the LAN.

On public networks, a load balancer or reverse proxy should direct traffic to the container. Using an internet-facing IP, DNS name, load balancer, reverse proxy or other mechanism, ensure consistent routing to the container.

#### Secure Communication
Use encryption (TLS/SSL) to secure the payload, especially when exposing RavenDB to public networks.
See more in [Requirements > Security](./security)


