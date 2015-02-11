#Enabling Remote Differential Compression

File synchronization feature relies on Remote Differential Compression algorithm built-in Windows. You need to enable it to ensure proper work of RavenFS.

##Windows Server 2012

1. Go to `Server Manager`.
2. Open `Add Roles and Features` wizard.
3. Select `Remote Differential Compression` feature and install it.

![Figure 1: Enabling RDC on Windows Server 2012](images\enable-rdc-windows-server-2012.png)


##Windows 8

1. Open `Control Panel`.
2. Go to `Programs and Features` and click on `Turn Windows features on or off` option.
3. Check `Remote Differential Compression API Support` and click `OK`.

![Figure 2: Enabling RDC on Windows 7 and 8](images\enable-rdc-windows-7-and-8.png)

