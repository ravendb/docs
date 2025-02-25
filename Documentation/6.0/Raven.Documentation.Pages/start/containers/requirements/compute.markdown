﻿# Computing Requirements for RavenDB in Containers

Whether you're deploying on-premises, in virtual machines, or in managed Kubernetes environments, understanding the computing requirements is key to ensuring smooth operation.

## **Container Runtime**

RavenDB requires a container runtime capable of running the Docker image. This includes common runtimes such as:

- Docker
- Podman
- containerd

Ensure your runtime supports the architecture and platform for your deployment.

## **Machine Requirements**

To achieve optimal performance, allocate resources according to your workload:

- **CPU:**
  A minimum of 2 cores is required for basic setups. For medium workloads, allocate at least 4 cores.
- **Memory:**
  At least 1 GB of RAM is essential for minimal setups. If additional memory is needed, consider using `swap` as an alternative for super-minimal setups.
- **Storage:**
  SSDs are recommended for low-latency I/O operations. Ensure sufficient capacity to accommodate your database size, along with extra space for indexing and backups.


## **Deployment Options**

#### On-Premise machines
Deploy RavenDB containers on **your physical servers**.
It provides full control over your hardware and networking.
It's suitable for environments with existing infrastructure.

All you need is one of the container runtimes, and a kernel.

#### Virtual Machines (VMs)
**Cloud-based or self-hosted VMs**. It's scalable and flexible, while maintaining control over resources.
AWS EC2, Azure Virtual Machines, or private data centers.

#### Kubernetes
Run RavenDB in managed Kubernetes clusters to simplify container orchestration and scalability.
This option supports dynamic workloads with features like autoscaling and node group management.

You should be able to deploy a **node group** to match your computing needs. Many providers are offering such service - EKS, AKS, GKE, etc.
Kubernetes always increases the cost of a solution **by far**, but the power it offers is often unmatched.


## **ARM Architecture Support**

RavenDB supports **ARM64**, allowing deployments on cost-efficient architectures.

- AWS Graviton instances.
- Azure Ampere-based virtual machines.
- Google Cloud Tau T2A instances.

The official RavenDB Docker image is compatible with both **x64** and **ARM64**, ensuring broad support across modern hardware.
