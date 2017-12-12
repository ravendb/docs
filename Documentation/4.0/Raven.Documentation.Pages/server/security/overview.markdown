# Overview

In the security section, we will review the security features in RavenDB and explain how to manage your secured server or cluster.

## Authentication

RavenDB uses x.509 certificate-based authentication. 
X.509 certificates are standardized, secured and widely used in many applications. They allow you to use TLS/SSL and HTTPS which keeps your communications encrypted and secured.

The idea of authentication in RavenDB is that the server holds the "server certificate" which was signed by a trusted SSL Certificate Authority or it can be self-signed.

The administrator issues client certificates for specific uses and assigns permissions to each client certificate. The client certificates can then be used by clients (RavenDB, browser, curl, etc...) to authenticate against the server.

In the Studio, administrators can use the [Certificates View](../../studio/server/certificates) to easily manage their certificates. It can be used to generate client certificates, register existing client certificates, import and export server certificates, rename, assign permissions and more.

#### Read more:

[Certificate Management](authentication/cerificate-management)

[Certificate Renewal & Rotation](authentication/certificate-renewal-and-rotation)

[Let's Encrypt Certificates](authentication/lets-encrypt-certificates)

[Manual Configuration](authentication/manual-configuration)

[Client Certificate Usage](authentication/client-certificate-usage)

[Authentication in the Cluster](authentication/authentication-in-the-cluster)

[Common Errors & Troubleshooting](authentication/common-errors-and-troubleshooting)


## Authorization

RavenDB uses x.509 certificates for authorization as well.

It is considered more secured to use client certificates for authorization, insdead of having usernames and passwords. 

It means that every client certificate is associated with a security clearance and access permissions which are assigned by an administrator. 

#### Read more:

[Security Clearance & Permissions](authorization/security-clearance-and-permissions)

[List of Security Clearances](authorization/list-of-security-clearances)


## Encryption

RavenDB offers full database encryption using libsodium. 

Encryption is implemented at the storage level, with the ChaCha20Poly1305 authenticated encryption scheme and 256 bits keys. 

When database encryption is on, all the features of a database are automatically encrypted - documents, indexes, metadata and every piece of data that is written to disk. The only time you would have decrypted data in memory is during the lifetime of a transaction, when requesting to read/write documents or when doing a query.

#### Read more:

[Theory & Design](encryption/theory-and-design)

[Database Encryption](encryption/database-encryption)

[Server Store Encryption](encryption/server-store-encryption)

[Secret Key Management](encryption/secret-key-management)

