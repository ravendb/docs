# Encryption At Rest

Encryption at rest is implemented at the storage layer, using Daniel J. Bernstein's `XChaCha20-Poly1305` authenticated encryption algorithm.

## What does it mean?

In `Voron`, the storage engine behind RavenDB, data is stored in memory mapped files. This includes documents, indexes, attachments and transactions which are written to the journal.

If your disk is stolen or lost, an attacker will have full access to the raw data files and without encryption turned on, the data can be read with very little effort.

On the other hand, when encryption is turned on - the raw data is encrypted and unreadable without possession of the secret key.

In RavenDB, encryption is done at the lowest possible layer, the storage engine. It is fully transparent to other levels of the server, making it super easy to use.

## How does it work?

As long as the database is idle and there are no requests to serve, everything is kept encrypted in the data files.

Once a request is made, RavenDB will start a transaction (either read or write) and decrypt just the necessary data into memory. Then it will serve the request, and when the transaction is finished, modified pages are encrypted and written back to the datafile.

{DANGER: Important things to be aware of:}
1. RavenDB makes sure that **no data is written to disk as plain text**. It will always be encrypted.  
2. Indexed fields (the actual data) will reside in memory as plain text.  
3. Data of the current transaction will reside in memory as plain text, and only for the duration of the transaction. When the transaction ends, all this memory is safely zeroed.  
4. Loading documents from the database (using the Studio, the Client API, REST API) means that they will be decrypted and sent to you as plain text.
{DANGER/}

{NOTE Due to the overhead of the encryption algorithm, performance can be decreased slightly. However, it doesn't affect the ACID properties of RavenDB which remains both transactional and secured./}


## What about Encryption in Transit?

To enable encryption in RavenDB, the user must first [enable authentication](../../../server/security/authentication/certificate-configuration) and HTTPS (by providing a certificate).

HTTPS (using the TLS 1.2 protocol) provides privacy and integrity of the data in transit. It protects against man-in-the-middle attacks, eavesdropping and tampering of the communication.

Using encryption together with HTTPS provides assurance that your data is safe both at rest and in transit.

## Related articles

- [Database Encryption](database-encryption)
- [Server Store Encryption](server-store-encryption)
- [Secret Key Management](secret-key-management)
