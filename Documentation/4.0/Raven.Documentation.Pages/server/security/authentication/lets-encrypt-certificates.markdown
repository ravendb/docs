# Authentication : Let's Encrypt Certificates

RavenDB 4.x uses X.509 certificates for authentication and authorization and has **built in support** for [Let's Encrypt](https://letsencrypt.org/).

## Obtain a Let's Encrypt Certificate

The [Setup Wizard Walkthrough](../../../start/installation/setup-wizard) explains how to obtain a free Let's Encrypt certificate for your server or cluster.

The certificate contains all of the domain names of the cluster in the ASN (Alternative Subject Name) property. For example, if you setup a 3 node cluster and choose the domain "example.ravendb.community", the certificate will contain 3 ASN entries:  

- a.example.ravendb.community  
- b.example.ravendb.community  
- c.example.ravendb.community  

This way, the same certificate is used in all the nodes of the cluster.

Let's Encrypt [recently announced](https://letsencrypt.org/2017/07/06/wildcard-certificates-coming-jan-2018.html) support for wildcard certificates, and RavenDB will start using them soon.

## Automatic Renewal

Let's Encrypt certificates have a [90-day lifetime policy](https://letsencrypt.org/2015/11/09/why-90-days.html).

In RavenDB, you don't need to worry about renewals. RavenDB takes care of this for you.

When there are 30 days left until expiration, RavenDB will initiate the certificate renewal and replacement process. The actual request to Let's Encrypt will happen on the nearest coming Saturday.

Once the renewed certificate is obtained, [it will be replaced](../../../server/security/authentication/certificate-renewal-and-rotation) in all the nodes of the cluster without needing to shut down any server.

{WARNING: Warning} 
Automatic renewals of certificates is available only if you obtained your certificate using the Setup Wizard and got your free RavenDB domain. It doesn't work for self-obtained certificates, even if issued by Let's Encrypt.
{WARNING/}

## Updating DNS records

At the moment, updating DNS records for your domain can only be acheived by running the Setup Wizard again.

We are working on a new dedicated page in our website that will allow to easily edit DNS records which are associated with your license. Once deployed, it will be described and explained here.

## Related articles

### Security 

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)
- [Common Errors and FAQ](../../../server/security/common-errors-and-faq)

### Client API

- [Setting up Authentication and Authorization](../../../client-api/setting-up-authentication-and-authorization)

### Installation

- [Secure Setup with a Let's Encrypt Certificate](../../../start/installation/setup-wizard#secure-setup-with-a-let)
