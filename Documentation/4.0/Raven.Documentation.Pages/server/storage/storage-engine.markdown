﻿# Storage: Storage Engine - Voron

RavenDB uses an in-house managed storage engine called Voron to persist your data (documents, indexes, and configuration). It's a high performance storage engine designed and optimized to the needs of RavenDB. It uses the following structures underneath that allow it to organize the data on persistent storage efficiently:

- B+Tree - Variable size key and value
- Fixed Sized B+Tree - `Int64` key and fixed size value (defined at creation time). It allows you to take advantage of various optimizations
- Raw Data Section – Allows you to store raw data (e.g. documents content) and gives an identifier that allows access to the data in O(1) time
- Table – Combination of Raw Data Sections with any number of indexes that under the hood are regular or Fixed Size B+Trees

## Transaction Support

Voron is a fully transactional storage engine. It uses a Write Ahead Journal (WAJ) to guarantee atomicity and durability features. All modifications made within a transaction
are written to a journal file (unbuffered I/O, write-through) before they are applied to the main data file (and synced to disk). The WAJ application is done in
the background. If the process stopped working and left some modifications not applied to the data file, the database will recover its state on load by replying
the transactions persisted in the journal files. As the journals are flushed and synced to disk before returning on each transaction commit it guarantees they
will survive the event of a process or system crash.

The Multi Versioning Concurrency Control (MVCC) is implemented with the usage of scratch files. They are temporary files which keep concurrent versions of the data for running transactions.
Each transaction has a snapshot of the database and can operate on that with a guarantee that a write transaction won't modify the data it's looking at.

Snapshot isolation for concurrent transactions is provided by Page Translation Tables.

## Single Write Model

Voron supports only single write processes (but there can be multiple read processes). Having only a single write transaction simplifies the handling of writes.
In order to provide high performance, RavenDB implements transaction merging on top of that what gives us a tremendous performance boost in high load scenarios.

In addition to that, Voron has the notion of async transaction commit (with a list of requirements that must happen to be exactly fit in the transaction merging portion in RavenDB),
and the actual transaction lock handoff / early lock released is handled at a higher layer with a lot more information about the system.

## Memory Mapped File

Voron is based on memory mapped files.

{INFO: Running on 32 bits}

Since RavenDB 4.0, Voron has no limits when running in 32 bits mode. The issue of running out of address space when mapping files into memory 
has been addressed by providing a dedicated pager (component responsible for mapping) for a 32 bits environments.

Instead of mapping an entire file, it maps just the pages that are required and only for the duration of the transaction.

{INFO/}

## Requirements

The storage hardware / file system must support:

* fsync
* `[Windows]` UNBUFFERED_IO / WRITE_THROUGH
* `[Windows]` [Hotfix for Windows 7 and Windows Server 2008 R2](https://support.microsoft.com/en-us/help/2731284/33-dos-error-code-when-memory-memory-mapped-files-are-cleaned-by-using)
* `[Posix]` O_DIRECT

## Limitations

- The key size must be less than 2025 bytes in UTF8

## Related Articles

### Transactions

- [Transaction Support in RavenDB](../../client-api/faq/transaction-support)

### Storage

- [Directory Structure](../../server/storage/directory-structure)

### Configuration

- [Storage](../../server/configuration/storage-configuration)


