package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Searching {

    //region search_20_2
    public static class Users_ByName extends AbstractIndexCreationTask {
        public Users_ByName() {
            map = "docs.Users.Select(user => new {" +
                "    Name = user.Name" +
                "})";

            index("Name", FieldIndexing.SEARCH);
        }
    }
    //endregion

    //region search_21_2
    public static class Users_Search extends AbstractIndexCreationTask {
        public Users_Search() {
            map = "docs.Users.Select(user => new {" +
                "    Query = new object[] {" +
                "        user.Name," +
                "        user.Hobbies," +
                "        user.Age" +
                "    }" +
                "}))";

            index("Query", FieldIndexing.SEARCH);
        }
    }
    //endregion
    
    //region index_all_fields
    public static class Products_ByAllValues extends AbstractIndexCreationTask {
        public static class IndexEntry {
            private String allValues;

            public String getAllValues() {
                return allValues;
            }

            public void setAllValues(String allValues) {
                this.allValues = allValues;
            }
        }

        public Products_ByAllValues() {
            map = "docs.Products.Select(product => new { " +
                  // Use the 'AsJson' method to convert the document into a JSON-like structure
                  // and call 'Select' to extract only the values of each property
                  "    allValues = this.AsJson(product).Select(x => x.Value) " +
                  "})";

            // Configure the index-field for FTS:
            // Set 'FieldIndexing.SEARCH' on index-field 'allValues'
            index("allValues", FieldIndexing.SEARCH);
            
            // Set the search engine type to Lucene:
            searchEngineType = SearchEngineType.LUCENE;
        }
    }
    //endregion    

    //region linq_extensions_search_user_class
    public static class User {
        private String id;
        private String name;
        private byte age;
        private List<String> hobbies;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public byte getAge() {
            return age;
        }

        public void setAge(byte age) {
            this.age = age;
        }

        public List<String> getHobbies() {
            return hobbies;
        }

        public void setHobbies(List<String> hobbies) {
            this.hobbies = hobbies;
        }
    }
    //endregion

    public Searching() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region search_3_0
                List<User> users = session
                    .query(User.class)
                    .search("Name", "John Adam")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_4_0
                List<User> users = session
                    .query(User.class)
                    .search("Hobbies", "looking for someone who likes sport books computers")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_5_0
                List<User> users = session
                    .query(User.class)
                    .search("Name", "Adam")
                    .search("Hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_6_0
                List<User> users = session
                    .query(User.class)
                    .search("Hobbies", "I love sport")
                    .boost(10)
                    .search("Hobbies", "but also like reading books")
                    .boost(5)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_7_0
                List<User> users = session
                    .query(User.class)
                    .search("Hobbies", "computers")
                    .search("Name", "James")
                    .whereEquals("Age", 20)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_8_0
                List<User> users = session
                    .query(User.class)
                    .search("Name", "Adam")
                    .andAlso()
                    .search("Hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_9_0
                List<User> users = session
                    .query(User.class)
                    .not()
                    .search("Name", "James")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_10_1
                List<User> users = session
                    .query(User.class)
                    .search("Name", "Adam")
                    .andAlso()
                    .not()
                    .search("Hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_11_0
                List<User> users = session
                    .query(User.class)
                    .search("Name", "Jo* Ad*")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_12_0
                List<User> users = session
                    .query(User.class)
                    .search("Name", "*oh* *da*")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_20_0
                List<User> users = session
                    .query(User.class, Users_ByName.class)
                    .search("Name", "John")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_21_0
                List<User> users = session
                    .query(User.class, Users_Search.class)
                    .search("Query", "John")
                    .toList();
                //endregion
            }
            
            try (IDocumentSession session = store.openSession()) {
                //region search_22
                List<Product> results = session
                    .query(Products_ByAllValues.IndexEntry.class, Products_ByAllValues.class)
                    .search("allValues", "tofu")
                    .ofType(Product.class)
                    .toList();
                    
                // * Results will contain all Product documents that have 'tofu'
                //   in ANY of their fields.
                //
                // * Search is case-insensitive since the default analyzer is used.
                //endregion
            }
        }
    }
}
