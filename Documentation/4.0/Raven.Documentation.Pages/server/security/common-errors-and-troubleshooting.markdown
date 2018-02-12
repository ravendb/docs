# Security : Common Errors & Troubleshooting

In this section, we review some of the common security configuration errors and troublshooting.

[Setup Wizard](#setup-wizard)  
[Authentication](#authentication)  
[Authorization](#authorization)  
[Encryption](#encryption)  

## Setup Wizard    

#### Ports are blocked by firewall

When configuring a VM in Azure, AWS or any other provider, you should define firewall rules to allow both the **Http** and **Tcp** ports you have chosen during setup.
This should be done both inside the VM operating system **and** in the web dashboard or management console.

If ports are blocked you'll get the following error.
{CODE-BLOCK:plain}
Setting up RavenDB in Let's Encrypt security mode failed.System.InvalidOperationException: Setting up RavenDB in Let's Encrypt security mode failed. ---> System.InvalidOperationException: Validation failed. ---> System.InvalidOperationException: Failed to simulate running the server with the supplied settings using: https://a.example.development.run:443 ---> System.InvalidOperationException: Client failed to contact webhost listening to 'https://a.example.development.run:443'.Are you blocked by a firewall? Make sure the port is open.Settings file: D:\RavenDB-4.0.0-windows-x64\Server\settings.json.IP addresses: 10.0.1.4:443.
{CODE-BLOCK/}

#### DNS is cached locally

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

#### Long DNS propogation time

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

## Authentication  

#### Getting the full error using powershell

You can use powershell to make requests using the REST API.

If you are having trouble using certificates, take a look at this example which prints the full error (replace the `/certificates/whoami` endpoint with yours).

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


#### Not using Tls1.2

The RavenDB clients use Tls1.2 by default. If you want to use other clients please make sure to use the Tls1.2 security protocol.

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

## Authorization    

## Encryption  
