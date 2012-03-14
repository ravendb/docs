#Polymorphic indexes

By default, RavenDB index only a specific entity type, and it ignores the inheritance hierarchy when it does so.

For example, let us assume that we have the following inheritance hierarchy:  
![Figure 1: Polymorphic indexes](/images/polymorphic_indexes_faq.png)

If we saved a Cat, it would have an Entity Name of "Cats" and if we saved a doc, it would have an Entity Name of "Dogs".

If we wanted to index cats by name, we would write:

    from cat in docs.Cats
    select new { cat.Name }

And for dogs:

    from dog in docs.Dogs
    select new { dog.Name }

This works, but each index would only give us results for the animal it has been defined for. But what happen if we wanted to query across all animals?

One option would be to modify the way we generate the Entity Name for subclasses of Animal, like so:

    var documentStore = new DocumentStore()
    {
        Conventions =
            {
                FindTypeTagName = type =>
                                    {
                                        if (typeof(WhereEntityIs.Animal).IsAssignableFrom(type))
                                            return "Animals";
                                          return DocumentConvention.DefaultTypeTagName(type);
                                    }
            }
    };

Using this method, we can now index on all animals using:

    from animal in docs.Animals
    select new { animal.Name }

But what happen when you don't want to modify the entity name of an entity?

You can create a polymorphic index using:

     from animal in docs.WhereEntityIs("Cats", "Dogs")
     select new { animal.Name }

That would generate an index that would match both Cats and Dogs.