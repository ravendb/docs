# Indexing Polymorphic Data
---

{NOTE: }

* By default, RavenDB indexes are defined on a specific entity type, referred to as a `Collection`,  
  and do not consider the inheritance hierarchy.

* In this page:
    * [The challenge](../indexes/indexing-polymorphic-data#the-challenge)
    * [Possible solutions:](../indexes/indexing-polymorphic-data#possible-solutions)
        * [Multi-map index](../indexes/indexing-polymorphic-data#multi-map-index)
        * [Polymorphic index](../indexes/indexing-polymorphic-data#polymorphic-index)
        * [Customize collection](../indexes/indexing-polymorphic-data#customize-collection)

{NOTE/}

---

{PANEL: The challenge}

Let's assume, for example, that we have the following inheritance hierarchy:

![Figure 1: Polymorphic indexes](images/polymorphic_indexes_faq.png)

<br>
**By default**:  
When saving a `Cat` document, it will be assigned to the "Cats" collection,  
while a `Dog` document will be placed in the "Dogs" collection.

If we intend to create a simple Map-index for Cat documents based on their names, we would write:

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_1@indexes\indexingPolymorphicData.js /}
{CODE-TAB:nodejs:Class class_1@indexes\indexingPolymorphicData.js /}
{CODE-TABS/}

And for Dogs:

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_2@indexes\indexingPolymorphicData.js /}
{CODE-TAB:nodejs:Class class_2@indexes\indexingPolymorphicData.js /}
{CODE-TABS/}

**The challenge**:  
Querying each index results in documents only from the specific collection the index was defined for.  
However, what if we need to query across ALL animal collections?

{PANEL/}

{PANEL: Possible solutions}

{NOTE: }

<a id="multi-map-index" /> **Multi-Map Index**:

---

Writing a [Multi-map index](../indexes/multi-map-indexes) enables getting results from all collections the index was defined for.

{CODE-TABS}
{CODE-TAB:nodejs:MultiMap_Index index_3@indexes\indexingPolymorphicData.js /}
{CODE-TABS/}

Query the Multi-map index:

{CODE-TABS}
{CODE-TAB:nodejs:Query query_1@indexes\indexingPolymorphicData.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "CatsAndDogs/ByName"
where name == "Mitzy"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="polymorphic-index" /> **Polymorphic index**:

---

Another option is to create a polymorphic-index.

Use method `WhereEntityIs` within your index definition to index documents from all collections  
listed in the method.

{CODE-TABS}
{CODE-TAB:nodejs:Polymorphic_index index_4@indexes\indexingPolymorphicData.js /}
{CODE-TABS/}

Query the polymorphic-index:

{CODE-TABS}
{CODE-TAB:nodejs:Query query_2@indexes\indexingPolymorphicData.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "CatsAndDogs/ByName"
where name == "Mitzy"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="customize-collection" /> **Customize collection**:

---

This option involves customizing the collection name that is assigned to documents created from  
subclasses of the _Animal_ class.

This is done by setting the [findCollectionName](../client-api/configuration/conventions#findcollectionname) convention on the document store.

{CODE:nodejs define_convention@indexes\indexingPolymorphicData.js /}

With the above convention in place, whenever a _Cat_ or a _Dog_ entity is saved, its document will be assigned the "Animals" collection instead of the default "Cats" or "Dogs" collection.

Now you can define a Map-index on the "Animals" collection:

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_5@indexes\indexingPolymorphicData.js /}
{CODE-TABS/}

Query the index:

{CODE-TABS}
{CODE-TAB:nodejs:Query query_3@indexes\indexingPolymorphicData.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Animals/ByName"
where name == "Mitzy"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

## Related Articles

### Indexes

- [Indexing basics](../indexes/indexing-basics)
- [Indexing related documents](../indexes/indexing-related-documents)
- [Indexing spatial data](../indexes/indexing-spatial-data)
- [Indexing hierarchical data](../indexes/indexing-hierarchical-data)

### Querying

- [Query overview](../client-api/session/querying/how-to-query)
