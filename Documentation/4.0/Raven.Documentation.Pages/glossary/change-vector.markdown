# Glosarry : Change Vector

### What is a change vector?
Change vectors are RavenDB implementation of the [Vector clock](https://en.wikipedia.org/wiki/Vector_clock) concept.
They give us partial order over modifications of documents in a RavenDB cluster.

### What are change vectors constructed from?
A change vector is constructed from entries, one per database this vector was modified upon.
It looks like this:  
`A:1-0tIXNUeUckSe73dUR6rjrA, B:7-kSXfVRAkKEmffZpyfkd+Zw`
This change vector is constructed from two entries,  
`A:1-0tIXNUeUckSe73dUR6rjrA` and `B:7-kSXfVRAkKEmffZpyfkd+Zw`.  
Each entry has the following structure `Node tag`:`ETag`-`Database id`, so `A:1-0tIXNUeUckSe73dUR6rjrA` means that
the vector was modified on node `A` its local ETag is `1` and the database id is `0tIXNUeUckSe73dUR6rjrA`.  
`ETags` were widly in use in previous versions of RavenDB and they are still used internally to determine order between elements (documents, attachments, revisions and more) on a single node.  
The database id is used for cases where the node tag is not unique, which could happen when using external replication or restoring from backup.

### How are change vector used to determine order?
Given two change vectors `X` and `Y` we would say that `X` >= `Y` if foreach entry of `X` the value of the ETag is greater or equal to the corresponding entry in `Y` and `Y` has no entries that `X` doesn't have.  
The same goes for <= we would say that `X` <= `Y` if foreach entry of `X` the value of the ETag is smaller or equal to the corresponding entry in `Y` and `X` has no entries that `Y` doesn't have.  
We would say that `X` <> `Y` (no order or conflicted) if `X` has an entry with a higher ETag value than `Y` and `Y` has a different entry where its ETag value is greater than X.
