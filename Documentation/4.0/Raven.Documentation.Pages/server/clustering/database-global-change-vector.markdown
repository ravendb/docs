{PANEL: Database Global Change Vector}  
The database global change vector is the same entity as the one used by the data it contains.  
The value of the _global change vector_ entries is determined by the maximum value of all the change vectors contained in its data.  

E.g, a database containing the following documents:  

* Document `A` with _change vector_ `[A:1-0tIXNUeUckSe73dUR6rjrA, B:7-kSXfVRAkKEmffZpyfkd+Zw]`
* Document `B` with _change vector_ `[B:3-kSXfVRAkKEmffZpyfkd+Zw, C:13-ASFfVrAllEmzzZpyrtlrGq]`

Will have the following _global change vector_:

* `[A:1-0tIXNUeUckSe73dUR6rjrA, B:7-kSXfVRAkKEmffZpyfkd+Zw, C:13-ASFfVrAllEmzzZpyrtlrGq]`

The global change vector is used in the replication process to determine which data is already contained by the database.
A document that all of his entries are lower or equal from/to the _global change vector_ is considered contained.
{PANEL/}

{NOTE: }

Note that if data is considered contained by a database it doesn't mean it is present in the database, it may already be overwritten by more up-to-date data.  

{NOTE/}
## Related Articles

- [Change Vector](../clustering/change-vector)
