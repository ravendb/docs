# Encryption at Rest

Encryption at rest is implemented at the storage level, using Daniel J. Bernstein's `XChaCha20-Poly1305` authenticated encryption algorithm.

**What does it mean?**

In `Voron`, the storage engine behind RavenDB, data is stored in memmory mapped files. This includes documents, indexes, attachments and transactions which are written to the journal.

If your disk is stolen or compromized, an attacker will have full access to the raw data files and with very little effort be able to read all of your documents.

On the other hand, if encryption is turned on - stolen/compromised files are useless without possesion of the secret key.

Because encryption is done at the lower levels of the storage engine, it makes it super easy to use. Everything is done transparentely behind the scenes and requires no user intervention. 


**How does it work?**

As long as the database is idle and there are no requests to serve, everything is kept encrypted in the data files.

Once a request is made, RavenDB will start a transaction (either read or write) and decrypt just the necessary pages into memory. Then it will serve the request, and when the transaction is finished, modified pages are encrypted and written back to the datafile.

In RavenDB 4.0 the page size is 8KB and will typically contain several documents, depending on their sizes.

At any point in time, only the data of the current transaction will reside in memory in plain text - and only for the duration of the transaction. When the transaction ends, all of the used memory is safely zeroed.

{NOTE Using encryption may reduce performance by a little bit. However, it **doesn't** affect the ACID properties of RavenDB which remeains both transactional and secured./}


**What about Encryption in Transit?**

To enable encryption in RavenDB, the user must first enable authentication and HTTPS (by providing a certificate).

HTTPS provides protection of the privacy and integrity of the data in transit. It protects against man-in-the-middle attacks, eavesdropping and tampering of the communication.

Using encryption together with HTTPS provides assurance that your data is safe both at rest and in transit.

## Related articles

- [Database Encryption](database-encryption)
- [Server Store Encryption](server-store-encryption)
- [Secret Key Management](secret-key-management)
