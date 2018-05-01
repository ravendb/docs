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
                "    name = user.name" +
                "})";

            index("name", FieldIndexing.SEARCH);
        }
    }
    //endregion

    public static class Users_Search extends AbstractIndexCreationTask {
        public Users_Search() {
            map = "docs.Users.Select(user => new {" +
                "    query = new object[] {" +
                "        user.name," +
                "        user.hobbies," +
                "        user.age" +
                "    }" +
                "}))";

            index("query", FieldIndexing.SEARCH);
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
                    .search("name", "John Adam")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_4_0
                List<User> users = session
                    .query(User.class)
                    .search("hobbies", "looking for someone who likes sport books computers")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_5_0
                List<User> users = session
                    .query(User.class)
                    .search("name", "Adam")
                    .search("hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_6_0
                List<User> users = session
                    .query(User.class)
                    .search("hobbies", "I love sport")
                    .boost(10)
                    .search("hobbies", "but also like reading books")
                    .boost(5)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_7_0
                List<User> users = session
                    .query(User.class)
                    .search("hobbies", "computers")
                    .search("name", "James")
                    .whereEquals("age", 20)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_8_0
                List<User> users = session
                    .query(User.class)
                    .search("name", "Adam")
                    .andAlso()
                    .search("hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_9_0
                List<User> users = session
                    .query(User.class)
                    .not()
                    .search("name", "James")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_10_1
                List<User> users = session
                    .query(User.class)
                    .search("name", "Adam")
                    .andAlso()
                    .not()
                    .search("hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_11_0
                List<User> users = session
                    .query(User.class)
                    .search("name", "Jo* Ad*")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_12_0
                List<User> users = session
                    .query(User.class)
                    .search("name", "*oh* *da*")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_20_0
                List<User> users = session
                    .query(User.class, Users_ByName.class)
                    .search("name", "John")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_21_0
                List<User> users = session
                    .query(User.class, Users_Search.class)
                    .search("query", "John")
                    .toList();
                //endregion
            }
        }
    }
}
