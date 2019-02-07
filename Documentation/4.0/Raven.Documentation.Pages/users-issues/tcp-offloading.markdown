#TCP Offloading

##Symptoms

- An exception is thrown in RavenDB Client when accessing RavenDB Server, because of a socket timeout
- A general slow operation of client system

##Cause

It appears that the machine hosting the RavenDB Server had the TCP offload functionality enabled, which seems to be causing connection drops, a problem experienced also by other applications like SQL Server.

##Resolution

Disable TCP Offloading.

##Further read

- [TCP Offloading Disabling](https://blogs.technet.microsoft.com/onthewire/2014/01/21/tcp-offloadingchimney-rsswhat-is-it-and-should-i-disable-it/)
