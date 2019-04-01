# Authentication: Let's Encrypt Certificates

RavenDB 4.x uses X.509 certificates for authentication and authorization and has **built in support** for [Let's Encrypt](https://letsencrypt.org/).

## Obtain a Let's Encrypt Certificate

The [Setup Wizard Walkthrough](../../../start/installation/setup-wizard) explains how to obtain a free Let's Encrypt certificate for your server or cluster.

It's a wildcard certificate, so if you choose the domain `example` during the wizard (with the community license), the generated certificate will have the common name (CN) `*.example.ravendb.community`.

## Automatic Renewal for Let's Encrypt certificates obtained via RavenDB

Let's Encrypt certificates have a [90-day lifetime policy](https://letsencrypt.org/2015/11/09/why-90-days.html).

In RavenDB, you don't need to worry about renewals. RavenDB takes care of this for you.

When there are 30 days left until expiration, RavenDB will initiate the certificate renewal and replacement process. The actual request to Let's Encrypt will happen on the nearest coming Saturday.

Once the renewed certificate is obtained, [it will be replaced](../../../server/security/authentication/certificate-renewal-and-rotation) in all the nodes of the cluster without needing to shut down any server.

{WARNING: Warning} 
Automatic certificate renewal is available only if you obtained your certificate using the Setup Wizard and got your free RavenDB domain. Self-obtained certificates will not renew automatically, even if issued by Let's Encrypt.
{WARNING/}

When running as a cluster, the replacement process is a distributed operation. It involves sending the new certificate to all nodes, and requires all nodes to confirm that they have recieved and replaced the certificate.

Only when all nodes have confirmed will the cluster start using this new certificate. 

If a node is not responding during the replacement, the operation will not complete until one of the following happens:

* The node will come back online. It should pick up the replacement command and join the replacement process automatically.

* There are only 3 days left for the expiration of the certificate. In this case, the cluster will complete the operation without the node which is down. **When bringing that node up, the certificate must be replaced manually.**

During the process you will receive alerts in the studio and in the logs indicating the status of the operation and any errors if they occur. The alerts are displayed for each node independently.

## Automatic Renewal for self-obtained certificates

When you set up RavenDB with your own Let's Encrypt certificate, the renewal mechanism will not work because RavenDB doesn't control your domain and cannot pass the Let's Encrypt challenge that proves ownership of a domain.
However, you can (quite easily) enable automatic renewals for your Let's Encrypt certificate via [Certbot](https://certbot.eff.org/).

First, install and configure certbot on your machine. Here's a [nice tutorial](https://medium.com/prog-code/lets-encrypt-wildcard-certificate-configuration-with-aws-route-53-9c15adb936a7) to get you started.
You should also download the apropriate DNS plugin for certbot. This example uses [Amazon's Route53](https://certbot-dns-route53.readthedocs.io/en/stable/), but [many other services](https://certbot.eff.org/docs/using.html#dns-plugins) are supported.

Set the credentials for your DNS service. In Route53 it's done by creating a user with an [IAM policy](https://certbot-dns-route53.readthedocs.io/en/stable/#sample-aws-policy-json) to allow changing DNS records. The credentials can then be set in the server as environment variables or via the AWS config file at `~/.aws/config`.

Now that certbot is ready, you can create an executable script that will run the certbot command whenever RavenDB asks it, this way the certificate will keep renewing itself. 

When using the `Security.Certificate.Exec` option, RavenDB expects to get the raw binary representation (byte array) of the .pfx certificate through the standard output.

Here's a little script, `certificate.sh`, that demonstrates this feature. It renews the certificate or creates it in the first run, uses `openssl` to convert the received file to .PFX and writes it to the standard output for RavenDB to consume. 
{CODE-BLOCK:bash}
certbot -d *.test.ravendb.cloud certonly --config-dir ~/.certbot/config --logs-dir ~/.certbot/logs --work-dir ~/.certbot/work --dns-route53 --dns-route53-propagation-seconds 30 --non-interactive --agree-tos -m name@mail.com > /dev/null 2>&1
openssl pkcs12 -inkey ~/.certbot/config/live/test.ravendb.cloud/privkey.pem -in ~/.certbot/config/live/test.ravendb.cloud/cert.pem -export -out ./cert.pfx -passout pass:
cat -u ~/.certbot/config/live/test.ravendb.cloud/cert.pfx
{CODE-BLOCK/}

{WARNING:Important}
Use unbuffered I/O (the -u flag) when writing the certificate to the standard output, otherwise RavenDB might get a partial file and fail loading the certificate.
{WARNING/}

To enable the script, add the following to settings.json:

{CODE-BLOCK:json}
"Security.Certificate.Exec": "/bin/bash",
"Security.Certificate.Exec.Arguments": "certificate.sh"
{CODE-BLOCK/}

Certbot is not available in Windows, but you can use a c# client called [Certes](https://github.com/fszlin/certes/), or [other similar projects](https://letsencrypt.org/docs/client-options/) that automate the certificate process. [See this example](../../../server/security/authentication/certificate-configuration) of how to write a file to standard output in Powershell.

## Manual Renewal

You may initiate the renewal process manually by going to the certificate view in the studio and clicking `Renew` on the server certificate. It will trigger the same certificate replacement process which was described in [Automatic Renewal](../../../server/security/authentication/lets-encrypt-certificates#automatic-renewal).

If a node is down and you click `Renew`, the cluster will complete the operation without the node that is down. **When bringing that node up, the certificate must be replaced manually.**


## Updating DNS records

Updating DNS records for your domain can be acheived by running the Setup Wizard again or by using a dedicated page at the RavenDB website.

You can easily edit the DNS records which are associated with your license using the [Customers Portal](https://customers.ravendb.net).

## Related articles

### Security 

- [Overview](../../../server/security/overview)
- [Certificate Management](../../../server/security/authentication/certificate-management)
- [Common Errors and FAQ](../../../server/security/common-errors-and-faq)

### Client API

- [Setting up Authentication and Authorization](../../../client-api/setting-up-authentication-and-authorization)

### Installation

- [Secure Setup with a Let's Encrypt Certificate](../../../start/installation/setup-wizard#secure-setup-with-a-let)
