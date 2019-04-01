# Indexes: Indexing Polymorphic Data

By default, RavenDB indexes operate only on a specific entity type, or a `Collection`, that ignores the inheritance hierarchy.

For example, let's assume that we have the following inheritance hierarchy:

![Figure 1: Polymorphic indexes](images/polymorphic_indexes_faq.png)

If we saved a `Cat`, it would have a collection set to "Cats" and if we saved a `Dog`, it would be in collection "Dogs".

If we wanted to index cats by name, we would write:

{CODE-BLOCK:csharp}
from cat in docs.Cats
select new { cat.name }
{CODE-BLOCK/}

And for dogs:

{CODE-BLOCK:csharp}
from dog in docs.Dogs
select new { dog.name }
{CODE-BLOCK/}

Although it works, each index would only give us results for the animal it has been defined on. But what if we wanted to query across all animals?

## Multi-Map Indexes

The easiest way to do this is by writing a multi-map index like this one:

{CODE-TABS}
{CODE-TAB:java:MultiMap multi_map_1@Indexes\IndexingPolymorphicData.java /}
{CODE-TAB:java:MultiMapJavaScript multi_map_5@Indexes/JavaScript.java /}
{CODE-TABS/}

And query it like this:

{CODE-TABS}
{CODE-TAB:java:Query multi_map_2@Indexes\IndexingPolymorphicData.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Animals/ByName'
where name = 'Mitzy'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Other Options

Another option would be to modify the way we generate the Collection for subclasses of `Animal`, like this:

{CODE:java other_ways_1@Indexes\IndexingPolymorphicData.java /}

Using this method, we can now index on all animals using:

{CODE-BLOCK:csharp}
from animal in docs.Animals
select new { animal.name }
{CODE-BLOCK/}

But what happens when you don't want to modify the entity name of an entity itself?

You can create a polymorphic index using:

{CODE-BLOCK:csharp}
from animal in docs.WhereEntityIs("Cats", "Dogs")
select new { animal.Name }
{CODE-BLOCK/}

It will generate an index that matches both Cats and Dogs.

## Related Articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Hierarchical Data](../indexes/indexing-hierarchical-data)

### Querying

- [Basics](../indexes/querying/basics)
