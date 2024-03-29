# Linux: Setting limits
---
Linux security limits may degrade RavenDB performance, and in an encrypted database even prevent actual functionality, 
even if physical resources allow higher performance. Additionally, debugging may be affected (i.e. core dump creation).  

Setting these limits in a persistant way can be achived by editing `/etc/security/limits.conf` to recommended values:  
```
* soft     core            unlimited
* hard     core            unlimited
* soft     nofile          131070
* hard     nofile          131070
* soft     nproc           131070
* hard     nproc           131070
* soft     memlock         1000
* hard     memlock         1000
```

Opening a larger ports range can help RavenDB's machine process a larger number of parallel requests.  
E.g., this can be achieved using ```sysctl -w net.ipv4.ip_local_port_range="10000 65535"```  
or by adding ```net.ipv4.ip_local_port_range=1024 65535``` to `/etc/sysctl.conf`.  
