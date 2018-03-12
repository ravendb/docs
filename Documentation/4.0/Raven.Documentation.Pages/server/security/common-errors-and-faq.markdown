# Security : Common Errors & FAQ

In this section, we review some of the common security configuration errors and explain how to handle them.

[Setup Wizard Issues](../../server/security/common-errors-and-faq#setup-wizard-issues)  
[Authentication Issues](../../server/security/common-errors-and-faq#authentication-issues)  
[Authorization Issues](../../server/security/common-errors-and-faq#authorization-issues)  
[Encryption Issues](../../server/security/common-errors-and-faq#encryption-issues)  

## Setup Wizard Issues  

### Server cannot bind to the provided private IP address

If the IP/port is not accessible on your machine, you'll get the following error.

{CODE-BLOCK:plain}
System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> 
System.InvalidOperationException: Validation failed. ---> 
System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.example.ravendb.community:4433  ---> 
System.InvalidOperationException: Failed to start webhost on node 'A'. The specified ip address might not be reachable due to network issues. 
It can  happen if the ip is external (behind a firewall, docker). If this is the case, try going back to the previous screen and add the same ip as an external ip.
Settings file:D:\temp\RavenDB-4.0.0-windows-x64\Server\settings.json.
IP addresses: 10.0.0.65:4433. 
---> Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking.UvException: Error -4092 EACCES permission denied
{CODE-BLOCK/}

This can be caused by two different reasons:

1. Your private IP address is not reachable inside the machine or you provided the wrong IP/port.
2. You are running behind a firewall (VM, docker...) and accidentally provided the external IP address during setup.

Make sure you provide the private IP address in the "IP Address / Hostname" field as seen in [this example](../../start/installation/setup-wizard#example-iii).


### Ports are blocked by the firewall

When configuring a VM in Azure, AWS or any other provider, you should define firewall rules to allow both the **HTTP** and **TCP** ports you have chosen during setup.
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

This error probably means that the DNS is cached. You can wait a while or reset the network DNS cache but in many cases the easiest solution is to [temporarily switch your DNS server to 8.8.8.8](https://developers.google.com/speed/public-dns/docs/using) 
You can click the Try Again button to restart the validation process of the Setup Wizard.

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

If this happens, there is nothing you can do except wait for DNS propogation. When it's updated in dns.google.com click the `Try Again` button.

### After installing with Let's Encrypt, can I change the DNS records?

Yes.  
You can run the setup wizard again.
If you supply different IP addresses then the wizard will update the DNS records of your domain.  
If you use a new domain or if you add/remove nodes in the new configuration then the wizard will also fetch a new certificate.

### If I already have the Zip file, can I avoid repeating the setup process?

Yes.  
You can use the Zip file to re-install or deploy the server/cluster elsewhere.  
Download a fresh copy of RavenDB and run the setup wizard. Then choose `Continue Cluster Setup` and select node A.
This will use the existing Zip file and same configuration and certificate which were previously chosen.  
When building a cluster, repeat this step with nodes B,C and so on.

### Can I change the (private) IP address RavenDB binds to?

Yes.  
Open the settings.json file located in the RavenDB folder, change the `ServerUrl` setting and restart the server.

## Authentication Issues  

### Authentication Error Occurred in Chrome or Edge

You cannot access the Studio using Chrome or Edge even though you have finished the setup wizard successfully and you also checked the box saying "Automatically register the admin client certificate in this (local) OS".

![Figure 1. Authentication Error](images/1.png)

{CODE-BLOCK:plain}
There were problems authenticating the request:
This server requires client certificate for authentication, but none was provided by the client.
{CODE-BLOCK/}

The solution is very simple. Close **all instances** of the browser and restart it.

### Authentication Error Occurred in Firefox

You cannot access the Studio using Firefox even though you have finished the setup wizard successfully and you also checked the box saying "Automatically register the admin client certificate in this (local) OS".

![Figure 2. Authentication Error](images/1.png)

{CODE-BLOCK:plain}
There were problems authenticating the request:
This server requires client certificate for authentication, but none was provided by the client.
{CODE-BLOCK/}

Firefox doesn't use the OS certificate store like Chrome or Edge. Please import the certificate manually. Then close **all instances** of the browser and restart it.

![Figure 3. Firefox Import](images/2.png)

### Cannot Import the Client Certificate to Firefox

You're trying to import the client certificate received from RavenDB to Firefox but get the following error:

![Figure 3. Unknown Reasons](images/3.png)

{CODE-BLOCK:plain}
The PKCS#12 operation failed for unknown reasons.
{CODE-BLOCK/}

Firefox fails to import a certificate which is not password protected.
To overcome this issue, use the RavenDB CLI to [generate a password protected certificate](../../server/administration/cli#generateclientcert). You can also add a password to the current certificate by using OpenSSL or by importing it to the OS store and exporting it back with a password.

Firefox **sometimes** fails to import a perfectly good certificate for no apparent reason and without a proper error message.

You can try to generate a new password protected certificate using the RavenDB CLI and import that instead.

If it didn't help, you can use any other client certificate you have which works with Firefox (even if it wasn't generated by RavenDB) and have RavenDB trust it. See [trusting an existing certificate](../../server/administration/cli#trustclientcert).

You can also generate your own self-signed client certificate by using OpenSSL or Powershell. 

This is a known issue which has been reported many times to Mozilla.  

Some references:

[Bugzilla: #1049435](https://bugzilla.mozilla.org/show_bug.cgi?id=1049435)  
[Bugzilla: #458161](https://bugzilla.mozilla.org/show_bug.cgi?id=458161)  
[mozilla.dev.tech.crypto issue](https://groups.google.com/forum/?fromgroups=#!topic/mozilla.dev.tech.crypto/RiIeY-R5Q4Y)  


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

### Not using TLS 1.2

The RavenDB clients use TLS 1.2 by default. If you want to use other clients please make sure to use the Tls1.2 security protocol.

Example error:

{CODE-BLOCK:plain}
The remote server returned an error: (400) Bad Request.
{"Url":"/admin/secrets/generate","Type":"Raven.Client.Exceptions.Security.InsufficientTransportLayerProtectionException","Message":"RavenDB requires clients to connect us
ing TLS 1.2, but the client used: 'Tls'.","Error":"Raven.Client.Exceptions.Security.InsufficientTransportLayerProtectionException: RavenDB requires clients to connect usi
ng TLS 1.2, but the client used: 'Tls'.\r\n   at Raven.Server.RavenServer.AuthenticateConnection.ThrowException() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\Ra
venServer.cs:line 570\r\n   at Raven.Server.Routing.RequestRouter.TryAuthorize(RouteInformation route, HttpContext context, DocumentDatabase database) in C:\\Builds\\Rave
nDB-Stable-4.0\\src\\Raven.Server\\Routing\\RequestRouter.cs:line 168\r\n   at Raven.Server.Routing.RequestRouter.<HandlePath>d__6.MoveNext() in C:\\Builds\\RavenDB-Stabl
e-4.0\\src\\Raven.Server\\Routing\\RequestRouter.cs:line 89\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.Except
ionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Runtime.
CompilerServices.TaskAwaiter`1.GetResult()\r\n   at System.Runtime.CompilerServices.ValueTaskAwaiter`1.GetResult()\r\n   at Raven.Server.RavenServerStartup.<RequestHandle
r>d__11.MoveNext() in C:\\Builds\\RavenDB-Stable-4.0\\src\\Raven.Server\\RavenServerStartup.cs:line 159"}
{CODE-BLOCK/}

In powershell for example it can be solved like this:

{CODE-BLOCK:plain}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
{CODE-BLOCK/}

## Authorization Issues  

Under construction

## Encryption Issues  

Under construction
