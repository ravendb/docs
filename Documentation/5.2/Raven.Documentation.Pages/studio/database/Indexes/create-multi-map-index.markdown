﻿# Create Multi-Map Index
---

{NOTE: }

* A **Multi-Map index** allows to index data from _multiple_ different collections.  

* RavenDB will generate a _single_ Multi-Map index.  
  The results of querying the Multi-Map index will include data from _all_ these collections.  

* Multi-Map indexes require that all the Map functions defined have the _same_ output structure.  

* In this page:  
  * [Creating Multi-Map index](../../../studio/database/indexes/create-map-index#edit-index-view)  
{NOTE/}

---

{PANEL: Creating Multi-Map index}

**Define a Map Function:**  
![Figure 1. Initial Map Function](images/create-multi-map-index-1.png)  

1. **Index Name** - An index name can be composed of letters, digits, `.`, `/`, `-`, and `_`. The name must be unique in the scope of the database.  
   * Uniqueness is evaluated in a _case-insensitive_ way - you can't create indexes named both `usersbyname` and `UsersByName`.  
   * The characters `_` and `/` are treated as equivalent - you can't create indexes named both `users/byname` and `users_byname`.  
   * If the index name contains the character `.`, it must have some other character on _both sides_ to be valid. `/./` is a valid index name, but 
   `./`, `/.`, and `/../` are all invalid.  

2. The **Map Function**

3. Add another map function to create a multi-map index.

{NOTE: }

The `Collection` field indexed in the above example is not mandatory but can be useful upon querying.
{NOTE/}
<br/>

**Add another Map Function:**  
![Figure 2. Add Another Map Function](images/create-multi-map-index-2.png)  

{NOTE: }

* Any number of additional Map functions can be added.  

* Each added Map should have the **same** output fields.  
  i.e. In the above example, the common indexed fields are: `Name` & `Collection`.  

* So when querying on this Multi-Map index, results will come from **both** Employees collection and Companies collection.  

* [Index field options](../../../studio/database/indexes/create-map-index#index-fields-options), 
  [Configuration](../../../studio/database/indexes/create-map-index#configuration) & 
  [Additional Sources](../../../studio/database/indexes/create-map-index#additional-sources) 
  can be defined for the Multi-Map index in the same way as done for a [Simple Map Index](../../../studio/database/indexes/create-map-index#create-multi-map-index).  
{NOTE/}
{PANEL/}


## Related Articles

### Indexes
- [Map Indexes](../../../indexes/map-indexes)
- [Multi-Map Indexes](../../../indexes/multi-map-indexes)
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio
- [Indexes Overview](../../../studio/database/indexes/indexes-overview)
- [Indexes List View](../../../studio/database/indexes/indexes-list-view)
- [Create Map Index](../../../studio/database/indexes/create-map-index)
- [Create Map-Reduce Index](../../../studio/database/indexes/create-map-reduce-index)
- [Index History View](../../../studio/database/indexes/index-history-view)

