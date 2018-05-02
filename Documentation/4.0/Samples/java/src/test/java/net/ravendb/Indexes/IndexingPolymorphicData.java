package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.HashSet;
import java.util.List;

public class IndexingPolymorphicData {
    public IndexingPolymorphicData() {
        //region multi_map_1
        IndexDefinition indexDefinition = new IndexDefinition();
        indexDefinition.setName("Animals/ByName");
        HashSet<String> maps = new HashSet<>();
        maps.add("docs.Cats.Select(c => new { name = c.name})");
        maps.add("docs.Dogs.Select(c => new { name = c.name})");
        indexDefinition.setMaps(maps);
        //endregion

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region multi_map_2
                List<Animal> results = session
                    .query(Animal.class, Query.index("Animals/ByName"))
                    .whereEquals("name", "Mitzy")
                    .toList();
                //endregion
            }
        }
    }

    public void otherWays() {
        //region other_ways_1
        try (IDocumentStore store = new DocumentStore()) {
            store.getConventions().setFindCollectionName(clazz -> {
                if (Animal.class.isAssignableFrom(clazz)) {
                    return "Animals";
                }

                return DocumentConventions.defaultGetCollectionName(clazz);
            });
        }
        //endregion
    }

    public interface IAnimal {
    }

    private static abstract class Animal implements IAnimal {

    }

    private static class Cat extends Animal {

    }

    private static class Dog extends Animal {

    }
}
