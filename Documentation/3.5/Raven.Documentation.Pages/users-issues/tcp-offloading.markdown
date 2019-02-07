#Tcp Offloading
##Symptoms: 
- Exception thrown in Raven client when accessing raven server,  exception was because of socket timeout
- General slow operation of client system

##Cause:
It appears that the machine hosting the RavenDB server had the TCP offload functionality enabled, which seems to be causing connection drops, a problem experienced also by other applications like SQL Server.

##Resolution:
Disabling TCP Offloading.

<hr/>

##Further read:
[TCP Offloading Disabling](https://blogs.technet.microsoft.com/onthewire/2014/01/21/tcp-offloadingchimney-rsswhat-is-it-and-should-i-disable-it/)
