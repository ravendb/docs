# Encryption: Database Encryption

In RavenDB you can create encrypted databases. Each encrypted database will have its own secret key which is used to encrypt and decrypt data.

## Creating An Encrypted Database Using The Studio

When creating an encrypted database [using the Studio](../../../studio/server/databases/create-new-database/encrypted), you will receive a secret key which will 
allow you to recover the encrypted data in case of a disaster, and when restoring from backup. During normal operations there is no need to supply the secret key to RavenDB.  
See [Secret Key Management](../../../server/security/encryption/secret-key-management) for more information.  

![Figure 1. Secret Key](images/1.png)

{DANGER: Danger}
Download, print, or copy and save the secret key in a safe place. It will NOT be available again!
{DANGER/}

## Creating An Encrypted Database Using The REST API And The Client API

Before creating the database, a secret key must be generated. Generating and storing secret keys is restricted to `Operator` or `ClusterAdmin` Security Clearances.
RavenDB uses a [cryptographically secure pseudo-random number generator](https://en.wikipedia.org/wiki/Cryptographically_secure_pseudorandom_number_generator) and 
it is recommended that you use it. If you must use your own secret key, please make sure it is 256 bits long and cryptographically secure.  

You must use a client certificate to make the request because the server is using authentication.

## Windows Example

Load the client certificate in PowerShell:
{CODE-BLOCK:powershell}
$cert = Get-PfxCertificate -FilePath C:\secrets\admin.client.certificate.example.pfx
{CODE-BLOCK/}

Make sure to use TLS 1.2:
{CODE-BLOCK:powershell}
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
{CODE-BLOCK/}

Ask RavenDB to generate a key for you: 
{CODE-BLOCK:powershell}
$response = Invoke-WebRequest https://your-server-url/admin/secrets/generate -Certificate $cert
{CODE-BLOCK/}

Then send the key to the RavenDB server on which the database will be created. Note that the database doesn't exist yet, but you will still need to supply its name. Make the following POST request to assign the secret key to a specific database:
{CODE-BLOCK:powershell}
$payload = [System.Text.Encoding]::ASCII.GetString($response.Content) 
Invoke-WebRequest https://your-server-url/admin/secrets?name=MyEncryptedDatabase -Certificate $cert -Method POST -Body $payload
{CODE-BLOCK/}

Finally, create the encrypted database using the Client API:
{CODE-TABS}
{CODE-TAB:csharp:Sync CreateEncryptedDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateEncryptedDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

## Linux Example

When generating a client certificate using RavenDB, you will receive a Zip file containing an admin client certificate (.pfx, .crt, .key).

First we will create a .pem certificate file from the .crt and .key files:
{CODE-BLOCK:plain}
cat admin.client.certificate.example.crt admin.client.certificate.example.key > clientCert.pem
{CODE-BLOCK/}

Ask RavenDB to generate a key for you: 
{CODE-BLOCK:plain}
key=$(curl --cert clientCert.pem  https://your-server-url/admin/secrets/generate)
{CODE-BLOCK/}

Then send the key to the RavenDB server on which the database will be created. Note that the database doesn't exist yet, but you will still need to supply its name. Make the following POST request to assign the secret key to a specific database:
{CODE-BLOCK:plain}
curl -X POST -H "Content-Type: text/plain" --data $key --cert clientCert.pem https://your-server-url/admin/secrets?name=MyEncryptedDatabase
{CODE-BLOCK/}

Finally, create the encrypted database using the Client API:
{CODE-TABS}
{CODE-TAB:csharp:Sync CreateEncryptedDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateEncryptedDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

## Remarks

Database encryption must be enabled when creating the database. If you wish to use encryption in an existing database, it must be exported and then imported back into a new encrypted database.

{DANGER: Indexing transaction size}
Indexing is most efficient when it is performed in the largest transactions possible. However, using encryption is very memory intensive, and if memory 
runs out before the transaction completes, the entire transaction will fail. To avoid this, you can limit the size of indexing batches in encrypted 
databases using [Indexing.Encrypted.TransactionSizeLimitInMb](../server/configuration/indexing-configuration#Indexing.Encrypted.TransactionSizeLimitInMb).  
{DANGER/}


## Related Articles

### Encryption

- [Encryption at Rest](../../../server/security/encryption/encryption-at-rest)
- [Server Store Encryption](../../../server/security/encryption/server-store-encryption)
- [Secret Key Management](../../../server/security/encryption/secret-key-management)

### Security

- [Overview](../../../server/security/overview)
