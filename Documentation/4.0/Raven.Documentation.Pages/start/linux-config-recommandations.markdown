# Linux Configuration Recommendations With RavenDB
RavenDB uses the resources of the machine it is running at, limited to the configuration limitation. In order to benfite higher resources usage, consider the following setup:

* Note that it might affect different system with a different behaivour.
* Some of the settings requires super user permissions

##### Set higher limit to max open file descriptors
In order to handle more connections to more databases and indexes at once, higher limit to open file descriptors (which affects also max number of network connections) is required

`ulimit -n 65535`
##### Enlarge local available port range
In order to serve more connections at once (heavy traffic usage) from clients, higher port range is required

`sysctl -w net.ipv4.ip_local_port_range="1024 65535"`
##### Reuse TIME-WAIT sockets
Time to make reuse of closing tcp sockets. Allows RavenDB handle more requests faster.

`sysctl -w net.ipv4.tcp_tw_reuse=1`
##### Recycle TIME-WAIT sockets
Similar usage to the above reuse. Not for use when working with NAT, or in Kernel 4.12+

`sysctl -w net.ipv4.tcp_tw_recycle=1`
##### Enlarge memory mapped max usage
RavenDB uses memory mapped files (few per database, indexes, etc). Upper limit is required in order to server bigger amount of databases and indexes

`sysctl -w vm.max_map_count=2000000`
##### Select swap device
Consider different swap physical drive.
For details on current swapping partitions and priorities use:
`swapon`


* Consider the following setup. 
* Note that it might affect different system with a different behaivour.
* Some of the settings requires super user permissions

##### Set higher limit to max open file descriptors
`ulimit -n 65535`
##### Enlarge local available port range
`sysctl -w net.ipv4.ip_local_port_range="1024 65535"`
##### Reuse TIME-WAIT sockets
`sysctl -w net.ipv4.tcp_tw_reuse=1`
##### Recycle TIME-WAIT sockets (not for use when working with NAT, or in Kernel 4.12+)
`sysctl -w net.ipv4.tcp_tw_recycle=1`
##### Enlarge memory mapped max usage
`sysctl -w vm.max_map_count=2000000`
##### Consider different swap physical drive. For details on current swapping partitions and priorities:
`swapon`

