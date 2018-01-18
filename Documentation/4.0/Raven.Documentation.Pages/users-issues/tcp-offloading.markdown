#Tcp Offloading
##Symptoms: 
- Exception thrown in RavenDB client when accessing RavenDB server,  exception was because of socket timeout
- General slow operation of client system

##Cause:
It appears that the machine hosting the RavenDB server had the TCP offload functionality enabled, which seems to be causing connection drops, a problem experienced also by other applications like SQL Server.

##Resolution:
Disabling TCP Offloading.

<hr/>

##Further read:
[TCP Offloading Disabling](http://blogs.technet.com/b/onthewire/archive/2014/01/21/tcp-offloading-chimney-amp-rss-what-is-it-and-should-i-disable-it.aspx)
