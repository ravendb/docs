# Authentication: Manual Certificate Configuration

In RavenDB, configuration values can be set using environment variables, command line arguments or using the [settings.json](../../configuration/configuration-options#json) file. For more details, please read the [Configuration Section.](../../configuration/configuration-options)  

{NOTE This section explains how to setup authentication **manually**. Please also take a look at the automated [Setup Wizard](../../../start/installation/setup-wizard) which lets you setup authentication in a much easier and faster way. /}

To enable authentication, either `Security.Certificate.Path` or `Security.Certificate.Exec` must be set in [settings.json](../../configuration/configuration-options#json).

RavenDB will accept PFX server certificates which contain the private key, are not expired and have the following fields:

- KeyUsage: DigitalSignature, KeyEncipherment
- ExtendedKeyUsage: Client Authentication, Server Authentication

The first way to enable authentication is to set `Security.Certificate.Path` with the path to your `.pfx` server certificate. You may supply the certificate password using `Security.Certificate.Password`. 

When providing a certificate for authentication, you **must** also set the `ServerUrl` configuration option to an HTTPS address.

For example, this is a typical [settings.json](../../configuration/configuration-options#json):

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "/home/RavenData",
    "Security.Certificate": {
        "Path": "/home/secrets/server.pfx",
        "Password": "s3cr7t p@$$w0rd"
    }
} 
{CODE-BLOCK/}

The second way to enable authentication is to set `Security.Certificate.Exec`. 

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use.

RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.

Let's look at an example - a simple powershell script called `give_me_cert.ps1`

{CODE-BLOCK:powershell}
try
{
    $thumbprint = $args[0]
    $cert = gci "cert:\CurrentUser\my\$thumbprint"
    $exportedCertBinary = $cert.Export("Pfx")
    $stdout = [System.Console]::OpenStandardOutput()
    $stdout.Write($exportedCertBinary, 0, $exportedCertBinary.Length)
}
catch
{
    write-error $_.Exception
    exit 3
}
{CODE-BLOCK/}

And [settings.json](../../configuration/configuration-options#json) will look something like this:

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "RavenData",
    "Security.Certificate.Exec": "powershell",
    "Security.Certificate.Exec.Arguments": "C:\\secrets\\give_me_cert.ps1 90F4BC16CA5E5CB535A6CD8DD78CBD3E88FC6FEA"
}
{CODE-BLOCK/}

{NOTE In all secure configurations, the `ServerUrl` must contain the same domain name that is used in the certificate (under the CN or ASN properties). /}

{INFO: Important}
When the server is running with a certificate for the first time, there are no client certificates registered in the server yet. The first action an administrator will do is to generate/register a new client certificate.
You can do this by using the [RavenDB CLI](../../../server/administration/cli#generateclientcert) (generateClientCert) or by using a client, see the [Client Certificate Usage](../../../server/security/authentication/client-certificate-usage) section for a detailed example.
{INFO/}

## Related articles

### Security

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)

### Installation

- [Manual Setup](../../../start/installation/manual)

### Configuration

- [Security Configuration](../../../server/configuration/security-configuration)
