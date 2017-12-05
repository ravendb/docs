# Linux Configuration Recommendations With RavenDB
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

