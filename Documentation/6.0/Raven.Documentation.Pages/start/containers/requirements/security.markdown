# Certificates & Renewal requirements for Running RavenDB in Containers


## **Overview**

After networking works properly [Containers > Requirements > Networking](./networking), let's focus on reliable and secure communication requirements.
RavenDB uses X.509 certificate-based authentication [Security Overview](../../../server/security/overview) for secured connection.


This documentation page will describe the crucial aspects of using certificates while running RavenDB in containers.

## Getting the certificate
#### Setup Wizard
You can use the `Setup.Mode: Initial` configuration option to run the Setup Wizard if you need it. It can generate the LetsEncrypt certificate for you, create a setup package for your other nodes, and more.
Check what it's fully capable of here: [Setup Wizard Docs](../../installation/setup-wizard)

Setup Wizard will be a no-go for you in some containerized or orchestrated setups because of the quirks of containerized (stateless) or declarative world.
Fortunately, RavenDB is aware of that and provides many different ways to handle it, like certificate management script injection, to cover your case.


#### Generate it on your own
If you need to generate it on your own behalf, you'll need to generate the .pfx certificate yourself (e.g., using OpenSSL or tools like certbot that talk to LetsEncrypt).
After getting your cert, start tuning the server to leverage it for security.

## Providing a certificate
RavenDB needs to get its server certificate. You can configure its retrieval from one of these origins:

- Path - A .pfx certificate stored under a path reachable for a container.
  **Security.Certificate.Path** configuration option value defines the path. [Security Configuration - Security.Certificate.Path](../../../server/configuration/security-configuration#security.certificate.path)

- Script - A script that returns your certificate by any means.
  You can obtain it from container environmental variables, secured vault, secret, etc.
  **Security.Certificate.Load.Exec** configuration option value defines the script path. [Security Configuration - Security.Certificate.Load.Exec](../../../server/configuration/security-configuration#security.certificate.load.exec)

These configuration options can be passed to RavenDB by settings.json, environmental variables, or command line arguments.
See more here: [Configuration Options](../../../server/configuration/configuration-options)

This way, RavenDB should be able to get its certificate.

## Certificate expiration
Expired certificates should be updated and replaced.

#### LetsEncrypt - Setup Wizard
LetsEncrypt certificate management automation allows RavenDB to refresh your LetsEncrypt certificate automatically.
To enable it, set `Setup.Mode` configuration option to `LetsEncrypt`.
Be aware that this automation will work **only when using Setup Wizard to obtain the LetsEncrypt certificate**, as RavenDB doesn't control your domain nor can access your backend used in the certificate load script.
To learn more about this, visit this site: [RavenDB Lets Encrypt Certificates Docs](../../../server/security/authentication/lets-encrypt-certificates)

You can learn about different `Raven.SetupMode` values here: [Core Configuration - Setup.Mode](../../../server/configuration/core-configuration#setup.mode)
Also, you need to provide us with an email that you will use for the Let's Encrypt matter. Use `Security.Certificate.LetsEncrypt.Email`.

#### Manual
To configure manual certificate replacement and updates, write scripts to supply RavenDB configuration:

- [Security.Certificate.Renew.Exec](../../../server/configuration/security-configuration#security.certificate.renew.exec) 
- [Security.Certificate.Change.Exec](../../../server/configuration/security-configuration#security.certificate.change.exec) 

It will allow RavenDB to execute your routines, which should:

- check if the certificate is ready to renew and do so if needed, then return it (**Renew**) 
- replace the old certificate (**Change**) 

To learn about manual certificate replacement, read this article: [Certificate Renewal And Rotation](../../../server/security/authentication/certificate-renewal-and-rotation)


### Conclusion
Some of these approaches may not apply in your containerized case, especially when storing your certificates in a secured vault.
Be careful and pick the way of handling certificates that suits your case the best.
A good rule of thumb is to pick the least complex solution that works for you.
The easiest is to rely on the automatic renewal provided by RavenDB.
Writing your own scripts for getting, updating, and replacing certificates may require more effort but may also suit you better.

