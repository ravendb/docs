# Server Store Encryption

The `Server Store` is a special database (called `System`) which is server-wide and **is not encrypted by default**.  
It contains internal RavenDB information such as the cluster state machine, database records, license information, client certificate definitions (without the private key), protected secret keys of encrypted databases and more.

## Enabling Server Store Encryption

This is an offline operation which should be performed only once using the [rvn tool](../../../server/administration/cli). 
It is recommended to do this at the very start, as part of the initial server setup, right after the server was launched for the first time (setup -> shutdown -> encrypt -> restart).
Server Store encryption is also possible at later times, even after creating databases and storing documents.

To encrypt the Server Store, make sure the server is not running. Then navigate to the RavenDB application folder where you can find the `rvn` tool. Run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
rvn server encrypt <path-to-system-dir>
{CODE-BLOCK/}

This operation encrypts the data and saves the secret key to the same directory.  
**The key file (secret.key.encrypted) is protected using RavenDB's secret protection policy**. Read about [Secret Key Management](../../../server/security/encryption/secret-key-management) to learn how RavenDB protects secrets.
Once encrypted, The server will only work for the current OS user or the current Master Key (whichever method was chosen to protect secrets). 

From this stage on, the Server Store is encrypted and adheres to the same principles of database encryption described [here](../../../server/security/encryption/encryption-at-rest#how-does-it-work).

## Restoring from backup

Under construction

## Moving the Server to a new machine/user

Under construction

## Changing the Windows user password

Under construction
