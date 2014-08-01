# Configuration : Enabling SSL

{INFO SSL can only be enabled when RavenDB is running as a Windows Service /}

By default, secure connectivity is disabled. To enable the SSL in RavenDB, you need to do the following:

1. change `Raven/UseSsl` configuration to `true`.  
2. tell RavenDB to use the specified X509 certificate. 

To tell RavenDB to use the specified X509 certificate execute the following command on the command line:   
<code>Raven.Server.exe /installSSL=PathToCertificate==CertificatePassword</code>    
E.g.   
<code>Raven.Server.exe /installSSL=C:\Temp\MyCertificate.pfx==MyPassword</code>  

To uninstall the certificate execute:    
<code>Raven.Server.exe /uninstallSSL=PathToCertificate==CertificatePassword</code>   

{NOTE The Studio might not work in some browsers (e.g. Firefox and Chrome) if the domain that certificate was issued for does not match the domain you are using it for. /}   

#### Related articles

TODO