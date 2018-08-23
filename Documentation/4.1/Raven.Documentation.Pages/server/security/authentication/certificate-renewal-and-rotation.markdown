# Authentication : Certificate Renewal & Rotation

X.509 certificates have expiration dates and must be renewed once in a while.

When using the Setup Wizard to obtain a Let's Encrypt certificate, you don't have to worry about this. Read about [Automatic Let's Encrypt Renewals in RavenDB](../../../server/security/authentication/lets-encrypt-certificates).

If you provided your own certificate to RavenDB, it is **your responsibility** to renew it. 

Once you have a new valid certificate for your server/cluster you need to make RavenDB use it instead of the currently loaded certificate. Replacing a certificate in the cluster is a distributed operation which requires all the nodes to confirm the replacement. The actual update will happen when all nodes of the cluster confirm the replacement or when there are 3 days left for expiration. 

You can also ignore these limits and replace the certificates immediately but beware of this option. Nodes which didn't confirm the replacement, will not be able to re-join the cluster and will have to be setup manually. This means the new certificate will have to be placed manually in that node and [settings.json](../../configuration/configuration-options#json) will have to be edited, to contain the new certificate path. Then a server restart is required.

{DANGER The new certificate must contain all of the cluster domain names in the CN or ASN properties of the certificate. Otherwise you will get an authentication error because SSL/TLS requires the domain in the certificate to match with the actual domain being used. /}

## Replace the Cluster Certificate using the Studio

This feature is under development and will be available in the Studio very soon. In the meantime please use the [RavenDB CLI command](../../../server/administration/cli#replaceclustercert).

## Related articles

### Security

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)

### Installation

- [Secure Setup with a Let's Encrypt Certificate](../../../start/installation/setup-wizard#secure-setup-with-a-let)
- [Secure Setup with Your Own Certificate](../../../start/installation/setup-wizard#secure-setup-with-your-own-certificate)

### Configuration

- [Security Configuration](../../../server/configuration/security-configuration)
