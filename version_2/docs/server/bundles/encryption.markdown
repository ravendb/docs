﻿#Encryption bundle

The encryption bundle introduces data encryption to RavenDB. By default it uses AES-128 encryption algorithm, but this can be easily changed if needed. Encryption is fully transparent for the end-user, it applies to all documents and in default configuration to all indexes as well.

##Configuration

If you want to setup new database with encryption bundle using the Studio, then please refer to [this](../../studio/Bundles/encryption) page.

Three possible configuration options are:   
* **Raven/Encryption/Algorithm** with [AssemblyQualifiedName](http://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx) as a value. Additionally provided type must be a subclass of [SymmetricAlgorithm](http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx) from `System.Security.Cryptography` namespace and must not be an abstract class    
* **Raven/Encryption/Key** a key used for encryption purposes with minimum length of 8 characters, base64 encoded    
* **Raven/Encryption/EncryptIndexes** Boolean value indicating if the indexes should be encrypted. Default: true   

###Global configuration

All configuration settings can be setup server-wide by adding them to server configuration file.

###Database configuration

All settings can be overridden per database during the database creation process.

{CODE encryption_1@Server\Bundles\Encryption.cs /}

Above example demonstrates how to create `EncryptedDB` with active encryption and with non-default encryption algorithm.

{NOTE All encryption settings can only be provided when database is being created. Changing them later will cause DB malfunction. /}

##Encryption key management

In RavenDB, we have two types of configurations. Server-wide, which is usually located at the `App.config` file and database specific, which is located at the System database. For the `App.config` file, we provide support for encrypting the file using [DPAPI](http://en.wikipedia.org/wiki/Data_Protection_API), using the standard .NET config file encryption system. For database specific values, we provide our own support for encrypting the values using DPAPI.

So, as the end result of above:    
*	Your documents and indexes are encrypted when they are on disk using strong encryption.    
*	You can use a server wide or database specific key for the encryption.   
*	Your encryption key is guarded using DPAPI.   
*	The data is safely encrypted on disk, and the OS guarantee that no one can access the encryption key.   

{NOTE It is your responsibility to backup the encryption key, because there is no way to recover data without it. /}
