# Polymorphic indexes

By default, RavenDB indexes operate only on a specific entity type, or a Collection, and it ignores the inheritance hierarchy when it does so.

For example, let us assume that we have the following inheritance hierarchy:

![Figure 1: Polymorphic indexes](images/polymorphic_indexes_faq.png)

If we saved a Cat, it would have an Entity Name of "Cats" and if we saved a doc, it would have an Entity Name of "Dogs".

If we wanted to index cats by name, we would write:

[code lang=csharp]
    from cat in docs.Cats
    select new { cat.Name }
[/code]

And for dogs:

[code lang=csharp]
    from dog in docs.Dogs
    select new { dog.Name }
[/code]

This works, but each index would only give us results for the animal it has been defined on. But what if we wanted to query across all animals?

## Multi-map indexes

The easiest way to do this is by writing a multi-map index like this one:

[code lang=csharp]
	public class AnimalsIndex : AbstractMultiMapIndexCreationTask
	{
		public AnimalsIndex()
		{
			AddMap<Cat>(cats => from c in cats
								select new { c.Name });

			AddMap<Dog>(dogs => from d in dogs
								select new { d.Name });
		}
	}
[/code]

And query it like this:

[code lang=csharp]
var results = session.Advanced.LuceneQuery<object>("IndexName").WhereEquals(";
[/code]

You can also use the Linq provider if your objects implement an interface, IAnimal for instance:

[code lang=csharp]
session.Query<IAnimal>("IndexName").Where(x => x.Name == "Mitzy");
[/code]

## Other ways

Another option would be to modify the way we generate the Entity-Name for subclasses of Animal, like so:

[code lang=csharp]
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
[/code]

Using this method, we can now index on all animals using:

[code lang=csharp]
    from animal in docs.Animals
    select new { animal.Name }
[/code]

But what happen when you don't want to modify the entity name of an entity?

You can create a polymorphic index using:

[code lang=csharp]
     from animal in docs.WhereEntityIs("Cats", "Dogs")
     select new { animal.Name }
[/code]

That would generate an index that would match both Cats and Dogs.
