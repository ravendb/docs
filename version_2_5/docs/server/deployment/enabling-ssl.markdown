# Enabling SSL

**Note: SSL can only be enabled when RavenDB is running as a Windows Service**

By default, secure connectivity is disabled. To enable the SSL in RavenDB, you need to change `Raven/UseSsl` configuration to `true`.  

The Next step is to tell RavenDB to use the specified X509 certificate. To do this execute the following command on the command line:   
<code>Raven.Server.exe /installSSL=PathToCertificate==CertificatePassword</code>    
E.g.   
<code>Raven.Server.exe /installSSL=C:\Temp\MyCertificate.pfx==MyPassword</code>  

To uninstall the certificate execute:    
<code>Raven.Server.exe /uninstallSSL=PathToCertificate==CertificatePassword</code>   