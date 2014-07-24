# Indexing polymorphic data

By default, RavenDB indexes operate only on a specific entity type, or a Collection, and it ignores the inheritance hierarchy when it does so.

For example, let us assume that we have the following inheritance hierarchy:

![Figure 1: Polymorphic indexes](images/polymorphic_indexes_faq.png)

If we saved a `Cat`, it would have an Entity-Name of "Cats" and if we saved a `Dog`, it would have an Entity-Name of "Dogs".

If we wanted to index cats by name, we would write:

{CODE-START:csharp /}
    from cat in docs.Cats
    select new { cat.Name }
{CODE-END/}

And for dogs:

{CODE-START:csharp /}
    from dog in docs.Dogs
    select new { dog.Name }
{CODE-END/}

This works, but each index would only give us results for the animal it has been defined on. But what if we wanted to query across all animals?

## Multi-map indexes

The easiest way to do this is by writing a multi-map index like this one:

{CODE multi_map_1@Indexes\IndexingPolymorphicData.cs /}

And query it like this:

{CODE multi_map_2@Indexes\IndexingPolymorphicData.cs /}

You can also use the Linq provider if your objects implement an interface, IAnimal for instance:

{CODE multi_map_3@Indexes\IndexingPolymorphicData.cs /}

## Other ways

Another option would be to modify the way we generate the Entity-Name for subclasses of Animal, like so:

{CODE other_ways_1@Indexes\IndexingPolymorphicData.cs /}

Using this method, we can now index on all animals using:

{CODE-START:csharp /}
    from animal in docs.Animals
    select new { animal.Name }
{CODE-END/}

But what happen when you don't want to modify the entity name of an entity?

You can create a polymorphic index using:

{CODE-START:csharp /}
     from animal in docs.WhereEntityIs("Cats", "Dogs")
     select new { animal.Name }
{CODE-END/}

That would generate an index that would match both Cats and Dogs.

#### Related articles

TODO