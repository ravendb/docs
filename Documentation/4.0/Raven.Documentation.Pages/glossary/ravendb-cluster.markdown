﻿# Glossary : Cluster

### What is a cluster?
A group of RavenDB servers which may or may not be on the same machine. 
This group of servers allows cluster-wide operations which execute on each node, using Raft to coordinate the execution.
If there is a leader, the cluster guarantees that at least (n/2) + 1 nodes would have the operation executed on them. (consensus quorum)
