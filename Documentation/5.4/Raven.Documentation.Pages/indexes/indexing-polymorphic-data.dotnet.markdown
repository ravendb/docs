# Indexing Polymorphic Data

---
{NOTE: }

* By default, RavenDB indexes are defined on a specific entity type, referred to as a `Collection`,  
  and do not consider the inheritance hierarchy.

* In this Page:  
   * [Polymorphic Data](../indexes/indexing-polymorphic-data#polymorphic-data)  
      * [Multi-Map Indexes](../indexes/indexing-polymorphic-data#multi-map-indexes)  
      * [Other Options](../indexes/indexing-polymorphic-data#other-options)  

{NOTE/}

---

{PANEL: Polymorphic Data}

Let's assume, for example, that we have the following inheritance hierarchy:

![Figure 1: Polymorphic indexes](images/polymorphic_indexes_faq.png)

When saving a `Cat` document, it will be assigned to the "Cats" collection,  
while a `Dog` document will be placed in the "Dogs" collection.

If we intend to create a simple Map-index for Cat documents based on their names, we would write:

{CODE-BLOCK:csharp}
from cat in docs.Cats
select new { cat.Name }
{CODE-BLOCK/}

And for dogs:

{CODE-BLOCK:csharp}
from dog in docs.Dogs
select new { dog.Name }
{CODE-BLOCK/}

{INFO: The challenge}
Querying each index results in documents only from the specific collection the index was defined for.  
However, what if we need to query across ALL animal collections?
{INFO/}

## Multi-Map Indexes

The easiest way to do this is by writing a multi-map index such as:

{CODE-TABS}
{CODE-TAB:csharp:MultiMap multi_map_1@Indexes\IndexingPolymorphicData.cs /}
{CODE-TAB:csharp:MultiMapJavaScript multi_map_5@Indexes/JavaScript.cs /}
{CODE-TABS/}

And query it like this:

{CODE-TABS}
{CODE-TAB:csharp:Query multi_map_3@Indexes\IndexingPolymorphicData.cs /}
{CODE-TAB:csharp:DocumentQuery multi_map_2@Indexes\IndexingPolymorphicData.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Animals/ByName'
where Name = 'Mitzy'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Other Options

Another option would be to modify the way we generate the Collection for subclasses of `Animal`:

{CODE other_ways_1@Indexes\IndexingPolymorphicData.cs /}

Using this method, we can now index on all animals using:

{CODE-BLOCK:csharp}
from animal in docs.Animals
select new { animal.Name }
{CODE-BLOCK/}

But what happens when you don't want to modify the entity name of an entity itself?

You can create a polymorphic index using:

{CODE-BLOCK:csharp}
from animal in docs.WhereEntityIs("Cats", "Dogs")
select new { animal.Name }
{CODE-BLOCK/}

It will generate an index that matches both Cats and Dogs.

{PANEL/}

## Related Articles

### Indexes

- [Indexing basics](../indexes/indexing-basics)
- [Indexing related documents](../indexes/indexing-related-documents)
- [Indexing spatial data](../indexes/indexing-spatial-data)
- [Indexing hierarchical data](../indexes/indexing-hierarchical-data)

### Querying

- [Query overview](../client-api/session/querying/how-to-query)
