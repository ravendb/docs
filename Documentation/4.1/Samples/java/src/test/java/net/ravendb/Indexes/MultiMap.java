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
                "    Name = c.Name " +
                "})");

            addMap( "docs.Dogs.Select(d => new { " +
                "    Name = d.Name " +
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
                "    Id = Id(c), " +
                "    Content = new string[] { " +
                "        c.Name " +
                "    }, " +
                "    DisplayName = c.Name, " +
                "    Collection = this.MetadataFor(c)[\"@collection\"] " +
                "})");

            addMap("docs.Products.Select(p => new { " +
                "    Id = Id(p), " +
                "    Content = new string[] { " +
                "        p.Name " +
                "    }, " +
                "    DisplayName = p.Name, " +
                "    Collection = this.MetadataFor(p)[\"@collection\"] " +
                "})");

            addMap("docs.Employees.Select(e => new { " +
                "    Id = Id(e), " +
                "    Content = new string[] { " +
                "        e.FirstName, " +
                "        e.LastName " +
                "    }, " +
                "    DisplayName = (e.FirstName + \" \") + e.LastName, " +
                "    Collection = this.MetadataFor(e)[\"@collection\"] " +
                "})");

            // mark 'content' field as analyzed which enables full text search operations
            index("Content", FieldIndexing.SEARCH);

            // storing fields so when projection (e.g. ProjectInto)
            // requests only those fields
            // then data will come from index only, not from storage
            store("Id", FieldStorage.YES);
            store("DisplayName", FieldStorage.YES);
            store("Collection", FieldStorage.YES);
        }
    }
    //endregion

    public MultiMap() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region multi_map_7
                List<IAnimal> results = session
                    .query(IAnimal.class, Animals_ByName.class)
                    .whereEquals("Name", "Mitzy")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region multi_map_1_1
                List<Smart_Search.Projection> results = session
                    .query(Smart_Search.Result.class, Smart_Search.class)
                    .search("Content", "Lau*")
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
