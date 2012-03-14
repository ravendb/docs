#How can I create indexes on startup?

Managing indexes creation is probably the most common task that requires you to manage how Raven works. In particular, you want some way to ensure that all the indexes that you expect are created on startup, so you code can call them.

The RavenDB Client API contains explicit support for this need. Here is how you do this.

* You define your index creation as a class, such as this one:

        public class Movies_ByActor : AbstractIndexCreationTask
        {
            public override IndexDefinition CreateIndexDefinition()
            {
                return new IndexDefinition<Movie>
                {
                    Map = movies => from movie in movies
                                    select new {movie.Name}
                }
                .ToIndexDefinition(DocumentStore.Conventions);
            }
        }

* Somewhere in your startup routine, you include the following line of code:

        IndexCreation.CreateIndexes(typeof(Movies_ByActor).Assembly, store);

And that is it, Raven will scan the provided assembly (you can also provide a MEF catalog, for more complex scenarios) and create all those indexes for you, skipping the creation if the new index definition matches the index definition in the database.

This also provide a small bit of convention, as you can see, the class name is Movies_ByActor, but the index name will be Movies/ByActor. You can override that by overriding the IndexName property
