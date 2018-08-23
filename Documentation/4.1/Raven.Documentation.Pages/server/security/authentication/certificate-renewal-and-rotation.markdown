# Authentication : Certificate Renewal & Rotation

X.509 certificates have expiration dates and must be renewed once in a while.

When using the Setup Wizard to obtain a Let's Encrypt certificate, you don't have to worry about this. Read about [Automatic Let's Encrypt Renewals in RavenDB](../../../server/security/authentication/lets-encrypt-certificates).

If you provided your own certificate to RavenDB, it is **your responsibility** to renew it. 

Once you have a new valid certificate for your server/cluster you need to make RavenDB use it instead of the currently loaded certificate. Replacing a certificate in the cluster is a distributed operation which requires all the nodes to confirm the replacement. The actual update will happen when all nodes of the cluster confirm the replacement or when there are 3 days left for expiration. 

You can also ignore these limits and replace the certificates immediately but beware of this option. Nodes which didn't confirm the replacement, will not be able to re-join the cluster and will have to be setup manually. This means the new certificate will have to be placed manually in that node and [settings.json](../../configuration/configuration-options#json) will have to be edited, to contain the new certificate path. Then a server restart is required.

{DANGER The new certificate must contain all of the cluster domain names in the CN or ASN properties of the certificate. Otherwise you will get an authentication error because SSL/TLS requires the domain in the certificate to match with the actual domain being used. /}

## Replace the Cluster Certificate using the Studio

Access the certificate view and click on `Cluster certificate` -> `Replace cluster certificate`. Upload the PFX file and name it.

This will start the certificate replacement process.

If you encounter a problem during the replacement process, and there are no alerts in tne studio telling you what's wrong, please check the logs for errors and exceptions.

If your logs are turned off, open Manage Server->Admin Logs in the Studio, and keep them open while you perform the certificate replacement.

When running in a cluster, please look at the logs of all nodes.

## Replace the Cluster Certificate using Powershell

Here is a little example of using the REST API directly with powershell to replace the cluster certificate:

{CODE-BLOCK:powershell}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$clientCert = Get-PfxCertificate -FilePath C:\path\to\client\cert\admin.client.certificate.raven.pfx

$newCert = get-content 'C:\path\to\server\cert\new.certificate.pfx' -Encoding Byte

$newCertBase64 = [System.Convert]::ToBase64String($newCert)

$payload = @{
    Name              = "MyNewCert";
    Certificate       = $newCertBase64;
} | ConvertTo-Json

$response = Invoke-WebRequest https://b.raven.development.run:8080/admin/certificates/replace-cluster-cert -Certificate $clientCert -Method POST -Body $payload -ContentType "application/json"
{CODE-BLOCK/}


## Related articles

### Security

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)

### Installation

- [Secure Setup with a Let's Encrypt Certificate](../../../start/installation/setup-wizard#secure-setup-with-a-let)
- [Secure Setup with Your Own Certificate](../../../start/installation/setup-wizard#secure-setup-with-your-own-certificate)

### Configuration

- [Security Configuration](../../../server/configuration/security-configuration)
