# Secret Key Management

One of the challenges in cryptosystems is "secret protection" - how to protect the encryption key.  
If the key is stored in plain text, then any user that can access the key can access the encrypted data. If the key is to be encrypted, another key is needed, and so on. 

In RavenDB this can be handled in two ways - depending on the user's choice:

1. [Providing a master key to RavenDB](#providing-a-master-key-to-ravendb)
2. [Relying on the OS protection methods](#relying-on-the-os-protection-methods)

## Providing a master key to RavenDB

If a master key is provided, RavenDB will use it to encrypt the secret keys of encrypted databases.

You can provide a master key by setting `Security.MasterKey.Exec` and `Security.MasterKey.Exec.Arguments` in `settings.json`. RavenDB will invoke a process you specify, so you can write your own scripts / mini programs and apply whatever logic you need. It creates a clean separation between RavenDB and the secret store in use. This option is useful when you want to protect your master key with other solutions such as "Azure Key Vault", "HashiCorp Vault" or even Hardware-Based Protection.

RavenDB expects to get a cryptographically secure 256-bit key through the standard output.

For example, the following C# Console Application (GiveMeKey.cs) will generate a random key and write it to the standard output. Obviously this is just an example, and your executable should supply the same key every time it is invoked.

{CODE-BLOCK:csharp}
using System;
using System.Security.Cryptography;

namespace GiveMeKey
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer = new byte[256 / 8];
            using (var cryptoRandom = new RNGCryptoServiceProvider())
            {
                cryptoRandom.GetBytes(buffer);
            }
            var stream = Console.OpenStandardOutput();
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
{CODE-BLOCK/}

And `settings.json` can look like this:

{CODE-BLOCK:json}
{
    "ServerUrl": "https://rvn-srv-1:8080",
    "Setup.Mode": "None",
    "DataDir": "RavenData",
    "Security.Certificate.Path": "your-server-cert.pfx",
    "Security.MasterKey.Exec": "C:\\secrets\\GiveMeKey.exe"
}
{CODE-BLOCK/}

Another way to provide a master key is to use a file containing the raw key bytes. In that case set `Security.MasterKey.Path` in `settings.json` with the file path. RavenDB expects a cryptographically secure 256-bit key.

## Relying on the OS protection methods

If a master key is not provided by the user, RavenDB will use the following default behavior:

In **Windows**, secret keys are encrypted and stored using the [Data Protection API](https://msdn.microsoft.com/en-us/library/ms995355.aspx), which means they can only be retrieved by the user who stored them.

In **Unix**, RavenDB will generate a random master key and store it in the user's Home folder with read/write permissions (octal 1600) only for the user who stored it. Then, RavenDB will use this master key to encrypt the secret keys of encrypted databases.


