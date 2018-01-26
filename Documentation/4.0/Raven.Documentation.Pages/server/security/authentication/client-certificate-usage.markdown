# Security : Authentication : Client Certificate Usage

In previous sections we described how to obtain a server certificate and how to configue RavenDB to use it. In this section we will cover how to use client certificates to connect to a RavenDB server.

## Obtaining your first client certificate

When RavenDB is running with a server certificate for the first time, there are no client certificates registered in the server yet. The first action an administrator will do is to generate/register an admin client certificate.

{NOTE This operation is only required when doing a **manual** secured setup. If you are using the automated [Setup Wizard](../../../start/installation/setup-wizard), an admin client certificate will be generated for you as part of the wizard. /}

### Example I - Using the RavenDB CLI

If you have access to the server, the simplest way is to use the RavenDB CLI:

{CODE-BLOCK:plain}
ravendb> generateClientCert <name> <path-to-output-folder> [password]
{CODE-BLOCK/}

Or if you wish to use your own client certificate:

{CODE-BLOCK:plain}
ravendb> trustClientCert <name> <path-to-pfx> [password]
{CODE-BLOCK/}

### Example II - Using Powershel and Wget in Windows 

You can use a client to make an HTTP request to the server. At this point you only have a **server certificate** and you will use it (acting as the client certificate).

Assume we started the server with the following settings.json:

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "c:/RavenData",
    "Security.Certificate": {
        "Path": "c:/secrets/server.pfx",
        "Password": "s3cr7t p@$$w0rd"
    }
} 
{CODE-BLOCK/}

We can use wget to request a `Cluster Admin` certificate. This will be the payload of the POST request:

{CODE-BLOCK:json}
{
    "Name": "cluster.admin.client.certificate",
    "SecurityClearance": "ClusterAdmin",
    "Password": "p@$$w0rd"
} 
{CODE-BLOCK/}

First, load the server certificate:

{CODE-BLOCK:powershell}
$cert = Get-PfxCertificate -FilePath c:/secrets/server.pfx
{CODE-BLOCK/}

Then make the request:

{CODE-BLOCK:powershell}
wget -UseBasicParsing -Method POST -Certificate $cert -OutFile "cluster.admin.cert.pfx" -Body '{"Name": "cluster.admin.client.certificate","SecurityClearance": "ClusterAdmin","Password": "p@$$w0rd"}' "https://rvn-srv-1:8080/admin/certificates"
{CODE-BLOCK/}

### Example III : Using Curl in Linux

### Example IV : Using the RavenDB Client

## Client Certificates & the RavenDB Client

