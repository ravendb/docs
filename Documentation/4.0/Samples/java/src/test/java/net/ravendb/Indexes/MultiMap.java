package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractMultiMapIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class MultiMap {

    //region multi_map_1
    public static class Dog extends Animal {

    }
    //endregion

    //region multi_map_2
    public static class Cate extends Animal {

    }
    //endregion

    //region multi_map_3
    public abstract static class Animal implements IAnimal {
        private String name;

        @Override
        public String getName() {
            return name;
        }

        @Override
        public void setName(String name) {
            this.name = name;
        }
    }
    //endregion

    //region multi_map_6
    public interface IAnimal {
        String getName();
        void setName(String name);
    }
    //endregion

    //region multi_map_4
    public static class Animals_ByName extends AbstractMultiMapIndexCreationTask {
        public Animals_ByName() {
            addMap( "docs.Cats.Select(c => new { " +
                "    name = c.name " +
                "})");

            addMap( "docs.Dogs.Select(d => new { " +
                "    name = d.name " +
                "})");
        }
    }
    //endregion

    //region multi_map_1_0
    public static class Smart_Search extends AbstractMultiMapIndexCreationTask {
        public static class Result {
            private String id;
            private String displayName;
            private String collection;
            private String content;

            public String getId() {
                return id;
            }

            public void setId(String id) {
                this.id = id;
            }

            public String getDisplayName() {
                return displayName;
            }

            public void setDisplayName(String displayName) {
                this.displayName = displayName;
            }

            public String getCollection() {
                return collection;
            }

            public void setCollection(String collection) {
                this.collection = collection;
            }

            public String getContent() {
                return content;
            }

            public void setContent(String content) {
                this.content = content;
            }
        }

        public static class Projection {
            private String id;
            private String displayName;
            private String collection;

            public String getId() {
                return id;
            }

            public void setId(String id) {
                this.id = id;
            }

            public String getDisplayName() {
                return displayName;
            }

            public void setDisplayName(String displayName) {
                this.displayName = displayName;
            }

            public String getCollection() {
                return collection;
            }

            public void setCollection(String collection) {
                this.collection = collection;
            }
        }

        public Smart_Search() {

            addMap("docs.Companies.Select(c => new { " +
                "    id = Id(c), " +
                "    content = new string[] { " +
                "        c.name " +
                "    }, " +
                "    displayName = c.name, " +
                "    collection = this.MetadataFor(c)[\"@collection\"] " +
                "})");

            addMap("docs.Products.Select(p => new { " +
                "    id = Id(p), " +
                "    content = new string[] { " +
                "        p.name " +
                "    }, " +
                "    displayName = p.name, " +
                "    collection = this.MetadataFor(p)[\"@collection\"] " +
                "})");

            addMap("docs.Employees.Select(e => new { " +
                "    id = Id(e), " +
                "    content = new string[] { " +
                "        e.firstName, " +
                "        e.lastName " +
                "    }, " +
                "    displayName = (e.firstName + \" \") + e.lastName, " +
                "    collection = this.MetadataFor(e)[\"@collection\"] " +
                "})");

            // mark 'content' field as analyzed which enables full text search operations
            index("content", FieldIndexing.SEARCH);

            // storing fields so when projection (e.g. ProjectInto)
            // requests only those fields
            // then data will come from index only, not from storage
            store("id", FieldStorage.YES);
            store("displayName", FieldStorage.YES);
            store("collection", FieldStorage.YES);
        }
    }
    //endregion

    public MultiMap() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region multi_map_7
                List<IAnimal> results = session
                    .query(IAnimal.class, Animals_ByName.class)
                    .whereEquals("name", "Mitzy")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region multi_map_1_1
                List<Smart_Search.Projection> results = session
                    .query(Smart_Search.Result.class, Smart_Search.class)
                    .search("content", "Lau*")
                    .selectFields(Smart_Search.Projection.class)
                    .toList();

                for (Smart_Search.Projection result : results) {
                    System.out.println(result.getCollection() + ": " + result.getDisplayName());
                    // Companies: Laughing Bacchus Wine Cellars
                    // Products: Laughing Lumberjack Lager
                    // Employees: Laura Callahan
                }
                //endregion
            }
        }
    }
}
