# Encryption: Secret Key Management
---

{NOTE: }

One of the challenges in cryptosystems is "secret protection" - how to protect the encryption key. 
If the key is stored in plain text then any user that can access the key can access the encrypted data. 
If the key is to be encrypted, another key is needed, and so on.  

In RavenDB this can be handled in one of two ways:  

1. [Providing a master key to RavenDB](../../../server/security/encryption/secret-key-management#providing-a-master-key-to-ravendb)  
2. [Relying on the OS protection methods](../../../server/security/encryption/secret-key-management#relying-on-the-os-protection-methods)  

{NOTE/}

---

{PANEL: Providing a Master Key to RavenDB}

If a master key is provided, RavenDB will use it to encrypt the secret keys of encrypted databases.  

You can provide a master key by setting `Security.MasterKey.Exec` and `Security.MasterKey.Exec.Arguments` in 
[settings.json](../../configuration/configuration-options#json). RavenDB will invoke a process you specify, so 
you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation 
between RavenDB and the secret store in use. This option is useful when you want to protect your master key 
with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection.  

RavenDB expects to get a cryptographically secure 256-bit key through the standard output.  

For example, the following C# Console Application (GiveMeKey.cs) will generate a random key and write it to the 
standard output. Obviously this is just an example and your executable should supply the same key every time it 
is invoked.  

{CODE-TABS}
{CODE-TAB:csharp:GiveMeKey.cs writing_key@Server\Security\GiveMeKey.cs /}
{CODE-TABS/}

And [settings.json](../../configuration/configuration-options#json) can look like this:  

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "RavenData",
    "Security.Certificate.Path": "your-server-cert.pfx",
    "Security.MasterKey.Exec": "C:\\secrets\\GiveMeKey.exe"
}
{CODE-BLOCK/}

Another way to provide a master key is to use a file containing the raw key bytes. In that case, set 
`Security.MasterKey.Path` in [settings.json](../../configuration/configuration-options#json) with the file path. 
RavenDB expects a cryptographically secure 256-bit key.  

{PANEL/}

{PANEL: Relying on the OS Protection Methods}

If a master key is not provided by the user RavenDB will use the following default behavior:  

In **Windows**, secret keys are encrypted and stored using the [Data Protection API (DPAPI)](https://docs.microsoft.com/en-us/previous-versions/ms995355(v=msdn.10)), which means they can only be retrieved by the user who stored them.  

In **Unix**, RavenDB will generate a random master key and store it in the user's home folder with read/write 
permissions (octal 1600) only for the user who stored it. Then, RavenDB will use this master key to encrypt the 
secret keys of encrypted databases.  

### Changing/Resetting a Windows User Password  

This section is relevant only to [Server Store encryption](../../../server/security/encryption/server-store-encryption) 
and only if you chose to rely on the **Windows** protection methods.  

Windows uses the **user password** to encrypt secrets in [DPAPI](https://docs.microsoft.com/en-us/previous-versions/ms995355(v=msdn.10)).
When a Windows password is **changed** the following actions are taken:  

- DPAPI receives notification from Winlogon during a password change operation.  
- DPAPI decrypts all the secrets that were encrypted with the user's old passwords.  
- DPAPI re-encrypts all the secrets with the user's new password.  

Changing a password this way is supported and RavenDB is not affected.  

On the other hand, if the password was **reset** (either by you or by the administrator), secrets **cannot be decrypted anymore**.  

If you still need to reset the password for some reason, please follow these steps to ensure that secret 
keys which are protected with DPAPI aren't lost.  

Navigate to the RavenDB application folder where you can find the `rvn` tool. 
Run the following get-key command for **every** encrypted database (including `System` if it's encrypted):  

{CODE-BLOCK:bash}
./rvn offline-operation get-key <path-to-database-dir>
{CODE-BLOCK/}

The output is the plaintext key which is not protected and not tied to a user.  

Now reset the Windows password.  

Then, run the following put-key command for **every** encrypted database. Supply the path of the 
database folder and the key you just got (using get-key):  

{CODE-BLOCK:bash}
./rvn offline-operation put-key <path-to-database-dir> <base64-plaintext-key>
{CODE-BLOCK/}

This operation takes the key and protects it with the new Windows user password.  
After doing this for all databases you can run the server and continue working.  

### Using the Admin JS Console

{DANGER: }
Do not use the console unless you're sure of what you're doing. Running a 
script in the Admin Console could cause your server to crash, loss of 
data, or other irreversible harm.  
{DANGER/}

The server's Admin Console is [found in the Studio](../../../studio/server/debug/admin-js-console). 
You can use it to access and change your master key. This method is useful for 
changing the key when you change your **Windows** user account.  

On the console page, select a database or the server. If you select a database, 
the master key is read only, and can be accessed with this script:  

{CODE-BLOCK:javascript }
return database.MasterKey
{CODE-BLOCK/}

The master key for a given database can be modified with the script:  

{CODE-BLOCK:javascript }
server.ServerStore.PutSecretKey(base64, name, overwrite)
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **base64** | `string` | The new master key for the database |
| **name** | `string` | The name of the database for which to change the key |
| **overwrite** | `bool` | Whether to overwrite an existing key. If this is false and the database has a master key, an exception will be thrown. Be sure that this is what you want to do. |

#### Changing the Windows User

To change your Windows user without losing access to your database, use the scripts described 
above to:  

1. Retrieve your current key
2. Switch your windows user
3. Inject the key back into your database

{CODE-BLOCK:javascript }
return database.MasterKey

// Save the returned key
// Change windows user

server.ServerStore.PutSecretKey(<master key>, <database name>, true)
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Encryption

- [Encryption at Rest](../../../server/security/encryption/encryption-at-rest)
- [Database Encryption](../../../server/security/encryption/database-encryption)
- [Server Store Encryption](../../../server/security/encryption/server-store-encryption)

### Security

- [Overview](../../../server/security/overview)

### Studio

- [Admin JS Console](../../../studio/server/debug/admin-js-console)
