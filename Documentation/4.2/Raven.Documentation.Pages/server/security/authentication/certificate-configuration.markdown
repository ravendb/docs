# Authentication: Manual Certificate Configuration

{NOTE: } This article explains how to set up authentication **manually**. Please also take a look at the automated 
[Setup Wizard](../../../start/installation/setup-wizard) which lets you set up authentication in a much easier and faster way.
The Setup Wizard can process certificates that you provide or can give you a free, highly secure certificate via Let's Encrypt.
We've developed default **automatic renewals** of certificates when setting up with the Setup Wizard together with Let's Encrypt.  

If you choose manual setup and/or to provide your own certificate, **you are responsible for their periodic renewal**. {NOTE/} 


- In RavenDB, configuration values can be set using environment variables, command line arguments or using the [settings.json](../../configuration/configuration-options#json) 
 file. For more details, please read the [Configuration Options.](../../configuration/configuration-options)  

- To enable authentication, either `Security.Certificate.Path` or `Security.Certificate.Load.Exec` must be set in [settings.json](../../configuration/configuration-options#json). 
 Please note that `Security.Certificate.Load.Exec` has replaced the old `Security.Certificate.Exec` as of 4.2 - [see FAQ](../../../server/security/common-errors-and-faq#automatic-cluster-certificate-renewal-following-migration-to-4.2).

{INFO: Important - Setting up client certificates}
When the server is running with a certificate for the first time, there are no client certificates registered in the server yet. The first action an administrator will do is to [generate/register a new client certificate](../../../server/security/authentication/client-certificate-usage).  

You can set up various client certificates with different security clearance levels and database permissions.  See [Certificate Management](../../../server/security/authentication/certificate-management) for more about permissions.  
{INFO/}

In this page:

* [Standard Manual Setup With Certificate Stored Locally](../../../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally)  
* [With Logic Foreign to RavenDB or External Certificate Storage](../../../server/security/authentication/certificate-configuration#with-logic-foreign-to-ravendb-or-external-certificate-storage)  
* [Step-by-step Guide to Installing Certificate](../../../server/security/authentication/certificate-configuration#step-by-step-guide-to-installing-certificate)  

{PANEL: }

### Standard Manual Setup With Certificate Stored Locally

RavenDB will accept `.pfx` server certificates which contain the private key, are not expired, and have the following fields:

- KeyUsage: DigitalSignature, KeyEncipherment
- ExtendedKeyUsage: Client Authentication, Server Authentication

The standard way to enable authentication is to set `Security.Certificate.Path` with the path to your `.pfx` server certificate. You may supply the certificate password using `Security.Certificate.Password`. 

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

{PANEL/}

{PANEL: }

### With Logic Foreign to RavenDB or External Certificate Storage

The second way to enable authentication is to set `Security.Certificate.Load.Exec`. 

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, so you can write your own scripts / mini-programs and apply whatever logic you need.  

> It creates a clean separation between RavenDB and the secret store in use.

RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.

Let's look at an example -

To use `Security.Certificate.Load.Exec` with a PowerShell script, the [settings.json](../../configuration/configuration-options#json) will look something like this:

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "RavenData",
    "Security.Certificate.Load.Exec": "powershell",
    "Security.Certificate.Load.Exec.Arguments": "C:\\secrets\\give_me_cert.ps1 90F4BC16CA5E5CB535A6CD8DD78CBD3E88FC6FEA"
}
{CODE-BLOCK/}

A simple powershell script called `give_me_cert.ps1` that matches the `settings.json` configuration:

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



{NOTE In all secure configurations, the `ServerUrl` must contain the same domain name that is used in the certificate (under the CN or ASN properties). /}

{PANEL/}

{PANEL: }

### Step-by-Step Guide to Installing Certificate

1. Set up file infrastructure and download server.
   - Create a [user account](https://ravendb.net/buy). You should get an email with your license key.  
   - [Download](https://ravendb.net/download) the RavenDB...zip server package.  
   - Extract the .zip into the folders where the server nodes will live.  
   - Store server certificate in your desired location with secure permissions.  

2. Configure the `settings.json` file in each node `Server` folder.  
   - Set the `ServerUrl`.  Make sure to use HTTPS.  
   - Set `Setup.Mode` to `None`.  
   - Set `DataDir` to the desired database storage folder on each machine.  
   - Set the `Security.Certificate.Path` if certificate is stored with [RavenDB logic on local machines](../../../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally) or `Security.Certificate.Load.Exec` if using [external location or logic](../../../server/security/authentication/certificate-configuration#with-logic-foreign-to-ravendb-or-external-certificate-storage).  
   - Make sure that the certificate .path or .load script lead to the correct certificate location.  

3. Click the `run.ps1` in the extracted server package.  It runs in PowerShell as a default, but you can open PowerShell as Admin, browse to the server directory and give the command from there as well.  
   - It will start up and launch a browser window that should give an error message about a missing client certificate.  

4. The PowerShell window will be running the server terminal. In the CLI [generateClientCert](../../../server/administration/cli#generateclientcert) command to generate a client certificate.  
   - In the following example the certificate will be named RavenDBClient, will be stored at C:\Users\administrator\Documents, and will have no password. If a password is required add it to the end of the command.  
    {CODE-BLOCK:plain}
    ravendb> generateClientCert RavenDBClient C:\Users\administrator\Documents
    {CODE-BLOCK/}
   - The following is a generic command.  
    {CODE-BLOCK:plain}
    ravendb> generateClientCert <name> <path-to-output-folder> [password]
    {CODE-BLOCK/}

5. Extract the contents of the .zip file generated into the folders where your nodes live.  
   - Install the certificate into the OS by double-clicking the `admin.client.certificate...pfx` file and complete the OS Certificate Import Wizard.  
    * To install it without a password, you can use the default settings by pressing Enter or Next all the way through.  In most cases, this is sufficient.  
    * To set a password on the certificate, do so in the Import Wizard.  You'll need to use that password every time you work with the certificate.  

      ![Set client certificate password](images/set-client-certificate-password.png "Set client certificate password")

6. Reopen the browser and paste the `ServerUrl` that you set in the `settings.json`. Select the certificate in the popup and hit ok. The [RavenDB Studio](../../../studio/overview) should now open.  
   - In the PowerShell window type quit to close down the server.  

7. Double-click `setup-as-service.ps1` in the extracted server package to expedite full cluster health in case a node goes down.  
   - It will set up the service, which [will launch the server automatically](../../../client-api/start/installation/running-as-service) every time the machine starts, but will fail to start if the Local Service account doesn't have access to all the required resources.  
   - Open the Services Manager for Windows, open the properties for the RavenDB service. Set the "Log On As" setting to the user created for the service account.  

8. Now the service should run and the Studio should be accessible by the user with the client certificate.  
   - See [Certificate Management](../../../server/security/authentication/certificate-management) for an easy way to generate various client certificates with customizable permissions.  

{PANEL/}


## Related articles

### Security

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)
- [Get Client Certificate via CLI, Wget, cURL](../../../server/security/authentication/client-certificate-usage)

### Installation

- [Manual Setup](../../../start/installation/manual)
- [Setup Wizard](../../../start/installation/setup-wizard)

### Configuration

- [Security Configuration](../../../server/configuration/security-configuration)
