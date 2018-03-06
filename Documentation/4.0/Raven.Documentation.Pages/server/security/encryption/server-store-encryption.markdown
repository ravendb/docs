# Server Store Encryption

The `Server Store` is a special database (called `System`) which is server-wide and **is not encrypted by default**.  
It contains information such as the cluster state machine, database records, compare-exchange values, client certificate definitions (without the private key), protected secret keys of encrypted databases and more.

## Enabling Server Store Encryption

This is an offline operation which should be performed only once using the [rvn tool](../../../server/administration/cli). 
It is recommended to do this at the very start, as part of the initial server setup, right after the server was launched for the first time (setup -> shutdown -> encrypt -> restart).
Server Store encryption is also possible at later times, even after creating databases and storing documents.

To encrypt the Server Store, make sure the server is not running. Then navigate to the RavenDB application folder where you can find the `rvn` tool. Run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
./rvn offline-operation encrypt <path-to-system-dir>
{CODE-BLOCK/}

This operation encrypts the data and saves the secret key to the same directory.  
**The key file (secret.key.encrypted) is protected using RavenDB's secret protection policy**. Read about [Secret Key Management](../../../server/security/encryption/secret-key-management) to learn how RavenDB protects secrets.
Once encrypted, The server will only work for the current OS user or the current Master Key (whichever method was chosen to protect secrets). 
Backups of an encrypted Server Store can only be restored by the same OS user or the same Master Key which was used during backup.

From this point on, the Server Store is encrypted and adheres to the same principles of database encryption described [here](../../../server/security/encryption/encryption-at-rest#how-does-it-work).

## Disabling Server Store Encryption

To decrypt the Server Store, make sure the server is not running. Then navigate to the RavenDB application folder where you can find the `rvn` tool. Run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
./rvn offline-operation decrypt <path-to-system-dir>
{CODE-BLOCK/}

The decryption is done using the key file (secret.key.encrypted) which was originally created when Server Store encryption was enabled.
From this point on, the Server Store is not encrypted anymore and the key file is deleted.

## Backup and Restore an Encrypted Server Store

Because of RavenDB's Secret Protection, the encrypted data is tied to a specific machine/user or to a supplied master key. The following instructions **assume that no changes were made** to the OS user or the master key between backup and restore. If any of them changed, move to the next section.

Navigate to the RavenDB application folder where you can find the `rvn` tool. Run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
./rvn offline-operation get-key <path-to-system-dir>
{CODE-BLOCK/}

The output is a folder called `Temp.Encryption` which contains an encrypted snapshot of the Server Store and a key file (secret.key.encrypted) which allows decryption of the server store and must be kept safely. 
Rename the folder and save it in a safe place. If you decide to separate the key from the backup data (recommended) you should make sure to return the key file to the folder before performing a restore.

To restore the encrypted Server Store from backup, first shutdown the server. 
Delete the current `System` folder and replace it with the backed up folder (don't forget to rename it to `System`).
Then restart the server.

## Moving the Server to a new machine or a different OS user

Because of RavenDB's Secret Protection, the encrypted data is tied to a specific machine/user or to a supplied master key. 
Moving the server to a new machine or switching the OS user is supported but requires the admin to perform several offline operations.

Navigate to the RavenDB application folder where you can find the `rvn` tool. Run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
./rvn offline-operation get-key <path-to-system-dir>
{CODE-BLOCK/}

The output is a folder called `Temp.Encryption` which contains an encrypted snapshot of the Server Store and a key file (secret.key.encrypted). 
Move the output folder to the new machine. Replace the System database with it.

On the new machine, run the following command and supply the path of the `System` database:

{CODE-BLOCK:bash}
./rvn offline-operation put-key <path-to-system-dir>
{CODE-BLOCK/}

This operation takes the key file which sits in the folder and protects it for the new OS user or new master key.

## Changing the OS user password

Under construction
