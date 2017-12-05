# Linux Configuration Recommendations With RavenDB
* Consider the following setup. 
* Note that it might affect different system with a different behaivour.
* Some of the settings requires super user permissions
```
ulimit -n 65535
sysctl -w net.ipv4.ip_local_port_range="1024 65535"
sysctl -w vm.max_map_count=2000000
sysctl -w net.ipv4.tcp_tw_reuse=1
sysctl -w net.ipv4.tcp_tw_recycle=1
```

* Consider different swap physical drive. For details on current swapping partitions and priorities:
```
swapon
```

