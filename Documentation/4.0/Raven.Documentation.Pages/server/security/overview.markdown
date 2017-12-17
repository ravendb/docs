# Overview

In the security section, we will review the security features in RavenDB and explain how to manage your secured server or cluster.

## Authentication

RavenDB uses X.509 certificate-based authentication. 
X.509 certificates are standardized, secured and widely used in many applications. They allow you to use TLS/SSL and HTTPS which keeps your communications encrypted and secured.

The idea of authentication in RavenDB is based on a fact that the server holds a server certificate, which is either signed by a trusted SSL Certificate Authority or self-signed. The server certificate is used by an administrator to generate client certificates with assigned permissions. Client certificates can be used for authentication, and authorization is granted according to the assigned permissions.

In the Studio, administrators can use the [Certificates View](../../studio/server/certificates) to easily manage their certificates. It can be used to generate client certificates, register existing client certificates, import and export server certificates, rename, assign permissions and more.

<strong>Read more:</strong>

[Certificate Configuration](authentication/certificate-configuration)

[Certificate Management](authentication/cerificate-management)

[Certificate Renewal & Rotation](authentication/certificate-renewal-and-rotation)

[Let's Encrypt Certificates](authentication/lets-encrypt-certificates)

[Client Certificate Usage](authentication/client-certificate-usage)

[Authentication in the Cluster](authentication/authentication-in-the-cluster)

[Common Errors & Troubleshooting](authentication/common-errors-and-troubleshooting)


## Authorization

Authorization in RavenDB is based on the same X.509 certificates.

Every client certificate is associated with a security clearance and access permissions per database. 

<strong>Read more:</strong>

[Security Clearance & Permissions](authorization/security-clearance-and-permissions)


## Encryption

RavenDB offers full database encryption using [libsodium](https://github.com/jedisct1/libsodium), a well-known battle tested encryption library. 

Encryption is implemented at the storage level, with ChaCha20-Poly1305 authenticated encryption using 256 bit keys. 

When database encryption is on, all the features of a database are automatically encrypted - documents, indexes and every piece of data that is written to disk.

<strong>Read more:</strong>

[Database Encryption](encryption/database-encryption)

[Server Store Encryption](encryption/server-store-encryption)

[Secret Key Management](encryption/secret-key-management)

