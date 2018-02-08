# Database Encryption

### Creating an encrypted database

In RavenDB, you can create encrypted databases. Each encrypted database will have its own secret key. 
If you wish to use encryption in an existing database, it must be exported and then imported back into a new encrypted database.

[Use the Studio](../../../studio/server/databases/create-new-database/encrypted) to create an encrypted database. You will recieve a secret key which will allow you to recover the encrypted data in case of a disaster. During normal operations there is no need to supply the secret key to RavenDB.  
See [Secret Key Management](secret-key-management) for more information.  

![Figure 1. Secret Key](images/1.png)

{DANGER: Danger}
Download, print, or copy and save the secret key in a safe place. It will NOT be available again!
{DANGER/}

You can also create an encrypted database by using the REST API and the Client API.  

Before creating the database, a secret key must be generated. Generating and storing secret keys is restricted to `Operator` or `ClusterAdmin` Security Clearances.
RavenDB uses a [cryptographically secure pseudo-random number generator](https://en.wikipedia.org/wiki/Cryptographically_secure_pseudorandom_number_generator) and it is recommended that you use it. If you must use your own secret key, please make sure it is 256 bits long and cryptographically secure.

Ask RavenDB to generate a new Base64 secret key by issuing the following GET request:  
    
{CODE-BLOCK:plain}
https://your-server-url/admin/generate
{CODE-BLOCK/}

Then, send the key to the RavenDB server on which the database will be created. Note that the database doesn't exist yet, but you will still need to supply its name. Make the following POST request to assign the secret key to a specific database:
{CODE-BLOCK:plain}
https://your-server-url/admin/secrets?name=MyEncryptedDatabase
{CODE-BLOCK/}

You need to supply the Base64 key as string content in the POST request body.

Finally, create the database:

{CODE-TABS}
{CODE-TAB:csharp:Sync CreateEncryptedDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateEncryptedDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}
