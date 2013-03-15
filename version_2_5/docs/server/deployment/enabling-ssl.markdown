# Enabling SSL

By default the secure connectivity is disabled, to enable the SSL in the RavenDB, one needs to change `Raven/UseSsl` configuration to `true`.

Next step is to tell RavenDB to use the specified X509 certificate. To do this execute the following command on the command line:   
<code>Raven.Server.exe /installSSL=PathToCertificate==CertificatePassword</code>    
E.g.   
<code>Raven.Server.exe /installSSL=C:\Temp\MyCertificate.pfx==MyPassword</code>  

To uninstall certificate execute:    
<code>Raven.Server.exe /uninstallSSL=PathToCertificate==CertificatePassword</code>   