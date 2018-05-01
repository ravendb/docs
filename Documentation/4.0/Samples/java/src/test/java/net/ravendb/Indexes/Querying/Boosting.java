package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Boosting {

    private static class User {
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

    public Boosting() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region boosting_1_0
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
                //region boosting_2_1
                List<User> users = session
                    .query(User.class)
                    .whereStartsWith("name", "G")
                    .boost(10)
                    .whereStartsWith("name", "A")
                    .boost(5)
                    .toList();
                //endregion
            }
        }
    }
}
