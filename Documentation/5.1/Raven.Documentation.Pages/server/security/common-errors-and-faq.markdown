# Security: Common Errors & FAQ

In this page, we review some of the common security configuration errors and explain how to handle them.

* [Setup Wizard Issues](../../server/security/common-errors-and-faq#setup-wizard-issues)  
* [Changing Configurations and Renewals Issues](../../server/security/common-errors-and-faq#changing-configurations-and-renewals-issues)  
* [Authentication Issues](../../server/security/common-errors-and-faq#authentication-issues)  
* [Encryption Issues](../../server/security/common-errors-and-faq#encryption-issues)  

{PANEL: Setup Wizard Issues}  

* [Server cannot bind to the provided private IP address](../../server/security/common-errors-and-faq#server-cannot-bind-to-the-provided-private-ip-address)
* [Ports are blocked by the firewall](../../server/security/common-errors-and-faq#ports-are-blocked-by-the-firewall)
* [DNS is cached locally](../../server/security/common-errors-and-faq#dns-is-cached-locally)
* [Long DNS propagation time](../../server/security/common-errors-and-faq#long-dns-propagation-time)
* [If I already have the Zip file, can I avoid repeating the setup process?](../../server/security/common-errors-and-faq#if-i-already-have-the-zip-file-can-i-avoid-repeating-the-setup-process)

---

### Server cannot bind to the provided private IP address

If the IP/port is not accessible on your machine, you'll get the following error.

{CODE-BLOCK:plain}
System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> 
System.InvalidOperationException: Validation failed. ---> 
System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.example.ravendb.community:4433  ---> 
System.InvalidOperationException: Failed to start webhost on node 'A'. The specified ip address might not be reachable due to network issues. 
It can happen if the ip is external (behind a firewall, docker). If this is the case, try going back to the previous screen and add the same ip as an external ip.
Settings file: D:\temp\RavenDB-4.0.0-windows-x64\Server\settings.json.
IP addresses: 10.0.0.65:4433. 
---> Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking.UvException: Error -4092 EACCES permission denied
{CODE-BLOCK/}

This can be caused by two different reasons:

1. Your private IP address is not reachable inside the machine or you provided the wrong IP/port.
2. You are running behind a firewall (VM, docker...) and accidentally provided the external IP address during setup.

Make sure you provide the private IP address in the "IP Address / Hostname" field as seen in [this example](../../start/installation/setup-wizard#example-iii---behind-a-firewall).

---

### Ports are blocked by the firewall

When configuring a VM in Azure, [AWS](../../start/installation/setup-examples/aws-windows-vm) or any other provider, you should define firewall rules to allow both the **HTTP** and **TCP** ports you have chosen during setup.
This should be done both inside the VM operating system **and** in the web dashboard or management console.

If ports are blocked you'll get the following error.
{CODE-BLOCK:plain}
Setting up RavenDB in Let's Encrypt security mode failed.
System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> 
System.InvalidOperationException: Validation failed. ---> 
System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.example.development.run:443 ---> 
System.InvalidOperationException: Client failed to contact webhost listening to 'https://a.example.development.run:443'.
Are you blocked by a firewall? Make sure the port is open.
Settings file: D:\RavenDB-4.0.0-windows-x64\Server\settings.json.
IP addresses: 10.0.1.4:443.
{CODE-BLOCK/}

---

### DNS is cached locally

Most networks cache DNS records. In some environments you can get an error such as this:

{CODE-BLOCK:plain}
Setting up RavenDB in Let's Encrypt security mode failed.
System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> 
System.InvalidOperationException: Validation failed. ---> 
System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.onenode.development.run ---> 
System.InvalidOperationException: Tried to resolve 'a.onenode.development.run' locally but got an outdated result.
Expected to get these ips: 127.0.0.1 while the actual result was: 10.0.0.65
If we try resolving through google's api (https://dns.google.com), it works well.
Try to clear your local/network DNS cache or wait a few minutes and try again.
Another temporary solution is to configure your local network connection to use google's DNS server (8.8.8.8).
{CODE-BLOCK/}

This error probably means that the DNS is cached. You can wait a few minutes or reset the network DNS cache, 
but in many cases the easiest solution is to [temporarily switch your DNS server to 8.8.8.8](https://developers.google.com/speed/public-dns/docs/using) 
You can click the Try Again button to restart the validation process of the Setup Wizard.

---

### Long DNS propagation time

If you are trying to modify existing DNS records, for example running the Setup Wizard again for the same domain name, you may encounter errors such as this:

{CODE-BLOCK:plain}
Setting up RavenDB in Let's Encrypt security mode failed.  

System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> 
System.InvalidOperationException: Validation failed. ---> 
System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.example.development.run ---> 
System.InvalidOperationException: Tried to resolve 'a.example.development.run' using google's api (https://dns.google.com). 
Expected to get these ips: 127.0.0.1 while google's actual result was: 10.0.0.65 
Please wait a while until DNS propagation is finished and try again. If you are trying to update existing DNS records, 
it might take hours to update because of DNS caching. If the issue persists, contact RavenDB's support.
{CODE-BLOCK/}  

If this happens, there is nothing you can do except wait for DNS propagation. When it's updated in dns.google.com click the `Try Again` button.  
You can keep track of your RavenDB clusters and their associated DNS records at the [Customer's Portal](https://customers.ravendb.net).

---

### If I already have the Zip file, can I avoid repeating the setup process?

Yes.  
You can use the Zip file to re-install or deploy the server/cluster elsewhere.  
Download a fresh copy of RavenDB and run the setup wizard. Then choose `Continue Cluster Setup` and select node A.
This will use the existing Zip file and the same configuration and certificate which were previously chosen.  
When building a cluster, repeat this step with nodes B, C, and so on.


{PANEL/}

{PANEL: Changing Configurations and Renewals Issues}

* [After installing with Let's Encrypt, can I change the DNS records?](../../server/security/common-errors-and-faq#after-installing-with-lets-encrypt-can-i-change-the-dns-records)
* [Can I change the (private) IP address RavenDB binds to?](../../server/security/common-errors-and-faq#can-i-change-the-private-ip-address-ravendb-binds-to)
* [The Let's Encrypt certificate is about to expire but doesn't renew automatically](../../server/security/common-errors-and-faq#the-lets-encrypt-certificate-is-about-to-expire-but-doesnt-renew-automatically)
* [What should I do when my license expires?](../../server/security/common-errors-and-faq#what-should-i-do-when-my-license-expires)
* [Let's Encrypt certificate permission errors after renewal](../../server/security/common-errors-and-faq#lets-encrypt-certificate-permission-errors-after-renewal)

---

### After installing with Let's Encrypt, can I change the DNS records?

Yes.  

1. The [Customers Portal](https://customers.ravendb.net) allows you to easily edit DNS records that are associated with your license.
2. You can run the setup wizard again.

If you supply different IP addresses then the wizard will update the DNS records of your domain.  
If you use a new domain or if you add/remove nodes in the new configuration then the wizard will also fetch a new certificate.

---

### Can I change the (private) IP address RavenDB binds to?

Yes.  
Open the [settings.json](../configuration/configuration-options#json) file located in the RavenDB Server installation folder, 
change the `ServerUrl` setting and restart the server.

---

### The Let's Encrypt certificate is about to expire but doesn't renew automatically

If you are getting the following error you must update the RavenDB server. 

{CODE-BLOCK:plain}
Failed to update certificate from Lets Encrypt, EXCEPTION: System.InvalidOperationException: 
Your license is associated with the following domains: ravendb.community but the PublicServerUrl 
configuration setting is: Raven.Server.Config.Settings.UriSetting.There is a mismatch, therefore 
cannot automatically renew the Lets Encrypt certificate. Please contact support.
{CODE-BLOCK/} 

If it's not the same error as above, please open [settings.json](../configuration/configuration-options#json) in your Server installation 
and make sure you have all of the fields defined properly. Take a look at the following example:

{CODE-BLOCK:JSON}
{
  "DataDir": "RavenData",
  "License.Eula.Accepted": true,
  "Security.Certificate.LetsEncrypt.Email": "your-email@example.com",
  "Setup.Mode": "LetsEncrypt",
  "Security.Certificate.Path": "cluster.server.certificate.aws.pfx",
  "ServerUrl": "https://172.31.30.163",
  "ServerUrl.Tcp": "tcp://172.31.30.163:38888",
  "ExternalIp": "35.130.249.162",
  "PublicServerUrl": "https://a.aws.development.run",
  "PublicServerUrl.Tcp": "tcp://a.aws.development.run:38888"
}
{CODE-BLOCK/}

Things to check:

* **Setup.Mode" must be "LetsEncrypt**  
  The automatic renewal process only works if you acquired your certificate through the RavenDB setup wizard and used LetsEncrypt.  
  If you did not set up your cluster with the setup wizard and with LetsEncrypt, you are responsible to renew your certificate periodically.  
  * To enable RavenDB's automatic certificate renewal, set up a new cluster with the setup wizard, create parallel databases, 
    reconfigure the [document store](../../client-api/creating-document-store) to connect to the new databases, 
    and [import the data](../../studio/database/tasks/import-data/import-from-ravendb).  
* **Security.Certificate.LetsEncrypt.Email**  
  must be identical to the e-mail which is associated with your license.  
* **PublicServerUrl and PublicServerUrl.Tcp**  
  must contain the same domain as the one chosen during the setup wizard and is associated with your license.  
* **ExternalIp**  
  should be defined only if you are running behind a firewall (cloud VM, docker, etc...).  

* If all of this looks right, and the certificate still doesn't renew automatically and there are no alerts telling you what's wrong, 
  you can contact support.  
  * Make sure to supply the server logs with your ticket. When running in a cluster, please provide the logs from all nodes.
  * If your logs are turned off, open `Manage Server`->`Admin Logs` in the Studio, and keep them open while you click the `Renew` button in the certificate view.

  ---

### What should I do when my license expires?

By default, the RavenDB license update server automatically renews licenses.
If your server is running offline or a firewall is blocking the connection with the license server, 
you will need to renew your license key manually.  

If your license is due to expire, be sure to **renew the license**.  
Do not set up a new license because this will cause domain name issues.  

If you get automatic Let's Encrypt certificate renewals, contact RavenDB support to renew the license because changing the 
license ID will cause a mismatch that will cancel the auto-certificate renewal.  

Copy your new license code key from the email that was given when registering your license.  
Paste it either: 

* Directly into the settings.json
  * e.g. `"License": paste your license code including curly brackets here` 
* Into the .json file `license.json`, to which the file path is provided in the settings.json
  * e.g. `"License.Path": "D:\\RavenDB\\Server\\license.json"`  

To renew your license via studio, navigate to the [Studio "About" tab](../../studio/server/license-management):  

![Renew License in Studio](images/renew-license-studio.png "Renew License in Studio")

![Paste License Renewal Key](images/paste-license-renewal-key.png "Paste License Renewal Key")  

  1. **Renew**.  
     Click to send an email with the renewal key to the email account that is associated with the license.  
  2. **Submit License**  
     Paste the renewal key from the email into the interface.
  3. **Submit button**  
     Click to complete the procedure.

---

### Let's Encrypt certificate permission errors after renewal

If you have External Replication or ETL to another cluster, or if you use 
your own Let's Encrypt certificates as client certificates, the next certificate 
renewal may cause permission issues that need to be handled manually.  

Learn how to handle this issue [here](../../server/security/authentication/solve-cluster-certificate-renewal-issue).  


{PANEL/}

{PANEL: Authentication Issues}

* [Authentication Error Occurred in Chrome or Edge](../../server/security/common-errors-and-faq#authentication-error-occurred-in-chrome-or-edge)
* [RavenDB is running as a service in Windows and Chrome doesn't use the client certificate from the OS store](../../server/security/common-errors-and-faq#ravendb-is-running-as-a-service-in-windows-and-chrome-doesnt-use-the-client-certificate-from-the-os-store)
* [Authentication Error Occurred in Firefox](../../server/security/common-errors-and-faq#authentication-error-occurred-in-firefox)
* [Cannot Import the Client Certificate to Firefox](../../server/security/common-errors-and-faq#cannot-import-the-client-certificate-to-firefox)
* [Getting the full error using PowerShell](../../server/security/common-errors-and-faq#getting-the-full-error-using-powershell)
* [Not using TLS](../../server/security/common-errors-and-faq#not-using-tls)
* [How to regain access to a server when you have physical access but no client certificate](../../server/security/common-errors-and-faq#how-to-regain-access-to-a-server-when-you-have-physical-access-but-no-client-certificate)
* [Certificate is not recognized when setting up on Azure App Services](../../server/security/common-errors-and-faq#certificate-is-not-recognized-when-setting-up-on-azure-app-services)
* [Automatic cluster certificate renewal following migration to 4.2](../../server/security/common-errors-and-faq#automatic-cluster-certificate-renewal-following-migration-to-4.2)

---

### Authentication Error Occurred in Chrome or Edge

You cannot access the Studio using Chrome or Edge even though you have finished the setup wizard successfully and 
you also checked the box saying "Automatically register the admin client certificate in this (local) OS".

![Figure 1. Authentication Error](images/1.png)

{CODE-BLOCK:plain}
There were problems authenticating the request:
This server requires client certificate for authentication, but none was provided by the client.
{CODE-BLOCK/}

1. The first solution is to close **all instances** of the browser and restart it.
2. If that didn't work, register the client certificate in the OS store.  
   In Windows, double click the .pfx certificate and click next all the way for default settings or set a different path and/or a password.  
   In Linux, import it directly to the browser.  
3. When the browser asks which certificate to use, if there are a few options and the wrong one was previously chosen for this server, you can either:
 * Remove the certificate from the browser's memory (In Chrome, Settings -> Privacy and security -> Manage certificates), 
   then reinstall the .pfx certificate with the process described above.
 * Open an incognito tab (ctrl + shift + N in Chrome) and paste the server URL into the address bar, then select the correct certificate name.  
4. In case none of the above works, you have an option to bring your own certificate and have RavenDB trust it. 
   It can be any client certificate that works in your OS and browser, even if it wasn't generated by RavenDB. 
   See [trusting an existing certificate](../../server/administration/cli#trustclientcert).  

**If you are running in Windows 7 or Windows Server 2008 or older**:  
The first thing to try is to install the **SERVER** certificate to the OS where your server is running, close **all instances** of the browser and restart it.

If the issue persists, please visit this page which explains the problem:
[Trusted Issuers List](https://support.microsoft.com/en-us/topic/failed-tls-connection-between-unified-communications-peers-generates-an-schannel-warning-9079a7df-1756-bf4d-20c7-42981a50f8df)

Follow the instructions for method 3: "Configure Schannel to no longer send the list of trusted root certificate authorities during the TLS/SSL handshake process". Set the following registry entry to false:

{CODE-BLOCK:plain}
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL
Value name: SendTrustedIssuerList  
Value type: REG_DWORD  
Value data: 0 (False)  
{CODE-BLOCK/}

---

### RavenDB is running as a service in Windows and Chrome doesn't use the client certificate from the OS store

That is because the certificate was installed to the OS by the service user (during setup) but Chrome can't access it because 
it's running as another user (or as Administrator).  
The solution is to import the certificate manually to Chrome (In Chrome, Settings -> Privacy and security -> Security -> Manage certificates) 
or to install the .pfx certificate again to the OS store, this time with the same user as Chrome.

---

### Authentication Error Occurred in Firefox

You cannot access the Studio using Firefox even though you have finished the setup wizard successfully and you also checked the box saying "Automatically register the admin client certificate in this (local) OS".

![Figure 2. Authentication Error](images/1.png)

{CODE-BLOCK:plain}
There were problems authenticating the request:
This server requires client certificate for authentication, but none was provided by the client.
{CODE-BLOCK/}

Firefox doesn't use the OS certificate store like Chrome or Edge. Please import the certificate manually (In Firefox, "Settings" -> "Privacy and Security" -> scroll down to Security and click "View Certificates" -> "Import").  
Then close **all instances** of the browser and restart it.

![Figure 3. Firefox Import](images/2.png)

---

### Cannot Import the Client Certificate to Firefox

You're trying to import the client certificate received from RavenDB to Firefox but get the following error:

![Figure 3. Unknown Reasons](images/3.png)

{CODE-BLOCK:plain}
The PKCS#12 operation failed for unknown reasons.
{CODE-BLOCK/}

Firefox fails to import a certificate which is not password protected.
To overcome this issue, use the RavenDB CLI to [generate a password protected certificate](../../server/administration/cli#generateclientcert). 
You can also add a password to the current certificate by using OpenSSL or by importing it to the OS store and exporting it back with a password.

Firefox **sometimes** fails to import a perfectly good certificate for no apparent reason and without a proper error message.

You can try to generate a new password-protected certificate using the RavenDB CLI and import that instead.

If it didn't help, you can use any other client certificate you have which works with Firefox (even if it wasn't generated by RavenDB) and have RavenDB trust it. See [trusting an existing certificate](../../server/administration/cli#trustclientcert).

You can also generate your own self-signed client certificate by using OpenSSL or Powershell. 

This is a known issue which has been reported many times to Mozilla.  

Some references:

[Bugzilla: #1049435](https://bugzilla.mozilla.org/show_bug.cgi?id=1049435)  
[Bugzilla: #458161](https://bugzilla.mozilla.org/show_bug.cgi?id=458161)  
[mozilla.dev.tech.crypto issue](https://groups.google.com/forum/?fromgroups=#!topic/mozilla.dev.tech.crypto/RiIeY-R5Q4Y)  

---

### Getting the full error using PowerShell

You can use PowerShell to make requests using the REST API.

If you are having trouble using certificates, take a look at this example which prints the full error (replace the server URL and the `/certificates/whoami` endpoint with yours).

{CODE-BLOCK:powershell}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$cert = Get-PfxCertificate -FilePath C:\secrets\admin.client.certificate.example.pfx

try {
    $response = Invoke-WebRequest https://a.example.development.run:8080/certificates/whoami -Certificate $cert 
}
catch {
    if ($_.Exception.Response -ne $null) {
        Write-Host $_.Exception.Message

        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        Write-Host $reader.ReadToEnd()
    }
    Write-Error $_.Exception
}
{CODE-BLOCK/}

---

### Not using TLS

The RavenDB clients use TLS 1.2 by default. If you want to use other clients please make sure to use the TLS security protocol version 1.2 or 1.3.

{CODE-TABS}
{CODE-TAB-BLOCK:json:Bad Request (400) sample response}
{  
   "Url":"/admin/secrets/generate",
   "Type":"Raven.Client.Exceptions.Security.InsufficientTransportLayerProtectionException",
   "Message":"RavenDB requires clients to connect using TLS 1.2, but the client used: 'Tls'.",
   "Error":"Raven.Client.Exceptions.Security.InsufficientTransportLayerProtectionException: RavenDB requires clients to connect using TLS 1.2, but the client used: 'Tls'.
       at Raven.Server.RavenServer.AuthenticateConnection.ThrowException() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\RavenServer.cs:line 570
       at Raven.Server.Routing.RequestRouter.TryAuthorize(RouteInformation route, HttpContext context, DocumentDatabase database) in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Routing\\RequestRouter.cs:line 168
       at Raven.Server.Routing.RequestRouter.<HandlePath>d__6.MoveNext() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Routing\\RequestRouter.cs:line 89
       --- End of stack trace from previous location where exception was thrown ---
       at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
       at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
       at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
       at System.Runtime.CompilerServices.ValueTaskAwaiter`1.GetResult()
       at Raven.Server.RavenServerStartup.<RequestHandler>d__11.MoveNext() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\RavenServerStartup.cs:line 159"
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

In powershell for example it can be solved like this:

{CODE-BLOCK:plain}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
{CODE-BLOCK/}

---

### How to regain access to a server when you have physical access but no client certificate

An admin client certificate can be generated through the [RavenDB CLI](../../server/administration/cli#generateclientcert If RavenDB runs as a console application, the CLI is just there. When running as a service, please use the `rvn admin-channel`.  
Use either the [generateClientCert](../../server/administration/cli#generateclientcert) command, or (if you already have a certificate) the [trustClientCert](../../server/administration/cli#trustclientcert) command.

Another way to gain access for an existing certificate is to add the [Security.WellKnownCertificates.Admin](../../server/configuration/security-configuration#security.wellknowncertificates.admin) configuration to `settings.json` with your existing certificate's thumbprint.
In this case, a server restart is required.

---

### Certificate is not recognized when setting up on Azure App Services

You may have gotten an error message like:

```
The credentials supplied to the package were not recognized (The SSL connection could not be established, see inner exception.)
```

1) In the app settings of your Azure App Services application, add the `WEBSITE_LOAD_USER_PROFILE = 1` option.  

2) Another solution is to use the `MachineKeySet` flag during certificate creation:  

{CODE-BLOCK:csharp}
[DocumentStore].Certificate = new X509Certificate2("[path to your pfx file]", 
                                                  (string)null, X509KeyStorageFlags.MachineKeySet);  
{CODE-BLOCK/}

---

### Automatic cluster certificate renewal following migration to 4.2

`Security.Certificate.Exec` was deprecated in 4.2 and replaced by `Security.Certificate.Load.Exec`. You can use your old `Security.Certificate.Exec` 
executable by simply moving it to this new path. The settings `Security.Certificate.Renew.Exec` and `Security.Certificate.Change.Exec` have been added 
for automatically persisting the certificate to the whole cluster. If you have your own mechanism for doing this, or are using a single node cluster, 
you still need to place empty scripts in the `Security.Certificate.Renew.Exec` and `Security.Certificate.Change.Exec` paths or an exception will be 
thrown.  

{PANEL/}

{PANEL: Encryption Issues}

### Insufficient Memory Exception

{CODE-BLOCK:plain}
Memory exception occurred: System.InsufficientMemoryException:  
Failed to increase the min working set size so we can lock 4,294,967,296 for  
D:\stackoverflow\RavenData\Databases\SO\Indexes\Auto_Questions_ByBody\Temp\compression.0000000000.buffers.  
With encrypted databases we lock some memory in order to avoid leaking secrets to disk.  
Treating this as a catastrophic error and aborting the current operation.
{CODE-BLOCK/}

When encryption is turned on, RavenDB locks memory in order to avoid leaking secrets to disk. Read more [here](../../server/security/encryption/encryption-at-rest#locking-memory).

By default, RavenDB treats this error as catastrophic and will not continue the operation.
You can change this behavior but it's not recommended and should be done only after a proper security analysis is performed, see the [Security Configuration Section](../../server/configuration/security-configuration#security.donotconsidermemorylockfailureascatastrophicerror).

If such a catastrophic error occurs in **Windows**, RavenDB will try to recover automatically by increasing the size of the minimum working set and retrying the operation.   
In **Linux**, it is the admin's responsibility to configure higher limits manually using:
{CODE-BLOCK:plain}
sudo prlimit --pid [process-id] --memlock=[new-limit-in-bytes]
{CODE-BLOCK/}

To figure out what the new limit should be, look at the exception thrown by RavenDB, which includes this size.

{PANEL/}

## Related Articles

### Installation

- [Setup Wizard](../../start/installation/setup-wizard)
- [Manual Setup](../../start/installation/manual)
- [Running in a Docker Container](../../start/installation/running-in-docker-container)

### Setup Examples

- [AWS Windows VM](../../start/installation/setup-examples/aws-windows-vm)
- [AWS Linux VM](../../start/installation/setup-examples/aws-linux-vm)
- [Docker on AWS Linux VM](../../start/installation/setup-examples/aws-docker-linux-vm)

### Security

- [Overview](../../server/security/overview)
