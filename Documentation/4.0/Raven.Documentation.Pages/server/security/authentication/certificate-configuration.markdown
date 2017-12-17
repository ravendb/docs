# Certificate Configuration

In RavenDB, configuration values can be set using environment variables, command line aruments or using the settings.json file. For more details, please read the [Configuration Section.](../../configuration/configuration-options)  

{NOTE This section explains how to setup authentication <strong>manually</strong>. Please also take a look at the automated [Setup Wizard](../../../start/setup-wizard) which lets you setup authentication in a much easier and faster way. /}

To enable authentication, either `Security.Certificate.Path` or `Security.Certificate.Exec` must be set in settings.json.

The first way to enable authentication is to set `Security.Certificate.Path` with the path to your .pfx server certificate. You may supply the certificate password using `Security.Certificate.Password`. 
When providing a certificate for authentication, you <strong>must</strong> also set the `ServerUrl` configuration option to an HTTPS address.

For example, this is a typical settings.json:

    {  
        "ServerUrl": "https://rvn-srv-1:8080",
        "Setup.Mode": "None",
        "DataDir": "RavenData",
        "Security.Certificate": {
            "Path": "/mnt/secrets/server.pfx",
            "Password": "s3cr7t p@$$w0rd"
        }
    }  


The second way to enable authentication is to set `Security.Certificate.Exec`. 

This option is useful when you want to protect your certificate (private key) with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection. RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use.

RavenDB excpects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.

Let's look at an example - a simple powershell script called give_me_cert.ps1

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

And settings.json will look something like this:

    {  
        "ServerUrl": "https://rvn-srv-1:8080",
        "Setup.Mode": "None",
        "DataDir": "RavenData",
        "Security.Certificate": {
		    "Exec": "powershell",
		    "Arguments": "d:/give_me_cert.ps1 90F4BC16CA5E5CB535A6CD8DD78CBD3E88FC6FEA"
        }
    } 
    

{NOTE In any secure configuration, the `ServerUrl` must contain the same domain name that is used in the certificate (under the CN or ASN properties). /}
