Linux: Setting limits
---
Linux security limits might degrade RavenDB performance (and in encrypted database it might prevent actual functionality, see : `TODO : https://github.com/ravendb/docs/pull/975`), even if physical resources allows higher performance. Also debugging might be affected (i.e. : core dump creation).
Setting these limits in a persistant to recommended values way can be achived by editing `/etc/security/limits.conf` with:
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

Opening larger ports range can help RavenDB's machine to recieve more parallel requests. This can be done, in example, using:
```
sysctl -w net.ipv4.ip_local_port_range="10000 65535"
```
or by adding the following to `/etc/sysctl.conf`:
```
net.ipv4.ip_local_port_range=1024 65535
```

